using CrystalBridgeService.Models;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace CrystalBridgeService.Services
{
    public sealed class CrystalReportService : ICrystalReportService
    {
        private sealed class SubmissionContext
        {
            public int? DocumentTypeId { get; set; }
            public string DocumentNumber { get; set; }
        }

        private readonly string _connectionString;
        private readonly string _reportsRootPath;

        public CrystalReportService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["FormBuilderDb"]?.ConnectionString
                ?? throw new InvalidOperationException("Missing connection string 'FormBuilderDb' in Web.config.");

            _reportsRootPath = ResolveReportsRootPath(ConfigurationManager.AppSettings["ReportsRootPath"]);
        }

        public ReportFileResult GenerateLayoutPdf(int idLayout, int idObject, string fileName, string printedByUserId)
        {
            if (idLayout <= 0)
                throw new ArgumentOutOfRangeException(nameof(idLayout));
            if (idObject <= 0)
                throw new ArgumentOutOfRangeException(nameof(idObject));

            var submissionContext = LoadSubmissionContext(idObject);
            var layout = LoadLayout(idLayout, submissionContext);
            if (layout == null)
                throw new InvalidOperationException("Layout not found or inactive.");

            string reportPath = ResolveReportPath(layout.LayoutPath);
            if (!File.Exists(reportPath))
                throw new FileNotFoundException("Report file not found.", reportPath);

            string safeFileName = string.IsNullOrWhiteSpace(fileName) ? "Report" : fileName.Trim();

            using (var reportDocument = new ReportDocument())
            {
                reportDocument.Load(reportPath);

                SetParameterIfExists(reportDocument, idObject, "DocKey@", "DocKey", "@DocKey");
                SetParameterIfExists(reportDocument, idObject, "ObjectId@", "ObjectId", "@ObjectId", "DocEntry@", "DocEntry", "SubmissionId@", "SubmissionId");
                SetParameterIfExists(reportDocument, layout.DocumentTypeId, "DocumentTypeId@", "DocumentTypeId", "@DocumentTypeId");
                SetParameterIfExists(reportDocument, layout.DocumentTypeId, "ObjectTypeId@", "ObjectTypeId", "@ObjectTypeId");

                if (!string.IsNullOrWhiteSpace(submissionContext.DocumentNumber))
                {
                    SetParameterIfExists(reportDocument, submissionContext.DocumentNumber,
                        "DocumentNumber@", "DocumentNumber",
                        "DocNumber@", "DocNumber",
                        "DocNum@", "DocNum");
                }

                SetParameterIfExists(reportDocument, NormalizePrintedByUserId(printedByUserId), "PrintedByUserID@", "PrintedByUserID");
                SetParameterIfExists(reportDocument, _reportsRootPath, "ApplicationPath@", "ApplicationPath");

                ApplyDatabaseLogon(reportDocument);

                using (var stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat))
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return new ReportFileResult
                    {
                        Content = memoryStream.ToArray(),
                        ContentType = "application/pdf",
                        FileName = safeFileName + ".pdf"
                    };
                }
            }
        }

        private SubmissionContext LoadSubmissionContext(int idObject)
        {
            const string submissionSql = @"
SELECT TOP 1
    DocumentTypeId,
    DocumentNumber
FROM dbo.FORM_SUBMISSIONS
WHERE Id = @Id
  AND IsDeleted = 0";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(submissionSql, connection))
            {
                command.Parameters.AddWithValue("@Id", idObject);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return new SubmissionContext();
                    }

                    var context = new SubmissionContext();
                    if (!reader.IsDBNull(0))
                        context.DocumentTypeId = reader.GetInt32(0);
                    if (!reader.IsDBNull(1))
                        context.DocumentNumber = reader.GetString(1);

                    return context;
                }
            }
        }

        private DynamicLayoutConfig LoadLayout(int idLayout, SubmissionContext submissionContext)
        {
            const string layoutByIdSql = @"
SELECT TOP 1
    Id,
    DocumentTypeId,
    LayoutName,
    LayoutPath
FROM dbo.CRYSTAL_LAYOUTS
WHERE Id = @Id
  AND (@SubmissionDocumentTypeId IS NULL OR DocumentTypeId = @SubmissionDocumentTypeId)
  AND IsActive = 1
  AND IsDeleted = 0";

            const string defaultLayoutByDocTypeSql = @"
SELECT TOP 1
    Id,
    DocumentTypeId,
    LayoutName,
    LayoutPath
FROM dbo.CRYSTAL_LAYOUTS
WHERE DocumentTypeId = @DocumentTypeId
  AND IsActive = 1
  AND IsDeleted = 0
ORDER BY IsDefault DESC, Id ASC";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var layoutByIdCmd = new SqlCommand(layoutByIdSql, connection))
                {
                    layoutByIdCmd.Parameters.AddWithValue("@Id", idLayout);
                    layoutByIdCmd.Parameters.AddWithValue("@SubmissionDocumentTypeId", (object?)submissionContext.DocumentTypeId ?? DBNull.Value);
                    using (var reader = layoutByIdCmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return MapLayout(reader);
                    }
                }

                if (!submissionContext.DocumentTypeId.HasValue || submissionContext.DocumentTypeId.Value <= 0)
                    return null;

                using (var defaultLayoutCmd = new SqlCommand(defaultLayoutByDocTypeSql, connection))
                {
                    defaultLayoutCmd.Parameters.AddWithValue("@DocumentTypeId", submissionContext.DocumentTypeId.Value);
                    using (var reader = defaultLayoutCmd.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        return MapLayout(reader);
                    }
                }
            }
        }

        private static DynamicLayoutConfig MapLayout(SqlDataReader reader)
        {
            return new DynamicLayoutConfig
            {
                Id = reader.GetInt32(0),
                DocumentTypeId = reader.GetInt32(1),
                LayoutName = reader.GetString(2),
                LayoutPath = reader.GetString(3)
            };
        }

        private string ResolveReportPath(string layoutPath)
        {
            if (string.IsNullOrWhiteSpace(layoutPath))
                throw new InvalidOperationException("LayoutPath is empty.");

            string normalized = layoutPath.Replace("/", "\\").TrimStart('\\');

            if (Path.IsPathRooted(normalized))
                return normalized;

            string[] candidates = BuildPathCandidates(normalized);
            for (int i = 0; i < candidates.Length; i++)
            {
                if (File.Exists(candidates[i]))
                    return candidates[i];
            }

            return candidates[0];
        }

        private string[] BuildPathCandidates(string normalizedPath)
        {
            string systemLayoutPrefix = Path.Combine("Attachments", "SystemLayout") + "\\";
            string reportsPrefix = "Reports\\";

            string primary = Path.GetFullPath(Path.Combine(_reportsRootPath, normalizedPath));
            string fromAppRoot = HostingEnvironment.MapPath("~/" + normalizedPath.Replace("\\", "/"));

            string withoutReportsPrefix = normalizedPath.StartsWith(reportsPrefix, StringComparison.OrdinalIgnoreCase)
                ? normalizedPath.Substring(reportsPrefix.Length)
                : normalizedPath;
            string fromReportsPrefix = Path.GetFullPath(Path.Combine(_reportsRootPath, withoutReportsPrefix));

            string withoutSystemLayoutPrefix = normalizedPath.StartsWith(systemLayoutPrefix, StringComparison.OrdinalIgnoreCase)
                ? normalizedPath.Substring(systemLayoutPrefix.Length)
                : normalizedPath;
            string fromSystemLayoutPrefix = Path.GetFullPath(Path.Combine(_reportsRootPath, withoutSystemLayoutPrefix));

            if (!string.IsNullOrWhiteSpace(fromAppRoot))
            {
                return new[]
                {
                    primary,
                    fromReportsPrefix,
                    fromSystemLayoutPrefix,
                    Path.GetFullPath(fromAppRoot)
                };
            }

            return new[] { primary, fromReportsPrefix, fromSystemLayoutPrefix };
        }

        private static object NormalizePrintedByUserId(string printedByUserId)
        {
            if (string.IsNullOrWhiteSpace(printedByUserId))
                return string.Empty;

            int parsedInt;
            if (int.TryParse(printedByUserId, out parsedInt))
                return parsedInt;

            return printedByUserId.Trim();
        }

        private static string ResolveReportsRootPath(string configuredPath)
        {
            string pathFromConfig = configuredPath == null ? string.Empty : configuredPath.Trim();
            string appDefault = HostingEnvironment.MapPath("~/Attachments/SystemLayout");
            string fallback = HostingEnvironment.MapPath("~/Reports");

            if (string.IsNullOrWhiteSpace(pathFromConfig))
            {
                if (!string.IsNullOrWhiteSpace(appDefault))
                    return appDefault;

                if (!string.IsNullOrWhiteSpace(fallback))
                    return fallback;

                throw new InvalidOperationException("Unable to resolve reports root path.");
            }

            if (Path.IsPathRooted(pathFromConfig))
                return Path.GetFullPath(pathFromConfig);

            string appRelative = HostingEnvironment.MapPath(pathFromConfig);
            if (!string.IsNullOrWhiteSpace(appRelative))
                return Path.GetFullPath(appRelative);

            if (!string.IsNullOrWhiteSpace(appDefault))
                return appDefault;

            if (!string.IsNullOrWhiteSpace(fallback))
                return fallback;

            throw new InvalidOperationException("Unable to resolve reports root path.");
        }

        private void ApplyDatabaseLogon(ReportDocument reportDocument)
        {
            var builder = new SqlConnectionStringBuilder(_connectionString);
            if (string.IsNullOrWhiteSpace(builder.DataSource) || string.IsNullOrWhiteSpace(builder.InitialCatalog))
                return;

            string user = builder.UserID ?? string.Empty;
            string pass = builder.Password ?? string.Empty;
            string source = builder.DataSource;
            string catalog = builder.InitialCatalog;

            reportDocument.SetDatabaseLogon(user, pass, source, catalog);

            for (int i = 0; i < reportDocument.DataSourceConnections.Count; i++)
                reportDocument.DataSourceConnections[i].SetConnection(source, catalog, user, pass);

            ApplyTableLogon(reportDocument, source, catalog, user, pass);

            foreach (ReportDocument subreport in reportDocument.Subreports)
            {
                subreport.SetDatabaseLogon(user, pass, source, catalog);
                ApplyTableLogon(subreport, source, catalog, user, pass);
            }
        }

        private static void ApplyTableLogon(ReportDocument reportDocument, string source, string catalog, string user, string pass)
        {
            foreach (Table table in reportDocument.Database.Tables)
            {
                var logon = table.LogOnInfo;
                logon.ConnectionInfo.ServerName = source;
                logon.ConnectionInfo.DatabaseName = catalog;
                logon.ConnectionInfo.UserID = user;
                logon.ConnectionInfo.Password = pass;
                logon.ConnectionInfo.IntegratedSecurity = string.IsNullOrWhiteSpace(user);
                table.ApplyLogOnInfo(logon);
            }
        }

        private static void SetParameterIfExists(ReportDocument reportDocument, object value, params string[] candidateNames)
        {
            if (candidateNames == null || candidateNames.Length == 0)
                return;

            if (SetParameterOnDocument(reportDocument, value, candidateNames))
                return;

            foreach (ReportDocument subreport in reportDocument.Subreports)
            {
                if (SetParameterOnDocument(subreport, value, candidateNames))
                    return;
            }
        }

        private static bool SetParameterOnDocument(ReportDocument reportDocument, object value, IEnumerable<string> candidateNames)
        {
            var parameters = reportDocument.DataDefinition.ParameterFields
                .Cast<ParameterFieldDefinition>()
                .Select(x => x.Name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var name in candidateNames)
            {
                if (!parameters.Contains(name))
                    continue;

                reportDocument.SetParameterValue(name, value);
                return true;
            }

            return false;
        }
    }
}
