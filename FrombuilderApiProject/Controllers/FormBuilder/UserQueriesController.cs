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
    public class UserQueriesController : ControllerBase
    {
        private readonly IUserQueriesService _userQueriesService;

        public UserQueriesController(IUserQueriesService userQueriesService)
        {
            _userQueriesService = userQueriesService ?? throw new ArgumentNullException(nameof(userQueriesService));
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
        /// Get all queries for the current user
        /// GET /api/UserQueries
        /// </summary>
        [HttpGet]
        [RequirePermission("UserQuery_Allow_View")]
        public async Task<ActionResult<ApiResponse>> GetAll()
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _userQueriesService.GetAllAsync(userId);
                return StatusCode(result.StatusCode, result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ApiResponse(401, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving queries: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get queries for the current user filtered by database name
        /// GET /api/UserQueries/database/{databaseName}
        /// </summary>
        [HttpGet("database/{databaseName}")]
        [RequirePermission("UserQuery_Allow_View")]
        public async Task<ActionResult<ApiResponse>> GetByDatabase(string databaseName)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _userQueriesService.GetByDatabaseAsync(userId, databaseName);
                return StatusCode(result.StatusCode, result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ApiResponse(401, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving queries: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get a specific query by ID
        /// GET /api/UserQueries/{id}
        /// </summary>
        [HttpGet("{id}")]
        [RequirePermission("UserQuery_Allow_View")]
        public async Task<ActionResult<ApiResponse>> GetById(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _userQueriesService.GetByIdAsync(id, userId);
                return StatusCode(result.StatusCode, result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ApiResponse(401, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving query: {ex.Message}"));
            }
        }

        #endregion

        #region POST Operations

        /// <summary>
        /// Create a new query for the current user
        /// POST /api/UserQueries
        /// </summary>
        [HttpPost]
        [RequirePermission("UserQuery_Allow_Create")]
        public async Task<ActionResult<ApiResponse>> Create([FromBody] CreateUserQueryDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse(400, "Invalid query data", ModelState));
                }

                var userId = GetCurrentUserId();
                var result = await _userQueriesService.CreateAsync(createDto, userId);

                if (result.StatusCode == 201)
                {
                    return CreatedAtAction(nameof(GetById), new { id = ((UserQueryDto)result.Data!).Id }, result);
                }

                return StatusCode(result.StatusCode, result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ApiResponse(401, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error creating query: {ex.Message}"));
            }
        }

        #endregion

        #region PUT Operations

        /// <summary>
        /// Update an existing query
        /// PUT /api/UserQueries/{id}
        /// </summary>
        [HttpPut("{id}")]
        [RequirePermission("UserQuery_Allow_Edit")]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] UpdateUserQueryDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse(400, "Invalid query data", ModelState));
                }

                var userId = GetCurrentUserId();
                var result = await _userQueriesService.UpdateAsync(id, updateDto, userId);
                return StatusCode(result.StatusCode, result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ApiResponse(401, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error updating query: {ex.Message}"));
            }
        }

        #endregion

        #region DELETE Operations

        /// <summary>
        /// Hard delete a query
        /// DELETE /api/UserQueries/{id}
        /// </summary>
        [HttpDelete("{id}")]
        [RequirePermission("UserQuery_Allow_Delete")]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _userQueriesService.DeleteAsync(id, userId);
                return StatusCode(result.StatusCode, result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ApiResponse(401, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error deleting query: {ex.Message}"));
            }
        }

        /// <summary>
        /// Soft delete a query
        /// DELETE /api/UserQueries/{id}/soft-delete
        /// </summary>
        [HttpDelete("{id}/soft-delete")]
        [RequirePermission("UserQuery_Allow_Delete")]
        public async Task<ActionResult<ApiResponse>> SoftDelete(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _userQueriesService.SoftDeleteAsync(id, userId);
                return StatusCode(result.StatusCode, result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ApiResponse(401, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error deleting query: {ex.Message}"));
            }
        }

        #endregion
    }
}

