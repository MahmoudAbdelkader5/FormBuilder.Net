using FormBuilder.Core.DTOS.FormBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Core.IServices.FormBuilder
{
    public interface ISapDynamicIntegrationService
    {
        Task<SapIntegrationSettingsDto?> GetSettingsByDocumentTypeAsync(int documentTypeId);
        Task<SapIntegrationSettingsDto> UpsertSettingsAsync(UpsertSapIntegrationSettingsDto dto);

        Task<List<SapFieldMappingDto>> GetFieldMappingsByFormBuilderIdAsync(int formBuilderId);
        Task<List<SapFieldMappingDto>> SaveFieldMappingsAsync(SaveSapFieldMappingsDto dto);

        Task<List<SapServiceLayerEndpointDto>> GetServiceLayerEndpointsAsync(int sapConfigId);
        Task<List<SapServiceLayerObjectFieldDto>> GetServiceLayerObjectFieldsAsync(int sapConfigId, string endpointName);
        Task<List<SapServiceLayerObjectFieldDto>> GetAllServiceLayerObjectFieldsAsync(int sapConfigId);
        Task<bool> ReLoginServiceLayerAsync(int sapConfigId);

        Task<SapIntegrationExecuteResultDto> ExecuteForSubmissionAsync(int submissionId, string eventType, int? stageId = null);

        Task<List<SapIntegrationLogDto>> GetLogsAsync(int? formId = null, int? submissionId = null, int? sapConfigId = null, int take = 100);
    }
}
