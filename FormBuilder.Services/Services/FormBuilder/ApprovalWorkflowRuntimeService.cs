using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.Domian.Entitys.froms;
using FormBuilder.Application.DTOs.ApprovalWorkflow;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Core.IServices.FormBuilder;
using FormBuilder.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FormBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using FormBuilder.Services.Services.FormBuilder;

namespace FormBuilder.Services
{
    public class ApprovalWorkflowRuntimeService : IApprovalWorkflowRuntimeService
    {
        private readonly IunitOfwork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDocumentApprovalHistoryService _historyService;
        private readonly IApprovalDelegationService _delegationService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AkhmanageItContext _identityContext;
        private readonly ILogger<ApprovalWorkflowRuntimeService> _logger;
        private readonly FormBuilder.Services.Services.Email.EmailNotificationService? _emailNotificationService;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IDocumentNumberGeneratorService _documentNumberGenerator;

        public ApprovalWorkflowRuntimeService(
            IunitOfwork unitOfWork,
            IMapper mapper,
            IDocumentApprovalHistoryService historyService,
            IApprovalDelegationService delegationService,
            AkhmanageItContext identityContext,
            ILogger<ApprovalWorkflowRuntimeService> logger,
            UserManager<IdentityUser> userManager = null,
            RoleManager<IdentityRole> roleManager = null,
            FormBuilder.Services.Services.Email.EmailNotificationService? emailNotificationService = null,
            IServiceScopeFactory scopeFactory = null,
            IDocumentNumberGeneratorService? documentNumberGenerator = null)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _historyService = historyService;
            _delegationService = delegationService;
            _identityContext = identityContext;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailNotificationService = emailNotificationService;
            _scopeFactory = scopeFactory;
            _documentNumberGenerator = documentNumberGenerator;
        }

        /// <summary>
        /// Executes an action with the triggers service resolved lazily to avoid circular dependency
        /// </summary>
        private async Task ExecuteWithTriggersServiceAsync(Func<IFormSubmissionTriggersService, Task> action)
        {
            if (_scopeFactory == null)
                return;

            try
            {
                await using var scope = _scopeFactory.CreateAsyncScope();
                var triggersService = scope.ServiceProvider.GetService<IFormSubmissionTriggersService>();
                if (triggersService != null)
                {
                    await action(triggersService);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Failed to resolve or execute IFormSubmissionTriggersService: {Error}", ex.Message);
            }
        }

        private void FireAndForgetEmail(Func<FormBuilder.Services.Services.Email.EmailNotificationService, Task> sendAsync, string logContext)
        {
            if (_scopeFactory == null)
            {
                _logger?.LogWarning("IServiceScopeFactory is null; cannot send email safely ({Context})", logContext);
                return;
            }

            _ = Task.Run(async () =>
            {
                try
                {
                    await using var scope = _scopeFactory.CreateAsyncScope();
                    var emailService = scope.ServiceProvider.GetRequiredService<FormBuilder.Services.Services.Email.EmailNotificationService>();
                    await sendAsync(emailService);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Failed to send email ({Context})", logContext);
                }
            });
        }

        /// <summary>
        /// Activates the first stage of the workflow for a submitted document
        /// </summary>
        public async Task<ApiResponse> ActivateStageForSubmissionAsync(int submissionId)
        {
            var submission = await _unitOfWork.FormSubmissionsRepository.SingleOrDefaultAsync(
                s => s.Id == submissionId && !s.IsDeleted, asNoTracking: false);

            if (submission == null)
                return new ApiResponse(404, "Submission not found");

            if (submission.Status != "Submitted")
                return new ApiResponse(400, "Submission must be in Submitted status");

            // Get document type and workflow
            var documentType = await _unitOfWork.DocumentTypeRepository.GetByIdAsync(submission.DocumentTypeId);
            if (documentType == null)
                return new ApiResponse(404, "Document type not found");

            // Prefer explicitly assigned workflow. If not assigned, fallback to an active workflow for the document type.
            APPROVAL_WORKFLOWS? workflow = null;
            if (documentType.ApprovalWorkflowId.HasValue)
            {
                workflow = await _unitOfWork.ApprovalWorkflowRepository.GetByIdAsync(documentType.ApprovalWorkflowId.Value);
            }
            if (workflow == null)
            {
                workflow = await _unitOfWork.ApprovalWorkflowRepository.GetActiveWorkflowByDocumentTypeIdAsync(submission.DocumentTypeId);
            }
            if (workflow == null || !workflow.IsActive)
                return new ApiResponse(400, "Approval workflow not found or inactive");

            // Get first stage (lowest StageOrder)
            var stages = await _unitOfWork.ApprovalStageRepository.GetAllAsync(s =>
                s.WorkflowId == workflow.Id && s.IsActive);

            var firstStage = stages.OrderBy(s => s.StageOrder).FirstOrDefault();
            if (firstStage == null)
                return new ApiResponse(400, "No active stages found in workflow");

            // Validate stage amount constraints before activation (if configured)
            var amountValidation = await ValidateStageAmountForSubmissionAsync(firstStage, submission.Id);
            if (amountValidation != null)
                return amountValidation;

            // Resolve approvers for the stage
            var approversResult = await ResolveApproversForStageAsync(firstStage.Id);
            if (approversResult.StatusCode != 200)
                return approversResult;

            // ✅ Persist current stage on the submission record
            submission.StageId = firstStage.Id;
            submission.UpdatedDate = DateTime.UtcNow;
            _unitOfWork.FormSubmissionsRepository.Update(submission);
            await _unitOfWork.CompleteAsyn();

            // ✅ TRIGGER: ApprovalRequired - Execute trigger when stage is assigned
            // This trigger handles: Resolve approvers, Apply delegation, Send emails, Create notifications
            var triggerExecuted = false;
            await ExecuteWithTriggersServiceAsync(async triggersService =>
            {
                await triggersService.ExecuteApprovalRequiredTriggerAsync(submission, firstStage.Id);
                triggerExecuted = true;
            });

            if (!triggerExecuted)
            {
                // IMPORTANT: Do NOT send emails directly here.
                // Email delivery is controlled ONLY via ALERT_RULES inside FormSubmissionTriggersService.
                // If triggers service can't be resolved, we skip email sending.
            }

            // ✅ Create DocuSign envelope if stage requires e-signing
            if (firstStage.RequiresAdobeSign)
            {
                await CreateDocuSignEnvelopeIfNeededAsync(submission, firstStage);
            }

            return new ApiResponse(200, "Stage activated successfully", new { StageId = firstStage.Id, StageName = firstStage.StageName });
        }

        /// <summary>
        /// Resolves approvers for a stage by:
        /// 1. Loading assignees (roles and users)
        /// 2. Resolving roles to users
        /// 3. Checking delegations
        /// 4. Building final approver list
        /// </summary>
        public async Task<ApiResponse> ResolveApproversForStageAsync(int stageId)
        {
            _logger?.LogInformation("Resolving approvers for stage {StageId}", stageId);

            var stage = await _unitOfWork.ApprovalStageRepository.GetByIdAsync(stageId);
            if (stage == null)
            {
                _logger?.LogWarning("Stage {StageId} not found", stageId);
                return new ApiResponse(404, "Stage not found");
            }

            var assignees = await _unitOfWork.ApprovalStageAssigneesRepository.GetByStageIdAsync(stageId);
            _logger?.LogInformation("Found {Count} assignees for stage {StageId}", assignees.Count(), stageId);

            // Log assignees details with type information
            var assigneesInfo = assignees.Select(a => new
            {
                a.Id,
                a.StageId,
                UserId = a.UserId?.ToString() ?? "null",
                UserIdType = a.UserId?.GetType().Name ?? "null",
                a.RoleId,
                a.IsActive
            }).ToList();
            _logger?.LogInformation("Assignees details for stage {StageId}: {Assignees}",
                stageId, JsonSerializer.Serialize(assigneesInfo));

            // Log all unique UserIds for debugging
            var allUserIds = assignees.Where(a => !string.IsNullOrWhiteSpace(a.UserId))
                .Select(a => a.UserId?.ToString() ?? "null")
                .Distinct()
                .ToList();
            _logger?.LogInformation("Unique UserIds in stage {StageId}: {UserIds}",
                stageId, string.Join(", ", allUserIds));

            var userIds = new HashSet<string>();

            static bool HasEntityValue(string? value)
            {
                return !string.IsNullOrWhiteSpace(value)
                       && !string.Equals(value.Trim(), "null", StringComparison.OrdinalIgnoreCase);
            }

            // IMPORTANT:
            // User-specific assignees may carry RoleId for metadata/display.
            // Role expansion must apply ONLY for pure role-based assignees (UserId is empty),
            // otherwise a single user assignment would incorrectly grant all users in that role.
            var roleIds = assignees
                .Where(a => !HasEntityValue(a.UserId) && HasEntityValue(a.RoleId))
                .Select(a => a.RoleId!.Trim())
                .Distinct()
                .ToList();

            _logger?.LogInformation("Found {Count} role-based assignees: {RoleIds}", roleIds.Count, string.Join(", ", roleIds));

            if (roleIds.Any())
            {
                var usersFromRoles = await ResolveUsersFromRolesAsync(roleIds);
                _logger?.LogInformation("Resolved {Count} users from roles: {UserIds}", usersFromRoles.Count, string.Join(", ", usersFromRoles));

                foreach (var userId in usersFromRoles)
                {
                    // Ensure UserId is stored as string
                    userIds.Add(userId?.ToString() ?? string.Empty);
                }
            }

            // Process user-based assignees
            var directUserIds = assignees
                .Where(a => HasEntityValue(a.UserId))
                .Select(a => a.UserId!.Trim())
                .Distinct()
                .ToList();

            _logger?.LogInformation("Found {Count} user-based assignees: {UserIds}", directUserIds.Count, string.Join(", ", directUserIds));

            foreach (var userId in directUserIds)
            {
                // Ensure UserId is stored as string for consistent comparison
                var userIdString = userId?.ToString()?.Trim() ?? string.Empty;

                // ✅ Resolve username to numeric UserId if needed (same logic as GetApprovalInboxAsync)
                var resolvedUserId = userIdString;
                if (!int.TryParse(userIdString, out _))
                {
                    // Input is not numeric, try to find user by username
                    var user = await _identityContext.TblUsers
                        .FirstOrDefaultAsync(u => u.Username.ToLower() == userIdString.ToLower());

                    if (user != null)
                    {
                        resolvedUserId = user.Id.ToString();
                        _logger?.LogInformation("✓ Resolved username '{Username}' to UserId '{NumericUserId}' in ResolveApproversForStageAsync",
                            userIdString, resolvedUserId);
                    }
                    else
                    {
                        _logger?.LogWarning("Could not find user with username '{Username}' in Tbl_User (ResolveApproversForStageAsync)", userIdString);
                        // Still use the original userIdString in case it's valid in another format
                    }
                }

                // Check for delegation using resolved userId
                var delegation = await _unitOfWork.ApprovalDelegationRepository
                    .GetActiveDelegationAsync(resolvedUserId, DateTime.UtcNow);

                if (delegation != null)
                {
                    _logger?.LogInformation("Found delegation for user {UserId}: delegated to {DelegatedUserId}", resolvedUserId, delegation.ToUserId);
                    // Use delegated user instead
                    userIds.Add(delegation.ToUserId?.ToString() ?? string.Empty);
                }
                else
                {
                    // Add both original and resolved userId for maximum compatibility
                    userIds.Add(resolvedUserId);
                    // Also add original if different (for backward compatibility)
                    if (resolvedUserId != userIdString)
                    {
                        userIds.Add(userIdString);
                    }
                }
            }

            _logger?.LogInformation("Final resolved approvers for stage {StageId}: {UserIds}", stageId, string.Join(", ", userIds));

            if (!userIds.Any())
            {
                _logger?.LogWarning("No approvers found for stage {StageId}", stageId);
                return new ApiResponse(400, "No approvers found for this stage");
            }

            var result = new ResolvedApproversDto
            {
                StageId = stageId,
                StageName = stage.StageName,
                UserIds = userIds.Where(u => !string.IsNullOrEmpty(u)).ToArray()
            };

            // Enforce minimum required approvers (if configured)
            if (stage.MinimumRequiredAssignees.HasValue && stage.MinimumRequiredAssignees.Value > 0)
            {
                var min = stage.MinimumRequiredAssignees.Value;
                if (result.UserIds == null || result.UserIds.Length < min)
                {
                    _logger?.LogWarning("Stage {StageId} requires minimum {Min} approvers but resolved {Count}",
                        stageId, min, result.UserIds?.Length ?? 0);
                    return new ApiResponse(400, $"Stage requires minimum {min} approvers, but only {result.UserIds?.Length ?? 0} were resolved");
                }
            }

            _logger?.LogInformation("Successfully resolved {Count} approvers for stage {StageId} ({StageName})", result.UserIds.Length, stageId, stage.StageName);

            return new ApiResponse(200, "Approvers resolved successfully", result);
        }

        /// <summary>
        /// Resolves users from role IDs using AkhmanageItContext (custom identity system)
        /// </summary>
        public async Task<List<string>> ResolveUsersFromRolesAsync(List<string> roleIds)
        {
            var userIds = new List<string>();

            if (roleIds == null || !roleIds.Any())
            {
                _logger?.LogWarning("No role IDs provided to ResolveUsersFromRolesAsync");
                return userIds;
            }

            _logger?.LogInformation("Resolving users from {Count} roles: {RoleIds}", roleIds.Count, string.Join(", ", roleIds));

            try
            {
                // Convert role IDs to integers (they might be stored as strings)
                var roleIdInts = new List<int>();
                foreach (var roleId in roleIds)
                {
                    if (int.TryParse(roleId, out int roleIdInt))
                    {
                        roleIdInts.Add(roleIdInt);
                    }
                    else
                    {
                        _logger?.LogWarning("Invalid role ID format: {RoleId}", roleId);
                    }
                }

                if (!roleIdInts.Any())
                {
                    _logger?.LogWarning("No valid role IDs found after parsing");
                    return userIds;
                }

                // Get users from TblUserGroupUser table where role is active
                var usersInRoles = await _identityContext.TblUserGroupUsers
                    .Include(ugu => ugu.IdUserGroupNavigation)
                    .Include(ugu => ugu.IdUserNavigation)
                    .Where(ugu => roleIdInts.Contains(ugu.IdUserGroup)
                        && ugu.IdUserGroupNavigation.IsActive
                        && ugu.IdUserNavigation.IsActive)
                    .Select(ugu => ugu.IdUser)
                    .Distinct()
                    .ToListAsync();

                _logger?.LogInformation("Found {Count} users in roles", usersInRoles.Count);

                // Convert user IDs to strings for consistent comparison
                userIds = usersInRoles.Select(id => id.ToString()).ToList();

                _logger?.LogInformation("Resolved {Count} user IDs from roles: {UserIds}", userIds.Count, string.Join(", ", userIds));

                // Fallback to ASP.NET Identity if available (for backward compatibility)
                // IMPORTANT:
                // - إذا الـ Role موجود في AkhmanageItContext لكنه IsActive = false، الاستعلام أعلاه لن يرجّع أي مستخدمين (usersInRoles.Count == 0)
                // - في الحالة دي بالذات إحنا *ما* نريد نبعث إيميل، فلازم ما نستخدم الـ fallback
                // - لذلك نتحقق أولاً هل في أي Role ACTIVE في AkhmanageItContext لنفس الـ IDs قبل ما نحاول الـ fallback
                if ((_roleManager != null || _userManager != null) && userIds.Count == 0)
                {
                    // لو مفيش أي Role Active مطابق، نخرج بدون أي fallback (علشان نحترم IsActive)
                    var hasAnyActiveRole = await _identityContext.TblUserGroups
                        .Where(g => roleIdInts.Contains(g.Id) && g.IsActive)
                        .AnyAsync();

                    if (hasAnyActiveRole)
                    {
                        _logger?.LogInformation("No users found via AkhmanageItContext for active roles, trying ASP.NET Identity fallback");
                        foreach (var roleId in roleIds)
                        {
                            var role = await _roleManager?.FindByIdAsync(roleId);
                            if (role != null)
                            {
                                var usersInRole = await _userManager?.GetUsersInRoleAsync(role.Name);
                                if (usersInRole != null)
                                {
                                    userIds.AddRange(
                                        usersInRole
                                            // في حالة وجود خاصية IsActive على هوية المستخدم
                                            .Where(u =>
                                                (u as dynamic)?.IsActive == null ||
                                                ((bool?)(u as dynamic).IsActive) == true)
                                            .Select(u => u.Id)
                                    );
                                }
                            }
                        }
                    }
                    else
                    {
                        _logger?.LogInformation("All matching roles in AkhmanageItContext are inactive or not found. Skipping ASP.NET Identity fallback to respect IsActive flag.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error resolving users from roles: {Message}", ex.Message);
            }

            return userIds.Distinct().ToList();
        }

        /// <summary>
        /// Checks if a user has active delegation
        /// </summary>
        public async Task<ApiResponse> CheckDelegationAsync(string userId)
        {
            var delegation = await _unitOfWork.ApprovalDelegationRepository
                .GetActiveDelegationAsync(userId, DateTime.UtcNow);

            if (delegation == null)
                return new ApiResponse(200, "No active delegation", new { HasDelegation = false });

            return new ApiResponse(200, "Active delegation found", new
            {
                HasDelegation = true,
                DelegationId = delegation.Id,
                ToUserId = delegation.ToUserId,
                StartDate = delegation.StartDate,
                EndDate = delegation.EndDate
            });
        }

        /// <summary>
        /// Processes approval actions (Approved, Rejected, Returned)
        /// </summary>
        public async Task<ApiResponse> ProcessApprovalActionAsync(ApprovalActionDto dto)
        {
            // Security check: Verify stageId > 0
            if (dto.StageId <= 0)
            {
                _logger?.LogError("SECURITY CHECK FAILED: Invalid stageId {StageId}. StageId must be greater than 0", dto.StageId);
                return new ApiResponse(400, "Invalid stageId. StageId must be greater than 0");
            }

            var submission = await _unitOfWork.FormSubmissionsRepository.SingleOrDefaultAsync(
                s => s.Id == dto.SubmissionId, asNoTracking: false);

            if (submission == null)
                return new ApiResponse(404, "Submission not found");

            var stage = await _unitOfWork.ApprovalStageRepository.GetByIdAsync(dto.StageId);
            if (stage == null)
                return new ApiResponse(404, "Stage not found");

            // Check if user is authorized to approve this stage
            var approversResult = await ResolveApproversForStageAsync(dto.StageId);
            if (approversResult.StatusCode != 200)
                return approversResult;

            var resolvedApprovers = approversResult.Data as ResolvedApproversDto;
            var normalizedActionUserId = dto.ActionByUserId?.Trim() ?? string.Empty;

            // Check if user is in the resolved approvers list (case-insensitive comparison)
            var isAuthorized = resolvedApprovers != null &&
                resolvedApprovers.UserIds != null &&
                resolvedApprovers.UserIds.Any(id =>
                    string.Equals(id?.Trim(), normalizedActionUserId, StringComparison.OrdinalIgnoreCase));

            if (!isAuthorized)
            {
                _logger?.LogWarning("User {UserId} is not authorized to approve stage {StageId}", normalizedActionUserId, dto.StageId);
                return new ApiResponse(403, "User is not authorized to approve this stage");
            }

            // Check for delegation
            var delegation = await _unitOfWork.ApprovalDelegationRepository
                .GetActiveDelegationAsync(dto.ActionByUserId, DateTime.UtcNow);

            string actualUserId = dto.ActionByUserId;
            string actionType = dto.ActionType;
            bool signatureRequestedForCurrentStage = false;

            if (delegation != null)
            {
                // User is acting on behalf of another user
                actualUserId = delegation.ToUserId;
                if (dto.ActionType == "Approved")
                {
                    actionType = "Approved (Delegated)";
                }
            }

            // Process action
            switch (dto.ActionType.ToLower())
            {
                case "approved":
                    // Move to next stage or complete if final stage
                    if (stage.IsFinalStage)
                    {
                        submission.Status = "Approved";
                        submission.StageId = null;
                    }
                    else
                    {
                        // Get next stage
                        var workflow = await _unitOfWork.ApprovalWorkflowRepository.GetByIdAsync(stage.WorkflowId);
                        var stages = await _unitOfWork.ApprovalStageRepository.GetAllAsync(s =>
                            s.WorkflowId == workflow.Id && s.IsActive);

                        var nextStage = stages
                            .Where(s => s.StageOrder > stage.StageOrder)
                            .OrderBy(s => s.StageOrder)
                            .FirstOrDefault();

                        if (nextStage == null)
                        {
                            submission.Status = "Approved";
                            submission.StageId = null;
                        }
                        else
                        {
                            // Validate next stage amount constraints before moving (if configured)
                            var amountValidation = await ValidateStageAmountForSubmissionAsync(nextStage, submission.Id);
                            if (amountValidation != null)
                                return amountValidation;

                            // ✅ TRIGGER: Move to next stage and auto-activate
                            submission.Status = "Submitted";
                            submission.StageId = nextStage.Id;
                        }
                    }
                    break;

                case "rejected":
                    submission.Status = "Rejected";
                    submission.StageId = dto.StageId;
                    break;

                case "returned":
                    // Return to previous stage or draft
                    var prevStage = await GetPreviousStageAsync(stage.Id);
                    if (prevStage == null)
                    {
                        submission.Status = "Draft";
                        submission.StageId = null;
                    }
                    else
                    {
                        submission.Status = "Submitted";
                        submission.StageId = prevStage.Id;
                    }
                    break;

                default:
                    return new ApiResponse(400, "Invalid action type. Must be Approved, Rejected, or Returned");
            }

            // Generate final number on approval for series configured with GenerateOn = Approval.
            if (dto.ActionType.Equals("approved", StringComparison.OrdinalIgnoreCase) &&
                submission.Status == "Approved")
            {
                var series = await _unitOfWork.DocumentSeriesRepository.GetByIdAsync(submission.SeriesId);
                if (series == null || !series.IsActive)
                    return new ApiResponse(400, "Document series not found or inactive.");

                if (series.GenerateOn.Equals("Approval", StringComparison.OrdinalIgnoreCase) &&
                    DocumentSeriesEngineRules.IsDraftDocumentNumber(submission.DocumentNumber))
                {
                    if (_documentNumberGenerator == null)
                        return new ApiResponse(500, "Document number generator service is not available.");

                    var generation = await _documentNumberGenerator.GenerateForSubmissionAsync(
                        submission.Id,
                        "Approval",
                        actualUserId);

                    if (!generation.Success || string.IsNullOrWhiteSpace(generation.DocumentNumber))
                    {
                        return new ApiResponse(500, generation.ErrorMessage ?? "Failed to generate document number.");
                    }

                    submission.DocumentNumber = generation.DocumentNumber;
                }
            }

            submission.UpdatedDate = DateTime.UtcNow;
            _unitOfWork.FormSubmissionsRepository.Update(submission);

            // Log history
            var historyDto = new DocumentApprovalHistoryCreateDto
            {
                SubmissionId = dto.SubmissionId,
                StageId = dto.StageId,
                ActionType = actionType,
                ActionByUserId = actualUserId,
                Comments = dto.Comments ?? string.Empty
            };

            var historyResult = await _historyService.CreateAsync(historyDto);
            await _unitOfWork.CompleteAsyn();

            // If action moved the document to another stage that requires e-sign, request DocuSign now.
            if (dto.ActionType.Equals("approved", StringComparison.OrdinalIgnoreCase) && submission.StageId.HasValue)
            {
                var activatedStage = await _unitOfWork.ApprovalStageRepository.GetByIdAsync(submission.StageId.Value);
                if (activatedStage != null && activatedStage.RequiresAdobeSign)
                {
                    signatureRequestedForCurrentStage = await CreateDocuSignEnvelopeIfNeededAsync(
                        submission,
                        activatedStage,
                        dto.ActionByUserId);
                }
            }

            // Get the created history record for triggers
            DOCUMENT_APPROVAL_HISTORY? createdHistory = null;
            if (historyResult.StatusCode == 200 && historyResult.Data != null)
            {
                // Try to get the history record by submission and stage
                var histories = await _unitOfWork.DocumentApprovalHistoryRepository.GetBySubmissionIdAsync(dto.SubmissionId);
                createdHistory = histories
                    .Where(h => h.StageId == dto.StageId && h.ActionType == actionType)
                    .OrderByDescending(h => h.ActionDate)
                    .FirstOrDefault();
            }

            // ✅ TRIGGER: Execute triggers based on action type
            if (createdHistory != null)
            {
                await ExecuteWithTriggersServiceAsync(async triggersService =>
                {
                    if (dto.ActionType.ToLower() == "approved")
                    {
                        await triggersService.ExecuteApprovalApprovedTriggerAsync(createdHistory, submission);
                    }
                    else if (dto.ActionType.ToLower() == "rejected")
                    {
                        await triggersService.ExecuteApprovalRejectedTriggerAsync(createdHistory, submission);
                    }
                    else if (dto.ActionType.ToLower() == "returned")
                    {
                        await triggersService.ExecuteApprovalReturnedTriggerAsync(createdHistory, submission);
                    }
                });
            }

            // Legacy code - keep for backward compatibility but triggers handle most actions
            if (dto.ActionType.ToLower() == "approved")
            {
                // Action 1: History already logged above ✅
                // Action 2: Final stage check already done above ✅
                // Action 3: Status update already done above ✅

                // IMPORTANT: Do NOT send emails directly here.
                // Email delivery is controlled ONLY via ALERT_RULES inside FormSubmissionTriggersService.

                // Action 5: Auto-activate next stage (if not final and next stage is assigned)
                if (!stage.IsFinalStage && submission.StageId.HasValue && submission.StageId.Value != dto.StageId)
                {
                    // Next stage is already assigned, trigger ApprovalRequired for next stage (NEW scope)
                    var submissionIdCopy = submission.Id;
                    var nextStageIdCopy = submission.StageId.Value;
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            // Resolve approvers for next stage
                            var nextStageApprovers = await ResolveApproversForStageAsync(nextStageIdCopy);
                            if (nextStageApprovers.StatusCode == 200)
                            {
                                var nextResolvedApprovers = nextStageApprovers.Data as ResolvedApproversDto;

                                if (nextResolvedApprovers?.UserIds != null && nextResolvedApprovers.UserIds.Any())
                                {
                                    // IMPORTANT: Do NOT send emails directly here.
                                    // Email delivery is controlled ONLY via ALERT_RULES inside FormSubmissionTriggersService.
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, "Failed to auto-activate next stage for submission {SubmissionId}", submissionIdCopy);
                        }
                    });
                }
            }
            else if (dto.ActionType.ToLower() == "rejected")
            {
                // ✅ TRIGGER: ApprovalRejected - Execute Actions
                // Action 1: Status already updated to "Rejected" ✅
                // Action 2: Document locking - Status = "Rejected" will be checked in edit methods ✅
                // IMPORTANT: Do NOT send emails directly here.
                // Email delivery is controlled ONLY via ALERT_RULES inside FormSubmissionTriggersService.
            }
            else if (dto.ActionType.ToLower() == "returned")
            {
                // ✅ TRIGGER: ApprovalReturned - Execute Actions
                // Action 1: Status already updated (Draft or previous stage) ✅
                // Action 2: Form is unlocked (Status = "Draft" allows editing) ✅
                // IMPORTANT: Do NOT send emails directly here.
                // Email delivery is controlled ONLY via ALERT_RULES inside FormSubmissionTriggersService.
            }
            else
            {
                // Send email for other action types
                if (_emailNotificationService != null)
                {
                    FireAndForgetEmail(
                        s => s.SendApprovalResultAsync(submission.Id, actionType, actualUserId, dto.Comments),
                        $"ApprovalResult {actionType} submissionId={submission.Id}");
                }
            }

            return new ApiResponse(200, $"Action '{dto.ActionType}' processed successfully", new
            {
                SubmissionId = submission.Id,
                Status = submission.Status,
                ActionType = actionType,
                SignatureRequested = signatureRequestedForCurrentStage
            });
        }

        public async Task<ApiResponse> RequestStageSignatureAsync(RequestStageSignatureDto dto)
        {
            if (dto == null)
                return new ApiResponse(400, "Request body is required");

            if (dto.SubmissionId <= 0)
                return new ApiResponse(400, "SubmissionId must be greater than 0");

            if (dto.StageId <= 0)
                return new ApiResponse(400, "StageId must be greater than 0");

            var submission = await _unitOfWork.FormSubmissionsRepository.SingleOrDefaultAsync(
                s => s.Id == dto.SubmissionId && !s.IsDeleted,
                asNoTracking: false);

            if (submission == null)
                return new ApiResponse(404, "Submission not found");

            var stage = await _unitOfWork.ApprovalStageRepository.GetByIdAsync(dto.StageId);
            if (stage == null || !stage.IsActive)
                return new ApiResponse(404, "Stage not found or inactive");

            if (!stage.RequiresAdobeSign)
                return new ApiResponse(400, "This stage does not require DocuSign");

            if (submission.StageId != stage.Id)
            {
                return new ApiResponse(400,
                    $"Cannot request signature for stage {stage.Id}. Current submission stage is {(submission.StageId.HasValue ? submission.StageId.Value : 0)}.");
            }

            var requested = await CreateDocuSignEnvelopeIfNeededAsync(submission, stage, dto.RequestedByUserId);
            if (!requested)
                return new ApiResponse(500, "Failed to create DocuSign envelope. Check DocuSign configuration and signer data.");

            return new ApiResponse(200, "DocuSign envelope requested successfully", new
            {
                dto.SubmissionId,
                dto.StageId,
                RequestedByUserId = string.IsNullOrWhiteSpace(dto.RequestedByUserId) ? "system" : dto.RequestedByUserId
            });
        }

        private async Task<ApiResponse?> ValidateStageAmountForSubmissionAsync(APPROVAL_STAGES stage, int submissionId)
        {
            if (!stage.MinAmount.HasValue && !stage.MaxAmount.HasValue)
                return null;

            // Defensive: should already be enforced on stage create/update, but runtime shouldn't accept invalid config.
            if (stage.MinAmount.HasValue && stage.MaxAmount.HasValue && stage.MinAmount.Value >= stage.MaxAmount.Value)
                return new ApiResponse(400, "Stage amount constraints are invalid (MinAmount must be less than MaxAmount)");

            if (string.IsNullOrWhiteSpace(stage.AmountFieldCode))
                return new ApiResponse(400, "Stage amount validation is configured but AmountFieldCode is missing");

            var values = await _unitOfWork.FormSubmissionValuesRepository.GetBySubmissionIdAsync(submissionId);
            var match = values.FirstOrDefault(v =>
                string.Equals(v.FieldCode?.Trim(), stage.AmountFieldCode.Trim(), StringComparison.OrdinalIgnoreCase));

            if (match == null)
                return new ApiResponse(400, $"Amount field '{stage.AmountFieldCode}' was not found on submission");

            if (!match.ValueNumber.HasValue)
                return new ApiResponse(400, $"Amount field '{stage.AmountFieldCode}' must be numeric");

            var amount = match.ValueNumber.Value;
            if (stage.MinAmount.HasValue && amount < stage.MinAmount.Value)
                return new ApiResponse(400, $"Amount must be >= {stage.MinAmount.Value}");

            if (stage.MaxAmount.HasValue && amount > stage.MaxAmount.Value)
                return new ApiResponse(400, $"Amount must be <= {stage.MaxAmount.Value}");

            return null;
        }

        /// <summary>
        /// Gets approval inbox for a user (documents waiting for their approval)
        /// </summary>
        public async Task<ApiResponse> GetApprovalInboxAsync(string userId)
        {
            _logger?.LogInformation("=== GetApprovalInboxAsync START ===");
            _logger?.LogInformation("Received userId: '{UserId}' (Type: {Type}, Length: {Length})",
                userId, userId?.GetType().Name ?? "null", userId?.Length ?? 0);

            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger?.LogWarning("UserId is null or empty");
                return new ApiResponse(400, "UserId is required");
            }

            // Normalize userId to string for consistent comparison
            var normalizedUserId = userId.Trim();
            _logger?.LogInformation("Normalized userId: '{NormalizedUserId}'", normalizedUserId);

            // ✅ NEW: Try to resolve username to numeric UserId from Tbl_User
            var numericUserId = normalizedUserId;
            if (!int.TryParse(normalizedUserId, out int parsedUserId))
            {
                // Input is not numeric, try to find user by username
                _logger?.LogInformation("Input '{NormalizedUserId}' is not numeric, attempting to resolve username...", normalizedUserId);
                var user = await _identityContext.TblUsers
                    .FirstOrDefaultAsync(u => u.Username.ToLower() == normalizedUserId.ToLower());

                if (user != null)
                {
                    numericUserId = user.Id.ToString();
                    _logger?.LogInformation("✓ Resolved username '{Username}' to UserId '{NumericUserId}' (User.Id={UserId})",
                        normalizedUserId, numericUserId, user.Id);
                }
                else
                {
                    _logger?.LogWarning("✗ Could not find user with username '{Username}' in Tbl_User. Will use '{NormalizedUserId}' as-is for comparison", normalizedUserId);
                }
            }
            else
            {
                // Input is already numeric
                numericUserId = parsedUserId.ToString();
                _logger?.LogInformation("Input '{NormalizedUserId}' is already numeric: '{NumericUserId}'", normalizedUserId, numericUserId);
            }

            _logger?.LogInformation("=== UserId Resolution Summary ===");
            _logger?.LogInformation("Original Input: '{NormalizedUserId}'", normalizedUserId);
            _logger?.LogInformation("Numeric UserId: '{NumericUserId}'", numericUserId);
            _logger?.LogInformation("Will compare with both values in approvers list");

            // Get all Stage Assignees for debugging
            var allAssignees = await _unitOfWork.ApprovalStageAssigneesRepository.GetAllAsync(a => a.IsActive);
            _logger?.LogInformation("Total active assignees in system: {Count}", allAssignees.Count());

            // Log all unique UserIds in the system
            var systemUserIds = allAssignees
                .Where(a => !string.IsNullOrWhiteSpace(a.UserId))
                .Select(a => a.UserId?.ToString() ?? "null")
                .Distinct()
                .ToList();

            _logger?.LogInformation("All UserIds in system: {UserIds}", string.Join(", ", systemUserIds));

            // Try to find user in assignees (for debugging) - compare with both original and numeric userId
            var matchingAssignees = allAssignees.Where(a =>
                !string.IsNullOrWhiteSpace(a.UserId) &&
                (string.Equals(a.UserId?.ToString()?.Trim(), normalizedUserId, StringComparison.OrdinalIgnoreCase) ||
                 string.Equals(a.UserId?.ToString()?.Trim(), numericUserId, StringComparison.OrdinalIgnoreCase))
            ).ToList();

            _logger?.LogInformation("Found {Count} matching assignees for userId '{NormalizedUserId}' or '{NumericUserId}'",
                matchingAssignees.Count, normalizedUserId, numericUserId);

            if (matchingAssignees.Any())
            {
                var matchingInfo = matchingAssignees.Select(a => new
                {
                    a.Id,
                    a.StageId,
                    UserId = a.UserId?.ToString() ?? "null",
                    a.RoleId
                }).ToList();
                _logger?.LogInformation("Matching assignees: {Assignees}", JsonSerializer.Serialize(matchingInfo));
            }
            else
            {
                _logger?.LogWarning("No matching assignees found for userId '{NormalizedUserId}'", normalizedUserId);
                _logger?.LogWarning("This might be why inbox is empty!");
            }

            // Get all active stages
            var allStages = await _unitOfWork.ApprovalStageRepository.GetAllAsync(s => s.IsActive);
            _logger?.LogInformation("Found {Count} active stages", allStages.Count());

            // Log all stages for debugging
            var stagesInfo = allStages.Select(s => new
            {
                s.Id,
                s.StageName,
                s.WorkflowId,
                s.StageOrder,
                s.IsActive
            }).ToList();
            _logger?.LogInformation("Active stages: {Stages}", JsonSerializer.Serialize(stagesInfo));

            var inboxItems = new List<ApprovalInboxDto>();

            foreach (var stage in allStages)
            {
                _logger?.LogInformation("Processing stage {StageId} ({StageName})", stage.Id, stage.StageName);

                // Resolve approvers for this stage
                var approversResult = await ResolveApproversForStageAsync(stage.Id);
                if (approversResult.StatusCode != 200)
                {
                    _logger?.LogWarning("Failed to resolve approvers for stage {StageId}: {StatusCode} - {Message}",
                        stage.Id, approversResult.StatusCode, approversResult.Message);
                    continue;
                }

                var resolvedApprovers = approversResult.Data as ResolvedApproversDto;
                if (resolvedApprovers == null)
                {
                    _logger?.LogWarning("Resolved approvers data is null for stage {StageId}", stage.Id);
                    continue;
                }

                // Log resolved approvers for debugging
                _logger?.LogInformation("Stage {StageId} resolved approvers: {UserIds}",
                    stage.Id, string.Join(", ", resolvedApprovers.UserIds ?? Array.Empty<string>()));

                // Check if user is in the resolved approvers list (case-insensitive comparison)
                // ✅ Compare with both username and numeric userId
                var isUserAssigned = false;
                if (resolvedApprovers.UserIds != null && resolvedApprovers.UserIds.Any())
                {
                    foreach (var approverId in resolvedApprovers.UserIds)
                    {
                        var trimmedApproverId = approverId?.Trim() ?? string.Empty;
                        var matchesNormalized = string.Equals(trimmedApproverId, normalizedUserId, StringComparison.OrdinalIgnoreCase);
                        var matchesNumeric = string.Equals(trimmedApproverId, numericUserId, StringComparison.OrdinalIgnoreCase);

                        if (matchesNormalized || matchesNumeric)
                        {
                            isUserAssigned = true;
                            _logger?.LogInformation("✓ Match found! ApproverId '{ApproverId}' matches UserId '{NormalizedUserId}' or NumericUserId '{NumericUserId}'",
                                trimmedApproverId, normalizedUserId, numericUserId);
                            break;
                        }
                    }

                    if (!isUserAssigned)
                    {
                        _logger?.LogWarning("✗ No match found. Looking for UserId '{NormalizedUserId}' or '{NumericUserId}' in approvers: {ApproverIds}",
                            normalizedUserId, numericUserId, string.Join(", ", resolvedApprovers.UserIds));
                    }
                }
                else
                {
                    _logger?.LogWarning("Resolved approvers list is null or empty for stage {StageId}", stage.Id);
                }

                // ✅ NEW: Check for delegations if user is not directly assigned
                // This handles Document-level, Workflow-level, and Global delegations
                if (!isUserAssigned && resolvedApprovers.UserIds != null && resolvedApprovers.UserIds.Any())
                {
                    _logger?.LogInformation("User {UserId} is not directly assigned. Checking for delegations...", normalizedUserId);

                    // We'll check delegations later when we have the submission context
                    // For now, we'll continue to get submissions and check delegations per submission
                }

                _logger?.LogInformation("User {UserId} (numeric: {NumericUserId}) is assigned to stage {StageId}: {IsAssigned}",
                    normalizedUserId, numericUserId, stage.Id, isUserAssigned);

                // Get submissions in this stage's workflow that are in Submitted status
                var workflow = await _unitOfWork.ApprovalWorkflowRepository.GetByIdAsync(stage.WorkflowId);

                // If workflow not found (might be deleted), check directly in database (including deleted)
                if (workflow == null)
                {
                    var workflowDirect = await _unitOfWork.ApprovalWorkflowRepository.GetAll()
                        .Where(w => w.Id == stage.WorkflowId)
                        .FirstOrDefaultAsync();

                    if (workflowDirect != null)
                    {
                        workflow = workflowDirect;
                        _logger?.LogWarning("Workflow {WorkflowId} found but deleted (IsDeleted={IsDeleted}, IsActive={IsActive}). Using it anyway for document type query.",
                            stage.WorkflowId, workflowDirect.IsDeleted, workflowDirect.IsActive);
                    }
                    else
                    {
                        _logger?.LogWarning("Workflow {WorkflowId} not found for stage {StageId}", stage.WorkflowId, stage.Id);
                        continue;
                    }
                }

                // Prefer required FK (workflow.DocumentTypeId). Keep a fallback to legacy reverse link
                // DOCUMENT_TYPES.ApprovalWorkflowId which may be null in existing data.
                var documentTypes = await _unitOfWork.DocumentTypeRepository.GetAll()
                    .Where(dt => (dt.Id == workflow.DocumentTypeId || dt.ApprovalWorkflowId == workflow.Id) && !dt.IsDeleted && dt.IsActive)
                    .ToListAsync();

                var documentTypeIds = documentTypes.Select(dt => dt.Id).ToList();
                _logger?.LogInformation("=== Stage {StageId} (WorkflowId={WorkflowId}) Processing ===", stage.Id, workflow.Id);
                _logger?.LogInformation("Found {Count} document types for workflow {WorkflowId}: {DocumentTypeIds}",
                    documentTypeIds.Count, workflow.Id, string.Join(", ", documentTypeIds));

                if (documentTypeIds.Count == 0)
                {
                    _logger?.LogWarning("⚠️ Stage {StageId}: No document types found for workflow {WorkflowId}. Skipping submissions check.", stage.Id, workflow.Id);
                    continue;
                }

                // Log document type details
                foreach (var dt in documentTypes)
                {
                    _logger?.LogInformation("DocumentType: Id={Id}, Name={Name}, ApprovalWorkflowId={WorkflowId}",
                        dt.Id, dt.Name, dt.ApprovalWorkflowId);
                }

                var submissions = await _unitOfWork.FormSubmissionsRepository.GetAllAsync(
                    s => documentTypeIds.Contains(s.DocumentTypeId) && s.Status == "Submitted");

                _logger?.LogInformation("Found {Count} submitted submissions for workflow {WorkflowId}", submissions.Count(), workflow.Id);

                // Log submission details
                foreach (var sub in submissions)
                {
                    _logger?.LogInformation("Submission: Id={Id}, DocumentNumber={DocNumber}, DocumentTypeId={DocTypeId}, Status={Status}",
                        sub.Id, sub.DocumentNumber, sub.DocumentTypeId, sub.Status);
                }

                foreach (var submission in submissions)
                {
                    _logger?.LogInformation("=== Checking submission {SubmissionId} (DocumentNumber: {DocumentNumber}) for stage {StageId} (WorkflowId: {WorkflowId}) ===",
                        submission.Id, submission.DocumentNumber, stage.Id, stage.WorkflowId);

                    // Check if this submission is in the current stage
                    var history = await _unitOfWork.DocumentApprovalHistoryRepository
                        .GetBySubmissionIdAsync(submission.Id);

                    var lastAction = history.OrderByDescending(h => h.ActionDate).FirstOrDefault();

                    if (lastAction != null)
                    {
                        _logger?.LogInformation("Submission {SubmissionId} last action: StageId={LastStageId}, ActionType={ActionType}, ActionDate={ActionDate}",
                            submission.Id, lastAction.StageId, lastAction.ActionType, lastAction.ActionDate);
                    }
                    else
                    {
                        _logger?.LogInformation("✓ Submission {SubmissionId} has no history (new submission) - should be in first stage", submission.Id);
                    }

                    // Determine if this submission is in the current stage
                    bool isCurrentStage = false;

                    if (lastAction == null)
                    {
                        // No history means this is a new submission - should be in first stage
                        // Get the first stage of this workflow
                        // Reuse the workflow variable from the outer scope (line 546)
                        _logger?.LogInformation("Submission {SubmissionId} has no approval history. Checking if stage {StageId} is the first stage of workflow {WorkflowId}",
                            submission.Id, stage.Id, workflow?.Id ?? 0);

                        if (workflow == null)
                        {
                            _logger?.LogWarning("✗ Workflow {WorkflowId} not found for stage {StageId}", stage.WorkflowId, stage.Id);
                            isCurrentStage = false;
                        }
                        else
                        {
                            var allStagesInWorkflow = await _unitOfWork.ApprovalStageRepository.GetAllAsync(s =>
                                s.WorkflowId == workflow.Id && s.IsActive);
                            var firstStage = allStagesInWorkflow.OrderBy(s => s.StageOrder).FirstOrDefault();

                            _logger?.LogInformation("Workflow {WorkflowId} has {Count} active stages. First stage: Id={FirstStageId}, Order={FirstStageOrder}",
                                workflow.Id, allStagesInWorkflow.Count(), firstStage?.Id ?? 0, firstStage?.StageOrder ?? 0);

                            if (firstStage == null)
                            {
                                _logger?.LogWarning("✗ No active stages found in workflow {WorkflowId}", workflow.Id);
                                isCurrentStage = false;
                            }
                            else
                            {
                                // Check if current stage is the first stage
                                isCurrentStage = stage.Id == firstStage.Id;
                                _logger?.LogInformation("Submission {SubmissionId} has no history. First stage: Id={FirstStageId}, Order={FirstStageOrder}, Name={FirstStageName}. Current stage: Id={CurrentStageId}, Order={CurrentStageOrder}, Name={CurrentStageName}. IsCurrentStage: {IsCurrentStage}",
                                    submission.Id,
                                    firstStage.Id, firstStage.StageOrder, firstStage.StageName,
                                    stage.Id, stage.StageOrder, stage.StageName,
                                    isCurrentStage);

                                if (!isCurrentStage)
                                {
                                    _logger?.LogWarning("✗ Submission {SubmissionId} is NOT in first stage. First stage ID={FirstStageId}, Current stage ID={CurrentStageId}",
                                        submission.Id, firstStage.Id, stage.Id);
                                }
                                else
                                {
                                    _logger?.LogInformation("✓ Submission {SubmissionId} IS in first stage (StageId={StageId})", submission.Id, stage.Id);
                                }
                            }
                        }
                    }
                    else if (lastAction.StageId == stage.Id)
                    {
                        // Last action was on this stage - check if it was approved (should move to next) or still pending
                        // If last action was "Approved", submission should have moved to next stage
                        // If last action was "Rejected" or "Returned", it might still be in this stage
                        if (lastAction.ActionType?.ToLower() == "approved")
                        {
                            isCurrentStage = false; // Already approved, should be in next stage
                            _logger?.LogInformation("Submission {SubmissionId} was already approved at stage {StageId}, not current",
                                submission.Id, stage.Id);
                        }
                        else
                        {
                            // Rejected or Returned - might still be in this stage
                            isCurrentStage = true;
                            _logger?.LogInformation("Submission {SubmissionId} last action was {ActionType} at stage {StageId}, still current",
                                submission.Id, lastAction.ActionType, stage.Id);
                        }
                    }
                    else
                    {
                        // Last action was on a different stage
                        // Get the stage of the last action to compare stage orders
                        var lastActionStage = await _unitOfWork.ApprovalStageRepository.GetByIdAsync(lastAction.StageId);
                        var currentStageForComparison = await _unitOfWork.ApprovalStageRepository.GetByIdAsync(stage.Id);

                        if (lastActionStage == null || currentStageForComparison == null)
                        {
                            _logger?.LogWarning("Could not find stage information. LastActionStageId={LastStageId}, CurrentStageId={CurrentStageId}",
                                lastAction.StageId, stage.Id);
                            isCurrentStage = false;
                        }
                        else if (lastActionStage.WorkflowId != currentStageForComparison.WorkflowId)
                        {
                            // Different workflows - not current
                            _logger?.LogWarning("Last action was on different workflow. LastWorkflowId={LastWorkflowId}, CurrentWorkflowId={CurrentWorkflowId}",
                                lastActionStage.WorkflowId, currentStageForComparison.WorkflowId);
                            isCurrentStage = false;
                        }
                        else
                        {
                            // Same workflow - check if last action was on a previous stage
                            // If last action was "Approved" on a previous stage, current stage should be active
                            // If last action was "Rejected" or "Returned", it might still be in the previous stage
                            bool isPreviousStage = lastActionStage.StageOrder < currentStageForComparison.StageOrder;

                            if (isPreviousStage && lastAction.ActionType?.ToLower() == "approved")
                            {
                                // Last action was approved on a previous stage - current stage should be active
                                isCurrentStage = true;
                                _logger?.LogInformation("Submission {SubmissionId} was approved at previous stage {LastStageId} (Order: {LastOrder}), current stage {CurrentStageId} (Order: {CurrentOrder}) is active",
                                    submission.Id, lastActionStage.Id, lastActionStage.StageOrder, currentStageForComparison.Id, currentStageForComparison.StageOrder);
                            }
                            else if (isPreviousStage)
                            {
                                // Last action was rejected/returned on a previous stage - might still be in previous stage
                                isCurrentStage = false;
                                _logger?.LogInformation("Submission {SubmissionId} last action was {ActionType} at previous stage {LastStageId} (Order: {LastOrder}), not in current stage {CurrentStageId} (Order: {CurrentOrder})",
                                    submission.Id, lastAction.ActionType, lastActionStage.Id, lastActionStage.StageOrder, currentStageForComparison.Id, currentStageForComparison.StageOrder);
                            }
                            else
                            {
                                // Last action was on a future stage - not current
                                isCurrentStage = false;
                                _logger?.LogInformation("Submission {SubmissionId} last action was on future stage {LastStageId} (Order: {LastOrder}), not in current stage {CurrentStageId} (Order: {CurrentOrder})",
                                    submission.Id, lastActionStage.Id, lastActionStage.StageOrder, currentStageForComparison.Id, currentStageForComparison.StageOrder);
                            }
                        }
                    }

                    _logger?.LogInformation("Submission {SubmissionId} (DocumentNumber: {DocumentNumber}) is in current stage {StageId}: {IsCurrentStage}",
                        submission.Id, submission.DocumentNumber, stage.Id, isCurrentStage);

                    if (isCurrentStage)
                    {
                        // ✅ Check if user is assigned directly OR through delegation
                        bool userCanApprove = isUserAssigned;

                        // If user is not directly assigned, check for delegations
                        if (!isUserAssigned && resolvedApprovers.UserIds != null && resolvedApprovers.UserIds.Any())
                        {
                            _logger?.LogInformation("Checking delegations for submission {SubmissionId}...", submission.Id);

                            // Check each original approver for delegations
                            foreach (var originalApproverId in resolvedApprovers.UserIds)
                            {
                                var resolvedDelegatedUserId = await _delegationService.ResolveDelegatedApproverAsync(
                                    originalApproverId,
                                    stage.WorkflowId,
                                    submission.Id);

                                if (resolvedDelegatedUserId != null)
                                {
                                    var delegatedUserIdNormalized = resolvedDelegatedUserId.Trim();

                                    // ✅ Enhanced comparison: Handle both numeric and username formats
                                    bool matches = false;

                                    // 1. Direct string comparison (case-insensitive)
                                    var matchesNormalized = string.Equals(delegatedUserIdNormalized, normalizedUserId, StringComparison.OrdinalIgnoreCase);
                                    var matchesNumeric = string.Equals(delegatedUserIdNormalized, numericUserId, StringComparison.OrdinalIgnoreCase);

                                    if (matchesNormalized || matchesNumeric)
                                    {
                                        matches = true;
                                    }
                                    else
                                    {
                                        // 2. If delegatedUserId is numeric, try to resolve it to username and compare
                                        if (int.TryParse(delegatedUserIdNormalized, out int delegatedUserIdInt))
                                        {
                                            var delegatedUser = await _identityContext.TblUsers
                                                .FirstOrDefaultAsync(u => u.Id == delegatedUserIdInt);

                                            if (delegatedUser != null)
                                            {
                                                var delegatedUsername = delegatedUser.Username?.Trim() ?? string.Empty;
                                                matches = string.Equals(delegatedUsername, normalizedUserId, StringComparison.OrdinalIgnoreCase);

                                                if (matches)
                                                {
                                                    _logger?.LogInformation("✓ DelegatedUserId '{DelegatedUserId}' (numeric) resolved to username '{DelegatedUsername}' which matches '{NormalizedUserId}'",
                                                        delegatedUserIdNormalized, delegatedUsername, normalizedUserId);
                                                }
                                            }

                                            // Also check if numericUserId matches
                                            if (!matches && string.Equals(delegatedUserIdNormalized, numericUserId, StringComparison.OrdinalIgnoreCase))
                                            {
                                                matches = true;
                                            }
                                        }
                                        // 3. If normalizedUserId is numeric, try to resolve delegatedUserId to numeric and compare
                                        else if (int.TryParse(normalizedUserId, out int normalizedUserIdInt))
                                        {
                                            // This case is already handled by matchesNumeric above
                                        }
                                        // 4. If both are usernames, try to resolve both to numeric IDs and compare
                                        else
                                        {
                                            var delegatedUser = await _identityContext.TblUsers
                                                .FirstOrDefaultAsync(u => u.Username.ToLower() == delegatedUserIdNormalized.ToLower());

                                            if (delegatedUser != null)
                                            {
                                                var delegatedUserNumericId = delegatedUser.Id.ToString();
                                                matches = string.Equals(delegatedUserNumericId, numericUserId, StringComparison.OrdinalIgnoreCase);

                                                if (matches)
                                                {
                                                    _logger?.LogInformation("✓ DelegatedUserId '{DelegatedUserId}' (username) resolved to numeric ID '{DelegatedUserNumericId}' which matches '{NumericUserId}'",
                                                        delegatedUserIdNormalized, delegatedUserNumericId, numericUserId);
                                                }
                                            }
                                        }
                                    }

                                    if (matches)
                                    {
                                        userCanApprove = true;
                                        _logger?.LogInformation("✓ Found delegation! Original approver '{OriginalApproverId}' delegated to '{DelegatedUserId}' which matches user '{UserId}' (normalized: '{NormalizedUserId}', numeric: '{NumericUserId}')",
                                            originalApproverId, resolvedDelegatedUserId, normalizedUserId, normalizedUserId, numericUserId);
                                        break;
                                    }
                                    else
                                    {
                                        _logger?.LogInformation("✗ Delegation found but ToUserId '{DelegatedUserId}' does not match user '{NormalizedUserId}' (numeric: '{NumericUserId}')",
                                            resolvedDelegatedUserId, normalizedUserId, numericUserId);
                                    }
                                }
                            }
                        }

                        // Only add to inbox if user can approve (directly or through delegation)
                        if (userCanApprove)
                        {
                            var documentType = await _unitOfWork.DocumentTypeRepository.GetByIdAsync(submission.DocumentTypeId);

                            inboxItems.Add(new ApprovalInboxDto
                            {
                                SubmissionId = submission.Id,
                                DocumentNumber = submission.DocumentNumber,
                                DocumentTypeName = documentType?.Name ?? "",
                                StageId = stage.Id,
                                StageName = stage.StageName,
                                StageOrder = stage.StageOrder,
                                SubmittedDate = submission.SubmittedDate,
                                SubmittedByUserId = submission.SubmittedByUserId,
                                SubmittedByUserName = "", // TODO: Fetch from users table if needed
                                Status = submission.Status,
                                IsAssigned = true, // ✅ User is assigned (directly or through delegation)
                                Approvers = resolvedApprovers?.UserIds ?? Array.Empty<string>(),
                                CanApprove = true // ✅ User can approve/reject
                            });

                            _logger?.LogInformation("✓ Added submission {SubmissionId} to inbox for user {UserId} at stage {StageId} (directly assigned: {IsDirectlyAssigned})",
                                submission.Id, normalizedUserId, stage.Id, isUserAssigned);
                        }
                        else
                        {
                            _logger?.LogInformation("✗ User {UserId} cannot approve submission {SubmissionId} (not assigned and no delegation found)",
                                normalizedUserId, submission.Id);
                        }
                    }
                    else
                    {
                        _logger?.LogInformation("✗ Submission {SubmissionId} is NOT in current stage {StageId}, skipping",
                            submission.Id, stage.Id);
                    }
                }
            }

            // ✅ NEW: Check for Document-level delegations directly
            // This handles cases where submissions have Document-level delegations
            // but their document types are not linked to the workflow we processed above
            _logger?.LogInformation("=== Checking Document-level Delegations Directly ===");

            // Get all active delegations for this user (ToUserId) - try both normalizedUserId and numericUserId
            var activeDelegationsByNumeric = await _unitOfWork.ApprovalDelegationRepository.GetActiveDelegationsByToUserIdAsync(numericUserId);
            var activeDelegationsByNormalized = await _unitOfWork.ApprovalDelegationRepository.GetActiveDelegationsByToUserIdAsync(normalizedUserId);

            // Combine and deduplicate
            var allActiveDelegations = activeDelegationsByNumeric
                .Concat(activeDelegationsByNormalized)
                .GroupBy(d => d.Id)
                .Select(g => g.First())
                .ToList();

            _logger?.LogInformation("Found {Count} total active delegations for user {UserId} (numeric: {NumericUserId})",
                allActiveDelegations.Count, normalizedUserId, numericUserId);

            // Log delegations by numeric userId
            _logger?.LogInformation("Delegations by numeric userId '{NumericUserId}': {Count}",
                numericUserId, activeDelegationsByNumeric.Count());
            foreach (var del in activeDelegationsByNumeric)
            {
                _logger?.LogInformation("  - Delegation Id={Id}, FromUserId={FromUserId}, ToUserId={ToUserId}, ScopeType={ScopeType}, ScopeId={ScopeId}",
                    del.Id, del.FromUserId, del.ToUserId, del.ScopeType, del.ScopeId);
            }

            // Log delegations by normalized userId
            _logger?.LogInformation("Delegations by normalized userId '{NormalizedUserId}': {Count}",
                normalizedUserId, activeDelegationsByNormalized.Count());
            foreach (var del in activeDelegationsByNormalized)
            {
                _logger?.LogInformation("  - Delegation Id={Id}, FromUserId={FromUserId}, ToUserId={ToUserId}, ScopeType={ScopeType}, ScopeId={ScopeId}",
                    del.Id, del.FromUserId, del.ToUserId, del.ScopeType, del.ScopeId);
            }

            var documentLevelDelegations = allActiveDelegations
                .Where(d => d.ScopeType == "Document" && d.ScopeId.HasValue)
                .ToList();

            _logger?.LogInformation("Found {Count} Document-level delegations for user {UserId}",
                documentLevelDelegations.Count, normalizedUserId);

            // Log all document-level delegations
            foreach (var del in documentLevelDelegations)
            {
                _logger?.LogInformation("Document-level delegation: Id={Id}, FromUserId={FromUserId}, ToUserId={ToUserId}, ScopeId={ScopeId}, IsActive={IsActive}, StartDate={StartDate}, EndDate={EndDate}",
                    del.Id, del.FromUserId, del.ToUserId, del.ScopeId, del.IsActive, del.StartDate, del.EndDate);
            }

            if (documentLevelDelegations.Count == 0)
            {
                _logger?.LogWarning("⚠️ No Document-level delegations found! This might be why submissions are not appearing.");
                _logger?.LogWarning("Check if delegations exist with ToUserId='{NormalizedUserId}' or ToUserId='{NumericUserId}'",
                    normalizedUserId, numericUserId);
            }

            foreach (var delegation in documentLevelDelegations)
            {
                var submissionId = delegation.ScopeId.Value;
                _logger?.LogInformation("=== Processing Document-level delegation ===");
                _logger?.LogInformation("Delegation: Id={Id}, SubmissionId={SubmissionId}, FromUserId={FromUserId}, ToUserId={ToUserId}, ScopeType={ScopeType}, IsActive={IsActive}",
                    delegation.Id, submissionId, delegation.FromUserId, delegation.ToUserId, delegation.ScopeType, delegation.IsActive);

                // Check if this submission is already in inbox
                if (inboxItems.Any(item => item.SubmissionId == submissionId))
                {
                    _logger?.LogInformation("✓ Submission {SubmissionId} already in inbox, skipping", submissionId);
                    continue;
                }

                // Get the submission
                var submission = await _unitOfWork.FormSubmissionsRepository.GetByIdAsync(submissionId);
                if (submission == null)
                {
                    _logger?.LogWarning("✗ Submission {SubmissionId} not found in database", submissionId);
                    continue;
                }

                _logger?.LogInformation("Submission found: Id={Id}, DocumentNumber={DocumentNumber}, DocumentTypeId={DocumentTypeId}, Status={Status}, SubmittedByUserId={SubmittedByUserId}, IsDeleted={IsDeleted}",
                    submission.Id, submission.DocumentNumber, submission.DocumentTypeId, submission.Status, submission.SubmittedByUserId, submission.IsDeleted);

                // Check if submission is deleted
                if (submission.IsDeleted)
                {
                    _logger?.LogInformation("✗ Submission {SubmissionId} is deleted, skipping", submissionId);
                    continue;
                }

                if (submission.Status != "Submitted")
                {
                    _logger?.LogInformation("✗ Submission {SubmissionId} status is '{Status}', not 'Submitted', skipping",
                        submissionId, submission.Status);
                    continue;
                }

                // Get the submission's workflow and current stage
                var submissionDocumentType = await _unitOfWork.DocumentTypeRepository.GetByIdAsync(submission.DocumentTypeId);
                if (submissionDocumentType == null)
                {
                    _logger?.LogWarning("DocumentType {DocumentTypeId} not found for submission {SubmissionId}",
                        submission.DocumentTypeId, submissionId);
                    continue;
                }

                // Find the workflow for this document type
                int? workflowId = null;
                if (submissionDocumentType.ApprovalWorkflowId.HasValue)
                {
                    workflowId = submissionDocumentType.ApprovalWorkflowId.Value;
                }
                else
                {
                    // Try to find workflow by DocumentTypeId
                    var workflowByDocType = await _unitOfWork.ApprovalWorkflowRepository.GetAll()
                        .Where(w => w.DocumentTypeId == submission.DocumentTypeId && w.IsActive && !w.IsDeleted)
                        .FirstOrDefaultAsync();
                    if (workflowByDocType != null)
                    {
                        workflowId = workflowByDocType.Id;
                    }
                }

                if (!workflowId.HasValue)
                {
                    _logger?.LogWarning("No workflow found for submission {SubmissionId} (DocumentTypeId={DocumentTypeId}, ApprovalWorkflowId={ApprovalWorkflowId})",
                        submissionId, submission.DocumentTypeId, submissionDocumentType.ApprovalWorkflowId);

                    // Try to find any active workflow that might be related
                    var allWorkflows = await _unitOfWork.ApprovalWorkflowRepository.GetAll()
                        .Where(w => w.IsActive && !w.IsDeleted)
                        .ToListAsync();

                    _logger?.LogInformation("Available workflows in system: {Workflows}",
                        string.Join(", ", allWorkflows.Select(w => $"Id={w.Id}, DocumentTypeId={w.DocumentTypeId}")));

                    // For Document-level delegation, we can still proceed if we can find any stage
                    // Let's try to find stages that might be related to this submission
                    _logger?.LogInformation("Attempting to find stages without workflow for submission {SubmissionId}", submissionId);
                    continue;
                }

                // Get the current stage for this submission
                var history = await _unitOfWork.DocumentApprovalHistoryRepository.GetBySubmissionIdAsync(submissionId);
                var lastAction = history.OrderByDescending(h => h.ActionDate).FirstOrDefault();

                APPROVAL_STAGES currentStage = null;
                if (lastAction == null)
                {
                    // No history - should be in first stage
                    var allStagesInWorkflow = await _unitOfWork.ApprovalStageRepository.GetAllAsync(s =>
                        s.WorkflowId == workflowId.Value && s.IsActive);
                    currentStage = allStagesInWorkflow.OrderBy(s => s.StageOrder).FirstOrDefault();
                }
                else
                {
                    // Get the stage from last action
                    currentStage = await _unitOfWork.ApprovalStageRepository.GetByIdAsync(lastAction.StageId);

                    // If last action was approved, get next stage
                    if (lastAction.ActionType?.ToLower() == "approved" && currentStage != null)
                    {
                        var allStagesInWorkflow = await _unitOfWork.ApprovalStageRepository.GetAllAsync(s =>
                            s.WorkflowId == workflowId.Value && s.IsActive);
                        var nextStage = allStagesInWorkflow
                            .Where(s => s.StageOrder > currentStage.StageOrder)
                            .OrderBy(s => s.StageOrder)
                            .FirstOrDefault();
                        if (nextStage != null)
                        {
                            currentStage = nextStage;
                        }
                    }
                }

                if (currentStage == null)
                {
                    _logger?.LogWarning("Could not determine current stage for submission {SubmissionId}", submissionId);
                    continue;
                }

                // Resolve approvers for this stage
                var approversResult = await ResolveApproversForStageAsync(currentStage.Id);
                if (approversResult.StatusCode != 200)
                {
                    _logger?.LogWarning("Failed to resolve approvers for stage {StageId}: {StatusCode} - {Message}",
                        currentStage.Id, approversResult.StatusCode, approversResult.Message);
                    continue;
                }

                var resolvedApprovers = approversResult.Data as ResolvedApproversDto;
                if (resolvedApprovers == null || resolvedApprovers.UserIds == null || !resolvedApprovers.UserIds.Any())
                {
                    _logger?.LogWarning("No approvers found for stage {StageId}", currentStage.Id);
                    continue;
                }

                // Check if the original approver (FromUserId) is in the resolved approvers
                var originalApproverId = delegation.FromUserId?.Trim() ?? string.Empty;

                _logger?.LogInformation("Checking if original approver '{OriginalApproverId}' is in resolved approvers list for stage {StageId}. Resolved approvers: {Approvers}",
                    originalApproverId, currentStage.Id, string.Join(", ", resolvedApprovers.UserIds ?? Array.Empty<string>()));

                // Try to resolve FromUserId to both username and numeric ID for comparison
                bool isOriginalApproverInList = false;
                string originalApproverNumericId = null;

                // First, try direct comparison
                isOriginalApproverInList = resolvedApprovers.UserIds.Any(a =>
                    string.Equals(a?.Trim(), originalApproverId, StringComparison.OrdinalIgnoreCase));

                // If not found and FromUserId is numeric, try to resolve to username
                if (!isOriginalApproverInList && int.TryParse(originalApproverId, out int fromUserIdInt))
                {
                    originalApproverNumericId = originalApproverId;
                    var fromUser = await _identityContext.TblUsers.FirstOrDefaultAsync(u => u.Id == fromUserIdInt);
                    if (fromUser != null)
                    {
                        var fromUsername = fromUser.Username?.Trim() ?? string.Empty;
                        isOriginalApproverInList = resolvedApprovers.UserIds.Any(a =>
                            string.Equals(a?.Trim(), fromUsername, StringComparison.OrdinalIgnoreCase));

                        if (isOriginalApproverInList)
                        {
                            _logger?.LogInformation("✓ Found match: FromUserId '{FromUserId}' (numeric) resolved to username '{FromUsername}' which matches approver in list",
                                originalApproverId, fromUsername);
                        }
                    }
                }
                // If not found and FromUserId is username, try to resolve to numeric ID
                else if (!isOriginalApproverInList)
                {
                    var fromUser = await _identityContext.TblUsers
                        .FirstOrDefaultAsync(u => u.Username.ToLower() == originalApproverId.ToLower());
                    if (fromUser != null)
                    {
                        originalApproverNumericId = fromUser.Id.ToString();
                        isOriginalApproverInList = resolvedApprovers.UserIds.Any(a =>
                            string.Equals(a?.Trim(), originalApproverNumericId, StringComparison.OrdinalIgnoreCase));

                        if (isOriginalApproverInList)
                        {
                            _logger?.LogInformation("✓ Found match: FromUserId '{FromUserId}' (username) resolved to numeric ID '{FromUserNumericId}' which matches approver in list",
                                originalApproverId, originalApproverNumericId);
                        }
                    }
                }

                if (!isOriginalApproverInList)
                {
                    _logger?.LogWarning("✗ Original approver '{OriginalApproverId}' (numeric: '{OriginalApproverNumericId}') is not in resolved approvers list for stage {StageId}. Resolved approvers: {Approvers}. Skipping submission {SubmissionId}",
                        originalApproverId, originalApproverNumericId ?? "N/A", currentStage.Id,
                        string.Join(", ", resolvedApprovers.UserIds ?? Array.Empty<string>()), submissionId);
                    continue;
                }

                _logger?.LogInformation("✓ Original approver '{OriginalApproverId}' is in resolved approvers list for stage {StageId}",
                    originalApproverId, currentStage.Id);

                // Verify that the delegation matches the current user
                var delegatedUserIdNormalized = delegation.ToUserId?.Trim() ?? string.Empty;
                var matchesNormalized = string.Equals(delegatedUserIdNormalized, normalizedUserId, StringComparison.OrdinalIgnoreCase);
                var matchesNumeric = string.Equals(delegatedUserIdNormalized, numericUserId, StringComparison.OrdinalIgnoreCase);

                if (!matchesNormalized && !matchesNumeric)
                {
                    // Try to resolve ToUserId to username or numeric ID
                    if (int.TryParse(delegatedUserIdNormalized, out int delegatedUserIdInt))
                    {
                        var delegatedUser = await _identityContext.TblUsers
                            .FirstOrDefaultAsync(u => u.Id == delegatedUserIdInt);
                        if (delegatedUser != null)
                        {
                            var delegatedUsername = delegatedUser.Username?.Trim() ?? string.Empty;
                            if (!string.Equals(delegatedUsername, normalizedUserId, StringComparison.OrdinalIgnoreCase))
                            {
                                _logger?.LogInformation("Delegation ToUserId '{DelegatedUserId}' does not match user '{UserId}', skipping",
                                    delegatedUserIdNormalized, normalizedUserId);
                                continue;
                            }
                        }
                    }
                    else
                    {
                        var delegatedUser = await _identityContext.TblUsers
                            .FirstOrDefaultAsync(u => u.Username.ToLower() == delegatedUserIdNormalized.ToLower());
                        if (delegatedUser != null)
                        {
                            var delegatedUserNumericId = delegatedUser.Id.ToString();
                            if (!string.Equals(delegatedUserNumericId, numericUserId, StringComparison.OrdinalIgnoreCase))
                            {
                                _logger?.LogInformation("Delegation ToUserId '{DelegatedUserId}' does not match user '{UserId}', skipping",
                                    delegatedUserIdNormalized, normalizedUserId);
                                continue;
                            }
                        }
                        else
                        {
                            _logger?.LogInformation("Delegation ToUserId '{DelegatedUserId}' does not match user '{UserId}', skipping",
                                delegatedUserIdNormalized, normalizedUserId);
                            continue;
                        }
                    }
                }

                // Add to inbox
                _logger?.LogInformation("✓ All checks passed! Adding submission {SubmissionId} to inbox", submissionId);
                _logger?.LogInformation("Submission details: DocumentNumber={DocumentNumber}, DocumentType={DocumentType}, Stage={StageName} (Id={StageId}, Order={StageOrder})",
                    submission.DocumentNumber, submissionDocumentType.Name, currentStage.StageName, currentStage.Id, currentStage.StageOrder);

                inboxItems.Add(new ApprovalInboxDto
                {
                    SubmissionId = submission.Id,
                    DocumentNumber = submission.DocumentNumber,
                    DocumentTypeName = submissionDocumentType.Name ?? "",
                    StageId = currentStage.Id,
                    StageName = currentStage.StageName,
                    StageOrder = currentStage.StageOrder,
                    SubmittedDate = submission.SubmittedDate,
                    SubmittedByUserId = submission.SubmittedByUserId,
                    SubmittedByUserName = "",
                    Status = submission.Status,
                    IsAssigned = true,
                    Approvers = resolvedApprovers.UserIds ?? Array.Empty<string>(),
                    CanApprove = true
                });

                _logger?.LogInformation("✓ Successfully added submission {SubmissionId} to inbox via Document-level delegation for user {UserId} at stage {StageId}",
                    submissionId, normalizedUserId, currentStage.Id);
            }

            _logger?.LogInformation("=== Document-level Delegations Processing Summary ===");
            _logger?.LogInformation("Total Document-level delegations processed: {Count}", documentLevelDelegations.Count);
            _logger?.LogInformation("Submissions added to inbox via Document-level delegations: {Count}",
                inboxItems.Count(item => documentLevelDelegations.Any(d => d.ScopeId == item.SubmissionId)));

            // ✅ NEW: Check for Global delegations
            // This handles cases where submissions have Global delegations
            // but their document types are not linked to any workflow
            _logger?.LogInformation("=== Checking Global Delegations ===");

            var globalDelegations = allActiveDelegations
                .Where(d => d.ScopeType == "Global")
                .ToList();

            _logger?.LogInformation("Found {Count} Global delegations for user {UserId}",
                globalDelegations.Count, normalizedUserId);

            if (globalDelegations.Any())
            {
                // Get all submitted submissions that are not already in inbox
                var allSubmittedSubmissions = await _unitOfWork.FormSubmissionsRepository.GetAllAsync(
                    s => s.Status == "Submitted" && !s.IsDeleted);

                _logger?.LogInformation("Found {Count} total submitted submissions in system", allSubmittedSubmissions.Count());

                foreach (var globalDelegation in globalDelegations)
                {
                    _logger?.LogInformation("Processing Global delegation: Id={Id}, FromUserId={FromUserId}, ToUserId={ToUserId}",
                        globalDelegation.Id, globalDelegation.FromUserId, globalDelegation.ToUserId);

                    // Verify that the delegation matches the current user
                    var delegatedUserIdNormalized = globalDelegation.ToUserId?.Trim() ?? string.Empty;
                    var matchesNormalized = string.Equals(delegatedUserIdNormalized, normalizedUserId, StringComparison.OrdinalIgnoreCase);
                    var matchesNumeric = string.Equals(delegatedUserIdNormalized, numericUserId, StringComparison.OrdinalIgnoreCase);

                    if (!matchesNormalized && !matchesNumeric)
                    {
                        // Try to resolve ToUserId to username or numeric ID
                        if (int.TryParse(delegatedUserIdNormalized, out int delegatedUserIdInt))
                        {
                            var delegatedUser = await _identityContext.TblUsers
                                .FirstOrDefaultAsync(u => u.Id == delegatedUserIdInt);
                            if (delegatedUser != null)
                            {
                                var delegatedUsername = delegatedUser.Username?.Trim() ?? string.Empty;
                                if (!string.Equals(delegatedUsername, normalizedUserId, StringComparison.OrdinalIgnoreCase))
                                {
                                    _logger?.LogInformation("Global delegation ToUserId '{DelegatedUserId}' does not match user '{UserId}', skipping",
                                        delegatedUserIdNormalized, normalizedUserId);
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            var delegatedUser = await _identityContext.TblUsers
                                .FirstOrDefaultAsync(u => u.Username.ToLower() == delegatedUserIdNormalized.ToLower());
                            if (delegatedUser != null)
                            {
                                var delegatedUserNumericId = delegatedUser.Id.ToString();
                                if (!string.Equals(delegatedUserNumericId, numericUserId, StringComparison.OrdinalIgnoreCase))
                                {
                                    _logger?.LogInformation("Global delegation ToUserId '{DelegatedUserId}' does not match user '{UserId}', skipping",
                                        delegatedUserIdNormalized, normalizedUserId);
                                    continue;
                                }
                            }
                            else
                            {
                                _logger?.LogInformation("Global delegation ToUserId '{DelegatedUserId}' does not match user '{UserId}', skipping",
                                    delegatedUserIdNormalized, normalizedUserId);
                                continue;
                            }
                        }
                    }

                    _logger?.LogInformation("✓ Global delegation matches user {UserId}", normalizedUserId);

                    // Process each submitted submission
                    _logger?.LogInformation("Processing {Count} submitted submissions for Global delegation", allSubmittedSubmissions.Count());
                    foreach (var submission in allSubmittedSubmissions)
                    {
                        _logger?.LogInformation("Checking submission {SubmissionId} (DocumentNumber={DocumentNumber}, DocumentTypeId={DocumentTypeId}, Status={Status}, StageId={StageId})",
                            submission.Id, submission.DocumentNumber, submission.DocumentTypeId, submission.Status, submission.StageId);

                        // Skip if already in inbox
                        if (inboxItems.Any(item => item.SubmissionId == submission.Id))
                        {
                            _logger?.LogInformation("Submission {SubmissionId} already in inbox, skipping", submission.Id);
                            continue;
                        }

                        // Get the submission's workflow and current stage
                        var submissionDocumentType = await _unitOfWork.DocumentTypeRepository.GetByIdAsync(submission.DocumentTypeId);
                        if (submissionDocumentType == null)
                        {
                            continue;
                        }

                        // Get the current stage for this submission
                        APPROVAL_STAGES currentStage = null;

                        // ✅ First, try to use StageId from submission if available
                        if (submission.StageId.HasValue)
                        {
                            currentStage = await _unitOfWork.ApprovalStageRepository.GetByIdAsync(submission.StageId.Value);
                            if (currentStage != null)
                            {
                                _logger?.LogInformation("Using StageId={StageId} from submission {SubmissionId}",
                                    submission.StageId.Value, submission.Id);
                            }
                        }

                        // If StageId not available or stage not found, try to determine from workflow
                        if (currentStage == null)
                        {
                            // Find the workflow for this document type
                            int? workflowId = null;
                            if (submissionDocumentType.ApprovalWorkflowId.HasValue)
                            {
                                workflowId = submissionDocumentType.ApprovalWorkflowId.Value;
                            }
                            else
                            {
                                // Try to find workflow by DocumentTypeId
                                var workflowByDocType = await _unitOfWork.ApprovalWorkflowRepository.GetAll()
                                    .Where(w => w.DocumentTypeId == submission.DocumentTypeId && w.IsActive && !w.IsDeleted)
                                    .FirstOrDefaultAsync();
                                if (workflowByDocType != null)
                                {
                                    workflowId = workflowByDocType.Id;
                                }
                            }

                            if (!workflowId.HasValue)
                            {
                                _logger?.LogInformation("No workflow found for submission {SubmissionId} (DocumentTypeId={DocumentTypeId}), and no StageId in submission. Skipping Global delegation check",
                                    submission.Id, submission.DocumentTypeId);
                                continue;
                            }

                            // Get the current stage from history
                            var history = await _unitOfWork.DocumentApprovalHistoryRepository.GetBySubmissionIdAsync(submission.Id);
                            var lastAction = history.OrderByDescending(h => h.ActionDate).FirstOrDefault();

                            if (lastAction == null)
                            {
                                // No history - should be in first stage
                                var allStagesInWorkflow = await _unitOfWork.ApprovalStageRepository.GetAllAsync(s =>
                                    s.WorkflowId == workflowId.Value && s.IsActive);
                                currentStage = allStagesInWorkflow.OrderBy(s => s.StageOrder).FirstOrDefault();
                            }
                            else
                            {
                                // Get the stage from last action
                                currentStage = await _unitOfWork.ApprovalStageRepository.GetByIdAsync(lastAction.StageId);

                                // If last action was approved, get next stage
                                if (lastAction.ActionType?.ToLower() == "approved" && currentStage != null)
                                {
                                    var allStagesInWorkflow = await _unitOfWork.ApprovalStageRepository.GetAllAsync(s =>
                                        s.WorkflowId == workflowId.Value && s.IsActive);
                                    var nextStage = allStagesInWorkflow
                                        .Where(s => s.StageOrder > currentStage.StageOrder)
                                        .OrderBy(s => s.StageOrder)
                                        .FirstOrDefault();
                                    if (nextStage != null)
                                    {
                                        currentStage = nextStage;
                                    }
                                }
                            }
                        }

                        if (currentStage == null)
                        {
                            _logger?.LogInformation("Could not determine current stage for submission {SubmissionId}, skipping", submission.Id);
                            continue;
                        }

                        // Resolve approvers for this stage
                        var approversResult = await ResolveApproversForStageAsync(currentStage.Id);
                        if (approversResult.StatusCode != 200)
                        {
                            continue;
                        }

                        var resolvedApprovers = approversResult.Data as ResolvedApproversDto;
                        if (resolvedApprovers == null || resolvedApprovers.UserIds == null || !resolvedApprovers.UserIds.Any())
                        {
                            continue;
                        }

                        // Check if the original approver (FromUserId) is in the resolved approvers
                        var originalApproverId = globalDelegation.FromUserId?.Trim() ?? string.Empty;

                        // Try to resolve FromUserId to both username and numeric ID for comparison
                        bool isOriginalApproverInList = false;
                        string originalApproverNumericId = null;

                        // First, try direct comparison
                        isOriginalApproverInList = resolvedApprovers.UserIds.Any(a =>
                            string.Equals(a?.Trim(), originalApproverId, StringComparison.OrdinalIgnoreCase));

                        // If not found and FromUserId is numeric, try to resolve to username
                        if (!isOriginalApproverInList && int.TryParse(originalApproverId, out int fromUserIdInt))
                        {
                            originalApproverNumericId = originalApproverId;
                            var fromUser = await _identityContext.TblUsers.FirstOrDefaultAsync(u => u.Id == fromUserIdInt);
                            if (fromUser != null)
                            {
                                var fromUsername = fromUser.Username?.Trim() ?? string.Empty;
                                isOriginalApproverInList = resolvedApprovers.UserIds.Any(a =>
                                    string.Equals(a?.Trim(), fromUsername, StringComparison.OrdinalIgnoreCase));
                            }
                        }
                        // If not found and FromUserId is username, try to resolve to numeric ID
                        else if (!isOriginalApproverInList)
                        {
                            var fromUser = await _identityContext.TblUsers
                                .FirstOrDefaultAsync(u => u.Username.ToLower() == originalApproverId.ToLower());
                            if (fromUser != null)
                            {
                                originalApproverNumericId = fromUser.Id.ToString();
                                isOriginalApproverInList = resolvedApprovers.UserIds.Any(a =>
                                    string.Equals(a?.Trim(), originalApproverNumericId, StringComparison.OrdinalIgnoreCase));
                            }
                        }

                        if (!isOriginalApproverInList)
                        {
                            _logger?.LogInformation("Original approver '{OriginalApproverId}' is not in resolved approvers list for stage {StageId}, skipping submission {SubmissionId}",
                                originalApproverId, currentStage.Id, submission.Id);
                            continue;
                        }

                        // Add to inbox
                        inboxItems.Add(new ApprovalInboxDto
                        {
                            SubmissionId = submission.Id,
                            DocumentNumber = submission.DocumentNumber,
                            DocumentTypeName = submissionDocumentType.Name ?? "",
                            StageId = currentStage.Id,
                            StageName = currentStage.StageName,
                            StageOrder = currentStage.StageOrder,
                            SubmittedDate = submission.SubmittedDate,
                            SubmittedByUserId = submission.SubmittedByUserId,
                            SubmittedByUserName = "",
                            Status = submission.Status,
                            IsAssigned = true,
                            Approvers = resolvedApprovers.UserIds ?? Array.Empty<string>(),
                            CanApprove = true
                        });

                        _logger?.LogInformation("✓ Added submission {SubmissionId} to inbox via Global delegation for user {UserId} at stage {StageId}",
                            submission.Id, normalizedUserId, currentStage.Id);
                    }
                }

                _logger?.LogInformation("=== Global Delegations Processing Summary ===");
                _logger?.LogInformation("Total Global delegations processed: {Count}", globalDelegations.Count);
                _logger?.LogInformation("Submissions added to inbox via Global delegations: {Count}",
                    inboxItems.Count(item => !documentLevelDelegations.Any(d => d.ScopeId == item.SubmissionId)));
            }
            else
            {
                _logger?.LogInformation("No Global delegations found for user {UserId}", normalizedUserId);
            }

            _logger?.LogInformation("Retrieved {Count} inbox items for user {UserId}", inboxItems.Count, normalizedUserId);

            if (inboxItems.Count == 0)
            {
                _logger?.LogWarning("=== INBOX IS EMPTY ===");
                _logger?.LogWarning("Possible reasons:");
                _logger?.LogWarning("1. User '{NormalizedUserId}' is not assigned to any stage", normalizedUserId);
                _logger?.LogWarning("2. No submissions in 'Submitted' status");
                _logger?.LogWarning("3. UserId format mismatch (check UserId vs UserName in Stage Assignees)");
                _logger?.LogWarning("4. Submissions are not in the correct stage");
            }
            else
            {
                var inboxInfo = inboxItems.Select(item => new
                {
                    item.SubmissionId,
                    item.DocumentNumber,
                    item.StageId,
                    item.StageName,
                    item.Status
                }).ToList();
                _logger?.LogInformation("Inbox items: {Items}", JsonSerializer.Serialize(inboxInfo));
            }

            _logger?.LogInformation("=== GetApprovalInboxAsync END ===");

            return new ApiResponse(200, "Inbox retrieved successfully", inboxItems);
        }

        private async Task<APPROVAL_STAGES> GetPreviousStageAsync(int currentStageId)
        {
            var currentStage = await _unitOfWork.ApprovalStageRepository.GetByIdAsync(currentStageId);
            if (currentStage == null)
                return null;

            var stages = await _unitOfWork.ApprovalStageRepository.GetAllAsync(s =>
                s.WorkflowId == currentStage.WorkflowId && s.IsActive);

            return stages
                .Where(s => s.StageOrder < currentStage.StageOrder)
                .OrderByDescending(s => s.StageOrder)
                .FirstOrDefault();
        }

        private async Task<bool> IsPreviousStageAsync(int submissionId, int stageId)
        {
            var currentStage = await _unitOfWork.ApprovalStageRepository.GetByIdAsync(stageId);
            if (currentStage == null)
                return false;

            var history = await _unitOfWork.DocumentApprovalHistoryRepository.GetBySubmissionIdAsync(submissionId);
            var lastAction = history.OrderByDescending(h => h.ActionDate).FirstOrDefault();

            if (lastAction == null)
                return false;

            var lastStage = await _unitOfWork.ApprovalStageRepository.GetByIdAsync(lastAction.StageId);
            return lastStage != null && lastStage.StageOrder < currentStage.StageOrder;
        }

        /// <summary>
        /// Gets detailed debug information about the approval inbox for a user
        /// </summary>
        public async Task<ApiResponse> GetInboxDebugInfoAsync(string userId)
        {
            _logger?.LogInformation("=== GetInboxDebugInfoAsync START ===");
            _logger?.LogInformation("Received userId: '{UserId}'", userId);

            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger?.LogWarning("UserId is null or empty");
                return new ApiResponse(400, "UserId is required");
            }

            var normalizedUserId = userId.Trim();

            // Resolve username to numeric UserId
            var numericUserId = normalizedUserId;
            if (!int.TryParse(normalizedUserId, out int parsedUserId))
            {
                var user = await _identityContext.TblUsers
                    .FirstOrDefaultAsync(u => u.Username.ToLower() == normalizedUserId.ToLower());
                if (user != null)
                {
                    numericUserId = user.Id.ToString();
                }
            }
            else
            {
                numericUserId = parsedUserId.ToString();
            }

            // Get detailed stage analysis
            var stageAnalysis = new List<object>();
            var allStages = await _unitOfWork.ApprovalStageRepository.GetAllAsync(s => s.IsActive);

            foreach (var stage in allStages)
            {
                var approversResult = await ResolveApproversForStageAsync(stage.Id);
                var resolvedApprovers = approversResult.Data as ResolvedApproversDto;

                var isUserAssigned = resolvedApprovers?.UserIds != null &&
                    resolvedApprovers.UserIds.Any(id =>
                        string.Equals(id?.Trim(), normalizedUserId, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(id?.Trim(), numericUserId, StringComparison.OrdinalIgnoreCase));

                var workflow = await _unitOfWork.ApprovalWorkflowRepository.GetByIdAsync(stage.WorkflowId);

                // If workflow not found, check directly in database (including deleted)
                if (workflow == null)
                {
                    var workflowDirect = await _unitOfWork.ApprovalWorkflowRepository.GetAll()
                        .Where(w => w.Id == stage.WorkflowId)
                        .FirstOrDefaultAsync();
                    _logger?.LogWarning("Stage {StageId}: Workflow {WorkflowId} not found via GetByIdAsync. Direct query result: {Found}, IsDeleted: {IsDeleted}, IsActive: {IsActive}",
                        stage.Id, stage.WorkflowId, workflowDirect != null ? "Found" : "Not Found", workflowDirect?.IsDeleted, workflowDirect?.IsActive);

                    // If workflow exists but is deleted, use it anyway for document type query (workaround)
                    if (workflowDirect != null)
                    {
                        workflow = workflowDirect;
                        _logger?.LogWarning("Stage {StageId}: Using deleted workflow {WorkflowId} for document type query (workaround)", stage.Id, stage.WorkflowId);
                    }
                }

                _logger?.LogInformation("Stage {StageId}: Workflow lookup - Stage.WorkflowId={WorkflowId}, Workflow found: {Found}",
                    stage.Id, stage.WorkflowId, workflow != null ? $"Yes (Id={workflow.Id}, IsDeleted={workflow.IsDeleted}, IsActive={workflow.IsActive})" : "No");

                IEnumerable<FormBuilder.Domian.Entitys.FromBuilder.DOCUMENT_TYPES> documentTypes;
                var documentTypeLogs = new List<string>();
                documentTypeLogs.Add($"Stage {stage.Id}: Workflow lookup - Stage.WorkflowId={stage.WorkflowId}, Workflow found: {(workflow != null ? $"Yes (Id={workflow.Id}, IsDeleted={workflow.IsDeleted}, IsActive={workflow.IsActive})" : "No")}");

                if (workflow != null)
                {
                    _logger?.LogInformation("Stage {StageId}: Looking for document types with ApprovalWorkflowId={WorkflowId}",
                        stage.Id, workflow.Id);
                    documentTypeLogs.Add($"Stage {stage.Id}: Looking for document types with ApprovalWorkflowId={workflow.Id}");

                    // First, check ALL document types in database (no filters) to see what exists
                    var allDocTypesInDb = await _unitOfWork.DocumentTypeRepository.GetAll()
                        .ToListAsync();
                    _logger?.LogInformation("Stage {StageId}: Total document types in DB (no filters): {Count}", stage.Id, allDocTypesInDb.Count);
                    documentTypeLogs.Add($"Stage {stage.Id}: Total document types in DB (no filters): {allDocTypesInDb.Count}");
                    foreach (var dt in allDocTypesInDb)
                    {
                        var logMsg = $"  - ALL DocumentType: Id={dt.Id}, Name={dt.Name}, ApprovalWorkflowId={dt.ApprovalWorkflowId}, IsActive={dt.IsActive}, IsDeleted={dt.IsDeleted}";
                        _logger?.LogInformation(logMsg);
                        documentTypeLogs.Add(logMsg);
                    }

                    // Now check with workflow filter only (prefer DocumentTypeId, keep legacy reverse link)
                    var docTypesByWorkflow = await _unitOfWork.DocumentTypeRepository.GetAll()
                        .Where(dt => dt.Id == workflow.DocumentTypeId || dt.ApprovalWorkflowId == workflow.Id)
                        .ToListAsync();
                    _logger?.LogInformation("Stage {StageId}: Document types with ApprovalWorkflowId={WorkflowId} (no other filters): {Count}",
                        stage.Id, workflow.Id, docTypesByWorkflow.Count);
                    documentTypeLogs.Add($"Stage {stage.Id}: Document types with ApprovalWorkflowId={workflow.Id} (no other filters): {docTypesByWorkflow.Count}");
                    foreach (var dt in docTypesByWorkflow)
                    {
                        var logMsg = $"  - By Workflow DocumentType: Id={dt.Id}, Name={dt.Name}, ApprovalWorkflowId={dt.ApprovalWorkflowId}, IsActive={dt.IsActive}, IsDeleted={dt.IsDeleted}";
                        _logger?.LogInformation(logMsg);
                        documentTypeLogs.Add(logMsg);
                    }

                    // Try direct query with all filters (prefer DocumentTypeId, keep legacy reverse link)
                    var directQuery = await _unitOfWork.DocumentTypeRepository.GetAll()
                        .Where(dt => (dt.Id == workflow.DocumentTypeId || dt.ApprovalWorkflowId == workflow.Id) && dt.IsActive && !dt.IsDeleted)
                        .ToListAsync();

                    _logger?.LogInformation("Stage {StageId}: Direct query with all filters found {Count} document types", stage.Id, directQuery.Count);
                    documentTypeLogs.Add($"Stage {stage.Id}: Direct query with all filters found {directQuery.Count} document types");
                    foreach (var dt in directQuery)
                    {
                        var logMsg = $"  - Direct Query DocumentType: Id={dt.Id}, Name={dt.Name}, ApprovalWorkflowId={dt.ApprovalWorkflowId}, IsActive={dt.IsActive}, IsDeleted={dt.IsDeleted}";
                        _logger?.LogInformation(logMsg);
                        documentTypeLogs.Add(logMsg);
                    }

                    // Use direct query results (override may not work properly with GetAllAsync)
                    documentTypes = directQuery;

                    // Log for debugging
                    _logger?.LogInformation("Stage {StageId} (WorkflowId={WorkflowId}): Found {Count} document types with filter ApprovalWorkflowId={WorkflowId} AND IsActive=true AND IsDeleted=false",
                        stage.Id, workflow.Id, documentTypes.Count(), workflow.Id);

                    // Log each document type found
                    foreach (var dt in documentTypes)
                    {
                        _logger?.LogInformation("  - DocumentType: Id={Id}, Name={Name}, ApprovalWorkflowId={WorkflowId}, IsActive={IsActive}, IsDeleted={IsDeleted}",
                            dt.Id, dt.Name, dt.ApprovalWorkflowId, dt.IsActive, dt.IsDeleted);
                    }

                    // If still empty, try without IsActive filter to see if that's the issue
                    if (!documentTypes.Any())
                    {
                        var allDocTypes = await _unitOfWork.DocumentTypeRepository.GetAll()
                            .Where(dt => (dt.Id == workflow.DocumentTypeId || dt.ApprovalWorkflowId == workflow.Id) && !dt.IsDeleted)
                            .ToListAsync();
                        _logger?.LogWarning("Stage {StageId}: Found {Count} document types without IsActive filter (only IsDeleted=false)",
                            stage.Id, allDocTypes.Count());

                        foreach (var dt in allDocTypes)
                        {
                            _logger?.LogWarning("  - DocumentType (no IsActive filter): Id={Id}, Name={Name}, ApprovalWorkflowId={WorkflowId}, IsActive={IsActive}, IsDeleted={IsDeleted}",
                                dt.Id, dt.Name, dt.ApprovalWorkflowId, dt.IsActive, dt.IsDeleted);
                        }

                        // Use results without IsActive filter if found
                        if (allDocTypes.Any())
                        {
                            documentTypes = allDocTypes;
                        }
                    }
                }
                else
                {
                    documentTypes = Enumerable.Empty<FormBuilder.Domian.Entitys.FromBuilder.DOCUMENT_TYPES>();
                    _logger?.LogWarning("Stage {StageId}: Workflow {WorkflowId} not found", stage.Id, stage.WorkflowId);
                    documentTypeLogs.Add($"Stage {stage.Id}: Workflow {stage.WorkflowId} not found - cannot query document types");
                }

                var documentTypeIds = documentTypes.Select(dt => dt.Id).ToList();
                _logger?.LogInformation("Stage {StageId}: Final DocumentTypeIds = [{Ids}]", stage.Id, string.Join(", ", documentTypeIds));
                var submissions = await _unitOfWork.FormSubmissionsRepository.GetAllAsync(
                    s => documentTypeIds.Contains(s.DocumentTypeId) && s.Status == "Submitted");

                var submissionDetails = new List<object>();
                foreach (var submission in submissions)
                {
                    var history = await _unitOfWork.DocumentApprovalHistoryRepository.GetBySubmissionIdAsync(submission.Id);
                    var lastAction = history.OrderByDescending(h => h.ActionDate).FirstOrDefault();

                    bool isCurrentStage = false;
                    string reason = "";

                    if (lastAction == null)
                    {
                        // No history - should be in first stage
                        var allStagesInWorkflow = await _unitOfWork.ApprovalStageRepository.GetAllAsync(s =>
                            s.WorkflowId == workflow.Id && s.IsActive);
                        var firstStage = allStagesInWorkflow.OrderBy(s => s.StageOrder).FirstOrDefault();

                        if (firstStage != null && stage.Id == firstStage.Id)
                        {
                            isCurrentStage = true;
                            reason = $"No history - submission should be in first stage (StageId={stage.Id})";
                        }
                        else
                        {
                            reason = $"No history - submission should be in first stage (StageId={firstStage?.Id ?? 0}), but checking stage {stage.Id}";
                        }
                    }
                    else
                    {
                        reason = $"Has history - LastAction: StageId={lastAction.StageId}, ActionType={lastAction.ActionType}";
                        // Simplified check - you can expand this
                        if (lastAction.StageId == stage.Id && lastAction.ActionType?.ToLower() != "approved")
                        {
                            isCurrentStage = true;
                        }
                    }

                    submissionDetails.Add(new
                    {
                        SubmissionId = submission.Id,
                        DocumentNumber = submission.DocumentNumber,
                        DocumentTypeId = submission.DocumentTypeId,
                        Status = submission.Status,
                        HasHistory = lastAction != null,
                        LastActionStageId = lastAction?.StageId,
                        LastActionType = lastAction?.ActionType,
                        IsCurrentStage = isCurrentStage,
                        Reason = reason
                    });
                }

                stageAnalysis.Add(new
                {
                    StageId = stage.Id,
                    StageName = stage.StageName,
                    WorkflowId = stage.WorkflowId,
                    StageOrder = stage.StageOrder,
                    IsUserAssigned = isUserAssigned,
                    ResolvedApprovers = resolvedApprovers?.UserIds ?? Array.Empty<string>(),
                    DocumentTypeIds = documentTypeIds,
                    SubmissionsCount = submissions.Count(),
                    SubmissionDetails = submissionDetails,
                    DocumentTypeLogs = documentTypeLogs
                });
            }

            var debugInfo = new
            {
                UserId = normalizedUserId,
                NumericUserId = numericUserId,
                Timestamp = DateTime.UtcNow,
                // Get all Stage Assignees
                AllAssignees = await GetAllAssigneesInfoAsync(),
                // Get matching assignees for this user
                MatchingAssignees = await GetMatchingAssigneesInfoAsync(normalizedUserId),
                // Get all active stages
                AllStages = await GetAllStagesInfoAsync(),
                // Detailed stage analysis
                StageAnalysis = stageAnalysis,
                // Get user's inbox items
                InboxItems = await GetInboxItemsInfoAsync(normalizedUserId),
                // Get all submissions in Submitted status
                SubmittedSubmissions = await GetSubmittedSubmissionsInfoAsync(),
                // Get approval history
                ApprovalHistory = await GetApprovalHistoryInfoAsync()
            };

            _logger?.LogInformation("=== GetInboxDebugInfoAsync END ===");

            return new ApiResponse(200, "Debug info retrieved successfully", debugInfo);
        }

        private async Task<object> GetAllAssigneesInfoAsync()
        {
            var allAssignees = await _unitOfWork.ApprovalStageAssigneesRepository.GetAllAsync(a => a.IsActive);
            return allAssignees.Select(a => new
            {
                a.Id,
                a.StageId,
                UserId = a.UserId?.ToString() ?? "null",
                a.RoleId,
                a.IsActive
            }).ToList();
        }

        private async Task<object> GetMatchingAssigneesInfoAsync(string userId)
        {
            var allAssignees = await _unitOfWork.ApprovalStageAssigneesRepository.GetAllAsync(a => a.IsActive);

            // ✅ Resolve username to numeric UserId (same logic as GetApprovalInboxAsync)
            var normalizedUserId = userId?.Trim() ?? string.Empty;
            var numericUserId = normalizedUserId;

            if (!int.TryParse(normalizedUserId, out _))
            {
                // Input is not numeric, try to find user by username
                var user = await _identityContext.TblUsers
                    .FirstOrDefaultAsync(u => u.Username.ToLower() == normalizedUserId.ToLower());

                if (user != null)
                {
                    numericUserId = user.Id.ToString();
                }
            }

            // Match by both original userId and resolved numeric userId
            var matching = allAssignees.Where(a =>
                !string.IsNullOrWhiteSpace(a.UserId) &&
                (string.Equals(a.UserId?.ToString()?.Trim(), normalizedUserId, StringComparison.OrdinalIgnoreCase) ||
                 string.Equals(a.UserId?.ToString()?.Trim(), numericUserId, StringComparison.OrdinalIgnoreCase))
            ).ToList();

            return matching.Select(a => new
            {
                a.Id,
                a.StageId,
                UserId = a.UserId?.ToString() ?? "null",
                a.RoleId,
                a.IsActive
            }).ToList();
        }

        private async Task<object> GetAllStagesInfoAsync()
        {
            var allStages = await _unitOfWork.ApprovalStageRepository.GetAllAsync(s => s.IsActive);
            return allStages.Select(s => new
            {
                s.Id,
                s.StageName,
                s.WorkflowId,
                s.StageOrder,
                s.IsActive,
                s.IsFinalStage
            }).ToList();
        }

        private async Task<object> GetInboxItemsInfoAsync(string userId)
        {
            var inboxResult = await GetApprovalInboxAsync(userId);
            if (inboxResult.StatusCode == 200 && inboxResult.Data is List<ApprovalInboxDto> inboxItems)
            {
                return inboxItems.Select(item => new
                {
                    item.SubmissionId,
                    item.DocumentNumber,
                    item.DocumentTypeName,
                    item.StageId,
                    item.StageName,
                    item.SubmittedDate,
                    item.SubmittedByUserId,
                    item.Status
                }).ToList();
            }
            return new List<object>();
        }

        private async Task<object> GetSubmittedSubmissionsInfoAsync()
        {
            var submissions = await _unitOfWork.FormSubmissionsRepository.GetAllAsync(s => s.Status == "Submitted");
            return submissions.Select(s => new
            {
                s.Id,
                s.DocumentNumber,
                s.DocumentTypeId,
                s.Status,
                s.SubmittedByUserId,
                s.SubmittedDate
            }).ToList();
        }

        private async Task<object> GetApprovalHistoryInfoAsync()
        {
            var allHistory = await _unitOfWork.DocumentApprovalHistoryRepository.GetAllApprovalHistoryAsync();
            return allHistory.Take(50).Select(h => new
            {
                h.Id,
                h.SubmissionId,
                h.StageId,
                h.ActionType,
                h.ActionByUserId,
                h.ActionDate,
                h.Comments
            }).ToList();
        }

        /// <summary>
        /// Creates DocuSign envelope for a stage that requires e-signing.
        /// Returns false when request could not be created.
        /// </summary>
        private async Task<bool> CreateDocuSignEnvelopeIfNeededAsync(
            FORM_SUBMISSIONS submission,
            APPROVAL_STAGES stage,
            string? requestedByUserId = null)
        {
            try
            {
                if (!stage.RequiresAdobeSign)
                    return false;

                var signerEmail = submission.SubmittedByUserId;
                var signerName = submission.SubmittedByUserId;

                if (!string.IsNullOrWhiteSpace(submission.SubmittedByUserId))
                {
                    var submittedBy = submission.SubmittedByUserId.Trim();
                    TblUser? user = null;

                    if (int.TryParse(submittedBy, out var userId))
                    {
                        user = await _identityContext.TblUsers.FirstOrDefaultAsync(u => u.Id == userId);
                    }
                    else
                    {
                        user = await _identityContext.TblUsers.FirstOrDefaultAsync(u => u.Username == submittedBy);
                    }

                    if (user != null)
                    {
                        signerEmail = string.IsNullOrWhiteSpace(user.Email) ? user.Username : user.Email;
                        signerName = string.IsNullOrWhiteSpace(user.Name) ? user.Username : user.Name;
                    }
                }

                if (string.IsNullOrWhiteSpace(signerEmail))
                {
                    _logger?.LogWarning("Cannot create DocuSign envelope: signer email not found for submission {SubmissionId}", submission.Id);
                    return false;
                }

                if (_scopeFactory == null)
                {
                    _logger?.LogWarning("IServiceScopeFactory is null; cannot create DocuSign envelope for submission {SubmissionId}", submission.Id);
                    return false;
                }

                await using var scope = _scopeFactory.CreateAsyncScope();
                var docuSignService = scope.ServiceProvider.GetService<IDocuSignService>();
                if (docuSignService == null)
                {
                    _logger?.LogWarning("IDocuSignService not found in DI container; skipping DocuSign for submission {SubmissionId}", submission.Id);
                    return false;
                }

                var requestedBy = string.IsNullOrWhiteSpace(requestedByUserId)
                    ? (string.IsNullOrWhiteSpace(submission.SubmittedByUserId) ? "system" : submission.SubmittedByUserId)
                    : requestedByUserId;

                _logger?.LogInformation(
                    "Attempting DocuSign envelope creation for submission {SubmissionId}, stage {StageId}, signer {SignerEmail}",
                    submission.Id,
                    stage.Id,
                    signerEmail);

                var created = await docuSignService.CreateSigningEnvelopeAsync(
                    submission.Id,
                    stage.Id,
                    signerEmail,
                    string.IsNullOrWhiteSpace(signerName) ? signerEmail : signerName,
                    requestedBy);

                if (created)
                {
                    _logger?.LogInformation("DocuSign envelope created for submission {SubmissionId}, stage {StageId}", submission.Id, stage.Id);
                }
                else
                {
                    _logger?.LogWarning("Failed to create DocuSign envelope for submission {SubmissionId}, stage {StageId}", submission.Id, stage.Id);
                }

                return created;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error creating DocuSign envelope for submission {SubmissionId}, stage {StageId}", submission.Id, stage.Id);
                // Do not throw - DocuSign failure must not block workflow transitions.
                return false;
            }
        }
    }
}
