using FormBuilder.API.Attributes;
using FormBuilder.API.Models;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Core.IServices.FormBuilder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TableMenusController : ControllerBase
    {
        private readonly ITableMenusService _tableMenusService;

        public TableMenusController(ITableMenusService tableMenusService)
        {
            _tableMenusService = tableMenusService ?? throw new ArgumentNullException(nameof(tableMenusService));
        }

        #region Menu Endpoints

        [HttpGet("menus")]
        public async Task<ActionResult<ApiResponse>> GetAllMenus()
        {
            try
            {
                var result = await _tableMenusService.GetAllMenusAsync();
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving menus: {ex.Message}"));
            }
        }

        [HttpGet("menus/{id}")]
        public async Task<ActionResult<ApiResponse>> GetMenuById(int id)
        {
            try
            {
                var result = await _tableMenusService.GetMenuByIdAsync(id);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving menu: {ex.Message}"));
            }
        }

        [HttpGet("menus/code/{menuCode}")]
        public async Task<ActionResult<ApiResponse>> GetMenuByCode(string menuCode)
        {
            try
            {
                var result = await _tableMenusService.GetMenuByCodeAsync(menuCode);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving menu: {ex.Message}"));
            }
        }

        [HttpPost("menus")]
        [RequirePermission("TableMenu_Allow_Create")]
        public async Task<ActionResult<ApiResponse>> CreateMenu([FromBody] CreateTableMenuDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse(400, "Invalid menu data", ModelState));
                }

                var result = await _tableMenusService.CreateMenuAsync(createDto);
                
                if (result.StatusCode == 201)
                {
                    return CreatedAtAction(nameof(GetMenuById), new { id = (result.Data as TableMenuDto)?.Id }, result);
                }

                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error creating menu: {ex.Message}"));
            }
        }

        [HttpPut("menus/{id}")]
        [RequirePermission("TableMenu_Allow_Edit")]
        public async Task<ActionResult<ApiResponse>> UpdateMenu(int id, [FromBody] UpdateTableMenuDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse(400, "Invalid menu data", ModelState));
                }

                var result = await _tableMenusService.UpdateMenuAsync(id, updateDto);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error updating menu: {ex.Message}"));
            }
        }

        [HttpDelete("menus/{id}")]
        [RequirePermission("TableMenu_Allow_Delete")]
        public async Task<ActionResult<ApiResponse>> DeleteMenu(int id)
        {
            try
            {
                var result = await _tableMenusService.DeleteMenuAsync(id);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error deleting menu: {ex.Message}"));
            }
        }

        [HttpDelete("menus/{id}/soft-delete")]
        [RequirePermission("TableMenu_Allow_Delete")]
        public async Task<ActionResult<ApiResponse>> SoftDeleteMenu(int id)
        {
            try
            {
                var result = await _tableMenusService.SoftDeleteMenuAsync(id);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error soft deleting menu: {ex.Message}"));
            }
        }

        #endregion

        #region Sub Menu Endpoints

        [HttpGet("sub-menus")]
        public async Task<ActionResult<ApiResponse>> GetAllSubMenus()
        {
            try
            {
                var result = await _tableMenusService.GetAllSubMenusAsync();
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving sub menus: {ex.Message}"));
            }
        }

        [HttpGet("sub-menus/menu/{menuId}")]
        public async Task<ActionResult<ApiResponse>> GetSubMenusByMenuId(int menuId)
        {
            try
            {
                var result = await _tableMenusService.GetSubMenusByMenuIdAsync(menuId);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving sub menus: {ex.Message}"));
            }
        }

        [HttpGet("sub-menus/{id}")]
        public async Task<ActionResult<ApiResponse>> GetSubMenuById(int id)
        {
            try
            {
                var result = await _tableMenusService.GetSubMenuByIdAsync(id);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving sub menu: {ex.Message}"));
            }
        }

        [HttpPost("sub-menus")]
        [RequirePermission("TableMenu_Allow_Create")]
        public async Task<ActionResult<ApiResponse>> CreateSubMenu([FromBody] CreateTableSubMenuDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse(400, "Invalid sub menu data", ModelState));
                }

                var result = await _tableMenusService.CreateSubMenuAsync(createDto);
                
                if (result.StatusCode == 201)
                {
                    return CreatedAtAction(nameof(GetSubMenuById), new { id = (result.Data as TableSubMenuDto)?.Id }, result);
                }

                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error creating sub menu: {ex.Message}"));
            }
        }

        [HttpPut("sub-menus/{id}")]
        [RequirePermission("TableMenu_Allow_Edit")]
        public async Task<ActionResult<ApiResponse>> UpdateSubMenu(int id, [FromBody] UpdateTableSubMenuDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse(400, "Invalid sub menu data", ModelState));
                }

                var result = await _tableMenusService.UpdateSubMenuAsync(id, updateDto);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error updating sub menu: {ex.Message}"));
            }
        }

        [HttpDelete("sub-menus/{id}")]
        [RequirePermission("TableMenu_Allow_Delete")]
        public async Task<ActionResult<ApiResponse>> DeleteSubMenu(int id)
        {
            try
            {
                var result = await _tableMenusService.DeleteSubMenuAsync(id);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error deleting sub menu: {ex.Message}"));
            }
        }

        [HttpDelete("sub-menus/{id}/soft-delete")]
        [RequirePermission("TableMenu_Allow_Delete")]
        public async Task<ActionResult<ApiResponse>> SoftDeleteSubMenu(int id)
        {
            try
            {
                var result = await _tableMenusService.SoftDeleteSubMenuAsync(id);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error soft deleting sub menu: {ex.Message}"));
            }
        }

        #endregion

        #region Menu Document Endpoints

        [HttpGet("menu-documents")]
        public async Task<ActionResult<ApiResponse>> GetAllMenuDocuments()
        {
            try
            {
                var result = await _tableMenusService.GetAllMenuDocumentsAsync();
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving menu documents: {ex.Message}"));
            }
        }

        [HttpGet("menu-documents/menu/{menuId}")]
        public async Task<ActionResult<ApiResponse>> GetMenuDocumentsByMenuId(int menuId)
        {
            try
            {
                var result = await _tableMenusService.GetMenuDocumentsByMenuIdAsync(menuId);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving menu documents: {ex.Message}"));
            }
        }

        [HttpGet("menu-documents/sub-menu/{subMenuId}")]
        public async Task<ActionResult<ApiResponse>> GetMenuDocumentsBySubMenuId(int subMenuId)
        {
            try
            {
                var result = await _tableMenusService.GetMenuDocumentsBySubMenuIdAsync(subMenuId);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving menu documents: {ex.Message}"));
            }
        }

        [HttpGet("menu-documents/{id}")]
        public async Task<ActionResult<ApiResponse>> GetMenuDocumentById(int id)
        {
            try
            {
                var result = await _tableMenusService.GetMenuDocumentByIdAsync(id);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving menu document: {ex.Message}"));
            }
        }

        [HttpPost("menu-documents")]
        [RequirePermission("TableMenu_Allow_Create")]
        public async Task<ActionResult<ApiResponse>> CreateMenuDocument([FromBody] CreateTableMenuDocumentDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse(400, "Invalid menu document data", ModelState));
                }

                var result = await _tableMenusService.CreateMenuDocumentAsync(createDto);
                
                if (result.StatusCode == 201)
                {
                    return CreatedAtAction(nameof(GetMenuDocumentById), new { id = (result.Data as TableMenuDocumentDto)?.Id }, result);
                }

                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error creating menu document: {ex.Message}"));
            }
        }

        [HttpPut("menu-documents/{id}")]
        [RequirePermission("TableMenu_Allow_Edit")]
        public async Task<ActionResult<ApiResponse>> UpdateMenuDocument(int id, [FromBody] UpdateTableMenuDocumentDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse(400, "Invalid menu document data", ModelState));
                }

                var result = await _tableMenusService.UpdateMenuDocumentAsync(id, updateDto);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error updating menu document: {ex.Message}"));
            }
        }

        [HttpDelete("menu-documents/{id}")]
        [RequirePermission("TableMenu_Allow_Delete")]
        public async Task<ActionResult<ApiResponse>> DeleteMenuDocument(int id)
        {
            try
            {
                var result = await _tableMenusService.DeleteMenuDocumentAsync(id);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error deleting menu document: {ex.Message}"));
            }
        }

        [HttpDelete("menu-documents/{id}/soft-delete")]
        [RequirePermission("TableMenu_Allow_Delete")]
        public async Task<ActionResult<ApiResponse>> SoftDeleteMenuDocument(int id)
        {
            try
            {
                var result = await _tableMenusService.SoftDeleteMenuDocumentAsync(id);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error soft deleting menu document: {ex.Message}"));
            }
        }

        #endregion

        #region Dashboard Endpoints (for end users)

        [HttpGet("dashboard")]
        [AllowAnonymous] // Allow anonymous, but permissions will be checked in service
        public async Task<ActionResult<ApiResponse>> GetDashboardMenus([FromQuery] List<string>? permissions = null)
        {
            try
            {
                var result = await _tableMenusService.GetDashboardMenusAsync(permissions);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving dashboard menus: {ex.Message}"));
            }
        }

        [HttpGet("dashboard/menus/{menuId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> GetDashboardMenuById(int menuId, [FromQuery] List<string>? permissions = null)
        {
            try
            {
                var result = await _tableMenusService.GetDashboardMenuByIdAsync(menuId, permissions);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error retrieving dashboard menu: {ex.Message}"));
            }
        }

        #endregion
    }
}
