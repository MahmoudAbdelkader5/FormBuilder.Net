using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Core.IServices.FormBuilder;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.Domian.Entitys.froms;
using formBuilder.Domian.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FormBuilder.Services.Services.FormBuilder
{
    public class SapDynamicIntegrationService : ISapDynamicIntegrationService
    {
        private const string ServiceLayerApiPath = "/b1s/v1";

        private static readonly XNamespace[] EdmNamespaces =
        {
            "http://docs.oasis-open.org/odata/ns/edm",      // OData v4
            "http://schemas.microsoft.com/ado/2009/11/edm", // OData v3
            "http://schemas.microsoft.com/ado/2008/09/edm"  // OData v2
        };

        private readonly IunitOfwork _unitOfWork;
        private readonly ISapHanaConfigsService _sapConfigsService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<SapDynamicIntegrationService> _logger;

        public SapDynamicIntegrationService(
            IunitOfwork unitOfWork,
            ISapHanaConfigsService sapConfigsService,
            IHttpClientFactory httpClientFactory,
            ILogger<SapDynamicIntegrationService> logger)
        {
            _unitOfWork = unitOfWork;
            _sapConfigsService = sapConfigsService;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<SapIntegrationSettingsDto?> GetSettingsByDocumentTypeAsync(int documentTypeId)
        {
            var entity = await _unitOfWork.AppDbContext.Set<SAP_INTEGRATION_SETTINGS>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.DocumentTypeId == documentTypeId && !x.IsDeleted);

            return entity == null ? null : ToSettingsDto(entity);
        }

        public async Task<SapIntegrationSettingsDto> UpsertSettingsAsync(UpsertSapIntegrationSettingsDto dto)
        {
            ValidateExecutionMode(dto.ExecutionMode, dto.TriggerStageId);
            var normalizedHttpMethod = NormalizeHttpMethod(dto.HttpMethod);

            var existing = await _unitOfWork.AppDbContext.Set<SAP_INTEGRATION_SETTINGS>()
                .FirstOrDefaultAsync(x => x.DocumentTypeId == dto.DocumentTypeId && !x.IsDeleted);

            if (existing == null)
            {
                existing = new SAP_INTEGRATION_SETTINGS
                {
                    DocumentTypeId = dto.DocumentTypeId,
                    CreatedDate = DateTime.UtcNow
                };
                _unitOfWork.AppDbContext.Set<SAP_INTEGRATION_SETTINGS>().Add(existing);
            }

            existing.SapConfigId = dto.SapConfigId;
            existing.TargetEndpoint = dto.TargetEndpoint.Trim();
            existing.HttpMethod = normalizedHttpMethod;
            existing.TargetObject = dto.TargetObject?.Trim();
            existing.ExecutionMode = dto.ExecutionMode.Trim();
            existing.TriggerStageId = dto.TriggerStageId;
            existing.BlockWorkflowOnError = dto.BlockWorkflowOnError;
            existing.IsActive = dto.IsActive;
            existing.UpdatedDate = DateTime.UtcNow;
            existing.IsDeleted = false;

            await _unitOfWork.CompleteAsyn();
            return ToSettingsDto(existing);
        }

        public async Task<List<SapFieldMappingDto>> GetFieldMappingsByFormBuilderIdAsync(int formBuilderId)
        {
            var tabIds = await _unitOfWork.AppDbContext.Set<FORM_TABS>()
                .AsNoTracking()
                .Where(t => t.FormBuilderId == formBuilderId && !t.IsDeleted)
                .Select(t => t.Id)
                .ToListAsync();

            var fields = await _unitOfWork.AppDbContext.Set<FORM_FIELDS>()
                .AsNoTracking()
                .Where(f => tabIds.Contains(f.TabId) && !f.IsDeleted)
                .Select(f => new { f.Id, f.FieldCode, f.FieldName })
                .ToListAsync();

            var fieldIds = fields.Select(x => x.Id).ToList();

            var mappings = await _unitOfWork.AppDbContext.Set<SAP_FIELD_MAPPINGS>()
                .AsNoTracking()
                .Where(m => fieldIds.Contains(m.FormFieldId) && !m.IsDeleted)
                .ToListAsync();

            return fields.Select(field =>
            {
                var map = mappings.FirstOrDefault(x => x.FormFieldId == field.Id);
                return new SapFieldMappingDto
                {
                    Id = map?.Id ?? 0,
                    FormFieldId = field.Id,
                    FieldCode = field.FieldCode ?? string.Empty,
                    FieldName = field.FieldName ?? string.Empty,
                    SapFieldName = map?.SapFieldName ?? string.Empty,
                    IsActive = map?.IsActive ?? false
                };
            }).ToList();
        }

        public async Task<List<SapFieldMappingDto>> SaveFieldMappingsAsync(SaveSapFieldMappingsDto dto)
        {
            var tabIds = await _unitOfWork.AppDbContext.Set<FORM_TABS>()
                .AsNoTracking()
                .Where(t => t.FormBuilderId == dto.FormBuilderId && !t.IsDeleted)
                .Select(t => t.Id)
                .ToListAsync();

            var formFields = await _unitOfWork.AppDbContext.Set<FORM_FIELDS>()
                .Where(f => tabIds.Contains(f.TabId) && !f.IsDeleted)
                .ToListAsync();

            var fieldIds = formFields.Select(f => f.Id).ToHashSet();
            var payload = dto.Mappings
                .Where(x => fieldIds.Contains(x.FormFieldId))
                .ToList();

            var existing = await _unitOfWork.AppDbContext.Set<SAP_FIELD_MAPPINGS>()
                .Where(m => fieldIds.Contains(m.FormFieldId) && !m.IsDeleted)
                .ToListAsync();

            var incomingByField = payload.ToDictionary(x => x.FormFieldId, x => x);

            foreach (var map in existing)
            {
                if (!incomingByField.TryGetValue(map.FormFieldId, out var incoming))
                {
                    map.IsDeleted = true;
                    map.IsActive = false;
                    map.UpdatedDate = DateTime.UtcNow;
                    continue;
                }

                map.SapFieldName = incoming.SapFieldName.Trim();
                map.Direction = "Outbound";
                map.IsActive = incoming.IsActive;
                map.IsDeleted = false;
                map.UpdatedDate = DateTime.UtcNow;
            }

            var existingFieldIds = existing.Select(x => x.FormFieldId).ToHashSet();
            foreach (var incoming in payload.Where(x => !existingFieldIds.Contains(x.FormFieldId)))
            {
                var entity = new SAP_FIELD_MAPPINGS
                {
                    FormFieldId = incoming.FormFieldId,
                    SapFieldName = incoming.SapFieldName.Trim(),
                    Direction = "Outbound",
                    IsActive = incoming.IsActive,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };
                _unitOfWork.AppDbContext.Set<SAP_FIELD_MAPPINGS>().Add(entity);
            }

            await _unitOfWork.CompleteAsyn();
            return await GetFieldMappingsByFormBuilderIdAsync(dto.FormBuilderId);
        }

        public async Task<List<SapServiceLayerEndpointDto>> GetServiceLayerEndpointsAsync(int sapConfigId)
        {
            var metadata = await GetServiceLayerMetadataAsync(sapConfigId);
            if (string.IsNullOrWhiteSpace(metadata))
                return new List<SapServiceLayerEndpointDto>();

            var doc = XDocument.Parse(metadata);
            var container = FindFirstByLocalName(doc, "EntityContainer");
            if (container == null)
                return new List<SapServiceLayerEndpointDto>();

            return FindByLocalName(container, "EntitySet")
                .Select(x => new SapServiceLayerEndpointDto
                {
                    Name = x.Attribute("Name")?.Value ?? string.Empty,
                    EntityType = x.Attribute("EntityType")?.Value
                })
                .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                .OrderBy(x => x.Name)
                .ToList();
        }

        public async Task<List<SapServiceLayerObjectFieldDto>> GetServiceLayerObjectFieldsAsync(int sapConfigId, string endpointName)
        {
            var metadata = await GetServiceLayerMetadataAsync(sapConfigId);
            if (string.IsNullOrWhiteSpace(metadata))
                return new List<SapServiceLayerObjectFieldDto>();

            var doc = XDocument.Parse(metadata);
            var container = FindFirstByLocalName(doc, "EntityContainer");
            var entitySet = container == null
                ? null
                : FindByLocalName(container, "EntitySet")
                .FirstOrDefault(x => string.Equals(x.Attribute("Name")?.Value, endpointName, StringComparison.OrdinalIgnoreCase));
            var entityTypeFullName = entitySet?.Attribute("EntityType")?.Value;
            if (string.IsNullOrWhiteSpace(entityTypeFullName))
                return new List<SapServiceLayerObjectFieldDto>();

            var entityTypeName = entityTypeFullName.Split('.').Last();
            var entity = FindByLocalName(doc, "EntityType")
                .FirstOrDefault(x => string.Equals(x.Attribute("Name")?.Value, entityTypeName, StringComparison.OrdinalIgnoreCase));

            if (entity == null)
                return new List<SapServiceLayerObjectFieldDto>();

            return FindByLocalName(entity, "Property")
                .Select(p => new SapServiceLayerObjectFieldDto
                {
                    Name = p.Attribute("Name")?.Value ?? string.Empty,
                    Type = p.Attribute("Type")?.Value ?? string.Empty,
                    Nullable = !string.Equals(p.Attribute("Nullable")?.Value, "false", StringComparison.OrdinalIgnoreCase)
                })
                .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                .OrderBy(x => x.Name)
                .ToList();
        }

        public async Task<List<SapServiceLayerObjectFieldDto>> GetAllServiceLayerObjectFieldsAsync(int sapConfigId)
        {
            var metadata = await GetServiceLayerMetadataAsync(sapConfigId);
            if (string.IsNullOrWhiteSpace(metadata))
                return new List<SapServiceLayerObjectFieldDto>();

            var doc = XDocument.Parse(metadata);
            return FindByLocalName(doc, "EntityType")
                .SelectMany(entity => FindByLocalName(entity, "Property"))
                .Select(p => new SapServiceLayerObjectFieldDto
                {
                    Name = p.Attribute("Name")?.Value ?? string.Empty,
                    Type = p.Attribute("Type")?.Value ?? string.Empty,
                    Nullable = !string.Equals(p.Attribute("Nullable")?.Value, "false", StringComparison.OrdinalIgnoreCase)
                })
                .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                .GroupBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First())
                .OrderBy(x => x.Name)
                .ToList();
        }

        private static XElement? FindFirstByLocalName(XContainer root, string localName)
        {
            return FindByLocalName(root, localName).FirstOrDefault();
        }

        private static IEnumerable<XElement> FindByLocalName(XContainer root, string localName)
        {
            foreach (var ns in EdmNamespaces)
            {
                foreach (var node in root.Descendants(ns + localName))
                    yield return node;
            }
        }

        public async Task<bool> ReLoginServiceLayerAsync(int sapConfigId)
        {
            // Force auth cycle (session/token validation) and metadata call.
            // If credentials/session are invalid, this will throw with a clear message.
            var connection = await GetServiceLayerConnectionAsync(sapConfigId);
            await SendMetadataWithSslFallbackAsync(connection);
            return true;
        }

        public async Task<SapIntegrationExecuteResultDto> ExecuteForSubmissionAsync(int submissionId, string eventType, int? stageId = null)
        {
            var result = new SapIntegrationExecuteResultDto
            {
                SubmissionId = submissionId,
                EventType = eventType ?? string.Empty,
                Status = "Skipped",
                Success = true
            };

            var submission = await _unitOfWork.AppDbContext.Set<FORM_SUBMISSIONS>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == submissionId && !x.IsDeleted);

            if (submission == null)
            {
                result.Success = false;
                result.Status = "Failed";
                result.ErrorMessage = "Submission not found.";
                return result;
            }

            result.FormId = submission.FormBuilderId;

            var settings = await _unitOfWork.AppDbContext.Set<SAP_INTEGRATION_SETTINGS>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.DocumentTypeId == submission.DocumentTypeId && !x.IsDeleted && x.IsActive);

            if (settings == null)
            {
                result.Status = "Skipped";
                result.ErrorMessage = "SAP integration is not enabled for this document type.";
                return result;
            }

            if (!ShouldRunForEvent(settings, eventType, stageId))
            {
                result.Status = "Skipped";
                result.ErrorMessage = $"SAP integration execution mode '{settings.ExecutionMode}' does not match event '{eventType}'.";
                return result;
            }

            result.SapConfigId = settings.SapConfigId;
            result.Endpoint = settings.TargetEndpoint;
            result.ShouldBlockWorkflow = settings.BlockWorkflowOnError;

            try
            {
                var fieldIds = await _unitOfWork.AppDbContext.Set<FORM_FIELDS>()
                    .AsNoTracking()
                    .Where(f => !f.IsDeleted &&
                                _unitOfWork.AppDbContext.Set<FORM_TABS>()
                                    .Any(t => t.Id == f.TabId && t.FormBuilderId == submission.FormBuilderId && !t.IsDeleted))
                    .Select(f => f.Id)
                    .ToListAsync();

                var mappings = await _unitOfWork.AppDbContext.Set<SAP_FIELD_MAPPINGS>()
                    .AsNoTracking()
                    .Where(m => fieldIds.Contains(m.FormFieldId) && !m.IsDeleted && m.IsActive)
                    .ToListAsync();

                if (!mappings.Any())
                {
                    result.Status = "Skipped";
                    result.ErrorMessage = "No SAP field mappings configured.";
                    return result;
                }

                var values = await _unitOfWork.AppDbContext.Set<FORM_SUBMISSION_VALUES>()
                    .AsNoTracking()
                    .Where(v => v.SubmissionId == submission.Id && !v.IsDeleted)
                    .ToListAsync();

                var valuesByField = values.GroupBy(v => v.FieldId).ToDictionary(g => g.Key, g => g.First());
                var payload = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

                foreach (var map in mappings)
                {
                    if (!valuesByField.TryGetValue(map.FormFieldId, out var valueEntity))
                        continue;

                    var normalizedPath = NormalizeSapFieldPathForPayload(
                        map.SapFieldName,
                        settings.TargetObject,
                        settings.TargetEndpoint);
                    SetPayloadValue(payload, normalizedPath, GetSubmissionValue(valueEntity));
                }

                if (payload.Count == 0)
                {
                    result.Status = "Skipped";
                    result.ErrorMessage = "No mapped submission values found.";
                    return result;
                }

                if (IsProductionOrdersRequest(settings.TargetEndpoint, settings.TargetObject) &&
                    !HasAnyNonEmptyPayloadValue(payload, "ItemCode", "ItemNo"))
                {
                    result.Success = false;
                    result.Status = "Failed";
                    result.ErrorMessage =
                        "Missing required SAP field for ProductionOrders. " +
                        "Add an active field mapping to ItemCode (or ItemNo) and ensure the submission has a value.";
                    result.RequestPayloadJson = JsonSerializer.Serialize(payload);
                    await AddIntegrationLogAsync(result);
                    return result;
                }

                var resolvedEndpoint = ResolveEndpointTemplate(settings.TargetEndpoint, payload, submission);
                result.Endpoint = resolvedEndpoint;
                result.RequestPayloadJson = JsonSerializer.Serialize(payload);
                var response = await SendToServiceLayerAsync(
                    settings.SapConfigId,
                    resolvedEndpoint,
                    settings.HttpMethod,
                    payload);
                result.ResponsePayloadJson = response.responseBody;
                result.Success = response.success;
                result.Status = response.success ? "Success" : "Failed";
                result.ErrorMessage = response.errorMessage;

                await AddIntegrationLogAsync(result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SAP execution failed for submission {SubmissionId}", submissionId);
                result.Success = false;
                result.Status = "Failed";
                result.ErrorMessage = ex.Message;
                await AddIntegrationLogAsync(result);
                return result;
            }
        }

        public async Task<List<SapIntegrationLogDto>> GetLogsAsync(int? formId = null, int? submissionId = null, int? sapConfigId = null, int take = 100)
        {
            var query = _unitOfWork.AppDbContext.Set<SAP_INTEGRATION_LOGS>()
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            if (formId.HasValue)
                query = query.Where(x => x.FormId == formId.Value);
            if (submissionId.HasValue)
                query = query.Where(x => x.SubmissionId == submissionId.Value);
            if (sapConfigId.HasValue)
                query = query.Where(x => x.SapConfigId == sapConfigId.Value);

            var list = await query
                .OrderByDescending(x => x.TimestampUtc)
                .Take(Math.Clamp(take, 1, 500))
                .ToListAsync();

            return list.Select(x => new SapIntegrationLogDto
            {
                Id = x.Id,
                FormId = x.FormId,
                SubmissionId = x.SubmissionId,
                SapConfigId = x.SapConfigId,
                Endpoint = x.Endpoint,
                EventType = x.EventType,
                Status = x.Status,
                RequestPayloadJson = x.RequestPayloadJson,
                ResponsePayloadJson = x.ResponsePayloadJson,
                ErrorMessage = x.ErrorMessage,
                TimestampUtc = x.TimestampUtc
            }).ToList();
        }

        private async Task AddIntegrationLogAsync(SapIntegrationExecuteResultDto result)
        {
            var log = new SAP_INTEGRATION_LOGS
            {
                FormId = result.FormId,
                SubmissionId = result.SubmissionId,
                SapConfigId = result.SapConfigId,
                Endpoint = result.Endpoint ?? string.Empty,
                EventType = result.EventType ?? string.Empty,
                Status = result.Status ?? "Failed",
                RequestPayloadJson = result.RequestPayloadJson,
                ResponsePayloadJson = result.ResponsePayloadJson,
                ErrorMessage = result.ErrorMessage,
                TimestampUtc = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            _unitOfWork.AppDbContext.Set<SAP_INTEGRATION_LOGS>().Add(log);
            await _unitOfWork.CompleteAsyn();
        }

        private static SapIntegrationSettingsDto ToSettingsDto(SAP_INTEGRATION_SETTINGS x)
        {
            return new SapIntegrationSettingsDto
            {
                Id = x.Id,
                DocumentTypeId = x.DocumentTypeId,
                SapConfigId = x.SapConfigId,
                TargetEndpoint = x.TargetEndpoint,
                HttpMethod = string.IsNullOrWhiteSpace(x.HttpMethod) ? "POST" : x.HttpMethod,
                TargetObject = x.TargetObject,
                ExecutionMode = x.ExecutionMode,
                TriggerStageId = x.TriggerStageId,
                BlockWorkflowOnError = x.BlockWorkflowOnError,
                IsActive = x.IsActive,
                CreatedDate = x.CreatedDate,
                UpdatedDate = x.UpdatedDate
            };
        }

        private static void ValidateExecutionMode(string mode, int? triggerStageId)
        {
            var normalized = (mode ?? string.Empty).Trim();
            if (normalized != "OnSubmit" && normalized != "OnFinalApproval" && normalized != "OnSpecificWorkflowStage")
            {
                throw new InvalidOperationException("ExecutionMode must be one of: OnSubmit, OnFinalApproval, OnSpecificWorkflowStage.");
            }

            if (normalized == "OnSpecificWorkflowStage" && (!triggerStageId.HasValue || triggerStageId.Value <= 0))
            {
                throw new InvalidOperationException("TriggerStageId is required when ExecutionMode is OnSpecificWorkflowStage.");
            }
        }

        private static bool ShouldRunForEvent(SAP_INTEGRATION_SETTINGS settings, string eventType, int? stageId)
        {
            var e = (eventType ?? string.Empty).Trim();
            return settings.ExecutionMode switch
            {
                "OnSubmit" => string.Equals(e, "FormSubmitted", StringComparison.OrdinalIgnoreCase) ||
                              string.Equals(e, "OnSubmit", StringComparison.OrdinalIgnoreCase),
                "OnFinalApproval" => string.Equals(e, "ApprovalApproved", StringComparison.OrdinalIgnoreCase) ||
                                     string.Equals(e, "OnFinalApproval", StringComparison.OrdinalIgnoreCase),
                "OnSpecificWorkflowStage" => (string.Equals(e, "ApprovalRequired", StringComparison.OrdinalIgnoreCase) ||
                                              string.Equals(e, "OnSpecificWorkflowStage", StringComparison.OrdinalIgnoreCase)) &&
                                             settings.TriggerStageId.HasValue &&
                                             stageId.HasValue &&
                                             settings.TriggerStageId.Value == stageId.Value,
                _ => false
            };
        }

        private static object? GetSubmissionValue(FORM_SUBMISSION_VALUES value)
        {
            if (!string.IsNullOrWhiteSpace(value.ValueString))
                return value.ValueString;
            if (value.ValueNumber.HasValue)
                return value.ValueNumber.Value;
            if (value.ValueDate.HasValue)
                return value.ValueDate.Value;
            if (value.ValueBool.HasValue)
                return value.ValueBool.Value;
            if (!string.IsNullOrWhiteSpace(value.ValueJson))
                return value.ValueJson;
            return null;
        }

        private static void SetPayloadValue(Dictionary<string, object?> payload, string targetPath, object? value)
        {
            var path = (targetPath ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(path))
                return;

            // Flat mapping: CardCode
            if (!path.Contains('.'))
            {
                payload[path] = value;
                return;
            }

            // Nested mapping: user.name -> { "user": { "name": value } }
            var segments = path.Split('.', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => x.Length > 0)
                .ToArray();

            if (segments.Length == 0)
                return;
            if (segments.Length == 1)
            {
                payload[segments[0]] = value;
                return;
            }

            var current = payload;
            for (int i = 0; i < segments.Length - 1; i++)
            {
                var key = segments[i];
                if (!current.TryGetValue(key, out var existing) ||
                    existing is not Dictionary<string, object?> child)
                {
                    child = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
                    current[key] = child;
                }
                current = child;
            }

            current[segments[^1]] = value;
        }

        private static string NormalizeSapFieldPathForPayload(string? sapFieldName, string? targetObject, string? endpoint)
        {
            var path = (sapFieldName ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(path) || !path.Contains('.'))
                return path;

            var segments = path.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (segments.Length < 2)
                return path;

            var firstSegment = segments[0];

            // Backward compatibility: older mappings may save endpoint/table prefixes such as
            // "ProductionOrders.ItemCode" or "OWOR.ItemCode", but Service Layer expects "ItemCode".
            if (string.Equals(firstSegment, (targetObject ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase) ||
                string.Equals(firstSegment, ExtractEndpointLeaf(endpoint), StringComparison.OrdinalIgnoreCase) ||
                IsLikelySapTableName(firstSegment))
            {
                return string.Join(".", segments.Skip(1));
            }

            return path;
        }

        private static string ExtractEndpointLeaf(string? endpoint)
        {
            var raw = (endpoint ?? string.Empty).Trim().Trim('/');
            if (string.IsNullOrWhiteSpace(raw))
                return string.Empty;

            var slashIdx = raw.LastIndexOf('/');
            var leaf = slashIdx >= 0 ? raw[(slashIdx + 1)..] : raw;
            var queryIdx = leaf.IndexOf('?');
            if (queryIdx >= 0)
                leaf = leaf[..queryIdx];
            var parenIdx = leaf.IndexOf('(');
            if (parenIdx >= 0)
                leaf = leaf[..parenIdx];

            return leaf.Trim();
        }

        private static bool IsLikelySapTableName(string segment)
        {
            if (string.IsNullOrWhiteSpace(segment))
                return false;

            // Typical SAP Business One table names are uppercase short codes (e.g. OWOR, OINV, RDR1).
            return Regex.IsMatch(segment, @"^[A-Z][A-Z0-9_]{2,9}$");
        }

        private async Task<string> GetServiceLayerMetadataAsync(int sapConfigId)
        {
            // First try direct metadata fetch from BaseUrl without login.
            // Some SAP environments allow $metadata access even when Login has DB-specific issues.
            var rawConnectionString = await _sapConfigsService.GetConnectionStringByIdAsync(sapConfigId);
            if (!string.IsNullOrWhiteSpace(rawConnectionString))
            {
                var values = ParseConnectionString(rawConnectionString);
                var type = values.TryGetValue("Type", out var t) ? t : null;
                if (string.Equals(type, "ServiceLayer", StringComparison.OrdinalIgnoreCase))
                {
                    var baseUrl = values.TryGetValue("BaseUrl", out var url) ? NormalizeServiceLayerBaseUrl(url) : null;
                    var verifySsl = !values.TryGetValue("VerifySsl", out var verifySslValue) ||
                                    !bool.TryParse(verifySslValue, out var parsedVerifySsl) ||
                                    parsedVerifySsl;

                    if (!string.IsNullOrWhiteSpace(baseUrl))
                    {
                        var publicMetadata = await TryGetServiceLayerMetadataWithoutAuthAsync(baseUrl, verifySsl);
                        if (!string.IsNullOrWhiteSpace(publicMetadata))
                            return publicMetadata;
                    }
                }
            }

            // Fallback to authenticated metadata call.
            var connection = await GetServiceLayerConnectionAsync(sapConfigId);
            return await SendMetadataWithSslFallbackAsync(connection);
        }

        private async Task<(bool success, string? responseBody, string? errorMessage)> SendToServiceLayerAsync(
            int sapConfigId,
            string endpoint,
            string? httpMethod,
            Dictionary<string, object?> payload)
        {
            var connection = await GetServiceLayerConnectionAsync(sapConfigId);
            var client = CreateExternalClient(connection.VerifySsl);
            var url = $"{connection.BaseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}";
            var method = NormalizeHttpMethod(httpMethod);
            var netMethod = method switch
            {
                "GET" => HttpMethod.Get,
                "PUT" => HttpMethod.Put,
                _ => HttpMethod.Post
            };

            try
            {
                using var req = new HttpRequestMessage(netMethod, url);
                ApplyAuthHeaders(req, connection);

                if (netMethod == HttpMethod.Get)
                {
                    req.Headers.Accept.ParseAdd("application/json");
                }
                else
                {
                    req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                }

                var res = await client.SendAsync(req);
                var body = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                    return (true, body, null);

                if (ShouldRetryProductionOrderItemAlias(endpoint, (int)res.StatusCode, body) &&
                    TryBuildProductionOrderItemAliasPayload(payload, out var aliasPayload))
                {
                    using var aliasReq = new HttpRequestMessage(netMethod, url);
                    ApplyAuthHeaders(aliasReq, connection);
                    if (netMethod == HttpMethod.Get)
                    {
                        aliasReq.Headers.Accept.ParseAdd("application/json");
                    }
                    else
                    {
                        aliasReq.Content = new StringContent(JsonSerializer.Serialize(aliasPayload), Encoding.UTF8, "application/json");
                    }

                    var aliasRes = await client.SendAsync(aliasReq);
                    var aliasBody = await aliasRes.Content.ReadAsStringAsync();
                    if (aliasRes.IsSuccessStatusCode)
                        return (true, aliasBody, null);

                    return (false, aliasBody, BuildSapFailureMessage("SAP Service Layer request failed", (int)aliasRes.StatusCode, aliasBody));
                }

                return (false, body, BuildSapFailureMessage("SAP Service Layer request failed", (int)res.StatusCode, body));
            }
            catch (HttpRequestException ex) when (connection.VerifySsl && IsSslCertificateValidationError(ex))
            {
                _logger.LogWarning(ex, "SSL validation failed while calling SAP endpoint {Endpoint}. Retrying with SSL validation disabled.", endpoint);
                var insecureClient = CreateExternalClient(false);
                using var insecureReq = new HttpRequestMessage(netMethod, url);
                ApplyAuthHeaders(insecureReq, connection);

                if (netMethod == HttpMethod.Get)
                {
                    insecureReq.Headers.Accept.ParseAdd("application/json");
                }
                else
                {
                    insecureReq.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                }

                var insecureRes = await insecureClient.SendAsync(insecureReq);
                var insecureBody = await insecureRes.Content.ReadAsStringAsync();
                if (insecureRes.IsSuccessStatusCode)
                    return (true, insecureBody, null);

                return (false, insecureBody, BuildSapFailureMessage("SAP Service Layer request failed", (int)insecureRes.StatusCode, insecureBody));
            }
        }

        private static string NormalizeHttpMethod(string? method)
        {
            var normalized = (method ?? "POST").Trim().ToUpperInvariant();
            return normalized switch
            {
                "GET" => "GET",
                "PUT" => "PUT",
                _ => "POST"
            };
        }

        private static bool IsProductionOrdersRequest(string? targetEndpoint, string? targetObject)
        {
            var endpointLeaf = ExtractEndpointLeaf(targetEndpoint);
            if (string.Equals(endpointLeaf, "ProductionOrders", StringComparison.OrdinalIgnoreCase))
                return true;

            return string.Equals((targetObject ?? string.Empty).Trim(), "ProductionOrders", StringComparison.OrdinalIgnoreCase);
        }

        private static bool HasNonEmptyPayloadValue(Dictionary<string, object?> payload, string key)
        {
            if (!payload.TryGetValue(key, out var value) || value == null)
                return false;

            if (value is string s)
                return !string.IsNullOrWhiteSpace(s);

            if (value is JsonElement el)
            {
                return el.ValueKind switch
                {
                    JsonValueKind.Null => false,
                    JsonValueKind.Undefined => false,
                    JsonValueKind.String => !string.IsNullOrWhiteSpace(el.GetString()),
                    _ => true
                };
            }

            return true;
        }

        private static bool HasAnyNonEmptyPayloadValue(Dictionary<string, object?> payload, params string[] keys)
        {
            return keys.Any(k => HasNonEmptyPayloadValue(payload, k));
        }

        private static bool ShouldRetryProductionOrderItemAlias(string endpoint, int statusCode, string? responseBody)
        {
            if (statusCode != 400)
                return false;

            if (!string.Equals(ExtractEndpointLeaf(endpoint), "ProductionOrders", StringComparison.OrdinalIgnoreCase))
                return false;

            var body = responseBody ?? string.Empty;
            return body.Contains("Property 'ItemCode' of 'ProductionOrder' is invalid", StringComparison.OrdinalIgnoreCase);
        }

        private static bool TryBuildProductionOrderItemAliasPayload(
            Dictionary<string, object?> originalPayload,
            out Dictionary<string, object?> aliasPayload)
        {
            aliasPayload = new Dictionary<string, object?>(originalPayload, StringComparer.OrdinalIgnoreCase);

            if (!aliasPayload.TryGetValue("ItemCode", out var itemCodeValue))
                return false;

            aliasPayload.Remove("ItemCode");
            aliasPayload["ItemNo"] = itemCodeValue;
            return true;
        }

        private static string ResolveEndpointTemplate(
            string endpointTemplate,
            Dictionary<string, object?> payload,
            FORM_SUBMISSIONS submission)
        {
            var template = (endpointTemplate ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(template))
                return template;

            if (!template.Contains('{'))
                return template;

            var metadata = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase)
            {
                ["SubmissionId"] = submission.Id,
                ["FormId"] = submission.FormBuilderId,
                ["DocumentTypeId"] = submission.DocumentTypeId,
                ["DocumentNumber"] = submission.DocumentNumber,
                ["Status"] = submission.Status
            };

            var unresolved = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var resolved = Regex.Replace(template, @"\{([A-Za-z0-9_.\-]+)\}", match =>
            {
                var key = match.Groups[1].Value;
                if (string.IsNullOrWhiteSpace(key))
                    return match.Value;

                if (metadata.TryGetValue(key, out var metadataValue))
                    return ToEndpointTokenValue(metadataValue, key);

                if (TryResolveTokenFromPayload(payload, key, out var payloadValue))
                    return ToEndpointTokenValue(payloadValue, key);

                unresolved.Add(key);
                return match.Value;
            });

            if (unresolved.Count > 0)
            {
                var unresolvedList = string.Join(", ", unresolved.OrderBy(x => x));
                throw new InvalidOperationException(
                    $"SAP endpoint contains unresolved placeholders: {unresolvedList}. " +
                    "Map these fields or remove them from TargetEndpoint.");
            }

            return resolved;
        }

        private static bool TryResolveTokenFromPayload(
            Dictionary<string, object?> payload,
            string token,
            out object? value)
        {
            if (payload.TryGetValue(token, out value))
                return true;

            var segments = token.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (segments.Length < 2)
                return false;

            object? current = payload;
            foreach (var segment in segments)
            {
                if (current is Dictionary<string, object?> objDict)
                {
                    if (!objDict.TryGetValue(segment, out current))
                        return false;
                    continue;
                }

                if (current is JsonElement jsonEl && jsonEl.ValueKind == JsonValueKind.Object)
                {
                    if (!jsonEl.TryGetProperty(segment, out var child))
                        return false;
                    current = child;
                    continue;
                }

                return false;
            }

            value = current;
            return true;
        }

        private static string ToEndpointTokenValue(object? value, string tokenName)
        {
            if (value == null)
                throw new InvalidOperationException($"SAP endpoint token '{tokenName}' resolved to null.");

            string raw = value switch
            {
                string s => s.Trim(),
                DateTime dt => dt.ToString("O", CultureInfo.InvariantCulture),
                DateTimeOffset dto => dto.ToString("O", CultureInfo.InvariantCulture),
                bool b => b ? "true" : "false",
                JsonElement el => JsonElementToString(el),
                IFormattable f => f.ToString(null, CultureInfo.InvariantCulture),
                _ => Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty
            };

            if (string.IsNullOrWhiteSpace(raw))
                throw new InvalidOperationException($"SAP endpoint token '{tokenName}' resolved to empty value.");

            return Uri.EscapeDataString(raw);
        }

        private static string JsonElementToString(JsonElement el)
        {
            return el.ValueKind switch
            {
                JsonValueKind.String => el.GetString() ?? string.Empty,
                JsonValueKind.Number => el.ToString(),
                JsonValueKind.True => "true",
                JsonValueKind.False => "false",
                _ => el.GetRawText()
            };
        }

        private async Task<ServiceLayerConnectionContext> GetServiceLayerConnectionAsync(int sapConfigId)
        {
            var rawConnectionString = await _sapConfigsService.GetConnectionStringByIdAsync(sapConfigId);
            if (string.IsNullOrWhiteSpace(rawConnectionString))
                throw new InvalidOperationException($"SAP connection {sapConfigId} is not configured.");

            var values = ParseConnectionString(rawConnectionString);
            if (!values.TryGetValue("Type", out var type) || !string.Equals(type, "ServiceLayer", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException($"SAP connection {sapConfigId} is not a Service Layer connection.");

            var baseUrl = values.TryGetValue("BaseUrl", out var url) ? NormalizeServiceLayerBaseUrl(url) : null;
            var authMethod = values.TryGetValue("AuthMethod", out var method) ? method : "Session";
            var verifySsl = !values.TryGetValue("VerifySsl", out var verifySslValue) || !bool.TryParse(verifySslValue, out var parsedVerifySsl) || parsedVerifySsl;
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new InvalidOperationException("BaseUrl is missing for Service Layer connection.");

            if (string.Equals(authMethod, "Token", StringComparison.OrdinalIgnoreCase))
            {
                var token = values.TryGetValue("Token", out var tokenValue) ? tokenValue : null;
                if (string.IsNullOrWhiteSpace(token))
                    throw new InvalidOperationException("Token authentication selected but Token is missing.");

                return new ServiceLayerConnectionContext
                {
                    BaseUrl = baseUrl,
                    AuthMethod = "Token",
                    AccessToken = token,
                    VerifySsl = verifySsl
                };
            }

            var companyDb = values.TryGetValue("CompanyDB", out var db) ? db : null;
            var userName = values.TryGetValue("UserName", out var user) ? user : null;
            var password = values.TryGetValue("Password", out var pass) ? pass : null;
            if (string.IsNullOrWhiteSpace(companyDb) || string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
                throw new InvalidOperationException("Service Layer session auth requires CompanyDB, UserName and Password.");

            var loginResponse = await LoginToServiceLayerAsync(baseUrl, companyDb, userName, password, verifySsl);
            return new ServiceLayerConnectionContext
            {
                BaseUrl = baseUrl,
                AuthMethod = "Session",
                SessionCookie = loginResponse,
                VerifySsl = verifySsl
            };
        }

        private async Task<string> LoginToServiceLayerAsync(string baseUrl, string companyDb, string userName, string password, bool verifySsl)
        {
            var client = CreateExternalClient(verifySsl);
            var payload = new
            {
                CompanyDB = companyDb,
                UserName = userName,
                Password = password
            };

            using var req = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl.TrimEnd('/')}/Login");
            req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            HttpResponseMessage res;
            string body;
            try
            {
                res = await client.SendAsync(req);
                body = await res.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex) when (verifySsl && IsSslCertificateValidationError(ex))
            {
                _logger.LogWarning(ex, "SSL validation failed during SAP login for {BaseUrl}. Retrying with SSL validation disabled.", baseUrl);

                var insecureClient = CreateExternalClient(false);
                using var insecureReq = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl.TrimEnd('/')}/Login");
                insecureReq.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                res = await insecureClient.SendAsync(insecureReq);
                body = await res.Content.ReadAsStringAsync();
            }

            if (!res.IsSuccessStatusCode)
            {
                if ((int)res.StatusCode == 401)
                    throw new InvalidOperationException(BuildSapFailureMessage("SAP login unauthorized", 401, body));

                throw new InvalidOperationException(BuildSapFailureMessage("SAP login failed", (int)res.StatusCode, body));
            }

            var cookies = new List<string>();
            if (res.Headers.TryGetValues("Set-Cookie", out var setCookies))
            {
                foreach (var c in setCookies)
                {
                    var pair = c.Split(';', 2)[0]?.Trim();
                    if (!string.IsNullOrWhiteSpace(pair))
                        cookies.Add(pair);
                }
            }

            if (cookies.Count > 0)
                return string.Join("; ", cookies);

            using var doc = JsonDocument.Parse(body);
            if (!doc.RootElement.TryGetProperty("SessionId", out var sessionIdEl))
                throw new InvalidOperationException("SAP login response does not contain SessionId.");

            var sessionId = sessionIdEl.GetString();
            if (string.IsNullOrWhiteSpace(sessionId))
                throw new InvalidOperationException("SAP login returned empty SessionId.");

            return $"B1SESSION={sessionId}";
        }

        private async Task<string> SendMetadataWithSslFallbackAsync(ServiceLayerConnectionContext connection)
        {
            try
            {
                var client = CreateExternalClient(connection.VerifySsl);
                using var req = new HttpRequestMessage(HttpMethod.Get, $"{connection.BaseUrl.TrimEnd('/')}/$metadata");
                ApplyAuthHeaders(req, connection);

                var res = await client.SendAsync(req);
                var body = await res.Content.ReadAsStringAsync();
                if (!res.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException(BuildSapFailureMessage("SAP metadata call failed", (int)res.StatusCode, body));
                }
                return body;
            }
            catch (HttpRequestException ex) when (connection.VerifySsl && IsSslCertificateValidationError(ex))
            {
                _logger.LogWarning(ex, "SSL validation failed while calling SAP metadata for {BaseUrl}. Retrying with SSL validation disabled.", connection.BaseUrl);
                var insecureClient = CreateExternalClient(false);
                using var insecureReq = new HttpRequestMessage(HttpMethod.Get, $"{connection.BaseUrl.TrimEnd('/')}/$metadata");
                ApplyAuthHeaders(insecureReq, connection);
                var insecureRes = await insecureClient.SendAsync(insecureReq);
                var insecureBody = await insecureRes.Content.ReadAsStringAsync();
                if (!insecureRes.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException(BuildSapFailureMessage("SAP metadata call failed", (int)insecureRes.StatusCode, insecureBody));
                }

                return insecureBody;
            }
        }

        private async Task<string?> TryGetServiceLayerMetadataWithoutAuthAsync(string baseUrl, bool verifySsl)
        {
            try
            {
                var client = CreateExternalClient(verifySsl);
                using var req = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl.TrimEnd('/')}/$metadata");
                var res = await client.SendAsync(req);
                var body = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                    return body;
            }
            catch (HttpRequestException ex) when (verifySsl && IsSslCertificateValidationError(ex))
            {
                try
                {
                    var insecureClient = CreateExternalClient(false);
                    using var insecureReq = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl.TrimEnd('/')}/$metadata");
                    var insecureRes = await insecureClient.SendAsync(insecureReq);
                    var insecureBody = await insecureRes.Content.ReadAsStringAsync();
                    if (insecureRes.IsSuccessStatusCode)
                        return insecureBody;
                }
                catch
                {
                    // Ignore and fallback to authenticated path.
                }
            }
            catch
            {
                // Ignore and fallback to authenticated path.
            }

            return null;
        }

        private static bool IsSslCertificateValidationError(Exception ex)
        {
            for (var current = ex; current != null; current = current.InnerException)
            {
                var message = current.Message ?? string.Empty;
                if (message.Contains("RemoteCertificate", StringComparison.OrdinalIgnoreCase) ||
                    message.Contains("SSL connection could not be established", StringComparison.OrdinalIgnoreCase) ||
                    message.Contains("Could not establish trust relationship", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static string BuildSapFailureMessage(string prefix, int statusCode, string? responseBody)
        {
            var sapError = ExtractSapErrorMessage(responseBody);
            var compactBody = string.IsNullOrWhiteSpace(responseBody)
                ? string.Empty
                : responseBody.Length > 500 ? responseBody[..500] + "..." : responseBody;

            if (!string.IsNullOrWhiteSpace(sapError))
                return $"{prefix}: HTTP {statusCode}. SAP Error: {sapError}";

            return string.IsNullOrWhiteSpace(compactBody)
                ? $"{prefix}: HTTP {statusCode}."
                : $"{prefix}: HTTP {statusCode}. Response: {compactBody}";
        }

        private static string? ExtractSapErrorMessage(string? responseBody)
        {
            if (string.IsNullOrWhiteSpace(responseBody))
                return null;

            try
            {
                using var doc = JsonDocument.Parse(responseBody);
                var root = doc.RootElement;
                if (root.TryGetProperty("error", out var errorEl) &&
                    errorEl.ValueKind == JsonValueKind.Object &&
                    errorEl.TryGetProperty("message", out var messageEl) &&
                    messageEl.ValueKind == JsonValueKind.Object &&
                    messageEl.TryGetProperty("value", out var valueEl) &&
                    valueEl.ValueKind == JsonValueKind.String)
                {
                    return valueEl.GetString();
                }
            }
            catch
            {
                // Keep fallback message if response is not JSON.
            }

            return null;
        }

        private HttpClient CreateExternalClient(bool verifySsl)
        {
            if (verifySsl)
                return _httpClientFactory.CreateClient("ExternalApi");

            var handler = new HttpClientHandler
            {
                AutomaticDecompression = System.Net.DecompressionMethods.All,
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            return new HttpClient(handler, disposeHandler: true);
        }

        private static void ApplyAuthHeaders(HttpRequestMessage req, ServiceLayerConnectionContext connection)
        {
            if (connection.AuthMethod == "Token")
            {
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", connection.AccessToken);
                return;
            }

            if (!string.IsNullOrWhiteSpace(connection.SessionCookie))
                req.Headers.Add("Cookie", connection.SessionCookie);
        }

        private static Dictionary<string, string> ParseConnectionString(string connectionString)
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var parts = connectionString.Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach (var p in parts)
            {
                var kv = p.Split('=', 2);
                if (kv.Length == 2)
                {
                    dict[kv[0].Trim()] = kv[1].Trim();
                }
            }
            return dict;
        }

        private static string NormalizeServiceLayerBaseUrl(string? baseUrl)
        {
            var normalized = (baseUrl ?? string.Empty).Trim().TrimEnd('/');
            if (string.IsNullOrWhiteSpace(normalized))
                return string.Empty;

            var idx = normalized.IndexOf(ServiceLayerApiPath, StringComparison.OrdinalIgnoreCase);
            if (idx >= 0)
                return normalized.Substring(0, idx + ServiceLayerApiPath.Length).TrimEnd('/');

            return normalized + ServiceLayerApiPath;
        }

        private sealed class ServiceLayerConnectionContext
        {
            public string BaseUrl { get; set; } = string.Empty;
            public string AuthMethod { get; set; } = "Session";
            public string? AccessToken { get; set; }
            public string? SessionCookie { get; set; }
            public bool VerifySsl { get; set; } = true;
        }
    }
}
