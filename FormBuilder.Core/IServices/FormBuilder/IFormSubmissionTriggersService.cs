using FormBuilder.Domian.Entitys.FormBuilder;
using System.Threading.Tasks;

namespace FormBuilder.Core.IServices.FormBuilder
{
    /// <summary>
    /// Service for handling event-based triggers in form submission workflow
    /// All triggers execute immediately after database state changes within the same transaction
    /// </summary>
    public interface IFormSubmissionTriggersService
    {
        /// <summary>
        /// FormSubmitted Trigger: Executes when a new submission is created with Status = "Submitted"
        /// Actions: Generate Document Number, Initialize workflow, Insert history, Send email, Create notification
        /// </summary>
        Task ExecuteFormSubmittedTriggerAsync(FORM_SUBMISSIONS submission);

        /// <summary>
        /// ApprovalRequired Trigger: Executes when StageId is assigned/changed and Status = "Pending" or "Submitted"
        /// Actions: Resolve approvers, Apply delegation, Send approval emails, Create notifications
        /// </summary>
        Task ExecuteApprovalRequiredTriggerAsync(FORM_SUBMISSIONS submission, int stageId);

        /// <summary>
        /// ApprovalApproved Trigger: Executes when ActionType = "Approved" in approval history
        /// Actions: Log history, Check final stage, Move to next stage or complete, Send notification
        /// </summary>
        Task ExecuteApprovalApprovedTriggerAsync(DOCUMENT_APPROVAL_HISTORY history, FORM_SUBMISSIONS submission);

        /// <summary>
        /// ApprovalRejected Trigger: Executes when ActionType = "Rejected" in approval history
        /// Actions: Update status to Rejected, Lock document, Send rejection email
        /// </summary>
        Task ExecuteApprovalRejectedTriggerAsync(DOCUMENT_APPROVAL_HISTORY history, FORM_SUBMISSIONS submission);

        /// <summary>
        /// ApprovalReturned Trigger: Executes when ActionType = "Returned" in approval history
        /// Actions: Update status to Returned, Unlock form for editing, Send return email
        /// </summary>
        Task ExecuteApprovalReturnedTriggerAsync(DOCUMENT_APPROVAL_HISTORY history, FORM_SUBMISSIONS submission);
    }
}



















