using FormBuilder.API.Models;
using FormBuilder.Core.DTOS.FormBuilder;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    /// <summary>
    /// Service for managing Document Type and Document Series within Form Builder context
    /// </summary>
    public interface IDocumentSettingsService
    {
        /// <summary>
        /// Get document settings for a specific form builder
        /// </summary>
        Task<ApiResponse> GetByFormBuilderIdAsync(int formBuilderId);

        /// <summary>
        /// Save document settings (Document Type + Series) for a form builder
        /// </summary>
        Task<ApiResponse> SaveDocumentSettingsAsync(SaveDocumentSettingsDto saveDto);

        /// <summary>
        /// Get document type and series for creating a new submission
        /// Returns the document type and default series for the form builder
        /// </summary>
        Task<ApiResponse> GetDocumentInfoForSubmissionAsync(int formBuilderId, int? projectId = null);
    }
}

