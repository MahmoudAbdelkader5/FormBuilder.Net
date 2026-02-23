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
    public class UsersController : ControllerBase
    {
        private readonly AkhmanageItContext _context;

        public UsersController(AkhmanageItContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.TblUsers
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.Name,
                    u.Email,
                    u.Phone,
                    u.IsActive,
                    u.CreatedDate,
                    u.UpdatedDate
                })
                .ToListAsync();

            return Ok(new ApiResponse(200, "Users retrieved successfully", users));
        }

        /// <summary>
        /// Get active users only
        /// </summary>
        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var users = await _context.TblUsers
                .Where(u => u.IsActive)
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.Name,
                    u.Email,
                    u.Phone,
                    u.IsActive,
                    u.CreatedDate,
                    u.UpdatedDate
                })
                .ToListAsync();

            return Ok(new ApiResponse(200, "Active users retrieved successfully", users));
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _context.TblUsers
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.Name,
                    u.Email,
                    u.Phone,
                    u.IsActive,
                    u.CreatedDate,
                    u.UpdatedDate
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new ApiResponse(404, "User not found"));
            }

            return Ok(new ApiResponse(200, "User retrieved successfully", user));
        }
    }
}






