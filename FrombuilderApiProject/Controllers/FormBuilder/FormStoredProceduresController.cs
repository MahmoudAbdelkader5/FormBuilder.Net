using FormBuilder.API.Attributes;
using FormBuilder.API.Models;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Core.IServices.FormBuilder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FormStoredProceduresController : ControllerBase
    {
        private readonly IFormStoredProceduresService _storedProceduresService;

        public FormStoredProceduresController(IFormStoredProceduresService storedProceduresService)
        {
            _storedProceduresService = storedProceduresService ?? throw new ArgumentNullException(nameof(storedProceduresService));
        }

        /// <summary>
        /// Get current user ID from claims
        /// </summary>
        private string GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || string.IsNullOrWhiteSpace(userIdClaim.Value))
            {
                throw new UnauthorizedAccessException("User ID not found in token");
            }
            return userIdClaim.Value;
        }

        #region GET Operations

        /// <summary>
        /// Get all active stored procedures
        /// GET /api/FormStoredProcedures
        /// </summary>
        [HttpGet]
        [RequirePermission("FormStoredProcedure_Allow_View")]
        public async Task<ActionResult<ApiResponse>> GetAll()
        {
            try
            {
                var result = await _storedProceduresService.GetAllAsync();
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving stored procedures: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get stored procedures filtered by usage type
        /// GET /api/FormStoredProcedures/usage-type/{usageType}
        /// </summary>
        [HttpGet("usage-type/{usageType?}")]
        [RequirePermission("FormStoredProcedure_Allow_View")]
        public async Task<ActionResult<ApiResponse>> GetByUsageType(string? usageType)
        {
            try
            {
                var result = await _storedProceduresService.GetByUsageTypeAsync(usageType);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving stored procedures: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get stored procedures filtered by database name
        /// GET /api/FormStoredProcedures/database/{databaseName}
        /// </summary>
        [HttpGet("database/{databaseName}")]
        [RequirePermission("FormStoredProcedure_Allow_View")]
        public async Task<ActionResult<ApiResponse>> GetByDatabase(string databaseName)
        {
            try
            {
                var result = await _storedProceduresService.GetByDatabaseAsync(databaseName);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving stored procedures: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get a specific stored procedure by ID
        /// GET /api/FormStoredProcedures/{id}
        /// </summary>
        [HttpGet("{id}")]
        [RequirePermission("FormStoredProcedure_Allow_View")]
        public async Task<ActionResult<ApiResponse>> GetById(int id)
        {
            try
            {
                var result = await _storedProceduresService.GetByIdAsync(id);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving stored procedure: {ex.Message}"));
            }
        }

        /// <summary>
        /// Validate a stored procedure (check if it exists and is active)
        /// GET /api/FormStoredProcedures/{id}/validate
        /// </summary>
        [HttpGet("{id}/validate")]
        [RequirePermission("FormStoredProcedure_Allow_View")]
        public async Task<ActionResult<ApiResponse>> Validate(int id)
        {
            try
            {
                var result = await _storedProceduresService.ValidateStoredProcedureAsync(id);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error validating stored procedure: {ex.Message}"));
            }
        }

        #endregion

        #region POST Operations

        /// <summary>
        /// Create a new stored procedure
        /// POST /api/FormStoredProcedures
        /// </summary>
        [HttpPost]
        [RequirePermission("FormStoredProcedure_Allow_Create")]
        public async Task<ActionResult<ApiResponse>> Create([FromBody] CreateStoredProcedureDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse(400, "Invalid stored procedure data", ModelState));
                }

                var userId = GetCurrentUserId();
                var result = await _storedProceduresService.CreateAsync(createDto, userId);

                if (result.StatusCode == 201)
                {
                    return CreatedAtAction(nameof(GetById), new { id = ((StoredProcedureDto)result.Data!).Id }, result);
                }

                return StatusCode(result.StatusCode, result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ApiResponse(401, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error creating stored procedure: {ex.Message}"));
            }
        }

        #endregion

        #region PUT Operations

        /// <summary>
        /// Update an existing stored procedure
        /// PUT /api/FormStoredProcedures/{id}
        /// </summary>
        [HttpPut("{id}")]
        [RequirePermission("FormStoredProcedure_Allow_Edit")]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] UpdateStoredProcedureDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse(400, "Invalid stored procedure data", ModelState));
                }

                var userId = GetCurrentUserId();
                var result = await _storedProceduresService.UpdateAsync(id, updateDto, userId);
                return StatusCode(result.StatusCode, result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ApiResponse(401, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error updating stored procedure: {ex.Message}"));
            }
        }

        #endregion

        #region DELETE Operations

        /// <summary>
        /// Hard delete a stored procedure
        /// DELETE /api/FormStoredProcedures/{id}
        /// </summary>
        [HttpDelete("{id}")]
        [RequirePermission("FormStoredProcedure_Allow_Delete")]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _storedProceduresService.DeleteAsync(id, userId);
                return StatusCode(result.StatusCode, result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ApiResponse(401, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error deleting stored procedure: {ex.Message}"));
            }
        }

        /// <summary>
        /// Soft delete a stored procedure
        /// DELETE /api/FormStoredProcedures/{id}/soft-delete
        /// </summary>
        [HttpDelete("{id}/soft-delete")]
        [RequirePermission("FormStoredProcedure_Allow_Delete")]
        public async Task<ActionResult<ApiResponse>> SoftDelete(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _storedProceduresService.SoftDeleteAsync(id, userId);
                return StatusCode(result.StatusCode, result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ApiResponse(401, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error deleting stored procedure: {ex.Message}"));
            }
        }

        #endregion
    }
}

