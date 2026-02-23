using FormBuilder.API.Attributes;
using FormBuilder.API.Models.DTOs;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DocumentTypesController : ControllerBase
    {
        private readonly IDocumentTypeService _documentTypeService;

        public DocumentTypesController(IDocumentTypeService documentTypeService)
        {
            _documentTypeService = documentTypeService;
        }

 
        [HttpGet]
        [RequirePermission("Document_Allow_View")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _documentTypeService.GetAllAsync();
            return result.ToActionResult();
        }

       
        [HttpGet("{id}")]
        [RequirePermission("Document_Allow_View")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _documentTypeService.GetByIdAsync(id);
            return result.ToActionResult();
        }

       
        [HttpGet("code/{code}")]
        [RequirePermission("Document_Allow_View")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var result = await _documentTypeService.GetByCodeAsync(code);
            return result.ToActionResult();
        }

      
        [HttpGet("active")]
        [RequirePermission("Document_Allow_View")]
        public async Task<IActionResult> GetActive()
        {
            var result = await _documentTypeService.GetActiveAsync();
            return result.ToActionResult();
        }

      
        [HttpGet("parent-menu/{parentMenuId}")]
        [RequirePermission("Document_Allow_View")]
        public async Task<IActionResult> GetByParentMenuId(int parentMenuId)
        {
            var result = await _documentTypeService.GetByParentMenuIdAsync(parentMenuId);
            return result.ToActionResult();
        }

        
        [HttpGet("parent-menu/null")]
        [RequirePermission("Document_Allow_View")]
        public async Task<IActionResult> GetRootMenuItems()
        {
            var result = await _documentTypeService.GetByParentMenuIdAsync(null);
            return result.ToActionResult();
        }

        
        [HttpPost]
        [RequirePermission("Document_Allow_Create")]
        public async Task<IActionResult> Create([FromBody] CreateDocumentTypeDto createDto)
        {
            var result = await _documentTypeService.CreateAsync(createDto);
            if (result.Success && result.Data != null)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
            }
            return result.ToActionResult();
        }

      
        [HttpPut("{id}")]
        [RequirePermission("Document_Allow_Edit")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDocumentTypeDto updateDto)
        {
            var result = await _documentTypeService.UpdateAsync(id, updateDto);
            if (result.Success) return NoContent();
            return result.ToActionResult();
        }

     

        
        [HttpDelete("{id}")]
        [RequirePermission("Document_Allow_Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _documentTypeService.DeleteAsync(id);
            if (result.Success) return NoContent();
            return result.ToActionResult();
        }
    }
}