using FormBuilder.API.Attributes;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Core.IServices.FormBuilder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SapIntegrationController : ControllerBase
    {
        private readonly ISapDynamicIntegrationService _sapIntegrationService;

        public SapIntegrationController(ISapDynamicIntegrationService sapIntegrationService)
        {
            _sapIntegrationService = sapIntegrationService;
        }

        [HttpGet("settings/{documentTypeId:int}")]
        [RequirePermission("SapHanaConfig_Allow_View")]
        public async Task<ActionResult<SapIntegrationSettingsDto>> GetSettings(int documentTypeId)
        {
            var data = await _sapIntegrationService.GetSettingsByDocumentTypeAsync(documentTypeId);
            if (data == null)
                return NotFound(new { message = "SAP integration settings not found for this document type." });
            return Ok(data);
        }

        [HttpPut("settings")]
        [RequirePermission("SapHanaConfig_Allow_Edit")]
        public async Task<ActionResult<SapIntegrationSettingsDto>> UpsertSettings([FromBody] UpsertSapIntegrationSettingsDto dto)
        {
            var data = await _sapIntegrationService.UpsertSettingsAsync(dto);
            return Ok(data);
        }

        [HttpGet("field-mappings/{formBuilderId:int}")]
        [RequirePermission("SapHanaConfig_Allow_View")]
        public async Task<ActionResult<List<SapFieldMappingDto>>> GetFieldMappings(int formBuilderId)
        {
            var data = await _sapIntegrationService.GetFieldMappingsByFormBuilderIdAsync(formBuilderId);
            return Ok(data);
        }

        [HttpPut("field-mappings")]
        [RequirePermission("SapHanaConfig_Allow_Edit")]
        public async Task<ActionResult<List<SapFieldMappingDto>>> SaveFieldMappings([FromBody] SaveSapFieldMappingsDto dto)
        {
            var data = await _sapIntegrationService.SaveFieldMappingsAsync(dto);
            return Ok(data);
        }

        [HttpGet("connections/{sapConfigId:int}/endpoints")]
        [RequirePermission("SapHanaConfig_Allow_View")]
        public async Task<ActionResult<List<SapServiceLayerEndpointDto>>> GetEndpoints(int sapConfigId)
        {
            try
            {
                var data = await _sapIntegrationService.GetServiceLayerEndpointsAsync(sapConfigId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("connections/{sapConfigId:int}/endpoints/{endpointName}/fields")]
        [RequirePermission("SapHanaConfig_Allow_View")]
        public async Task<ActionResult<List<SapServiceLayerObjectFieldDto>>> GetObjectFields(int sapConfigId, string endpointName)
        {
            try
            {
                var data = await _sapIntegrationService.GetServiceLayerObjectFieldsAsync(sapConfigId, endpointName);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("connections/{sapConfigId:int}/fields")]
        [RequirePermission("SapHanaConfig_Allow_View")]
        public async Task<ActionResult<List<SapServiceLayerObjectFieldDto>>> GetAllObjectFields(int sapConfigId)
        {
            try
            {
                var data = await _sapIntegrationService.GetAllServiceLayerObjectFieldsAsync(sapConfigId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("connections/{sapConfigId:int}/relogin")]
        [RequirePermission("SapHanaConfig_Allow_Manage")]
        public async Task<IActionResult> ReLogin(int sapConfigId)
        {
            try
            {
                await _sapIntegrationService.ReLoginServiceLayerAsync(sapConfigId);
                return Ok(new { success = true, message = "SAP re-login successful." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("execute/submissions/{submissionId:int}")]
        [RequirePermission("SapHanaConfig_Allow_Manage")]
        public async Task<ActionResult<SapIntegrationExecuteResultDto>> Execute(
            int submissionId,
            [FromQuery] string eventType = "OnSubmit",
            [FromQuery] int? stageId = null)
        {
            var data = await _sapIntegrationService.ExecuteForSubmissionAsync(submissionId, eventType, stageId);
            return Ok(data);
        }

        [HttpGet("logs")]
        [RequirePermission("SapHanaConfig_Allow_View")]
        public async Task<ActionResult<List<SapIntegrationLogDto>>> GetLogs(
            [FromQuery] int? formId = null,
            [FromQuery] int? submissionId = null,
            [FromQuery] int? sapConfigId = null,
            [FromQuery] int take = 100)
        {
            var data = await _sapIntegrationService.GetLogsAsync(formId, submissionId, sapConfigId, take);
            return Ok(data);
        }
    }
}
