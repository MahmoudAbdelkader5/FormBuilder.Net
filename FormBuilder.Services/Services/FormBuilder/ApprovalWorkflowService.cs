using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Application.DTOs.ApprovalWorkflow;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.API.Models;
using FormBuilder.Services.Services.Base;
using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.Common;
using Microsoft.Extensions.Localization;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FormBuilder.Services
{
    public class ApprovalWorkflowService : BaseService<APPROVAL_WORKFLOWS, ApprovalWorkflowDto, ApprovalWorkflowCreateDto, ApprovalWorkflowUpdateDto>, IApprovalWorkflowService
    {
        private readonly IunitOfwork _unitOfWork;
        private readonly IStringLocalizer<ApprovalWorkflowService>? _localizer;

        public ApprovalWorkflowService(IunitOfwork unitOfWork, IMapper mapper, IStringLocalizer<ApprovalWorkflowService>? localizer = null) 
            : base(unitOfWork, mapper, null)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _localizer = localizer;
        }

        protected override IBaseRepository<APPROVAL_WORKFLOWS> Repository => _unitOfWork.ApprovalWorkflowRepository;

        public override async Task<ServiceResult<IEnumerable<ApprovalWorkflowDto>>> GetAllAsync(Expression<Func<APPROVAL_WORKFLOWS, bool>>? filter = null)
        {
            var items = await _unitOfWork.ApprovalWorkflowRepository.GetAllAsync(filter,
                x => x.DOCUMENT_TYPES,
                x => x.APPROVAL_STAGES);

            var dtos = _mapper.Map<IEnumerable<ApprovalWorkflowDto>>(items);
            return ServiceResult<IEnumerable<ApprovalWorkflowDto>>.Ok(dtos);
        }

        public async Task<ApiResponse> GetAllAsync()
        {
            var result = await base.GetAllAsync();
            return ConvertToApiResponse(result);
        }

        public override async Task<ServiceResult<ApprovalWorkflowDto>> GetByIdAsync(int id, bool asNoTracking = false)
        {
            var entities = await _unitOfWork.ApprovalWorkflowRepository.GetAllAsync(x => x.Id == id,
                x => x.DOCUMENT_TYPES,
                x => x.APPROVAL_STAGES);

            var entity = entities.FirstOrDefault();
            if (entity == null)
                return ServiceResult<ApprovalWorkflowDto>.NotFound();

            var dto = _mapper.Map<ApprovalWorkflowDto>(entity);
            return ServiceResult<ApprovalWorkflowDto>.Ok(dto);
        }

        public async Task<ApiResponse> GetByIdAsync(int id)
        {
            var result = await base.GetByIdAsync(id);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> CreateAsync(ApprovalWorkflowCreateDto dto)
        {
            var result = await base.CreateAsync(dto);

            // Keep DOCUMENT_TYPES.ApprovalWorkflowId in sync with the created workflow.
            // Runtime inbox logic may rely on this reverse link.
            if (result.Success && result.Data != null)
            {
                try
                {
                    var workflowId = result.Data.Id;
                    var documentTypeId = dto.DocumentTypeId;

                    var documentType = await _unitOfWork.DocumentTypeRepository.GetByIdAsync(documentTypeId);
                    if (documentType != null)
                    {
                        documentType.ApprovalWorkflowId = workflowId;
                        documentType.UpdatedDate = DateTime.UtcNow;
                        _unitOfWork.DocumentTypeRepository.Update(documentType);
                        await _unitOfWork.CompleteAsyn();
                    }
                }
                catch
                {
                    // Best-effort sync. Do not fail workflow creation if reverse link update fails.
                }
            }
            
            // إنشاء Stage افتراضي تلقائياً عند إنشاء Workflow جديد
            if (dto.CreateDefaultStage && result.Success && result.Data != null)
            {
                var workflowDto = result.Data;
                var defaultStage = new APPROVAL_STAGES
                {
                    WorkflowId = workflowDto.Id,
                    StageName = "المرحلة الأولى", // Default Stage
                    StageOrder = 1,
                    IsFinalStage = true,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                 _unitOfWork.ApprovalStageRepository.Add(defaultStage);
                await _unitOfWork.CompleteAsyn();
            }
            
            return ConvertToApiResponse(result);
        }

        protected override async Task<ValidationResult> ValidateCreateAsync(ApprovalWorkflowCreateDto dto)
        {
            // Block auto-created "default workflow" patterns coming from UI or integrations.
            // User requirement: do not create "Default Workflow for ..." records at all.
            if (!string.IsNullOrWhiteSpace(dto?.Name) &&
                dto.Name.Trim().StartsWith("Default Workflow for", StringComparison.OrdinalIgnoreCase))
            {
                return ValidationResult.Failure("Creating 'Default Workflow for ...' is not allowed.");
            }

            var nameExists = await _unitOfWork.ApprovalWorkflowRepository.AnyAsync(x => x.Name == dto.Name);
            if (nameExists)
            {
                var message = _localizer?["ApprovalWorkflow_NameExists"] ?? "Workflow name already exists";
                return ValidationResult.Failure(message);
            }

            // Validate DocumentTypeId exists
            var documentTypeExists = await _unitOfWork.DocumentTypeRepository.AnyAsync(dt => dt.Id == dto.DocumentTypeId);
            if (!documentTypeExists)
            {
                var message = _localizer?["ApprovalWorkflow_InvalidDocumentTypeId"] ?? "Invalid document type ID. The specified document type does not exist.";
                return ValidationResult.Failure(message);
            }

            return ValidationResult.Success();
        }

        public override async Task<ServiceResult<ApprovalWorkflowDto>> UpdateAsync(int id, ApprovalWorkflowUpdateDto dto)
        {
            return await UpdateAsyncInternal(id, dto);
        }

        // Explicit interface implementation for ApiResponse - calls the ServiceResult override via base class
        async Task<ApiResponse> IApprovalWorkflowService.UpdateAsync(int id, ApprovalWorkflowUpdateDto dto)
        {
            // Call the override by invoking it through the base class method signature
            // We use a helper to avoid signature conflicts
            var result = await UpdateAsyncInternal(id, dto);
            return ConvertToApiResponse(result);
        }

        // Helper method to avoid signature conflict - contains the actual update logic
        private async Task<ServiceResult<ApprovalWorkflowDto>> UpdateAsyncInternal(int id, ApprovalWorkflowUpdateDto dto)
        {
            if (dto == null)
            {
                var message = _localizer?["Common_PayloadRequired"] ?? "Payload is required";
                return ServiceResult<ApprovalWorkflowDto>.BadRequest(message);
            }

            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id, asNoTracking: false);
            if (entity == null)
            {
                var message = _localizer?["Common_ResourceNotFound"] ?? "Resource not found";
                return ServiceResult<ApprovalWorkflowDto>.NotFound(message);
            }

            var validation = await ValidateUpdateAsync(id, dto, entity);
            if (!validation.IsValid)
            {
                var message = validation.ErrorMessage ?? (_localizer?["Common_ValidationFailed"] ?? "Validation failed");
                return ServiceResult<ApprovalWorkflowDto>.BadRequest(message);
            }

            // Store original DocumentTypeId before mapping
            var originalDocumentTypeId = entity.DocumentTypeId;

            _mapper.Map(dto, entity);
            entity.UpdatedDate = DateTime.UtcNow;

            // Final safeguard: Ensure DocumentTypeId is valid after mapping
            if (entity.DocumentTypeId <= 0)
            {
                // Restore original value if invalid
                entity.DocumentTypeId = originalDocumentTypeId;
                var message = _localizer?["ApprovalWorkflow_InvalidDocumentTypeId"] ?? "Invalid document type ID. Document type ID must be greater than zero.";
                return ServiceResult<ApprovalWorkflowDto>.BadRequest(message);
            }

            // Verify DocumentTypeId exists if it was changed
            if (entity.DocumentTypeId != originalDocumentTypeId)
            {
                var documentTypeExists = await _unitOfWork.DocumentTypeRepository.AnyAsync(dt => dt.Id == entity.DocumentTypeId);
                if (!documentTypeExists)
                {
                    // Restore original value if new value doesn't exist
                    entity.DocumentTypeId = originalDocumentTypeId;
                    var message = _localizer?["ApprovalWorkflow_InvalidDocumentTypeId"] ?? "Invalid document type ID. The specified document type does not exist.";
                    return ServiceResult<ApprovalWorkflowDto>.BadRequest(message);
                }
            }

            Repository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            return ServiceResult<ApprovalWorkflowDto>.Ok(_mapper.Map<ApprovalWorkflowDto>(entity));
        }

        protected override async Task<ValidationResult> ValidateUpdateAsync(int id, ApprovalWorkflowUpdateDto dto, APPROVAL_WORKFLOWS entity)
        {
            if (!string.IsNullOrEmpty(dto.Name) && dto.Name != entity.Name)
            {
                var exists = await _unitOfWork.ApprovalWorkflowRepository.AnyAsync(x => x.Name == dto.Name && x.Id != id);
                if (exists)
                {
                    var message = _localizer?["ApprovalWorkflow_NameExists"] ?? "Workflow name already exists";
                    return ValidationResult.Failure(message);
                }
            }

            // Validate DocumentTypeId if it's being updated
            if (dto.DocumentTypeId.HasValue)
            {
                // Reject invalid DocumentTypeId values (0 or negative)
                if (dto.DocumentTypeId.Value <= 0)
                {
                    var message = _localizer?["ApprovalWorkflow_InvalidDocumentTypeId"] ?? "Invalid document type ID. Document type ID must be greater than zero.";
                    return ValidationResult.Failure(message);
                }

                // Only validate existence if the value is actually changing
                if (dto.DocumentTypeId.Value != entity.DocumentTypeId)
                {
                    var documentTypeExists = await _unitOfWork.DocumentTypeRepository.AnyAsync(dt => dt.Id == dto.DocumentTypeId.Value);
                    if (!documentTypeExists)
                    {
                        var message = _localizer?["ApprovalWorkflow_InvalidDocumentTypeId"] ?? "Invalid document type ID. The specified document type does not exist.";
                        return ValidationResult.Failure(message);
                    }
                }
            }

            return ValidationResult.Success();
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            // Use Repository.SingleOrDefaultAsync directly (without IsDeleted filter) to get entity even if already deleted
            // This is needed because GetByIdAsync excludes deleted records
            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id, asNoTracking: false);
            
            if (entity == null)
            {
                var message = _localizer?["ApprovalWorkflow_NotFound"] ?? "Approval workflow not found";
                return new ApiResponse(404, message);
            }

            // Check if already deleted
            if (entity.IsDeleted)
            {
                return new ApiResponse(200, "Approval workflow is already deleted");
            }

            // Soft Delete - Always use soft delete
            entity.IsDeleted = true;
            entity.DeletedDate = DateTime.UtcNow;
            entity.IsActive = false;
            entity.UpdatedDate = DateTime.UtcNow;
            
            // Use repository Update method directly to ensure changes are tracked
            _unitOfWork.ApprovalWorkflowRepository.Update(entity);
            await _unitOfWork.CompleteAsyn();
            
            return new ApiResponse(200, "Approval workflow deleted successfully");
        }

        public async Task<ApiResponse> ToggleActiveAsync(int id, bool isActive)
        {
            var result = await base.ToggleActiveAsync(id, isActive);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> GetByNameAsync(string name)
        {
            var entity = await _unitOfWork.ApprovalWorkflowRepository.GetByNameAsync(name);
            if (entity == null)
            {
                var message = _localizer?["ApprovalWorkflow_NotFound"] ?? "Workflow not found";
                return new ApiResponse(404, message);
            }

            var dto = _mapper.Map<ApprovalWorkflowDto>(entity);
            return new ApiResponse(200, "Workflow retrieved", dto);
        }

        public async Task<ApiResponse> GetActiveAsync()
        {
            var result = await base.GetActiveAsync();
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> ExistsAsync(int id)
        {
            var exists = await _unitOfWork.ApprovalWorkflowRepository.AnyAsync(x => x.Id == id);
            return new ApiResponse(200, "Existence checked", exists);
        }

        public async Task<ApiResponse> NameExistsAsync(string name, int? excludeId = null)
        {
            var exists = await _unitOfWork.ApprovalWorkflowRepository.NameExistsAsync(name, excludeId);
            return new ApiResponse(200, "Name existence checked", exists);
        }

        /// <summary>
        /// يتأكد من وجود Stage افتراضي للـ Workflow - يُنشئ واحد إذا لم يكن موجود
        /// </summary>
        public async Task<ApiResponse> EnsureDefaultStageAsync(int workflowId)
        {
            var workflow = await _unitOfWork.ApprovalWorkflowRepository.GetByIdAsync(workflowId);
            if (workflow == null)
            {
                return new ApiResponse(404, "Workflow not found");
            }

            // التحقق من وجود stages
            var existingStages = await _unitOfWork.ApprovalStageRepository.GetAllAsync(s => s.WorkflowId == workflowId);
            if (existingStages.Any())
            {
                return new ApiResponse(200, "Workflow already has stages", new { StagesCount = existingStages.Count() });
            }

            // إنشاء Stage افتراضي
            var defaultStage = new APPROVAL_STAGES
            {
                WorkflowId = workflowId,
                StageName = "المرحلة الأولى",
                StageOrder = 1,
                IsFinalStage = true,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            _unitOfWork.ApprovalStageRepository.Add(defaultStage);
            await _unitOfWork.CompleteAsyn();

            return new ApiResponse(200, "Default stage created successfully", new { StageId = defaultStage.Id, StageName = defaultStage.StageName });
        }

        /// <summary>
        /// يُصلح جميع الـ Workflows التي ليس لها stages
        /// </summary>
        public async Task<ApiResponse> FixAllWorkflowsWithoutStagesAsync()
        {
            var allWorkflows = await _unitOfWork.ApprovalWorkflowRepository.GetAllAsync();
            var fixedCount = 0;

            foreach (var workflow in allWorkflows)
            {
                var stages = await _unitOfWork.ApprovalStageRepository.GetAllAsync(s => s.WorkflowId == workflow.Id);
                if (!stages.Any())
                {
                    var defaultStage = new APPROVAL_STAGES
                    {
                        WorkflowId = workflow.Id,
                        StageName = "المرحلة الأولى",
                        StageOrder = 1,
                        IsFinalStage = true,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow
                    };

                    _unitOfWork.ApprovalStageRepository.Add(defaultStage);
                    fixedCount++;
                }
            }

            if (fixedCount > 0)
            {
                await _unitOfWork.CompleteAsyn();
            }

            return new ApiResponse(200, $"Fixed {fixedCount} workflow(s) without stages", new { FixedCount = fixedCount });
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
