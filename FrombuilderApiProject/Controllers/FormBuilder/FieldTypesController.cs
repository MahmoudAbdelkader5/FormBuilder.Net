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

    public class FieldTypesController : ControllerBase
    {
        private readonly IFieldTypesService _fieldTypesService;

        public FieldTypesController(IFieldTypesService fieldTypesService)
        {
            _fieldTypesService = fieldTypesService;
        }

        /// <summary>
        /// Get all field types
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAll()
        {
            try
            {
                var result = await _fieldTypesService.GetAllAsync();
                if (result.Success)
                {
                    return Ok(new ApiResponse(200, "Field types retrieved successfully", result.Data));
                }
                return StatusCode(result.StatusCode, new ApiResponse(result.StatusCode, result.ErrorMessage ?? "Error retrieving field types"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving field types: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get field type by ID
        /// </summary>
        /// <param name="id">Field type ID</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetById(int id)
        {
            try
            {
                var result = await _fieldTypesService.GetByIdAsync(id);
                if (result.Success)
                {
                    return Ok(new ApiResponse(200, "Field type retrieved successfully", result.Data));
                }
                return StatusCode(result.StatusCode, new ApiResponse(result.StatusCode, result.ErrorMessage ?? "Field type not found"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving field type: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get field type by type name
        /// </summary>
        /// <param name="typeName">Type name</param>
        [HttpGet("name/{typeName}")]
        public async Task<ActionResult<ApiResponse>> GetByTypeName(string typeName)
        {
            try
            {
                var result = await _fieldTypesService.GetByTypeNameAsync(typeName);
                if (result.Success)
                {
                    return Ok(new ApiResponse(200, "Field type retrieved successfully", result.Data));
                }
                return StatusCode(result.StatusCode, new ApiResponse(result.StatusCode, result.ErrorMessage ?? "Field type not found"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving field type: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get active field types
        /// </summary>
        [HttpGet("active")]
        public async Task<ActionResult<ApiResponse>> GetActive()
        {
            try
            {
                var result = await _fieldTypesService.GetActiveAsync();
                if (result.Success)
                {
                    return Ok(new ApiResponse(200, "Active field types retrieved successfully", result.Data));
                }
                return StatusCode(result.StatusCode, new ApiResponse(result.StatusCode, result.ErrorMessage ?? "Error retrieving active field types"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving active field types: {ex.Message}"));
            }
        }

        /// <summary>
        /// Create new field type
        /// </summary>
        /// <param name="createDto">Field type data</param>
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Create([FromBody] CreateFieldTypeDto createDto)
        {
            try
            {
                var result = await _fieldTypesService.CreateAsync(createDto);
                if (result.Success && result.Data != null)
                {
                    return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, 
                        new ApiResponse(201, "Field type created successfully", result.Data));
                }
                return StatusCode(result.StatusCode, new ApiResponse(result.StatusCode, result.ErrorMessage ?? "Error creating field type"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error creating field type: {ex.Message}"));
            }
        }

        /// <summary>
        /// Update field type
        /// </summary>
        /// <param name="id">Field type ID</param>
        /// <param name="updateDto">Updated field type data</param>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] UpdateFieldTypeDto updateDto)
        {
            try
            {
                var result = await _fieldTypesService.UpdateAsync(id, updateDto);
                if (result.Success)
                {
                    return Ok(new ApiResponse(200, "Field type updated successfully", result.Data));
                }
                return StatusCode(result.StatusCode, new ApiResponse(result.StatusCode, result.ErrorMessage ?? "Error updating field type"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error updating field type: {ex.Message}"));
            }
        }

        /// <summary>
        /// Delete field type
        /// </summary>
        /// <param name="id">Field type ID</param>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            try
            {
                var result = await _fieldTypesService.DeleteAsync(id);
                if (result.Success)
                {
                    return Ok(new ApiResponse(200, "Field type deleted successfully", result.Data));
                }
                return StatusCode(result.StatusCode, new ApiResponse(result.StatusCode, result.ErrorMessage ?? "Error deleting field type"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error deleting field type: {ex.Message}"));
            }
        }

        /// <summary>
        /// Toggle active status of field type
        /// </summary>
        /// <param name="id">Field type ID</param>
        /// <param name="isActive">Active status</param>
        [HttpPatch("{id}/toggle-active")]
        public async Task<ActionResult<ApiResponse>> ToggleActive(int id, [FromQuery] bool isActive)
        {
            try
            {
                var result = await _fieldTypesService.ToggleActiveAsync(id, isActive);
                if (result.Success)
                {
                    return Ok(new ApiResponse(200, "Field type status updated successfully", result.Data));
                }
                return StatusCode(result.StatusCode, new ApiResponse(result.StatusCode, result.ErrorMessage ?? "Error updating field type status"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error updating field type status: {ex.Message}"));
            }
        }

        /// <summary>
        /// Check if field type exists
        /// </summary>
        /// <param name="id">Field type ID</param>
        [HttpGet("{id}/exists")]
        public async Task<ActionResult<ApiResponse>> Exists(int id)
        {
            try
            {
                var result = await _fieldTypesService.ExistsAsync(id);
                return Ok(new ApiResponse(200, result.Data ? "Field type exists" : "Field type does not exist", result.Data));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error checking field type existence: {ex.Message}"));
            }
        }

        /// <summary>
        /// Check if type name exists
        /// </summary>
        /// <param name="typeName">Type name</param>
        /// <param name="excludeId">ID to exclude from check</param>
        [HttpGet("name/{typeName}/exists")]
        public async Task<ActionResult<ApiResponse>> TypeNameExists(string typeName, [FromQuery] int? excludeId = null)
        {
            try
            {
                var result = await _fieldTypesService.TypeNameExistsAsync(typeName, excludeId);
                return Ok(new ApiResponse(200, result.Data ? "Type name exists" : "Type name does not exist", result.Data));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error checking type name existence: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get usage count for field type
        /// </summary>
        /// <param name="id">Field type ID</param>
        [HttpGet("{id}/usage-count")]
        public async Task<ActionResult<ApiResponse>> GetUsageCount(int id)
        {
            try
            {
                var result = await _fieldTypesService.GetUsageCountAsync(id);
                if (result.Success)
                {
                    return Ok(new ApiResponse(200, "Usage count retrieved successfully", result.Data));
                }
                return StatusCode(result.StatusCode, new ApiResponse(result.StatusCode, result.ErrorMessage ?? "Error getting usage count"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error getting usage count: {ex.Message}"));
            }
        }
    }
}

