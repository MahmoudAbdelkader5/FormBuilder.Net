using FormBuilder.API.DTOs;
using FormBuilder.API.Models;
using FormBuilder.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class GridColumnOptionsController : ControllerBase
    {
        private readonly IGridColumnOptionsService _gridColumnOptionsService;

        public GridColumnOptionsController(IGridColumnOptionsService gridColumnOptionsService)
        {
            _gridColumnOptionsService = gridColumnOptionsService;
        }

        /// <summary>
        /// Get all grid column options
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAll()
        {
            try
            {
                var response = await _gridColumnOptionsService.GetAllAsync();
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving grid column options: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get grid column option by ID
        /// </summary>
        /// <param name="id">Option ID</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetById(int id)
        {
            try
            {
                var response = await _gridColumnOptionsService.GetByIdAsync(id);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving grid column option: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get grid column options by column ID
        /// </summary>
        /// <param name="columnId">Column ID</param>
        [HttpGet("column/{columnId}")]
        public async Task<ActionResult<ApiResponse>> GetByColumnId(int columnId)
        {
            try
            {
                var response = await _gridColumnOptionsService.GetByColumnIdAsync(columnId);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving grid column options: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get active grid column options by column ID
        /// </summary>
        /// <param name="columnId">Column ID</param>
        [HttpGet("column/{columnId}/active")]
        public async Task<ActionResult<ApiResponse>> GetActiveByColumnId(int columnId)
        {
            try
            {
                var response = await _gridColumnOptionsService.GetActiveByColumnIdAsync(columnId);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving active grid column options: {ex.Message}"));
            }
        }

        /// <summary>
        /// Create new grid column option
        /// </summary>
        /// <param name="createDto">Option data</param>
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Create([FromBody] CreateGridColumnOptionDto createDto)
        {
            try
            {
                var response = await _gridColumnOptionsService.CreateAsync(createDto);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error creating grid column option: {ex.Message}"));
            }
        }

        /// <summary>
        /// Create multiple grid column options
        /// </summary>
        /// <param name="createDtos">List of option data</param>
        [HttpPost("bulk")]
        public async Task<ActionResult<ApiResponse>> CreateBulk([FromBody] List<CreateGridColumnOptionDto> createDtos)
        {
            try
            {
                var response = await _gridColumnOptionsService.CreateBulkAsync(createDtos);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error creating grid column options: {ex.Message}"));
            }
        }

        /// <summary>
        /// Update grid column option
        /// </summary>
        /// <param name="id">Option ID</param>
        /// <param name="updateDto">Updated option data</param>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] UpdateGridColumnOptionDto updateDto)
        {
            try
            {
                var response = await _gridColumnOptionsService.UpdateAsync(id, updateDto);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error updating grid column option: {ex.Message}"));
            }
        }

        /// <summary>
        /// Delete grid column option
        /// </summary>
        /// <param name="id">Option ID</param>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            try
            {
                var response = await _gridColumnOptionsService.DeleteAsync(id);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error deleting grid column option: {ex.Message}"));
            }
        }

        /// <summary>
        /// Soft delete grid column option
        /// </summary>
        /// <param name="id">Option ID</param>
        [HttpDelete("{id}/soft")]
        public async Task<ActionResult<ApiResponse>> SoftDelete(int id)
        {
            try
            {
                var response = await _gridColumnOptionsService.SoftDeleteAsync(id);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error soft deleting grid column option: {ex.Message}"));
            }
        }

        /// <summary>
        /// Toggle active status of grid column option
        /// </summary>
        /// <param name="id">Option ID</param>
        /// <param name="isActive">Active status</param>
        [HttpPatch("{id}/toggle-active")]
        public async Task<ActionResult<ApiResponse>> ToggleActive(int id, [FromQuery] bool isActive)
        {
            try
            {
                var response = await _gridColumnOptionsService.ToggleActiveAsync(id, isActive);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error toggling grid column option status: {ex.Message}"));
            }
        }

        /// <summary>
        /// Check if option exists
        /// </summary>
        /// <param name="id">Option ID</param>
        [HttpGet("{id}/exists")]
        public async Task<ActionResult<ApiResponse>> Exists(int id)
        {
            try
            {
                var response = await _gridColumnOptionsService.ExistsAsync(id);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error checking option existence: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get default option for a column
        /// </summary>
        /// <param name="columnId">Column ID</param>
        [HttpGet("column/{columnId}/default")]
        public async Task<ActionResult<ApiResponse>> GetDefaultOption(int columnId)
        {
            try
            {
                var response = await _gridColumnOptionsService.GetDefaultOptionAsync(columnId);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving default option: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get options count for a column
        /// </summary>
        /// <param name="columnId">Column ID</param>
        [HttpGet("column/{columnId}/count")]
        public async Task<ActionResult<ApiResponse>> GetOptionsCount(int columnId)
        {
            try
            {
                var response = await _gridColumnOptionsService.GetOptionsCountAsync(columnId);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error getting options count: {ex.Message}"));
            }
        }

        /// <summary>
        /// Check if column has options
        /// </summary>
        /// <param name="columnId">Column ID</param>
        [HttpGet("column/{columnId}/has-options")]
        public async Task<ActionResult<ApiResponse>> ColumnHasOptions(int columnId)
        {
            try
            {
                var response = await _gridColumnOptionsService.ColumnHasOptionsAsync(columnId);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error checking column options: {ex.Message}"));
            }
        }
    }
}

