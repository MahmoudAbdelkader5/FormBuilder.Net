using FormBuilder.Core.DTOS.FormRules;
using System.Threading.Tasks;

namespace FormBuilder.Core.IServices.FormBuilder
{
    /// <summary>
    /// Service interface for CopyToDocument action execution
    /// </summary>
    public interface ICopyToDocumentService
    {
        /// <summary>
        /// Execute CopyToDocument action
        /// </summary>
        /// <param name="config">CopyToDocument configuration</param>
        /// <param name="sourceSubmissionId">Source submission ID (if not provided in config)</param>
        /// <param name="actionId">Action ID that triggered this copy (for audit)</param>
        /// <param name="ruleId">Rule ID that contains the action (for audit)</param>
        /// <param name="executedByUserId">User ID who triggered the action (for audit)</param>
        /// <returns>Result of the copy operation</returns>
        Task<CopyToDocumentResultDto> ExecuteCopyToDocumentAsync(
            CopyToDocumentActionDto config,
            int sourceSubmissionId,
            int? actionId = null,
            int? ruleId = null,
            string? executedByUserId = null);
    }
}

