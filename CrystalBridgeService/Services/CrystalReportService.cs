using CrystalBridgeService.Models;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace CrystalBridgeService.Services
{
    public sealed class CrystalReportService : ICrystalReportService
    {
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

            var layout = LoadLayout(idLayout);
            if (layout == null)
                throw new InvalidOperationException("Layout not found or inactive.");

            string reportPath = ResolveReportPath(layout.LayoutPath);
            if (!File.Exists(reportPath))
                throw new FileNotFoundException("Report file not found.", reportPath);

            string safeFileName = string.IsNullOrWhiteSpace(fileName) ? "Report" : fileName.Trim();

            using (var reportDocument = new ReportDocument())
            {
                reportDocument.Load(reportPath);

                SetParameterIfExists(reportDocument, "DocKey@", idObject);
                SetParameterIfExists(reportDocument, "ObjectId@", layout.DocumentTypeId);
                SetParameterIfExists(reportDocument, "PrintedByUserID@", NormalizePrintedByUserId(printedByUserId));
                SetParameterIfExists(reportDocument, "ApplicationPath@", _reportsRootPath);

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

        private DynamicLayoutConfig LoadLayout(int idLayout)
        {
            const string sql = @"
SELECT TOP 1
    Id,
    DocumentTypeId,
    LayoutName,
    LayoutPath
FROM dbo.CRYSTAL_LAYOUTS
WHERE Id = @Id
  AND IsActive = 1
  AND IsDeleted = 0";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", idLayout);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return new DynamicLayoutConfig
                    {
                        Id = reader.GetInt32(0),
                        DocumentTypeId = reader.GetInt32(1),
                        LayoutName = reader.GetString(2),
                        LayoutPath = reader.GetString(3)
                    };
                }
            }
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
        }

        private static void SetParameterIfExists(ReportDocument reportDocument, string name, object value)
        {
            var exists = reportDocument.DataDefinition.ParameterFields
                .Cast<ParameterFieldDefinition>()
                .Any(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (exists)
                reportDocument.SetParameterValue(name, value);
        }
    }
}
