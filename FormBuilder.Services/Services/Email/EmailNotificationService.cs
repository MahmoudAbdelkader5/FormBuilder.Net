using FormBuilder.Core.IServices;
using FormBuilder.Domain.Interfaces;
using FormBuilder.Domian.Entitys.froms;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domian.Entitys.FromBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using formBuilder.Domian.Interfaces;
using FormBuilder.Core.Models;
using System.Text.RegularExpressions;

namespace FormBuilder.Services.Services.Email
{
    /// <summary>
    /// Service for sending email notifications related to form submissions and approvals
    /// </summary>
    public class EmailNotificationService
    {
        private readonly IEmailService _emailService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IunitOfwork _unitOfWork;
        private readonly AkhmanageItContext _identityContext;
        private readonly ILogger<EmailNotificationService> _logger;

        public EmailNotificationService(
            IEmailService emailService,
            IEmailTemplateService emailTemplateService,
            IunitOfwork unitOfWork,
            AkhmanageItContext identityContext,
            ILogger<EmailNotificationService> logger)
        {
            _emailService = emailService;
            _emailTemplateService = emailTemplateService;
            _unitOfWork = unitOfWork;
            _identityContext = identityContext;
            _logger = logger;
        }

        /// <summary>
        /// Sends submission confirmation email to specific recipients from ALERT_RULES
        /// </summary>
        public async Task SendSubmissionConfirmationToRecipientsAsync(int submissionId, IEnumerable<string> recipientUserIds, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger?.LogInformation("Starting SendSubmissionConfirmationToRecipientsAsync for submission {SubmissionId} with {Count} recipient user IDs: {UserIds}",
                    submissionId, recipientUserIds?.Count() ?? 0, string.Join(", ", recipientUserIds ?? Enumerable.Empty<string>()));

                var submission = await _unitOfWork.FormSubmissionsRepository.GetByIdAsync(submissionId);
                if (submission == null)
                {
                    _logger?.LogWarning("Submission {SubmissionId} not found for email notification", submissionId);
                    return;
                }

                _logger?.LogInformation("Submission found: DocumentNumber={DocumentNumber}, DocumentTypeId={DocumentTypeId}, SubmittedByUserId={SubmittedByUserId}",
                    submission.DocumentNumber, submission.DocumentTypeId, submission.SubmittedByUserId);

                var documentType = await _unitOfWork.DocumentTypeRepository.GetByIdAsync(submission.DocumentTypeId);
                var series = await _unitOfWork.DocumentSeriesRepository.GetByIdAsync(submission.SeriesId);
                var projectName = series?.PROJECTS?.Name ?? string.Empty;

                var templateData = new Dictionary<string, string>
                {
                    { "DocumentNumber", submission.DocumentNumber ?? string.Empty },
                    { "SubmissionId", submission.Id.ToString() },
                    { "DocumentType", documentType?.Name ?? string.Empty },
                    { "ProjectName", projectName },
                    { "SubmittedBy", submission.SubmittedByUserId },
                    { "SystemUrl", "" } // Will be added by template service
                };

                _logger?.LogInformation("Resolving email template for DocumentTypeId={DocumentTypeId}, TriggerType=FormSubmitted, TemplateCode=SubmissionConfirmation",
                    submission.DocumentTypeId);

                var (subject, body, smtpConfigId) = await ResolveProcessedTemplateAsync(
                    submission.DocumentTypeId,
                    triggerType: "FormSubmitted",
                    templateCode: "SubmissionConfirmation",
                    templateData,
                    cancellationToken);

                _logger?.LogInformation("Template resolved. Subject={Subject}, SmtpConfigId={SmtpConfigId}, BodyLength={BodyLength}",
                    subject, smtpConfigId?.ToString() ?? "null", body?.Length ?? 0);

                // Get emails for all recipient user IDs
                var recipientEmails = new List<string>();
                foreach (var userId in recipientUserIds ?? Enumerable.Empty<string>())
                {
                    _logger?.LogInformation("Resolving email for userId: {UserId}", userId);
                    var email = await ResolveRecipientEmailAsync(submissionId, userId, cancellationToken);
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        recipientEmails.Add(email);
                        _logger?.LogInformation("Added email {Email} for userId {UserId}", email, userId);
                    }
                    else
                    {
                        _logger?.LogWarning("No email found for userId {UserId}", userId);
                    }
                }

                if (recipientEmails.Any())
                {
                    _logger?.LogInformation("Sending email to {Count} recipients: {Emails}. Subject: {Subject}, SmtpConfigId: {SmtpConfigId}",
                        recipientEmails.Count, string.Join(", ", recipientEmails), subject, smtpConfigId?.ToString() ?? "null");

                    bool emailSent = false;
                    if (smtpConfigId.HasValue)
                    {
                        emailSent = await _emailService.SendEmailAsync(recipientEmails, subject, body, smtpConfigId.Value, isHtml: true, cancellationToken: cancellationToken);
                    }
                    else
                    {
                        emailSent = await _emailService.SendEmailAsync(recipientEmails, subject, body, isHtml: true, cancellationToken: cancellationToken);
                    }

                    if (emailSent)
                    {
                        _logger?.LogInformation("Submission confirmation email sent successfully to {Count} recipients for submission {SubmissionId}: {Emails}", 
                            recipientEmails.Count, submissionId, string.Join(", ", recipientEmails));
                    }
                    else
                    {
                        _logger?.LogError("Failed to send submission confirmation email to {Count} recipients for submission {SubmissionId}. " +
                            "Check SMTP configuration and logs for details.",
                            recipientEmails.Count, submissionId);
                    }
                }
                else
                {
                    // Fallback: if configured recipients did not resolve to valid emails,
                    // notify the submission owner when possible.
                    var ownerEmail = await ResolveRecipientEmailAsync(submissionId, submission.SubmittedByUserId, cancellationToken);
                    if (!string.IsNullOrWhiteSpace(ownerEmail))
                    {
                        _logger?.LogWarning("No valid configured recipient emails for submission {SubmissionId}. Falling back to submission owner email {Email}",
                            submissionId, ownerEmail);

                        if (smtpConfigId.HasValue)
                            await _emailService.SendEmailAsync(new[] { ownerEmail }, subject, body, smtpConfigId.Value, isHtml: true, cancellationToken: cancellationToken);
                        else
                            await _emailService.SendEmailAsync(new[] { ownerEmail }, subject, body, isHtml: true, cancellationToken: cancellationToken);

                        _logger?.LogInformation("Fallback submission confirmation email sent to owner for submission {SubmissionId}", submissionId);
                        return;
                    }

                    _logger?.LogWarning("No valid email addresses found for {Count} recipient user IDs of submission {SubmissionId}. " +
                        "RecipientUserIds: {UserIds}",
                        recipientUserIds?.Count() ?? 0, submissionId, string.Join(", ", recipientUserIds ?? Enumerable.Empty<string>()));
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to send submission confirmation email for submission {SubmissionId}. Error: {ErrorMessage}. StackTrace: {StackTrace}",
                    submissionId, ex.Message, ex.StackTrace);
                // Don't throw - email failure should not block workflow
            }
        }

        /// <summary>
        /// Sends submission confirmation email (legacy method - uses SubmittedByUserId)
        /// </summary>
        public async Task SendSubmissionConfirmationAsync(int submissionId, CancellationToken cancellationToken = default)
        {
            try
            {
                var submission = await _unitOfWork.FormSubmissionsRepository.GetByIdAsync(submissionId);
                if (submission == null)
                {
                    _logger?.LogWarning("Submission {SubmissionId} not found for email notification", submissionId);
                    return;
                }

                var documentType = await _unitOfWork.DocumentTypeRepository.GetByIdAsync(submission.DocumentTypeId);
                var series = await _unitOfWork.DocumentSeriesRepository.GetByIdAsync(submission.SeriesId);
                var projectName = series?.PROJECTS?.Name ?? string.Empty;
                var submittedByUser = await ResolveRecipientEmailAsync(submission.Id, submission.SubmittedByUserId, cancellationToken);

                if (string.IsNullOrWhiteSpace(submittedByUser))
                {
                    _logger?.LogWarning("User email not found for userId: {UserId}", submission.SubmittedByUserId);
                    return;
                }

                var templateData = new Dictionary<string, string>
                {
                    { "DocumentNumber", submission.DocumentNumber ?? string.Empty },
                    { "SubmissionId", submission.Id.ToString() },
                    { "DocumentType", documentType?.Name ?? string.Empty },
                    { "ProjectName", projectName },
                    { "SubmittedBy", submission.SubmittedByUserId },
                    { "SystemUrl", "" } // Will be added by template service
                };

                var (subject, body, smtpConfigId) = await ResolveProcessedTemplateAsync(
                    submission.DocumentTypeId,
                    triggerType: "FormSubmitted",
                    templateCode: "SubmissionConfirmation",
                    templateData,
                    cancellationToken);

                if (smtpConfigId.HasValue)
                    await _emailService.SendEmailWithRetryAsync(submittedByUser, subject, body, smtpConfigId.Value, isHtml: true, cancellationToken: cancellationToken);
                else
                    await _emailService.SendEmailWithRetryAsync(submittedByUser, subject, body, isHtml: true, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to send submission confirmation email for submission {SubmissionId}", submissionId);
                // Don't throw - email failure should not block workflow
            }
        }

        /// <summary>
        /// Sends approval required email to approvers
        /// </summary>
        public async Task SendApprovalRequiredAsync(int submissionId, int stageId, IEnumerable<string> approverUserIds, CancellationToken cancellationToken = default)
        {
            try
            {
                var submission = await _unitOfWork.FormSubmissionsRepository.GetByIdAsync(submissionId);
                if (submission == null)
                {
                    _logger?.LogWarning("Submission {SubmissionId} not found for email notification", submissionId);
                    return;
                }

                var stage = await _unitOfWork.ApprovalStageRepository.GetByIdAsync(stageId);
                var documentType = await _unitOfWork.DocumentTypeRepository.GetByIdAsync(submission.DocumentTypeId);
                var series = await _unitOfWork.DocumentSeriesRepository.GetByIdAsync(submission.SeriesId);
                var projectName = series?.PROJECTS?.Name ?? string.Empty;
                var submittedByUser = await GetUserEmailAsync(submission.SubmittedByUserId);

                var templateData = new Dictionary<string, string>
                {
                    { "DocumentNumber", submission.DocumentNumber ?? string.Empty },
                    { "SubmissionId", submission.Id.ToString() },
                    { "DocumentType", documentType?.Name ?? string.Empty },
                    { "ProjectName", projectName },
                    { "SubmittedBy", submission.SubmittedByUserId },
                    { "ApprovalStage", stage?.StageName ?? string.Empty },
                    { "SystemUrl", "" } // Will be added by template service
                };

                var (subject, body, smtpConfigId) = await ResolveProcessedTemplateAsync(
                    submission.DocumentTypeId,
                    triggerType: "ApprovalRequired",
                    templateCode: "ApprovalRequired",
                    templateData,
                    cancellationToken);

                var approverEmails = new List<string>();
                foreach (var approverUserId in approverUserIds)
                {
                    var email = await ResolveRecipientEmailAsync(submissionId, approverUserId, cancellationToken);
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        approverEmails.Add(email);
                    }
                }

                if (approverEmails.Any())
                {
                    if (smtpConfigId.HasValue)
                        await _emailService.SendEmailAsync(approverEmails, subject, body, smtpConfigId.Value, isHtml: true, cancellationToken: cancellationToken);
                    else
                        await _emailService.SendEmailAsync(approverEmails, subject, body, isHtml: true, cancellationToken: cancellationToken);
                }
                else
                {
                    _logger?.LogWarning("No valid approver emails found for submission {SubmissionId}", submissionId);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to send approval required email for submission {SubmissionId}", submissionId);
                // Don't throw - email failure should not block workflow
            }
        }

        /// <summary>
        /// Sends approval result email (Approved/Rejected/Returned) to specific recipients from ALERT_RULES
        /// </summary>
        public async Task SendApprovalResultToRecipientsAsync(int submissionId, string actionType, string approverUserId, string? comments, IEnumerable<string> recipientUserIds, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger?.LogInformation("Starting to send approval result email for submission {SubmissionId}, actionType: {ActionType} to {Count} recipients", 
                    submissionId, actionType, recipientUserIds?.Count() ?? 0);

                var submission = await _unitOfWork.FormSubmissionsRepository.GetByIdAsync(submissionId);
                if (submission == null)
                {
                    _logger?.LogWarning("Submission {SubmissionId} not found for email notification", submissionId);
                    return;
                }

                var documentType = await _unitOfWork.DocumentTypeRepository.GetByIdAsync(submission.DocumentTypeId);
                var series = await _unitOfWork.DocumentSeriesRepository.GetByIdAsync(submission.SeriesId);
                var projectName = series?.PROJECTS?.Name ?? string.Empty;
                var approverName = await GetUserNameAsync(approverUserId);

                var templateData = new Dictionary<string, string>
                {
                    { "DocumentNumber", submission.DocumentNumber ?? string.Empty },
                    { "SubmissionId", submission.Id.ToString() },
                    { "DocumentType", documentType?.Name ?? string.Empty },
                    { "ProjectName", projectName },
                    { "ActionType", actionType },
                    { "ApproverName", approverName ?? approverUserId },
                    { "ActionDate", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC") },
                    { "Comments", comments ?? string.Empty },
                    { "SystemUrl", "" } // Will be added by template service
                };

                var (subject, body, smtpConfigId) = await ResolveProcessedTemplateAsync(
                    submission.DocumentTypeId,
                    triggerType: $"Approval{actionType}",
                    templateCode: "ApprovalResult",
                    templateData,
                    cancellationToken);

                // Get emails for all recipient user IDs
                var recipientEmails = new List<string>();
                foreach (var userId in recipientUserIds ?? Enumerable.Empty<string>())
                {
                    _logger?.LogInformation("Resolving email for userId: {UserId} (approval result)", userId);
                    var email = await ResolveRecipientEmailAsync(submissionId, userId, cancellationToken);
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        recipientEmails.Add(email);
                        _logger?.LogInformation("Added email {Email} for userId {UserId}", email, userId);
                    }
                    else
                    {
                        _logger?.LogWarning("No email found for userId {UserId} (approval result)", userId);
                    }
                }

                if (recipientEmails.Any())
                {
                    _logger?.LogInformation("Sending approval result email to {Count} recipients: {Emails}. Subject: {Subject}, ActionType: {ActionType}, SmtpConfigId: {SmtpConfigId}",
                        recipientEmails.Count, string.Join(", ", recipientEmails), subject, actionType, smtpConfigId?.ToString() ?? "null");

                    bool emailSent = false;
                    if (smtpConfigId.HasValue)
                    {
                        emailSent = await _emailService.SendEmailAsync(recipientEmails, subject, body, smtpConfigId.Value, isHtml: true, cancellationToken: cancellationToken);
                    }
                    else
                    {
                        emailSent = await _emailService.SendEmailAsync(recipientEmails, subject, body, isHtml: true, cancellationToken: cancellationToken);
                    }

                    if (emailSent)
                    {
                        _logger?.LogInformation("Approval result email sent successfully to {Count} recipients for submission {SubmissionId}: {Emails}", 
                            recipientEmails.Count, submissionId, string.Join(", ", recipientEmails));
                    }
                    else
                    {
                        _logger?.LogError("Failed to send approval result email to {Count} recipients for submission {SubmissionId}. Check SMTP configuration and logs for details.",
                            recipientEmails.Count, submissionId);
                    }
                }
                else
                {
                    // Fallback: if configured recipients did not resolve to valid emails,
                    // notify the submission owner when possible.
                    var ownerEmail = await ResolveRecipientEmailAsync(submissionId, submission.SubmittedByUserId, cancellationToken);
                    if (!string.IsNullOrWhiteSpace(ownerEmail))
                    {
                        _logger?.LogWarning("No valid configured recipient emails for approval result of submission {SubmissionId}. Falling back to submission owner email {Email}",
                            submissionId, ownerEmail);

                        if (smtpConfigId.HasValue)
                            await _emailService.SendEmailAsync(new[] { ownerEmail }, subject, body, smtpConfigId.Value, isHtml: true, cancellationToken: cancellationToken);
                        else
                            await _emailService.SendEmailAsync(new[] { ownerEmail }, subject, body, isHtml: true, cancellationToken: cancellationToken);

                        _logger?.LogInformation("Fallback approval result email sent to owner for submission {SubmissionId}", submissionId);
                        return;
                    }

                    _logger?.LogWarning("No valid email addresses found for {Count} recipient user IDs of submission {SubmissionId}. " +
                        "RecipientUserIds: {UserIds}",
                        recipientUserIds?.Count() ?? 0, submissionId, string.Join(", ", recipientUserIds ?? Enumerable.Empty<string>()));
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to send approval result email for submission {SubmissionId}", submissionId);
                // Don't throw - email failure should not block workflow
            }
        }

        /// <summary>
        /// Sends approval result email (Approved/Rejected/Returned) - legacy method uses SubmittedByUserId
        /// </summary>
        public async Task SendApprovalResultAsync(int submissionId, string actionType, string approverUserId, string? comments = null, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger?.LogInformation("Starting to send approval result email for submission {SubmissionId}, actionType: {ActionType}", submissionId, actionType);

                var submission = await _unitOfWork.FormSubmissionsRepository.GetByIdAsync(submissionId);
                if (submission == null)
                {
                    _logger?.LogWarning("Submission {SubmissionId} not found for email notification", submissionId);
                    return;
                }

                _logger?.LogInformation("Submission found: DocumentNumber={DocumentNumber}, SubmittedByUserId={SubmittedByUserId}", 
                    submission.DocumentNumber, submission.SubmittedByUserId);

                var documentType = await _unitOfWork.DocumentTypeRepository.GetByIdAsync(submission.DocumentTypeId);
                var series = await _unitOfWork.DocumentSeriesRepository.GetByIdAsync(submission.SeriesId);
                var projectName = series?.PROJECTS?.Name ?? string.Empty;
                var approverName = await GetUserNameAsync(approverUserId);
                var submittedByUser = await ResolveRecipientEmailAsync(submission.Id, submission.SubmittedByUserId, cancellationToken);

                _logger?.LogInformation("Retrieved user email: {Email} for userId: {UserId}", submittedByUser, submission.SubmittedByUserId);

                if (string.IsNullOrWhiteSpace(submittedByUser))
                {
                    _logger?.LogWarning("User email not found for userId: {UserId}. Email will not be sent.", submission.SubmittedByUserId);
                    return;
                }

                var templateData = new Dictionary<string, string>
                {
                    { "DocumentNumber", submission.DocumentNumber ?? string.Empty },
                    { "SubmissionId", submission.Id.ToString() },
                    { "DocumentType", documentType?.Name ?? string.Empty },
                    { "ProjectName", projectName },
                    { "ActionType", actionType },
                    { "ApproverName", approverName ?? approverUserId },
                    { "ActionDate", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC") },
                    { "Comments", comments ?? string.Empty },
                    { "SystemUrl", "" } // Will be added by template service
                };

                _logger?.LogInformation("Processing email template for ApprovalResult");

                var (subject, body, smtpConfigId) = await ResolveProcessedTemplateAsync(
                    submission.DocumentTypeId,
                    triggerType: $"Approval{actionType}",
                    templateCode: "ApprovalResult",
                    templateData,
                    cancellationToken);

                _logger?.LogInformation("Email template processed. Subject: {Subject}. Sending email to: {Email}", subject, submittedByUser);

                if (smtpConfigId.HasValue)
                    await _emailService.SendEmailWithRetryAsync(submittedByUser, subject, body, smtpConfigId.Value, isHtml: true, cancellationToken: cancellationToken);
                else
                    await _emailService.SendEmailWithRetryAsync(submittedByUser, subject, body, isHtml: true, cancellationToken: cancellationToken);

                _logger?.LogInformation("Approval result email sent successfully to {Email} for submission {SubmissionId}", submittedByUser, submissionId);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to send approval result email for submission {SubmissionId}. Error: {ErrorMessage}", submissionId, ex.Message);
                // Don't throw - email failure should not block workflow
            }
        }

        private async Task<string?> GetUserEmailAsync(string userId)
        {
            try
            {
                _logger?.LogInformation("Getting email for userId: {UserId}", userId);

                // If the identifier already looks like an email address, use it directly.
                if (!string.IsNullOrWhiteSpace(userId) && userId.Contains("@"))
                {
                    _logger?.LogInformation("UserId looks like an email address. Using it directly: {Email}", userId);
                    return userId;
                }

                if (int.TryParse(userId, out int userIdInt))
                {
                    var user = await _identityContext.TblUsers
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => u.Id == userIdInt);
                    
                    if (user == null)
                    {
                        _logger?.LogWarning("User not found for userId (int): {UserId}", userIdInt);
                        return null;
                    }

                    // Prefer Email, but only use Username if it looks like an email address
                    var email = user.Email;
                    if (string.IsNullOrWhiteSpace(email))
                    {
                        _logger?.LogWarning("User {UserId} ({Username}) has no email address. Checking if username is email: {Username}", 
                            userIdInt, user.Username, user.Username);
                        
                        // Only use username as email if it contains @ symbol
                        if (!string.IsNullOrWhiteSpace(user.Username) && user.Username.Contains("@"))
                        {
                            email = user.Username;
                            _logger?.LogInformation("Using username as email for user {UserId}: {Email}", userIdInt, email);
                        }
                        else
                        {
                            _logger?.LogWarning("User {UserId} ({Username}) has no valid email address. Username is not an email format.", 
                                userIdInt, user.Username);
                            return null;
                        }
                    }
                    else
                    {
                        _logger?.LogInformation("Found email for user {UserId}: {Email}", userIdInt, email);
                    }

                    return email;
                }
                else
                {
                    var user = await _identityContext.TblUsers
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => u.Username.ToLower() == userId.ToLower());
                    
                    if (user == null)
                    {
                        _logger?.LogWarning("User not found for userId (string): {UserId}", userId);
                        return null;
                    }

                    var email = user.Email;
                    if (string.IsNullOrWhiteSpace(email))
                    {
                        _logger?.LogWarning("User {UserId} ({Username}) has no email address. Checking if username is email: {Username}", 
                            userId, user.Username, user.Username);
                        
                        // Only use username as email if it contains @ symbol
                        if (!string.IsNullOrWhiteSpace(user.Username) && user.Username.Contains("@"))
                        {
                            email = user.Username;
                            _logger?.LogInformation("Using username as email for user {UserId}: {Email}", userId, email);
                        }
                        else
                        {
                            _logger?.LogWarning("User {UserId} ({Username}) has no valid email address. Username is not an email format.", 
                                userId, user.Username);
                            return null;
                        }
                    }
                    else
                    {
                        _logger?.LogInformation("Found email for user {UserId}: {Email}", userId, email);
                    }

                    return email;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to get user email for userId: {UserId}. Error: {ErrorMessage}", userId, ex.Message);
                return null;
            }
        }

        private async Task<string?> ResolveRecipientEmailAsync(int submissionId, string userId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(userId)) return null;

            if (userId.Contains("@"))
            {
                _logger?.LogInformation("Recipient identifier already looks like email. Using it directly: {Email}", userId);
                return userId;
            }

            if (userId.Equals("public-user", StringComparison.OrdinalIgnoreCase))
            {
                var publicEmail = await GetPublicSubmissionEmailAsync(submissionId, cancellationToken);
                if (!string.IsNullOrWhiteSpace(publicEmail))
                {
                    _logger?.LogInformation("Resolved public submission email for submission {SubmissionId}: {Email}", submissionId, publicEmail);
                    return publicEmail;
                }

                _logger?.LogWarning("Public submission email not found for submission {SubmissionId}", submissionId);
                return null;
            }

            return await GetUserEmailAsync(userId);
        }

        private async Task<string?> GetPublicSubmissionEmailAsync(int submissionId, CancellationToken cancellationToken)
        {
            try
            {
                var values = await (
                    from v in _unitOfWork.AppDbContext.Set<FORM_SUBMISSION_VALUES>().AsNoTracking()
                    join f in _unitOfWork.AppDbContext.Set<FORM_FIELDS>().AsNoTracking() on v.FieldId equals f.Id
                    join t in _unitOfWork.AppDbContext.Set<FIELD_TYPES>().AsNoTracking() on f.FieldTypeId equals t.Id into ft
                    from t in ft.DefaultIfEmpty()
                    where v.SubmissionId == submissionId
                    select new
                    {
                        v.ValueString,
                        v.ValueJson,
                        f.FieldName,
                        f.FieldCode,
                        TypeName = t != null ? t.TypeName : null,
                        DataType = t != null ? t.DataType : null
                    }).ToListAsync(cancellationToken);

                if (!values.Any()) return null;

                // Priority 1: fields that look like email fields
                foreach (var v in values.Where(v => LooksLikeEmailField(v.FieldName, v.FieldCode, v.TypeName, v.DataType)))
                {
                    var email = ExtractEmail(v.ValueString) ?? ExtractEmail(v.ValueJson);
                    if (!string.IsNullOrWhiteSpace(email)) return email;
                }

                // Priority 2: any value that looks like an email
                foreach (var v in values)
                {
                    var email = ExtractEmail(v.ValueString) ?? ExtractEmail(v.ValueJson);
                    if (!string.IsNullOrWhiteSpace(email)) return email;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to resolve public submission email for submission {SubmissionId}", submissionId);
                return null;
            }
        }

        private static string? ExtractEmail(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;
            var match = Regex.Match(input, @"[A-Za-z0-9._%+\-]+@[A-Za-z0-9.\-]+\.[A-Za-z]{2,}");
            return match.Success ? match.Value : null;
        }

        private static bool LooksLikeEmailField(string? fieldName, string? fieldCode, string? typeName, string? dataType)
        {
            return (!string.IsNullOrWhiteSpace(fieldName) && fieldName.Contains("email", StringComparison.OrdinalIgnoreCase)) ||
                   (!string.IsNullOrWhiteSpace(fieldCode) && fieldCode.Contains("email", StringComparison.OrdinalIgnoreCase)) ||
                   (!string.IsNullOrWhiteSpace(typeName) && typeName.Contains("email", StringComparison.OrdinalIgnoreCase)) ||
                   (!string.IsNullOrWhiteSpace(dataType) && dataType.Contains("email", StringComparison.OrdinalIgnoreCase));
        }

        private async Task<string?> GetUserNameAsync(string userId)
        {
            try
            {
                if (int.TryParse(userId, out int userIdInt))
                {
                    var user = await _identityContext.TblUsers.FirstOrDefaultAsync(u => u.Id == userIdInt);
                    return user?.Username;
                }
                else
                {
                    return userId; // Already a username
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to get user name for userId: {UserId}", userId);
                return userId;
            }
        }

        private async Task<(string Subject, string Body, int? SmtpConfigId)> ResolveProcessedTemplateAsync(
            int documentTypeId,
            string triggerType,
            string templateCode,
            Dictionary<string, string> data,
            CancellationToken cancellationToken)
        {
            // Add SystemUrl to data if not present
            if (!data.ContainsKey("SystemUrl"))
            {
                data["SystemUrl"] = string.Empty; // will be filled by template service fallback if needed
            }

            // Priority 1: Use template selected on active alert rule (if set)
            var alertTemplateId = await _unitOfWork.AppDbContext.Set<ALERT_RULES>()
                .AsNoTracking()
                .Where(ar =>
                    ar.DocumentTypeId == documentTypeId &&
                    ar.TriggerType == triggerType &&
                    (ar.NotificationType == "Email" || ar.NotificationType == "Both") &&
                    ar.IsActive &&
                    !ar.IsDeleted &&
                    ar.EmailTemplateId.HasValue)
                .OrderByDescending(ar => ar.UpdatedDate ?? ar.CreatedDate)
                .Select(ar => (int?)ar.EmailTemplateId)
                .FirstOrDefaultAsync(cancellationToken);

            EMAIL_TEMPLATES? template = null;

            if (alertTemplateId.HasValue)
            {
                template = await _unitOfWork.AppDbContext.Set<EMAIL_TEMPLATES>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == alertTemplateId.Value && t.IsActive && !t.IsDeleted, cancellationToken);
            }

            // Priority 2: Default template for this document type
            if (template == null)
            {
                template = await _unitOfWork.AppDbContext.Set<EMAIL_TEMPLATES>()
                    .AsNoTracking()
                    .Where(t => t.DocumentTypeId == documentTypeId &&
                                t.TemplateCode.ToLower() == templateCode.ToLower() &&
                                t.IsDefault &&
                                t.IsActive &&
                                !t.IsDeleted)
                    .OrderByDescending(t => t.UpdatedDate ?? t.CreatedDate)
                    .FirstOrDefaultAsync(cancellationToken);
            }

            // Priority 3: Any default template with this code
            if (template == null)
            {
                template = await _unitOfWork.AppDbContext.Set<EMAIL_TEMPLATES>()
                    .AsNoTracking()
                    .Where(t => t.TemplateCode.ToLower() == templateCode.ToLower() &&
                                t.IsDefault &&
                                t.IsActive &&
                                !t.IsDeleted)
                    .OrderByDescending(t => t.UpdatedDate ?? t.CreatedDate)
                    .FirstOrDefaultAsync(cancellationToken);
            }

            if (template != null)
            {
                var subject = _emailTemplateService.ProcessTemplate(template.SubjectTemplate ?? string.Empty, data);
                var body = _emailTemplateService.ProcessTemplate(template.BodyTemplateHtml ?? string.Empty, data);
                return (subject, body, template.SmtpConfigId);
            }

            // Fallback to appsettings-based templates (legacy)
            var (fallbackSubject, fallbackBody) = await _emailTemplateService.GetProcessedTemplateAsync(templateCode, data, cancellationToken);
            return (fallbackSubject, fallbackBody, null);
        }
    }
}
