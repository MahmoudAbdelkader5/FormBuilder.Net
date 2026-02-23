using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using FormBuilder.Core.DTOS.FormRules;
using FormBuilder.Core.IServices.FormBuilder;
using FormBuilder.Domain.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domian.Entitys.froms;
using formBuilder.Domian.Interfaces;

namespace FormBuilder.Services.Services.FormBuilder
{
    /// <summary>
    /// Service for executing CopyToDocument actions from form rules
    /// This service is called from triggers to execute CopyToDocument actions when events occur
    /// </summary>
    public class CopyToDocumentActionExecutorService
    {
        private readonly IunitOfwork _unitOfWork;
        private readonly ILogger<CopyToDocumentActionExecutorService>? _logger;
        private readonly ICopyToDocumentService _copyToDocumentService;

        public CopyToDocumentActionExecutorService(
            IunitOfwork unitOfWork,
            ILogger<CopyToDocumentActionExecutorService>? logger,
            ICopyToDocumentService copyToDocumentService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _copyToDocumentService = copyToDocumentService;
        }

        /// <summary>
        /// Execute CopyToDocument actions for a specific event (OnFormSubmitted, OnApprovalCompleted, etc.)
        /// </summary>
        /// <param name="submissionId">Submission ID that triggered the event</param>
        /// <param name="eventType">Event type: OnFormSubmitted, OnApprovalCompleted, OnDocumentApproved, OnRuleMatched</param>
        /// <param name="executedByUserId">User ID who triggered the event</param>
        public async Task ExecuteCopyToDocumentActionsForEventAsync(
            int submissionId,
            string eventType,
            string? executedByUserId = null)
        {
            try
            {
                _logger?.LogInformation("Executing CopyToDocument actions for event {EventType}, submission {SubmissionId}", eventType, submissionId);

                // Load submission
                var submission = await _unitOfWork.FormSubmissionsRepository.GetByIdAsync(submissionId);
                if (submission == null)
                {
                    _logger?.LogWarning("Submission {SubmissionId} not found for CopyToDocument action execution", submissionId);
                    return;
                }

                // Load active rules for the form that have CopyToDocument actions
                var activeRules = (await _unitOfWork.FORM_RULESRepository.GetActiveRulesByFormIdAsync(submission.FormBuilderId))
                    .Where(r => !r.IsDeleted)
                    .ToList();

                foreach (var rule in activeRules)
                {
                    try
                    {
                        // Load rule actions from FORM_RULE_ACTIONS table
                        var actions = await _unitOfWork.Repositary<FORM_RULE_ACTIONS>()
                            .GetAllAsync(filter: a => a.RuleId == rule.Id && !a.IsDeleted);
                        var copyToDocumentActions = actions
                            .Where(a => a.IsActive && !a.IsDeleted && a.ActionType == "CopyToDocument")
                            .OrderBy(a => a.ActionOrder)
                            .ToList();

                        if (!copyToDocumentActions.Any())
                            continue;

                        // Evaluate rule condition to determine if actions should execute
                        // For event-based triggers, we need to check if the rule condition matches
                        // For now, we'll execute CopyToDocument actions if the rule is active
                        // In the future, we can add event-specific conditions

                        foreach (var action in copyToDocumentActions)
                        {
                            try
                            {
                                // Parse CopyToDocument configuration from action.Value
                                CopyToDocumentActionDto? config = null;
                                if (!string.IsNullOrEmpty(action.Value))
                                {
                                    try
                                    {
                                        config = JsonSerializer.Deserialize<CopyToDocumentActionDto>(action.Value);
                                    }
                                    catch (JsonException ex)
                                    {
                                        _logger?.LogError(ex, "Failed to parse CopyToDocument configuration for action {ActionId}", action.Id);
                                        continue;
                                    }
                                }

                                if (config == null)
                                {
                                    _logger?.LogWarning("CopyToDocument configuration is null for action {ActionId}", action.Id);
                                    continue;
                                }

                                // Auto-fill SourceDocumentTypeId and SourceFormId from submission if not provided
                                // This ensures backward compatibility with existing configurations
                                if (config.SourceDocumentTypeId <= 0 && submission.DocumentTypeId > 0)
                                {
                                    config.SourceDocumentTypeId = submission.DocumentTypeId;
                                    _logger?.LogInformation("Auto-filled SourceDocumentTypeId from submission: {SourceDocumentTypeId}", config.SourceDocumentTypeId);
                                }

                                if (config.SourceFormId <= 0)
                                {
                                    config.SourceFormId = submission.FormBuilderId;
                                    _logger?.LogInformation("Auto-filled SourceFormId from submission: {SourceFormId}", config.SourceFormId);
                                }

                                // Validate that required fields are now set
                                if (config.SourceDocumentTypeId <= 0)
                                {
                                    _logger?.LogWarning("CopyToDocument action {ActionId} missing SourceDocumentTypeId and submission has no DocumentTypeId", action.Id);
                                    continue;
                                }

                                if (config.SourceFormId <= 0)
                                {
                                    _logger?.LogWarning("CopyToDocument action {ActionId} missing SourceFormId", action.Id);
                                    continue;
                                }

                                // Set default InitialStatus if not provided
                                if (string.IsNullOrWhiteSpace(config.InitialStatus))
                                {
                                    config.InitialStatus = "Draft";
                                }

                                // Execute CopyToDocument action
                                var result = await _copyToDocumentService.ExecuteCopyToDocumentAsync(
                                    config,
                                    submissionId,
                                    action.Id,
                                    rule.Id,
                                    executedByUserId);

                                if (result.Success)
                                {
                                    _logger?.LogInformation("CopyToDocument action {ActionId} executed successfully. TargetDocumentId: {TargetDocumentId}",
                                        action.Id, result.TargetDocumentId);
                                }
                                else
                                {
                                    _logger?.LogWarning("CopyToDocument action {ActionId} failed: {ErrorMessage}",
                                        action.Id, result.ErrorMessage);
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger?.LogError(ex, "Error executing CopyToDocument action {ActionId} for rule {RuleId}",
                                    action.Id, rule.Id);
                                // Continue with next action
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "Error processing rule {RuleId} for CopyToDocument actions", rule.Id);
                        // Continue with next rule
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error executing CopyToDocument actions for event {EventType}, submission {SubmissionId}",
                    eventType, submissionId);
                // Don't throw - action execution failures shouldn't break the main workflow
            }
        }
    }
}

