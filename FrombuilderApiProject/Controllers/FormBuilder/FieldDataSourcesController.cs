using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Core.IServices.FormBuilder;
using FormBuilder.API.Models;
using FormBuilder.Core.DTOS.FormBuilder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]


    public class FieldDataSourcesController : ControllerBase
    {
        private readonly IFieldDataSourcesService _fieldDataSourcesService;

        public FieldDataSourcesController(IFieldDataSourcesService fieldDataSourcesService)
        {
            _fieldDataSourcesService = fieldDataSourcesService;
        }

        // ================================
        // GET ALL FIELD DATA SOURCES
        // ================================
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllFieldDataSources()
        {
            try
            {
                // You'll need to add this method to your IFieldDataSourcesService interface
                var result = await _fieldDataSourcesService.GetAllAsync();
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving all field data sources: {ex.Message}"));
            }
        }

        // ================================
        // GET ALL DATA SOURCES BY FIELD ID
        // ================================
        [HttpGet("field/{fieldId}")]
        public async Task<ActionResult<ApiResponse>> GetByFieldId(int fieldId)
        {
            try
            {
                var result = await _fieldDataSourcesService.GetByFieldIdAsync(fieldId);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving field data sources: {ex.Message}"));
            }
        }

        // ================================
        // GET ACTIVE DATA SOURCES BY FIELD ID
        // ================================
        [HttpGet("field/{fieldId}/active")]
        public async Task<ActionResult<ApiResponse>> GetActiveByFieldId(int fieldId)
        {
            try
            {
                var result = await _fieldDataSourcesService.GetActiveByFieldIdAsync(fieldId);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving active field data sources: {ex.Message}"));
            }
        }

        // ================================
        // GET DATA SOURCE BY ID
        // ================================
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetById(int id)
        {
            try
            {
                var result = await _fieldDataSourcesService.GetByIdAsync(id);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving field data source: {ex.Message}"));
            }
        }

        // ================================
        // CREATE FIELD DATA SOURCE
        // ================================
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateFieldDataSource([FromBody] CreateFieldDataSourceDto createFieldDataSourceDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse(400, "Invalid field data source data", ModelState));
                }

                var result = await _fieldDataSourcesService.CreateAsync(createFieldDataSourceDto);

                if (result.StatusCode == 200)
                {
                    return CreatedAtAction(nameof(GetById), new { id = result.Data?.GetType().GetProperty("Id")?.GetValue(result.Data) }, result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error creating field data source: {ex.Message}"));
            }
        }

        // ================================
        // CREATE BULK FIELD DATA SOURCES
        // ================================
        [HttpPost("bulk")]
        public async Task<ActionResult<ApiResponse>> CreateBulkFieldDataSources([FromBody] List<CreateFieldDataSourceDto> createFieldDataSourceDtos)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse(400, "Invalid field data sources data", ModelState));
                }

                var result = await _fieldDataSourcesService.CreateBulkAsync(createFieldDataSourceDtos);

                if (result.StatusCode == 200)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error creating field data sources: {ex.Message}"));
            }
        }

        // ================================
        // UPDATE FIELD DATA SOURCE
        // ================================
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateFieldDataSource(int id, [FromBody] UpdateFieldDataSourceDto updateFieldDataSourceDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse(400, "Invalid field data source data", ModelState));
                }

                var result = await _fieldDataSourcesService.UpdateAsync(id, updateFieldDataSourceDto);

                if (result.StatusCode == 200)
                {
                    return Ok(result);
                }
                else if (result.StatusCode == 404)
                {
                    return NotFound(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error updating field data source: {ex.Message}"));
            }
        }

        // ================================
        // DELETE FIELD DATA SOURCE (HARD DELETE)
        // ================================
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteFieldDataSource(int id)
        {
            try
            {
                var result = await _fieldDataSourcesService.DeleteAsync(id);

                if (result.StatusCode == 200)
                {
                    return Ok(result);
                }
                else if (result.StatusCode == 404)
                {
                    return NotFound(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error deleting field data source: {ex.Message}"));
            }
        }

        // ================================
        // SOFT DELETE FIELD DATA SOURCE
        // ================================
        [HttpDelete("soft-delete/{id}")]
        public async Task<ActionResult<ApiResponse>> SoftDeleteFieldDataSource(int id)
        {
            try
            {
                var result = await _fieldDataSourcesService.SoftDeleteAsync(id);

                if (result.StatusCode == 200)
                {
                    return Ok(result);
                }
                else if (result.StatusCode == 404)
                {
                    return NotFound(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error soft deleting field data source: {ex.Message}"));
            }
        }

        // ================================
        // GET DATA SOURCE BY FIELD ID AND TYPE
        // ================================
        [HttpGet("field/{fieldId}/type/{sourceType}")]
        public async Task<ActionResult<ApiResponse>> GetByFieldIdAndType(int fieldId, string sourceType)
        {
            try
            {
                var result = await _fieldDataSourcesService.GetByFieldIdAndTypeAsync(fieldId, sourceType);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving field data source by type: {ex.Message}"));
            }
        }

        // ================================
        // GET DATA SOURCES COUNT FOR FIELD
        // ================================
        [HttpGet("field/{fieldId}/count")]
        public async Task<ActionResult<ApiResponse>> GetDataSourcesCount(int fieldId)
        {
            try
            {
                var result = await _fieldDataSourcesService.GetDataSourcesCountAsync(fieldId);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving data sources count: {ex.Message}"));
            }
        }

        // ================================
        // GET FIELD OPTIONS (For Frontend)
        // ================================
        [HttpGet("field-options")]
        [AllowAnonymous] // Allow anonymous for public forms
        public async Task<ActionResult<ApiResponse>> GetFieldOptions(
            [FromQuery] int fieldId,
            [FromQuery] string? context = null)
        {
            try
            {
                Dictionary<string, object>? contextDict = null;
                if (!string.IsNullOrEmpty(context))
                {
                    contextDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(context);
                }

                var result = await _fieldDataSourcesService.GetFieldOptionsAsync(fieldId, contextDict);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving field options: {ex.Message}"));
            }
        }

        [HttpPost("field-options")]
        [AllowAnonymous] // Allow anonymous for public forms
        public async Task<ActionResult<ApiResponse>> GetFieldOptionsPost(
            [FromBody] FormBuilder.Core.DTOS.FormBuilder.GetFieldOptionsRequestDto request)
        {
            try
            {
                var result = await _fieldDataSourcesService.GetFieldOptionsAsync(
                    request.FieldId, 
                    request.Context, 
                    request.RequestBodyJson);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving field options: {ex.Message}"));
            }
        }

        // ================================
        // PREVIEW DATA SOURCE (For Admin)
        // ================================
        [HttpPost("preview")]
        public async Task<ActionResult<ApiResponse>> PreviewDataSource(
            [FromBody] FormBuilder.Core.DTOS.FormBuilder.PreviewDataSourceRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse(400, "Invalid request data", ModelState));
                }

                var result = await _fieldDataSourcesService.PreviewDataSourceAsync(request);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error previewing data source: {ex.Message}"));
            }
        }

        // ================================
        // INSPECT API STRUCTURE (Get Available Fields)
        // ================================
        [HttpPost("inspect-api")]
        public async Task<ActionResult<ApiResponse>> InspectApi(
            [FromBody] FormBuilder.Core.DTOS.FormBuilder.InspectApiRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse(400, "Invalid request data", ModelState));
                }

                var result = await _fieldDataSourcesService.InspectApiAsync(request);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error inspecting API: {ex.Message}"));
            }
        }

        // ================================
        // GET AVAILABLE LOOKUP TABLES
        // ================================
        [HttpGet("lookup-tables")]
        public async Task<ActionResult<ApiResponse>> GetAvailableLookupTables([FromQuery] string? database = null)
        {
            try
            {
                var result = await _fieldDataSourcesService.GetAvailableLookupTablesAsync(database);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving available lookup tables: {ex.Message}"));
            }
        }

        // ================================
        // GET LOOKUP TABLE COLUMNS
        // ================================
        [HttpGet("lookup-tables/{tableName}/columns")]
        public async Task<ActionResult<ApiResponse>> GetLookupTableColumns(string tableName, [FromQuery] string? database = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tableName))
                {
                    return BadRequest(new ApiResponse(400, "Table name is required"));
                }

                var result = await _fieldDataSourcesService.GetLookupTableColumnsAsync(tableName, database);
                
                if (result.StatusCode == 404)
                {
                    return NotFound(result);
                }

                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving table columns: {ex.Message}"));
            }
        }
    }
}