using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.DTOS.FormBuilder
{
    public class SapIntegrationSettingsDto
    {
        public int Id { get; set; }
        public int DocumentTypeId { get; set; }
        public int SapConfigId { get; set; }
        public string TargetEndpoint { get; set; } = string.Empty;
        public string HttpMethod { get; set; } = "POST";
        public string? TargetObject { get; set; }
        public string ExecutionMode { get; set; } = "OnSubmit";
        public int? TriggerStageId { get; set; }
        public bool BlockWorkflowOnError { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class UpsertSapIntegrationSettingsDto
    {
        [Required]
        public int DocumentTypeId { get; set; }

        [Required]
        public int SapConfigId { get; set; }

        [Required, StringLength(200)]
        public string TargetEndpoint { get; set; } = string.Empty;

        [Required, StringLength(10)]
        public string HttpMethod { get; set; } = "POST"; // GET | POST | PUT

        [StringLength(200)]
        public string? TargetObject { get; set; }

        [Required, StringLength(50)]
        public string ExecutionMode { get; set; } = "OnSubmit"; // OnSubmit | OnFinalApproval | OnSpecificWorkflowStage

        public int? TriggerStageId { get; set; }
        public bool BlockWorkflowOnError { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class SapFieldMappingDto
    {
        public int Id { get; set; }
        public int FormFieldId { get; set; }
        public string FieldCode { get; set; } = string.Empty;
        public string FieldName { get; set; } = string.Empty;
        public string SapFieldName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    public class SaveSapFieldMappingsDto
    {
        [Required]
        public int FormBuilderId { get; set; }

        [Required]
        public List<SaveSapFieldMappingItemDto> Mappings { get; set; } = new();
    }

    public class SaveSapFieldMappingItemDto
    {
        [Required]
        public int FormFieldId { get; set; }

        [Required, StringLength(200)]
        public string SapFieldName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }

    public class SapServiceLayerEndpointDto
    {
        public string Name { get; set; } = string.Empty;
        public string? EntityType { get; set; }
    }

    public class SapServiceLayerObjectFieldDto
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool Nullable { get; set; }
    }

    public class SapIntegrationExecuteResultDto
    {
        public bool Success { get; set; }
        public int FormId { get; set; }
        public int SubmissionId { get; set; }
        public int SapConfigId { get; set; }
        public string Endpoint { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public string Status { get; set; } = "Skipped";
        public string? ErrorMessage { get; set; }
        public string? RequestPayloadJson { get; set; }
        public string? ResponsePayloadJson { get; set; }
        public bool ShouldBlockWorkflow { get; set; }
    }

    public class SapIntegrationLogDto
    {
        public int Id { get; set; }
        public int FormId { get; set; }
        public int SubmissionId { get; set; }
        public int SapConfigId { get; set; }
        public string Endpoint { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? RequestPayloadJson { get; set; }
        public string? ResponsePayloadJson { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime TimestampUtc { get; set; }
    }
}
