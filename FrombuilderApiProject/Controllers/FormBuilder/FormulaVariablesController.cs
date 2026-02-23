using FormBuilder.Application.DTOs.Formula;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Domian.Entitys.FormBuilder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class FormulaVariablesController : ControllerBase
    {
        private readonly IFormulaVariableService _service;

        public FormulaVariablesController(IFormulaVariableService service)
        {
            _service = service;
        }

        // GET: api/FormulaVariables
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        // GET: api/FormulaVariables/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        // POST: api/FormulaVariables
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FormulaVariableCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        // PUT: api/FormulaVariables/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] FormulaVariableUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        // DELETE: api/FormulaVariables/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        // GET: api/FormulaVariables/Exists/5
        [HttpGet("Exists/{id:int}")]
        public async Task<IActionResult> Exists(int id)
        {
            var result = await _service.ExistsAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
