using formBuilder.Domian.Interfaces;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.Services.Services.Base;
using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.Common;
using FormBuilder.API.Models;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace FormBuilder.Services
{
    public class AttachmentTypeService : BaseService<ATTACHMENT_TYPES, AttachmentTypeDto, CreateAttachmentTypeDto, UpdateAttachmentTypeDto>, IAttachmentTypeService
    {
        private readonly IunitOfwork _unitOfWork;

        public AttachmentTypeService(IunitOfwork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        protected override IBaseRepository<ATTACHMENT_TYPES> Repository => _unitOfWork.AttachmentTypeRepository;

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

        public async Task<ApiResponse> GetByCodeAsync(string code)
        {
            var attachmentType = await _unitOfWork.AttachmentTypeRepository.GetByCodeAsync(code);
            if (attachmentType == null)
                return new ApiResponse(404, "Attachment type not found");

            var attachmentTypeDto = _mapper.Map<AttachmentTypeDto>(attachmentType);
            return new ApiResponse(200, "Attachment type retrieved successfully", attachmentTypeDto);
        }

        public async Task<ApiResponse> GetActiveAsync()
        {
            var result = await base.GetActiveAsync();
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> CreateAsync(CreateAttachmentTypeDto createDto)
        {
            var result = await base.CreateAsync(createDto);
            return ConvertToApiResponse(result);
        }

        protected override async Task<ValidationResult> ValidateCreateAsync(CreateAttachmentTypeDto dto)
        {
            var codeExists = await _unitOfWork.AttachmentTypeRepository.CodeExistsAsync(dto.Code);
            if (codeExists)
                return ValidationResult.Failure("Attachment type code already exists");

            return ValidationResult.Success();
        }

        public async Task<ApiResponse> UpdateAsync(int id, UpdateAttachmentTypeDto updateDto)
        {
            var result = await base.UpdateAsync(id, updateDto);
            return ConvertToApiResponse(result);
        }

        protected override async Task<ValidationResult> ValidateUpdateAsync(int id, UpdateAttachmentTypeDto dto, ATTACHMENT_TYPES entity)
        {
            // Check if code already exists (excluding current record)
            if (!string.IsNullOrEmpty(dto.Code) && dto.Code != entity.Code)
            {
                var codeExists = await _unitOfWork.AttachmentTypeRepository.CodeExistsAsync(dto.Code, id);
                if (codeExists)
                    return ValidationResult.Failure("Attachment type code already exists");
            }

            return ValidationResult.Success();
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var result = await base.DeleteAsync(id);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> ToggleActiveAsync(int id, bool isActive)
        {
            var result = await base.ToggleActiveAsync(id, isActive);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> ExistsAsync(int id)
        {
            var exists = await _unitOfWork.AttachmentTypeRepository.AnyAsync(s => s.Id == id);
            return new ApiResponse(200, "Attachment type existence checked successfully", exists);
        }

        // ================================
        // HELPER METHODS
        // ================================
        private ApiResponse ConvertToApiResponse<T>(ServiceResult<T> result)
        {
            if (result.Success)
                return new ApiResponse(result.StatusCode, "Success", result.Data);
            else
                return new ApiResponse(result.StatusCode, result.ErrorMessage);
        }

        private ApiResponse ConvertToApiResponse(ServiceResult<bool> result)
        {
            if (result.Success)
                return new ApiResponse(result.StatusCode, "Success", result.Data);
            else
                return new ApiResponse(result.StatusCode, result.ErrorMessage);
        }
    }
}
