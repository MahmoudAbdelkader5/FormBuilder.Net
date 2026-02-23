using FormBuilder.API.DTOs;
using FormBuilder.API.Models;
using FormBuilder.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Administration")]

    public class GridColumnDataSourcesController : ControllerBase
    {
        private readonly IGridColumnDataSourcesService _gridColumnDataSourcesService;

        public GridColumnDataSourcesController(IGridColumnDataSourcesService gridColumnDataSourcesService)
        {
            _gridColumnDataSourcesService = gridColumnDataSourcesService;
        }

        /// <summary>
        /// Get all grid column data sources
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAll()
        {
            try
            {
                var response = await _gridColumnDataSourcesService.GetAllAsync();
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving grid column data sources: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get grid column data source by ID
        /// </summary>
        /// <param name="id">Data source ID</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetById(int id)
        {
            try
            {
                var response = await _gridColumnDataSourcesService.GetByIdAsync(id);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving grid column data source: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get grid column data sources by column ID
        /// </summary>
        /// <param name="columnId">Column ID</param>
        [HttpGet("column/{columnId}")]
        public async Task<ActionResult<ApiResponse>> GetByColumnId(int columnId)
        {
            try
            {
                var response = await _gridColumnDataSourcesService.GetByColumnIdAsync(columnId);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving grid column data sources: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get active grid column data sources by column ID
        /// </summary>
        /// <param name="columnId">Column ID</param>
        [HttpGet("column/{columnId}/active")]
        public async Task<ActionResult<ApiResponse>> GetActiveByColumnId(int columnId)
        {
            try
            {
                var response = await _gridColumnDataSourcesService.GetActiveByColumnIdAsync(columnId);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving active grid column data sources: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get grid column data source by column ID and source type
        /// </summary>
        /// <param name="columnId">Column ID</param>
        /// <param name="sourceType">Source type (Static, LookupTable, API)</param>
        [HttpGet("column/{columnId}/type/{sourceType}")]
        public async Task<ActionResult<ApiResponse>> GetByColumnIdAndType(int columnId, string sourceType)
        {
            try
            {
                var response = await _gridColumnDataSourcesService.GetByColumnIdAndTypeAsync(columnId, sourceType);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving grid column data source: {ex.Message}"));
            }
        }

        /// <summary>
        /// Create new grid column data source
        /// </summary>
        /// <param name="createDto">Data source data</param>
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Create([FromBody] CreateGridColumnDataSourceDto createDto)
        {
            try
            {
                var response = await _gridColumnDataSourcesService.CreateAsync(createDto);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error creating grid column data source: {ex.Message}"));
            }
        }

        /// <summary>
        /// Update grid column data source
        /// </summary>
        /// <param name="id">Data source ID</param>
        /// <param name="updateDto">Updated data source data</param>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] UpdateGridColumnDataSourceDto updateDto)
        {
            try
            {
                var response = await _gridColumnDataSourcesService.UpdateAsync(id, updateDto);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error updating grid column data source: {ex.Message}"));
            }
        }

        /// <summary>
        /// Delete grid column data source
        /// </summary>
        /// <param name="id">Data source ID</param>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            try
            {
                var response = await _gridColumnDataSourcesService.DeleteAsync(id);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error deleting grid column data source: {ex.Message}"));
            }
        }

        /// <summary>
        /// Toggle active status of grid column data source
        /// </summary>
        /// <param name="id">Data source ID</param>
        /// <param name="isActive">Active status</param>
        [HttpPatch("{id}/toggle-active")]
        public async Task<ActionResult<ApiResponse>> ToggleActive(int id, [FromQuery] bool isActive)
        {
            try
            {
                var response = await _gridColumnDataSourcesService.ToggleActiveAsync(id, isActive);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error toggling grid column data source status: {ex.Message}"));
            }
        }

        /// <summary>
        /// Check if data source exists
        /// </summary>
        /// <param name="id">Data source ID</param>
        [HttpGet("{id}/exists")]
        public async Task<ActionResult<ApiResponse>> Exists(int id)
        {
            try
            {
                var response = await _gridColumnDataSourcesService.ExistsAsync(id);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error checking data source existence: {ex.Message}"));
            }
        }

        /// <summary>
        /// Check if column has data sources
        /// </summary>
        /// <param name="columnId">Column ID</param>
        [HttpGet("column/{columnId}/has-sources")]
        public async Task<ActionResult<ApiResponse>> ColumnHasDataSources(int columnId)
        {
            try
            {
                var response = await _gridColumnDataSourcesService.ColumnHasDataSourcesAsync(columnId);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error checking column data sources: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get data sources count for a column
        /// </summary>
        /// <param name="columnId">Column ID</param>
        [HttpGet("column/{columnId}/count")]
        public async Task<ActionResult<ApiResponse>> GetDataSourcesCount(int columnId)
        {
            try
            {
                var response = await _gridColumnDataSourcesService.GetDataSourcesCountAsync(columnId);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error getting data sources count: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get column options (from data source) - Route version
        /// </summary>
        /// <param name="columnId">Column ID</param>
        [HttpGet("column/{columnId}/options")]
        public async Task<ActionResult<ApiResponse>> GetColumnOptions(int columnId)
        {
            try
            {
                var response = await _gridColumnDataSourcesService.GetColumnOptionsAsync(columnId);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving column options: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get column options (from data source) - Query version (similar to field-options)
        /// </summary>
        /// <param name="columnId">Column ID</param>
        /// <param name="context">Optional context as JSON string</param>
        [HttpGet("column-options")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous] // Allow anonymous for public forms
        public async Task<ActionResult<ApiResponse>> GetColumnOptionsQuery(
            [FromQuery] int columnId,
            [FromQuery] string? context = null)
        {
            try
            {
                Dictionary<string, object>? contextDict = null;
                if (!string.IsNullOrEmpty(context))
                {
                    contextDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(context);
                }

                var result = await _gridColumnDataSourcesService.GetColumnOptionsAsync(columnId, contextDict);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving column options: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get column options (from data source) - POST version (similar to field-options)
        /// </summary>
        /// <param name="request">Request DTO with columnId, context, and requestBodyJson</param>
        [HttpPost("column-options")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous] // Allow anonymous for public forms
        public async Task<ActionResult<ApiResponse>> GetColumnOptionsPost(
            [FromBody] FormBuilder.Core.DTOS.FormBuilder.GetColumnOptionsRequestDto request)
        {
            try
            {
                var result = await _gridColumnDataSourcesService.GetColumnOptionsAsync(
                    request.ColumnId, 
                    request.Context, 
                    request.RequestBodyJson);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving column options: {ex.Message}"));
            }
        }
    }
}

