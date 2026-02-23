using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Application.DTOs.ApprovalWorkflow;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Domian.Interfaces;
using FormBuilder.API.Models;
using FormBuilder.Services.Services.Base;
using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.Common;
using FormBuilder.Core.IServices.FormBuilder;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FormBuilder.Services
{
    public class ApprovalStageService : BaseService<APPROVAL_STAGES, ApprovalStageDto, ApprovalStageCreateDto, ApprovalStageUpdateDto>, IApprovalStageService
    {
        private readonly IFormFieldService _formFieldService;

        public ApprovalStageService(IunitOfwork unitOfWork, IMapper mapper, IFormFieldService formFieldService) : base(unitOfWork, mapper)
        {
            _formFieldService = formFieldService ?? throw new ArgumentNullException(nameof(formFieldService));
        }

        protected override IBaseRepository<APPROVAL_STAGES> Repository => _unitOfWork.ApprovalStageRepository;

        public async Task<ApiResponse> GetAllAsync(int workflowId)
        {
            Expression<Func<APPROVAL_STAGES, bool>> filter = workflowId > 0 
                ? (s => s.WorkflowId == workflowId) 
                : null;
            
            var result = await base.GetAllAsync(filter);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> GetByIdAsync(int id)
        {
            var result = await base.GetByIdAsync(id);
            return ConvertToApiResponse(result);
        }

        protected override async Task<ValidationResult> ValidateCreateAsync(ApprovalStageCreateDto dto)
        {
            // Validate StageName uniqueness within the same Workflow
            var stageNameExists = await Repository.AnyAsync(s => 
                s.WorkflowId == dto.WorkflowId && 
                s.StageName == dto.StageName && 
                !s.IsDeleted);
                
            if (stageNameExists)
            {
                return ValidationResult.Failure("Stage name already exists in this workflow");
            }

            // Validate StageOrder uniqueness within the same Workflow
            var stageOrderExists = await Repository.AnyAsync(s => 
                s.WorkflowId == dto.WorkflowId && 
                s.StageOrder == dto.StageOrder && 
                !s.IsDeleted);
                
            if (stageOrderExists)
            {
                return ValidationResult.Failure("Stage order already exists in this workflow");
            }

            // Validate MinAmount < MaxAmount when both are provided
            if (dto.MinAmount.HasValue && dto.MaxAmount.HasValue && dto.MinAmount.Value >= dto.MaxAmount.Value)
            {
                return ValidationResult.Failure("Min Amount must be less than Max Amount");
            }

            return ValidationResult.Success();
        }

        protected override async Task<ValidationResult> ValidateUpdateAsync(int id, ApprovalStageUpdateDto dto, APPROVAL_STAGES entity)
        {
            var workflowId = dto.WorkflowId ?? entity.WorkflowId;
            
            // Validate StageName uniqueness if it's being changed
            if (!string.IsNullOrEmpty(dto.StageName) && dto.StageName != entity.StageName)
            {
                var stageNameExists = await Repository.AnyAsync(s => 
                    s.WorkflowId == workflowId && 
                    s.StageName == dto.StageName && 
                    s.Id != id && 
                    !s.IsDeleted);
                    
                if (stageNameExists)
                {
                    return ValidationResult.Failure("Stage name already exists in this workflow");
                }
            }

            // Validate StageOrder uniqueness if it's being changed
            if (dto.StageOrder.HasValue && dto.StageOrder.Value != entity.StageOrder)
            {
                var stageOrderExists = await Repository.AnyAsync(s => 
                    s.WorkflowId == workflowId && 
                    s.StageOrder == dto.StageOrder.Value && 
                    s.Id != id && 
                    !s.IsDeleted);
                    
                if (stageOrderExists)
                {
                    return ValidationResult.Failure("Stage order already exists in this workflow");
                }
            }

            // Validate MinAmount < MaxAmount when both are provided
            var minAmount = dto.MinAmount ?? entity.MinAmount;
            var maxAmount = dto.MaxAmount ?? entity.MaxAmount;
            if (minAmount.HasValue && maxAmount.HasValue && minAmount.Value >= maxAmount.Value)
            {
                return ValidationResult.Failure("Min Amount must be less than Max Amount");
            }

            return ValidationResult.Success();
        }

        public new async Task<ApiResponse> CreateAsync(ApprovalStageCreateDto dto)
        {
            var result = await base.CreateAsync(dto);
            return ConvertToApiResponse(result);
        }

        public new async Task<ApiResponse> UpdateAsync(int id, ApprovalStageUpdateDto dto)
        {
            var result = await base.UpdateAsync(id, dto);
            return ConvertToApiResponse(result);
        }

        public new async Task<ApiResponse> DeleteAsync(int id)
        {
            // Use Repository.SingleOrDefaultAsync directly (without IsDeleted filter) to get entity even if already deleted
            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id, asNoTracking: false);
            if (entity == null)
            {
                return new ApiResponse(404, "Approval stage not found");
            }

            // Check if already deleted
            if (entity.IsDeleted)
            {
                return new ApiResponse(200, "Approval stage is already deleted");
            }

            // Soft Delete - Always use soft delete
            entity.IsDeleted = true;
            entity.DeletedDate = DateTime.UtcNow;
            entity.IsActive = false;
            entity.UpdatedDate = DateTime.UtcNow;
            
            // Use repository Update method directly to ensure changes are tracked
            _unitOfWork.ApprovalStageRepository.Update(entity);
            await _unitOfWork.CompleteAsyn();
            
            return new ApiResponse(200, "Approval stage deleted successfully");
        }

        public new async Task<ApiResponse> ToggleActiveAsync(int id, bool isActive)
        {
            var result = await base.ToggleActiveAsync(id, isActive);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> GetFormFieldsByWorkflowIdAsync(int workflowId)
        {
            // Get workflow to find DocumentType
            var workflow = await _unitOfWork.ApprovalWorkflowRepository.GetByIdAsync(workflowId);
            if (workflow == null)
            {
                return new ApiResponse(404, "Approval workflow not found");
            }

            // Get DocumentType to find FormBuilderId
            var documentType = await _unitOfWork.DocumentTypeRepository.GetByIdAsync(workflow.DocumentTypeId);
            if (documentType == null || !documentType.FormBuilderId.HasValue)
            {
                return new ApiResponse(404, "Document type not found or has no associated form builder");
            }

            // Get form fields entities directly (to access FIELD_TYPES navigation property)
            var formFields = await _unitOfWork.FormFieldRepository.GetFieldsByFormIdAsync(documentType.FormBuilderId.Value);

            // Filter only numeric fields (int, decimal, number) and map to DTO
            var numericFields = formFields
                .Where(f => f.FIELD_TYPES != null && 
                           (f.FIELD_TYPES.DataType?.ToLower() == "int" || 
                            f.FIELD_TYPES.DataType?.ToLower() == "decimal" || 
                            f.FIELD_TYPES.DataType?.ToLower().Contains("number") == true))
                .Select(f => new
                {
                    id = f.Id,
                    fieldCode = f.FieldCode,
                    fieldName = f.FieldName,
                    dataType = f.FIELD_TYPES?.DataType
                })
                .ToList();

            return new ApiResponse(200, "Form fields retrieved successfully", numericFields);
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
