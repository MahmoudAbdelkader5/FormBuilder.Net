using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Application.DTOS;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    /// <summary>
    /// Service for managing Document Type and Document Series within Form Builder context
    /// </summary>
    public interface IFormBuilderDocumentSettingsService
    {
        /// <summary>
        /// Get Document Settings (Document Type + Series) for a specific Form Builder
        /// </summary>
        Task<ServiceResult<DocumentSettingsDto>> GetDocumentSettingsAsync(int formBuilderId);

        /// <summary>
        /// Save Document Settings (Document Type + Series) for a Form Builder
        /// Creates or updates Document Type and manages Document Series
        /// </summary>
        Task<ServiceResult<DocumentSettingsDto>> SaveDocumentSettingsAsync(SaveDocumentSettingsDto dto);

        /// <summary>
        /// Delete Document Settings for a Form Builder (removes Document Type and all Series)
        /// </summary>
        Task<ServiceResult<bool>> DeleteDocumentSettingsAsync(int formBuilderId);

        /// <summary>
        /// Auto-configure default Document Settings for a Form Builder
        /// Creates a default Document Type and Series if they don't exist
        /// </summary>
        Task<ServiceResult<DocumentSettingsDto>> AutoConfigureDefaultsAsync(int formBuilderId, int projectId, string? documentCode = null, string? seriesCode = null);
    }
}

