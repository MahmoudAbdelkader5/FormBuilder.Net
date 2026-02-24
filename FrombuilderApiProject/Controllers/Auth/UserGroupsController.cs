using FormBuilder.API.Models;
using FormBuilder.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FormBuilder.API.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class UserGroupsController : ControllerBase
    {
        private readonly AkhmanageItContext _context;

        public UserGroupsController(AkhmanageItContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all user groups
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var userGroups = await _context.TblUserGroups
                .Select(ug => new
                {
                    ug.Id,
                    ug.Name,
                    ug.ForeignName,
                    ug.Description,
                    ug.IsActive,
                    ug.CreatedDate,
                    ug.UpdatedDate
                })
                .ToListAsync();

            return Ok(new ApiResponse(200, "User groups retrieved successfully", userGroups));
        }

        /// <summary>
        /// Get active user groups only
        /// </summary>
        [HttpGet("active")]
        public async Task<IActionResult> GetActive(CancellationToken cancellationToken = default)
        {
            var userGroups = await _context.TblUserGroups
                .Where(ug => ug.IsActive)
                .Select(ug => new
                {
                    ug.Id,
                    ug.Name,
                    ug.ForeignName,
                    ug.Description,
                    ug.IsActive,
                    ug.CreatedDate,
                    ug.UpdatedDate
                })
                .ToListAsync();

            return Ok(new ApiResponse(200, "Active user groups retrieved successfully", userGroups));
        }

        /// <summary>
        /// Get user group by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
        {
            var userGroup = await _context.TblUserGroups
                .Where(ug => ug.Id == id)
                .Select(ug => new
                {
                    ug.Id,
                    ug.Name,
                    ug.ForeignName,
                    ug.Description,
                    ug.IsActive,
                    ug.CreatedDate,
                    ug.UpdatedDate
                })
                .FirstOrDefaultAsync();

            if (userGroup == null)
            {
                return NotFound(new ApiResponse(404, "User group not found"));
            }

            return Ok(new ApiResponse(200, "User group retrieved successfully", userGroup));
        }
    }
}
