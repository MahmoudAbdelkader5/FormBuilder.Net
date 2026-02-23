using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Application.DTOs.Formula;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Domian.Entitys.froms;
using FormBuilder.Domian.Interfaces;
using FormBuilder.API.Models;
using FormBuilder.Services.Services.Base;
using FormBuilder.Application.DTOS;
using AutoMapper;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Services
{
    public class FormulaVariableService : BaseService<FORMULA_VARIABLES, FormulaVariableDto, FormulaVariableCreateDto, FormulaVariableUpdateDto>, IFormulaVariableService
    {
        private readonly IunitOfwork _unitOfWork;

        public FormulaVariableService(IunitOfwork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        protected override IBaseRepository<FORMULA_VARIABLES> Repository => _unitOfWork.FormulaVariablesRepository;

        public async Task<ApiResponse> GetAllAsync()
        {
            var result = await base.GetAllAsync();
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> GetByIdAsync(int id)
        {
            var result = await base.GetByIdAsync(id);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> CreateAsync(FormulaVariableCreateDto dto)
        {
            var result = await base.CreateAsync(dto);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> UpdateAsync(int id, FormulaVariableUpdateDto dto)
        {
            var result = await base.UpdateAsync(id, dto);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var result = await base.DeleteAsync(id);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> ExistsAsync(int id)
        {
            var exists = await _unitOfWork.FormulaVariablesRepository.AnyAsync(x => x.Id == id);
            return new ApiResponse(200, "Existence checked", exists);
        }

        // ===============================
        //          HELPER METHODS
        // ===============================
        private ApiResponse ConvertToApiResponse<T>(ServiceResult<T> result)
        {
            if (result.Success)
                return new ApiResponse(result.StatusCode, "Success", result.Data);
            else
                return new ApiResponse(result.StatusCode, result.ErrorMessage);
        }
    }
}
