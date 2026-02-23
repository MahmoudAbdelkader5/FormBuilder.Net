using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Application.DTOs.ApprovalWorkflow;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.API.Models;
using FormBuilder.Services.Services.Base;
using FormBuilder.Application.DTOS;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Services
{
    public class ApprovalDelegationService : BaseService<APPROVAL_DELEGATIONS, ApprovalDelegationDto, ApprovalDelegationCreateDto, ApprovalDelegationUpdateDto>, IApprovalDelegationService
    {
        public ApprovalDelegationService(IunitOfwork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override IBaseRepository<APPROVAL_DELEGATIONS> Repository => _unitOfWork.ApprovalDelegationRepository;

        public async Task<ApiResponse> GetAllAsync(string fromUserId = null)
        {
            IEnumerable<APPROVAL_DELEGATIONS> delegations;
            
            if (!string.IsNullOrWhiteSpace(fromUserId))
            {
                delegations = await _unitOfWork.ApprovalDelegationRepository.GetActiveDelegationsByFromUserIdAsync(fromUserId);
            }
            else
            {
                // Filter out soft-deleted items
                delegations = await Repository.GetAllAsync(d => !d.IsDeleted);
            }

            var dtos = _mapper.Map<IEnumerable<ApprovalDelegationDto>>(delegations);
            return new ApiResponse(200, "Success", dtos);
        }

        public async Task<ApiResponse> GetByIdAsync(int id)
        {
            var result = await base.GetByIdAsync(id);
            return ConvertToApiResponse(result);
        }

        public new async Task<ApiResponse> CreateAsync(ApprovalDelegationCreateDto dto)
        {
            // Validation: EndDate must be after StartDate
            if (dto.EndDate <= dto.StartDate)
            {
                return new ApiResponse(400, "EndDate must be after StartDate");
            }

            // Validation: ScopeType must be valid
            if (string.IsNullOrWhiteSpace(dto.ScopeType) || 
                !new[] { "Global", "Workflow", "Document" }.Contains(dto.ScopeType))
            {
                return new ApiResponse(400, "ScopeType must be 'Global', 'Workflow', or 'Document'");
            }

            // Validation: ScopeId must be provided for Workflow and Document scopes
            if (dto.ScopeType != "Global" && !dto.ScopeId.HasValue)
            {
                return new ApiResponse(400, $"ScopeId is required for {dto.ScopeType} scope type");
            }

            // Validation: ScopeId must be null for Global scope
            if (dto.ScopeType == "Global" && dto.ScopeId.HasValue)
            {
                return new ApiResponse(400, "ScopeId must be null for Global scope type");
            }

            // Validation: Check for overlapping delegations for the same scope
            // Only one active delegation per scope
            var now = DateTime.UtcNow;
            var existingDelegation = await _unitOfWork.ApprovalDelegationRepository.GetActiveDelegationByScopeAsync(
                dto.FromUserId,
                dto.ScopeType,
                dto.ScopeId,
                now);

            if (existingDelegation != null)
            {
                // Check if dates overlap
                if (dto.StartDate <= existingDelegation.EndDate && dto.EndDate >= existingDelegation.StartDate)
                {
                    return new ApiResponse(400, $"An active delegation already exists for this scope ({dto.ScopeType}) with overlapping date range");
                }
            }

            var result = await base.CreateAsync(dto);
            return ConvertToApiResponse(result);
        }

        public new async Task<ApiResponse> UpdateAsync(int id, ApprovalDelegationUpdateDto dto)
        {
            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id, asNoTracking: false);
            if (entity == null)
            {
                return new ApiResponse(404, "Delegation not found");
            }

            // Validation: If both dates are provided, EndDate must be after StartDate
            var startDate = dto.StartDate ?? entity.StartDate;
            var endDate = dto.EndDate ?? entity.EndDate;
            if (endDate <= startDate)
            {
                return new ApiResponse(400, "EndDate must be after StartDate");
            }

            // Validation: Check for overlapping delegations if ToUserId or dates are being changed
            var toUserId = dto.ToUserId ?? entity.ToUserId;
            var datesChanged = (dto.StartDate.HasValue && dto.StartDate.Value != entity.StartDate) ||
                              (dto.EndDate.HasValue && dto.EndDate.Value != entity.EndDate);
            
            if (toUserId != entity.ToUserId || datesChanged)
            {
                var overlappingDelegation = await Repository.GetAllAsync(d => 
                    d.Id != id &&
                    d.FromUserId == entity.FromUserId && 
                    d.ToUserId == toUserId && 
                    !d.IsDeleted &&
                    // Check if dates overlap
                    startDate <= d.EndDate && 
                    endDate >= d.StartDate);
                    
                if (overlappingDelegation.Any())
                {
                    return new ApiResponse(400, "A delegation already exists for the same users with overlapping date range");
                }
            }

            var result = await base.UpdateAsync(id, dto);
            return ConvertToApiResponse(result);
        }

        public new async Task<ApiResponse> DeleteAsync(int id)
        {
            // Use Repository.SingleOrDefaultAsync directly (without IsDeleted filter) to get entity even if already deleted
            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id, asNoTracking: false);
            if (entity == null)
            {
                return new ApiResponse(404, "Approval delegation not found");
            }

            // Check if already deleted
            if (entity.IsDeleted)
            {
                return new ApiResponse(200, "Approval delegation is already deleted");
            }

            // Soft Delete - Always use soft delete
            entity.IsDeleted = true;
            entity.DeletedDate = DateTime.UtcNow;
            entity.IsActive = false;
            entity.UpdatedDate = DateTime.UtcNow;
            
            // Use repository Update method directly to ensure changes are tracked
            _unitOfWork.ApprovalDelegationRepository.Update(entity);
            await _unitOfWork.CompleteAsyn();
            
            return new ApiResponse(200, "Approval delegation deleted successfully");
        }

        public async Task<ApiResponse> GetActiveDelegationsAsync(string userId)
        {
            var delegations = await _unitOfWork.ApprovalDelegationRepository.GetActiveDelegationsByFromUserIdAsync(userId);
            var dtos = _mapper.Map<IEnumerable<ApprovalDelegationDto>>(delegations);
            return new ApiResponse(200, "Success", dtos);
        }

        public async Task<ApiResponse> GetActiveDelegationsByToUserIdAsync(string toUserId)
        {
            if (string.IsNullOrWhiteSpace(toUserId))
            {
                return new ApiResponse(400, "ToUserId is required");
            }

            var delegations = await _unitOfWork.ApprovalDelegationRepository.GetActiveDelegationsByToUserIdAsync(toUserId);
            var dtos = _mapper.Map<IEnumerable<ApprovalDelegationDto>>(delegations);
            return new ApiResponse(200, "Success", dtos);
        }

        public async Task<ApiResponse> GetAllDelegationsByToUserIdAsync(string toUserId)
        {
            if (string.IsNullOrWhiteSpace(toUserId))
            {
                return new ApiResponse(400, "ToUserId is required");
            }

            // Get all delegations (including inactive and deleted) for debugging
            var allDelegations = await _unitOfWork.ApprovalDelegationRepository.GetAllDelegationsByToUserIdAsync(toUserId);
            var dtos = _mapper.Map<IEnumerable<ApprovalDelegationDto>>(allDelegations);
            return new ApiResponse(200, "Success", dtos);
        }

        /// <summary>
        /// Resolves the delegated approver based on priority: Document -> Workflow -> Global
        /// Returns the delegated user ID if found, null otherwise
        /// </summary>
        public async Task<string?> ResolveDelegatedApproverAsync(
            string originalApproverId, 
            int? workflowId, 
            int? submissionId)
        {
            if (string.IsNullOrWhiteSpace(originalApproverId))
            {
                return null;
            }

            var now = DateTime.UtcNow;

            // 1. Document-Level (Highest Priority)
            if (submissionId.HasValue)
            {
                var docDelegation = await _unitOfWork.ApprovalDelegationRepository.GetActiveDelegationByScopeAsync(
                    originalApproverId,
                    "Document",
                    submissionId.Value,
                    now);

                if (docDelegation != null)
                {
                    return docDelegation.ToUserId;
                }
            }

            // 2. Workflow-Level (Medium Priority)
            if (workflowId.HasValue)
            {
                var workflowDelegation = await _unitOfWork.ApprovalDelegationRepository.GetActiveDelegationByScopeAsync(
                    originalApproverId,
                    "Workflow",
                    workflowId.Value,
                    now);

                if (workflowDelegation != null)
                {
                    return workflowDelegation.ToUserId;
                }
            }

            // 3. Global (Lowest Priority)
            var globalDelegation = await _unitOfWork.ApprovalDelegationRepository.GetActiveDelegationByScopeAsync(
                originalApproverId,
                "Global",
                null,
                now);

            if (globalDelegation != null)
            {
                return globalDelegation.ToUserId;
            }

            // No delegation found → return original approver
            return null;
        }

        private ApiResponse ConvertToApiResponse<T>(ServiceResult<T> result)
        {
            if (result.Success)
                return new ApiResponse(result.StatusCode, "Success", result.Data);
            else
                return new ApiResponse(result.StatusCode, result.ErrorMessage);
        }
    }
}

