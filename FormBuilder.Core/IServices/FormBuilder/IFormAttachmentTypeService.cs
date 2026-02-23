using FormBuilder.API.Models;
using FormBuilder.API.Models;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.API.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IFormAttachmentTypeService
    {
        // ================================
        // GET OPERATIONS
        // ================================

        /// <summary>
        /// Get all form attachment types with FormBuilder and AttachmentType includes
        /// </summary>
        Task<ApiResponse> GetAllAsync();

        /// <summary>
        /// Get form attachment type by ID with FormBuilder and AttachmentType includes
        /// </summary>
        Task<ApiResponse> GetByIdAsync(int id);

        /// <summary>
        /// Get form attachment types by FormBuilder ID with AttachmentType includes
        /// </summary>
        Task<ApiResponse> GetByFormBuilderIdAsync(int formBuilderId);

        /// <summary>
        /// Get form attachment types by AttachmentType ID with FormBuilder includes
        /// </summary>
        Task<ApiResponse> GetByAttachmentTypeIdAsync(int attachmentTypeId);

        /// <summary>
        /// Get all active form attachment types with FormBuilder and AttachmentType includes
        /// </summary>
        Task<ApiResponse> GetActiveAsync();

        /// <summary>
        /// Get active form attachment types by FormBuilder ID with AttachmentType includes
        /// </summary>
        Task<ApiResponse> GetActiveByFormBuilderIdAsync(int formBuilderId);

        /// <summary>
        /// Get mandatory form attachment types by FormBuilder ID with AttachmentType includes
        /// </summary>
        Task<ApiResponse> GetMandatoryByFormBuilderIdAsync(int formBuilderId);

        // ================================
        // CREATE OPERATIONS
        // ================================

        /// <summary>
        /// Create a new form attachment type association
        /// </summary>
        Task<ApiResponse> CreateAsync(CreateFormAttachmentTypeDto createDto);

        /// <summary>
        /// Create multiple form attachment type associations in bulk
        /// </summary>
        Task<ApiResponse> CreateBulkAsync(List<CreateFormAttachmentTypeDto> createDtos);

        // ================================
        // UPDATE OPERATIONS
        // ================================

        /// <summary>
        /// Update an existing form attachment type association
        /// </summary>
        Task<ApiResponse> UpdateAsync(int id, UpdateFormAttachmentTypeDto updateDto);

        /// <summary>
        /// Toggle the active status of a form attachment type
        /// </summary>
        Task<ApiResponse> ToggleActiveAsync(int id, bool isActive);

        /// <summary>
        /// Toggle the mandatory status of a form attachment type
        /// </summary>
        Task<ApiResponse> ToggleMandatoryAsync(int id, bool isMandatory);

        // ================================
        // DELETE OPERATIONS
        // ================================

        /// <summary>
        /// Delete a form attachment type association by ID
        /// </summary>
        Task<ApiResponse> DeleteAsync(int id);

        /// <summary>
        /// Delete all form attachment type associations by FormBuilder ID
        /// </summary>
        Task<ApiResponse> DeleteByFormBuilderIdAsync(int formBuilderId);

        // ================================
        // VALIDATION & CHECK OPERATIONS
        // ================================

        /// <summary>
        /// Check if a form attachment type exists by ID
        /// </summary>
        Task<ApiResponse> ExistsAsync(int id);

        /// <summary>
        /// Check if a form attachment type is active
        /// </summary>
        Task<ApiResponse> IsActiveAsync(int id);

        /// <summary>
        /// Check if a form builder has any mandatory attachments
        /// </summary>
        Task<ApiResponse> HasMandatoryAttachmentsAsync(int formBuilderId);
    }
}