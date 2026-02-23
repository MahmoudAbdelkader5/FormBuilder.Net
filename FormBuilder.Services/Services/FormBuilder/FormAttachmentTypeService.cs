using formBuilder.Domian.Interfaces;
using FormBuilder.API.Models.DTOs;
using FormBuilder.API.Models;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Services.Services.Base;
using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.Common;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Services
{
    public class FormAttachmentTypeService : BaseService<FORM_ATTACHMENT_TYPES, FormAttachmentTypeDto, CreateFormAttachmentTypeDto, UpdateFormAttachmentTypeDto>, IFormAttachmentTypeService
    {
        private readonly IunitOfwork _unitOfWork;

        public FormAttachmentTypeService(IunitOfwork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        protected override IBaseRepository<FORM_ATTACHMENT_TYPES> Repository => _unitOfWork.FormAttachmentTypeRepository;

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

        public async Task<ApiResponse> GetByFormBuilderIdAsync(int formBuilderId)
        {
            var formAttachmentTypes = await _unitOfWork.FormAttachmentTypeRepository.GetByFormBuilderIdAsync(formBuilderId);
            var dtos = _mapper.Map<IEnumerable<FormAttachmentTypeDto>>(formAttachmentTypes);
            return new ApiResponse(200, "Form attachment types retrieved successfully", dtos);
        }

        public async Task<ApiResponse> GetByAttachmentTypeIdAsync(int attachmentTypeId)
        {
            var formAttachmentTypes = await _unitOfWork.FormAttachmentTypeRepository.GetByAttachmentTypeIdAsync(attachmentTypeId);
            var dtos = _mapper.Map<IEnumerable<FormAttachmentTypeDto>>(formAttachmentTypes);
            return new ApiResponse(200, "Form attachment types retrieved successfully", dtos);
        }

        public async Task<ApiResponse> GetActiveAsync()
        {
            var result = await base.GetActiveAsync();
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> GetActiveByFormBuilderIdAsync(int formBuilderId)
        {
            var formAttachmentTypes = await _unitOfWork.FormAttachmentTypeRepository.GetActiveByFormBuilderIdAsync(formBuilderId);
            var dtos = _mapper.Map<IEnumerable<FormAttachmentTypeDto>>(formAttachmentTypes);
            return new ApiResponse(200, "Active form attachment types retrieved successfully", dtos);
        }

        public async Task<ApiResponse> GetMandatoryByFormBuilderIdAsync(int formBuilderId)
        {
            var formAttachmentTypes = await _unitOfWork.FormAttachmentTypeRepository.GetMandatoryByFormBuilderIdAsync(formBuilderId);
            var dtos = _mapper.Map<IEnumerable<FormAttachmentTypeDto>>(formAttachmentTypes);
            return new ApiResponse(200, "Mandatory form attachment types retrieved successfully", dtos);
        }

        public async Task<ApiResponse> CreateAsync(CreateFormAttachmentTypeDto createDto)
        {
            var result = await base.CreateAsync(createDto);
            return ConvertToApiResponse(result);
        }

        protected override async Task<ValidationResult> ValidateCreateAsync(CreateFormAttachmentTypeDto dto)
        {
            var formBuilderExists = await _unitOfWork.FormBuilderRepository.AnyAsync(e => e.Id == dto.FormBuilderId);
            if (!formBuilderExists)
                return ValidationResult.Failure("Invalid form builder ID");

            var attachmentTypeExists = await _unitOfWork.AttachmentTypeRepository.AnyAsync(e => e.Id == dto.AttachmentTypeId);
            if (!attachmentTypeExists)
                return ValidationResult.Failure("Invalid attachment type ID");

            var exists = await _unitOfWork.FormAttachmentTypeRepository.ExistsAsync(dto.FormBuilderId, dto.AttachmentTypeId);
            if (exists)
                return ValidationResult.Failure("Form attachment type association already exists");

            return ValidationResult.Success();
        }

        public async Task<ApiResponse> CreateBulkAsync(List<CreateFormAttachmentTypeDto> createDtos)
        {
            if (createDtos == null || !createDtos.Any())
                return new ApiResponse(400, "No form attachment types provided");

            var entities = new List<FORM_ATTACHMENT_TYPES>();

            foreach (var createDto in createDtos)
            {
                var formBuilderExists = await _unitOfWork.FormBuilderRepository.AnyAsync(e => e.Id == createDto.FormBuilderId);
                if (!formBuilderExists)
                    return new ApiResponse(400, $"Invalid form builder ID: {createDto.FormBuilderId}");

                var attachmentTypeExists = await _unitOfWork.AttachmentTypeRepository.AnyAsync(e => e.Id == createDto.AttachmentTypeId);
                if (!attachmentTypeExists)
                    return new ApiResponse(400, $"Invalid attachment type ID: {createDto.AttachmentTypeId}");

                var exists = await _unitOfWork.FormAttachmentTypeRepository.ExistsAsync(createDto.FormBuilderId, createDto.AttachmentTypeId);
                if (!exists)
                {
                    var entity = _mapper.Map<FORM_ATTACHMENT_TYPES>(createDto);
                    entities.Add(entity);
                }
            }

            if (entities.Any())
            {
                _unitOfWork.FormAttachmentTypeRepository.AddRange(entities);
                await _unitOfWork.CompleteAsyn();
            }

            var resultDtos = _mapper.Map<IEnumerable<FormAttachmentTypeDto>>(entities);
            return new ApiResponse(200, "Form attachment types created successfully", resultDtos);
        }

        public async Task<ApiResponse> UpdateAsync(int id, UpdateFormAttachmentTypeDto updateDto)
        {
            var result = await base.UpdateAsync(id, updateDto);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var result = await base.DeleteAsync(id);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> DeleteByFormBuilderIdAsync(int formBuilderId)
        {
            var count = await _unitOfWork.FormAttachmentTypeRepository.DeleteByFormBuilderIdAsync(formBuilderId);
            return new ApiResponse(200, $"{count} form attachment types deleted successfully", count);
        }

        public async Task<ApiResponse> ToggleActiveAsync(int id, bool isActive)
        {
            var result = await base.ToggleActiveAsync(id, isActive);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> ToggleMandatoryAsync(int id, bool isMandatory)
        {
            var entity = await _unitOfWork.FormAttachmentTypeRepository.GetByIdAsync(id);
            if (entity == null)
                return new ApiResponse(404, "Form attachment type not found");

            entity.IsMandatory = isMandatory;
            entity.UpdatedDate = DateTime.UtcNow;
            _unitOfWork.FormAttachmentTypeRepository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            var updatedEntity = await _unitOfWork.FormAttachmentTypeRepository.GetByIdAsync(id);
            var dto = _mapper.Map<FormAttachmentTypeDto>(updatedEntity);
            var message = isMandatory ? "set as mandatory" : "set as optional";
            return new ApiResponse(200, $"Form attachment type {message} successfully", dto);
        }

        public async Task<ApiResponse> ExistsAsync(int id)
        {
            var exists = await _unitOfWork.FormAttachmentTypeRepository.AnyAsync(e => e.Id == id);
            return new ApiResponse(200, "Form attachment type existence checked successfully", exists);
        }

        public async Task<ApiResponse> IsActiveAsync(int id)
        {
            var isActive = await _unitOfWork.FormAttachmentTypeRepository.IsActiveAsync(id);
            return new ApiResponse(200, "Form attachment type active status checked successfully", isActive);
        }

        public async Task<ApiResponse> HasMandatoryAttachmentsAsync(int formBuilderId)
        {
            var hasMandatory = await _unitOfWork.FormAttachmentTypeRepository.HasMandatoryAttachmentsAsync(formBuilderId);
            return new ApiResponse(200, "Mandatory attachments check completed successfully", hasMandatory);
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
