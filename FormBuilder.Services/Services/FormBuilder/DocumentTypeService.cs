using AutoMapper;
using FormBuilder.API.Models.DTOs;
using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.Common;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Domian.Entitys.FromBuilder;
using formBuilder.Domian.Interfaces;
using FormBuilder.Services.Services.Base;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FormBuilder.Services
{
    public class DocumentTypeService
        : BaseService<DOCUMENT_TYPES, DocumentTypeDto, CreateDocumentTypeDto, UpdateDocumentTypeDto>,
          IDocumentTypeService
    {
        private readonly IStringLocalizer<DocumentTypeService>? _localizer;

        public DocumentTypeService(IunitOfwork unitOfWork, IMapper mapper, IStringLocalizer<DocumentTypeService>? localizer = null)
            : base(unitOfWork, mapper, null)
        {
            _localizer = localizer;
        }

        protected override IBaseRepository<DOCUMENT_TYPES> Repository => _unitOfWork.DocumentTypeRepository;

        public async Task<ServiceResult<IEnumerable<DocumentTypeDto>>> GetAllAsync(Expression<Func<DOCUMENT_TYPES, bool>>? filter = null)
        {
            var list = await _unitOfWork.DocumentTypeRepository.GetAllAsync(filter, dt => dt.FORM_BUILDER, dt => dt.ParentMenu);
            return ServiceResult<IEnumerable<DocumentTypeDto>>.Ok(_mapper.Map<IEnumerable<DocumentTypeDto>>(list));
        }

        public async Task<ServiceResult<PagedResult<DocumentTypeDto>>> GetPagedAsync(int page = 1, int pageSize = 20, Expression<Func<DOCUMENT_TYPES, bool>>? filter = null)
        {
            // leverage base pagination on GetAll with includes
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 20;

            var query = Repository.GetAll();
            if (filter != null) query = query.Where(filter);
            query = query
                .Include(dt => dt.FORM_BUILDER)
                .Include(dt => dt.ParentMenu);

            var total = await query.CountAsync();
            var items = await query
                .OrderBy(dt => dt.MenuOrder)
                .ThenBy(dt => dt.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mapped = _mapper.Map<IEnumerable<DocumentTypeDto>>(items);
            return ServiceResult<PagedResult<DocumentTypeDto>>.Ok(new PagedResult<DocumentTypeDto>(mapped, total, page, pageSize));
        }

        public async Task<ServiceResult<DocumentTypeDto>> GetByIdAsync(int id, bool asNoTracking = false)
        {
            var entity = await _unitOfWork.DocumentTypeRepository.GetByIdAsync(id);
            if (entity == null) return ServiceResult<DocumentTypeDto>.NotFound();
            return ServiceResult<DocumentTypeDto>.Ok(_mapper.Map<DocumentTypeDto>(entity));
        }

        public async Task<ServiceResult<DocumentTypeDto>> GetByCodeAsync(string code, bool asNoTracking = false)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                var message = _localizer?["DocumentType_CodeRequired"] ?? "Code is required";
                return ServiceResult<DocumentTypeDto>.BadRequest(message);
            }

            var entity = await _unitOfWork.DocumentTypeRepository.GetByCodeAsync(code.Trim());
            if (entity == null) return ServiceResult<DocumentTypeDto>.NotFound();

            return ServiceResult<DocumentTypeDto>.Ok(_mapper.Map<DocumentTypeDto>(entity));
        }

        public async Task<ServiceResult<IEnumerable<DocumentTypeDto>>> GetByFormBuilderIdAsync(int formBuilderId, bool asNoTracking = false)
        {
            var list = await _unitOfWork.DocumentTypeRepository.GetByFormBuilderIdAsync(formBuilderId);
            return ServiceResult<IEnumerable<DocumentTypeDto>>.Ok(_mapper.Map<IEnumerable<DocumentTypeDto>>(list));
        }

        public async Task<ServiceResult<IEnumerable<DocumentTypeDto>>> GetActiveAsync()
        {
            var list = await _unitOfWork.DocumentTypeRepository.GetActiveAsync();
            return ServiceResult<IEnumerable<DocumentTypeDto>>.Ok(_mapper.Map<IEnumerable<DocumentTypeDto>>(list));
        }

        public async Task<ServiceResult<IEnumerable<DocumentTypeDto>>> GetByParentMenuIdAsync(int? parentMenuId, bool asNoTracking = false)
        {
            var list = await _unitOfWork.DocumentTypeRepository.GetByParentMenuIdAsync(parentMenuId);
            return ServiceResult<IEnumerable<DocumentTypeDto>>.Ok(_mapper.Map<IEnumerable<DocumentTypeDto>>(list));
        }

        public override async Task<ServiceResult<DocumentTypeDto>> CreateAsync(CreateDocumentTypeDto createDto)
        {
            // Normalize ParentMenuId: treat 0 as null (no parent)
            if (createDto.ParentMenuId.HasValue && createDto.ParentMenuId.Value <= 0)
            {
                createDto.ParentMenuId = null;
            }

            // Validate ApprovalWorkflowId if provided
            if (createDto.ApprovalWorkflowId.HasValue)
            {
                var workflow = await _unitOfWork.ApprovalWorkflowRepository.GetByIdAsync(createDto.ApprovalWorkflowId.Value);
                if (workflow == null)
                {
                    var message = _localizer?["DocumentType_WorkflowNotFound"] ?? "Approval workflow not found";
                    return ServiceResult<DocumentTypeDto>.BadRequest(message);
                }
                if (!workflow.IsActive)
                {
                    var message = _localizer?["DocumentType_WorkflowNotActive"] ?? "Approval workflow is not active";
                    return ServiceResult<DocumentTypeDto>.BadRequest(message);
                }
                // Note: Workflow can belong to any DocumentType, validation is done at assignment level
            }

            return await base.CreateAsync(createDto);
        }

        public override async Task<ServiceResult<DocumentTypeDto>> UpdateAsync(int id, UpdateDocumentTypeDto updateDto)
        {
            // Normalize ParentMenuId: treat 0 as null (no parent)
            if (updateDto.ParentMenuId.HasValue && updateDto.ParentMenuId.Value <= 0)
            {
                updateDto.ParentMenuId = null;
            }

            // Get the current entity to check if ParentMenuId is changing
            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id, asNoTracking: false);
            if (entity == null)
            {
                var message = _localizer?["Common_ResourceNotFound"] ?? "Resource not found";
                return ServiceResult<DocumentTypeDto>.NotFound(message);
            }

            // Validate ApprovalWorkflowId if provided
            if (updateDto.ApprovalWorkflowId.HasValue)
            {
                var workflow = await _unitOfWork.ApprovalWorkflowRepository.GetByIdAsync(updateDto.ApprovalWorkflowId.Value);
                if (workflow == null)
                {
                    var message = _localizer?["DocumentType_WorkflowNotFound"] ?? "Approval workflow not found";
                    return ServiceResult<DocumentTypeDto>.BadRequest(message);
                }
                if (!workflow.IsActive)
                {
                    var message = _localizer?["DocumentType_WorkflowNotActive"] ?? "Approval workflow is not active";
                    return ServiceResult<DocumentTypeDto>.BadRequest(message);
                }
                // Ensure workflow belongs to the same document type
                if (workflow.DocumentTypeId != entity.Id)
                {
                    var message = _localizer?["DocumentType_WorkflowMismatch"] ?? "Approval workflow does not belong to this document type";
                    return ServiceResult<DocumentTypeDto>.BadRequest(message);
                }
            }

            // Check if ParentMenuId is being changed (removed or changed)
            bool parentMenuIdChanged = updateDto.ParentMenuId.HasValue 
                ? (entity.ParentMenuId != updateDto.ParentMenuId.Value)
                : (entity.ParentMenuId.HasValue);

            // Get dbContext once for use throughout the method
            var dbContext = _unitOfWork.AppDbContext;

            // If removing parent relationship (setting to null) and this entity has children
            if (parentMenuIdChanged && (!updateDto.ParentMenuId.HasValue) && entity.ParentMenuId.HasValue)
            {
                // Use raw SQL to update children directly - this bypasses EF tracking issues
                // and works even if the constraint is still RESTRICT
                var childrenCount = await dbContext.Database.ExecuteSqlRawAsync(
                    "UPDATE DOCUMENT_TYPES SET ParentMenuId = NULL, UpdatedDate = GETUTCDATE() WHERE ParentMenuId = {0}",
                    id);

                if (childrenCount > 0)
                {
                    // Reload the entity to ensure we have the latest state
                    entity = await Repository.SingleOrDefaultAsync(e => e.Id == id, asNoTracking: false);
                    if (entity == null)
                    {
                        var message = _localizer?["Common_ResourceNotFound"] ?? "Resource not found";
                        return ServiceResult<DocumentTypeDto>.NotFound(message);
                    }
                }
            }

            // Prevent circular reference: check if trying to set parent to self or descendant
            if (updateDto.ParentMenuId.HasValue && updateDto.ParentMenuId.Value == id)
            {
                var message = _localizer?["DocumentType_CannotBeParentOfItself"] ?? "A document type cannot be its own parent";
                return ServiceResult<DocumentTypeDto>.BadRequest(message);
            }

            // Check for circular reference (parent cannot be a descendant)
            if (updateDto.ParentMenuId.HasValue)
            {
                var isDescendant = await IsDescendantAsync(id, updateDto.ParentMenuId.Value);
                if (isDescendant)
                {
                    var message = _localizer?["DocumentType_CircularReference"] ?? "Cannot set parent: would create circular reference";
                    return ServiceResult<DocumentTypeDto>.BadRequest(message);
                }
            }

            // Use raw SQL to update the entity directly - this bypasses EF tracking issues
            // and prevents conflicts with Foreign Key constraints
            var sqlParams = new List<object>();
            var updateFields = new List<string>();
            int paramIndex = 0;

            if (!string.IsNullOrWhiteSpace(updateDto.Name))
            {
                updateFields.Add($"Name = {{{paramIndex}}}");
                sqlParams.Add(updateDto.Name);
                paramIndex++;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Code))
            {
                updateFields.Add($"Code = {{{paramIndex}}}");
                sqlParams.Add(updateDto.Code);
                paramIndex++;
            }

            if (updateDto.FormBuilderId.HasValue)
            {
                updateFields.Add($"FormBuilderId = {{{paramIndex}}}");
                sqlParams.Add(updateDto.FormBuilderId.Value);
                paramIndex++;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.MenuCaption))
            {
                updateFields.Add($"MenuCaption = {{{paramIndex}}}");
                sqlParams.Add(updateDto.MenuCaption);
                paramIndex++;
            }

            if (updateDto.MenuOrder.HasValue)
            {
                updateFields.Add($"MenuOrder = {{{paramIndex}}}");
                sqlParams.Add(updateDto.MenuOrder.Value);
                paramIndex++;
            }

            // Handle ParentMenuId: use NULL in SQL if value is null
            if (updateDto.ParentMenuId.HasValue)
            {
                updateFields.Add($"ParentMenuId = {{{paramIndex}}}");
                sqlParams.Add(updateDto.ParentMenuId.Value);
                paramIndex++;
            }
            else
            {
                updateFields.Add("ParentMenuId = NULL");
            }

            if (updateDto.IsActive.HasValue)
            {
                updateFields.Add($"IsActive = {{{paramIndex}}}");
                sqlParams.Add(updateDto.IsActive.Value);
                paramIndex++;
            }

            if (updateDto.ApprovalWorkflowId.HasValue)
            {
                updateFields.Add($"ApprovalWorkflowId = {{{paramIndex}}}");
                sqlParams.Add(updateDto.ApprovalWorkflowId.Value);
                paramIndex++;
            }
            // If explicit null is supported (depends on DTO), handle it here. 
            // Assuming the DTO is nullable and null implies no change unless we want to allow unassigning.
            // If the user wants to unassign, they might need a separate flag or specific value like -1. 
            // For now, this only updates if a value is provided. 


            updateFields.Add("UpdatedDate = GETUTCDATE()");

            if (updateFields.Any())
            {
                // Add id as the last parameter for WHERE clause
                sqlParams.Add(id);
                var sql = $"UPDATE DOCUMENT_TYPES SET {string.Join(", ", updateFields)} WHERE Id = {{{paramIndex}}}";
                await dbContext.Database.ExecuteSqlRawAsync(sql, sqlParams.ToArray());
            }

            // Reload the entity to return updated data
            entity = await Repository.SingleOrDefaultAsync(e => e.Id == id, asNoTracking: true);
            if (entity == null)
            {
                var message = _localizer?["Common_ResourceNotFound"] ?? "Resource not found";
                return ServiceResult<DocumentTypeDto>.NotFound(message);
            }

            return ServiceResult<DocumentTypeDto>.Ok(_mapper.Map<DocumentTypeDto>(entity));
        }

        public override async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id, asNoTracking: false);
            if (entity == null)
            {
                var message = _localizer?["Common_ResourceNotFound"] ?? "Resource not found";
                return ServiceResult<bool>.NotFound(message);
            }

            // DocumentSeries is no longer linked directly to DocumentType.
            var documentSeriesCount = 0;
            
            // Check if there are form submissions using this document type
            var submissionsCount = await _unitOfWork.FormSubmissionsRepository.CountAsync(fs => fs.DocumentTypeId == id);

            // If there are related data, use soft delete instead of hard delete
            if (documentSeriesCount > 0 || submissionsCount > 0)
            {
                // Update children to remove parent relationship
                var dbContext = _unitOfWork.AppDbContext;
                await dbContext.Database.ExecuteSqlRawAsync(
                    "UPDATE DOCUMENT_TYPES SET ParentMenuId = NULL, UpdatedDate = GETUTCDATE() WHERE ParentMenuId = {0}",
                    id);

                entity.IsActive = false;
                entity.UpdatedDate = DateTime.UtcNow;
                Repository.Update(entity);
                await _unitOfWork.CompleteAsyn();
                
                return ServiceResult<bool>.Ok(true);
            }

            try
            {
            // Get all children (including inactive ones) that have this as parent
            var dbContext = _unitOfWork.AppDbContext;
            var childrenCount = await dbContext.Database.ExecuteSqlRawAsync(
                "UPDATE DOCUMENT_TYPES SET ParentMenuId = NULL, UpdatedDate = GETUTCDATE() WHERE ParentMenuId = {0}",
                id);

            if (childrenCount > 0)
            {
                // Verify children were updated
                var remainingChildren = await Repository.GetAll(dt => dt.ParentMenuId == id)
                    .CountAsync();
                
                if (remainingChildren > 0)
                {
                    var message = _localizer?["DocumentType_UnableToRemoveParentFromChildren"] 
                        ?? $"Unable to remove parent relationship from {remainingChildren} child document type(s). Please manually update them first.";
                    return ServiceResult<bool>.BadRequest(message);
                }
            }

                // Soft Delete
            entity.IsDeleted = true;
            entity.DeletedDate = DateTime.UtcNow;
            entity.IsActive = false;
            Repository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            return ServiceResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                // Check if it's a foreign key constraint violation
                if (ex.Message.Contains("REFERENCE constraint") || 
                    ex.Message.Contains("FK_DOCUMENT_SERIES") ||
                    ex.Message.Contains("FK_FORM_SUBMISSIONS") ||
                    ex.InnerException?.Message?.Contains("REFERENCE constraint") == true)
                {
                    // Try soft delete as fallback
                    try
                    {
                        await _unitOfWork.AppDbContext.Database.ExecuteSqlRawAsync(
                            "UPDATE DOCUMENT_TYPES SET ParentMenuId = NULL, UpdatedDate = GETUTCDATE() WHERE ParentMenuId = {0}",
                            id);

                        entity.IsActive = false;
                        entity.UpdatedDate = DateTime.UtcNow;
                        Repository.Update(entity);
                        await _unitOfWork.CompleteAsyn();
                        
                        return ServiceResult<bool>.Ok(true);
                    }
                    catch
                    {
                        var finalSeriesCount = 0;
                        var finalSubmissionsCount = await _unitOfWork.FormSubmissionsRepository.CountAsync(fs => fs.DocumentTypeId == id);
                        
                        var errorMessage = _localizer?["DocumentType_CannotDelete"] 
                            ?? $"Cannot delete document type: There are {finalSeriesCount} document series and {finalSubmissionsCount} submission(s) associated with it. The document type has been deactivated instead.";
                        return ServiceResult<bool>.BadRequest(errorMessage);
                    }
                }
                
                var message = _localizer?["DocumentType_DeleteError"] ?? $"Error deleting document type: {ex.Message}";
                return ServiceResult<bool>.Error(message);
            }
        }

        /// <summary>
        /// Check if targetId is a descendant of sourceId (to prevent circular references)
        /// </summary>
        private async Task<bool> IsDescendantAsync(int sourceId, int targetId)
        {
            var current = await Repository.SingleOrDefaultAsync(dt => dt.Id == targetId);
            if (current == null) return false;

            // Traverse up the parent chain
            while (current.ParentMenuId.HasValue)
            {
                if (current.ParentMenuId.Value == sourceId)
                {
                    return true; // Found circular reference
                }
                current = await Repository.SingleOrDefaultAsync(dt => dt.Id == current.ParentMenuId.Value);
                if (current == null) break;
            }

            return false;
        }

        public async Task<ServiceResult<bool>> ToggleActiveAsync(int id, bool isActive)
        {
            var entity = await Repository.SingleOrDefaultAsync(dt => dt.Id == id);
            if (entity == null) return ServiceResult<bool>.NotFound();

            entity.IsActive = isActive;
            entity.UpdatedDate = DateTime.UtcNow;
            Repository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            return ServiceResult<bool>.Ok(true);
        }

        public async Task<ServiceResult<bool>> ExistsAsync(int id)
        {
            var exists = await Repository.AnyAsync(dt => dt.Id == id);
            return ServiceResult<bool>.Ok(exists);
        }

        public async Task<ServiceResult<bool>> CodeExistsAsync(string code, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                var message = _localizer?["DocumentType_CodeRequired"] ?? "Code is required";
                return ServiceResult<bool>.BadRequest(message);
            }

            var exists = await _unitOfWork.DocumentTypeRepository.CodeExistsAsync(code.Trim(), excludeId);
            return ServiceResult<bool>.Ok(exists);
        }

        protected override async Task<ValidationResult> ValidateCreateAsync(CreateDocumentTypeDto dto)
        {
            if (dto == null)
            {
                var message = _localizer?["Common_PayloadRequired"] ?? "Payload is required";
                return ValidationResult.Failure(message);
            }

            var exists = await _unitOfWork.DocumentTypeRepository.CodeExistsAsync(dto.Code);
            if (exists)
            {
                var message = _localizer?["DocumentType_CodeExists", dto.Code] ?? $"Document type code '{dto.Code}' already exists.";
                return ValidationResult.Failure(message);
            }

            if (dto.FormBuilderId.HasValue)
            {
                var formBuilderExists = await _unitOfWork.FormBuilderRepository.AnyAsync(e => e.Id == dto.FormBuilderId.Value);
                if (!formBuilderExists)
                {
                    var message = _localizer?["DocumentType_InvalidFormBuilderId"] ?? "Invalid form builder ID";
                    return ValidationResult.Failure(message);
                }
            }

            if (dto.ParentMenuId.HasValue && dto.ParentMenuId.Value > 0)
            {
                var parentExists = await _unitOfWork.DocumentTypeRepository.AnyAsync(e => e.Id == dto.ParentMenuId.Value);
                if (!parentExists) return ValidationResult.Failure("Invalid parent menu ID");
            }

            return ValidationResult.Success();
        }

        protected override async Task<ValidationResult> ValidateUpdateAsync(int id, UpdateDocumentTypeDto dto, DOCUMENT_TYPES entity)
        {
            if (dto == null) return ValidationResult.Failure("Payload is required");

            if (!string.IsNullOrWhiteSpace(dto.Code) && !string.Equals(dto.Code, entity.Code, StringComparison.OrdinalIgnoreCase))
            {
                var exists = await _unitOfWork.DocumentTypeRepository.CodeExistsAsync(dto.Code, id);
                if (exists) return ValidationResult.Failure($"Document type code '{dto.Code}' already exists.");
            }

            if (dto.FormBuilderId.HasValue)
            {
                var formBuilderExists = await _unitOfWork.FormBuilderRepository.AnyAsync(e => e.Id == dto.FormBuilderId.Value);
                if (!formBuilderExists) return ValidationResult.Failure("Invalid form builder ID");
            }

            if (dto.ParentMenuId.HasValue && dto.ParentMenuId.Value > 0)
            {
                var parentExists = await _unitOfWork.DocumentTypeRepository.AnyAsync(e => e.Id == dto.ParentMenuId.Value);
                if (!parentExists) return ValidationResult.Failure("Invalid parent menu ID");
            }

            return ValidationResult.Success();
        }
    }
}
