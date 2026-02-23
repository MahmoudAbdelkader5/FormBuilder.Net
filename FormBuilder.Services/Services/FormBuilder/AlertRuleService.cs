using AutoMapper;
using FormBuilder.API.Models;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domain.Interfaces.Repositories;
using FormBuilder.Domain.Interfaces.Services;
using formBuilder.Domian.Interfaces;
using FormBuilder.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Services.Services.FormBuilder
{
    public class AlertRuleService : IAlertRuleService
    {
        private readonly IunitOfwork _unitOfWork;
        private readonly IAlertRuleRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<AlertRuleService> _logger;
        private readonly AkhmanageItContext _identityContext;

        public AlertRuleService(
            IunitOfwork unitOfWork,
            IAlertRuleRepository repository,
            IMapper mapper,
            ILogger<AlertRuleService> logger,
            AkhmanageItContext identityContext)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _identityContext = identityContext;
        }

        public async Task<ApiResponse> GetAllAsync()
        {
            try
            {
                var rules = await _repository.GetAllAsync(
                    ar => !ar.IsDeleted,
                    ar => ar.DOCUMENT_TYPES,
                    ar => ar.EMAIL_TEMPLATES);
                var dtos = _mapper.Map<IEnumerable<AlertRuleDto>>(rules);
                
                // Map navigation properties
                foreach (var dto in dtos)
                {
                    var rule = rules.FirstOrDefault(r => r.Id == dto.Id);
                    if (rule != null)
                    {
                        dto.DocumentTypeName = rule.DOCUMENT_TYPES?.Name ?? string.Empty;
                        dto.EmailTemplateName = rule.EMAIL_TEMPLATES?.TemplateName ?? string.Empty;
                    }
                }

                return new ApiResponse(200, "Alert rules retrieved successfully", dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving alert rules");
                return new ApiResponse(500, $"Error retrieving alert rules: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetByIdAsync(int id)
        {
            try
            {
                var rule = await _repository.GetByIdAsync(id);
                if (rule == null)
                    return new ApiResponse(404, "Alert rule not found");

                var dto = _mapper.Map<AlertRuleDto>(rule);
                dto.DocumentTypeName = rule.DOCUMENT_TYPES?.Name ?? string.Empty;
                dto.EmailTemplateName = rule.EMAIL_TEMPLATES?.TemplateName ?? string.Empty;

                return new ApiResponse(200, "Alert rule retrieved successfully", dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving alert rule {Id}", id);
                return new ApiResponse(500, $"Error retrieving alert rule: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetByDocumentTypeIdAsync(int documentTypeId)
        {
            try
            {
                var rules = await _repository.GetByDocumentTypeIdAsync(documentTypeId);
                var dtos = _mapper.Map<IEnumerable<AlertRuleDto>>(rules);
                
                foreach (var dto in dtos)
                {
                    var rule = rules.FirstOrDefault(r => r.Id == dto.Id);
                    if (rule != null)
                    {
                        dto.DocumentTypeName = rule.DOCUMENT_TYPES?.Name ?? string.Empty;
                        dto.EmailTemplateName = rule.EMAIL_TEMPLATES?.TemplateName ?? string.Empty;
                    }
                }

                return new ApiResponse(200, "Alert rules retrieved successfully", dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving alert rules for document type {DocumentTypeId}", documentTypeId);
                return new ApiResponse(500, $"Error retrieving alert rules: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetByTriggerTypeAsync(string triggerType)
        {
            try
            {
                var rules = await _repository.GetByTriggerTypeAsync(triggerType);
                var dtos = _mapper.Map<IEnumerable<AlertRuleDto>>(rules);
                
                foreach (var dto in dtos)
                {
                    var rule = rules.FirstOrDefault(r => r.Id == dto.Id);
                    if (rule != null)
                    {
                        dto.DocumentTypeName = rule.DOCUMENT_TYPES?.Name ?? string.Empty;
                        dto.EmailTemplateName = rule.EMAIL_TEMPLATES?.TemplateName ?? string.Empty;
                    }
                }

                return new ApiResponse(200, "Alert rules retrieved successfully", dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving alert rules for trigger type {TriggerType}", triggerType);
                return new ApiResponse(500, $"Error retrieving alert rules: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetActiveByDocumentTypeAndTriggerAsync(int documentTypeId, string triggerType)
        {
            try
            {
                var rules = await _repository.GetActiveByDocumentTypeAndTriggerAsync(documentTypeId, triggerType);
                var dtos = _mapper.Map<IEnumerable<AlertRuleDto>>(rules);
                
                foreach (var dto in dtos)
                {
                    var rule = rules.FirstOrDefault(r => r.Id == dto.Id);
                    if (rule != null)
                    {
                        dto.DocumentTypeName = rule.DOCUMENT_TYPES?.Name ?? string.Empty;
                        dto.EmailTemplateName = rule.EMAIL_TEMPLATES?.TemplateName ?? string.Empty;
                    }
                }

                return new ApiResponse(200, "Active alert rules retrieved successfully", dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active alert rules for document type {DocumentTypeId} and trigger {TriggerType}", 
                    documentTypeId, triggerType);
                return new ApiResponse(500, $"Error retrieving alert rules: {ex.Message}");
            }
        }

        public async Task<ApiResponse> CreateAsync(CreateAlertRuleDto createDto)
        {
            try
            {
                if (createDto == null)
                    return new ApiResponse(400, "DTO is required");

                // Validate document type exists
                var documentType = await _unitOfWork.DocumentTypeRepository.GetByIdAsync(createDto.DocumentTypeId);
                if (documentType == null)
                    return new ApiResponse(404, "Document type not found");

                // Check if rule name already exists for this document type
                var nameExists = await _repository.RuleNameExistsAsync(createDto.DocumentTypeId, createDto.RuleName);
                if (nameExists)
                    return new ApiResponse(400, $"Rule name '{createDto.RuleName}' already exists for this document type");

                // Validate trigger type
                var validTriggerTypes = new[] { "FormSubmitted", "ApprovalRequired", "ApprovalApproved", "ApprovalRejected", "ApprovalReturned" };
                if (!validTriggerTypes.Contains(createDto.TriggerType))
                    return new ApiResponse(400, $"Invalid trigger type. Must be one of: {string.Join(", ", validTriggerTypes)}");

                // Validate notification type
                var validNotificationTypes = new[] { "Email", "Internal", "Both" };
                if (string.IsNullOrWhiteSpace(createDto.NotificationType) || !validNotificationTypes.Contains(createDto.NotificationType))
                    return new ApiResponse(400, $"Invalid notification type. Must be one of: {string.Join(", ", validNotificationTypes)}");

                // Validate target users have email if email notifications are enabled
                if (createDto.NotificationType == "Email" || createDto.NotificationType == "Both")
                {
                    var invalidTargets = await FindInvalidEmailTargetsAsync(createDto.TargetUserId);
                    if (invalidTargets.Any())
                        return new ApiResponse(400, $"TargetUserId contains users without email: {string.Join(", ", invalidTargets)}");
                }

                // Validate email template exists (if provided)
                if (createDto.EmailTemplateId.HasValue)
                {
                    var templateExists = await _unitOfWork.AppDbContext.Set<EMAIL_TEMPLATES>()
                        .AnyAsync(t => t.Id == createDto.EmailTemplateId.Value && t.IsActive && !t.IsDeleted);
                    if (!templateExists)
                        return new ApiResponse(404, "Email template not found");
                }

                var entity = _mapper.Map<ALERT_RULES>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.UpdatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                entity.ConditionJson = createDto.ConditionJson ?? "{}";

                _repository.Add(entity);
                await _unitOfWork.CompleteAsyn();

                // Reload with includes to populate navigation props (DOCUMENT_TYPES / EMAIL_TEMPLATES)
                var created = await _repository.GetByIdAsync(entity.Id);
                var dto = _mapper.Map<AlertRuleDto>(created ?? entity);
                dto.DocumentTypeName = created?.DOCUMENT_TYPES?.Name ?? documentType.Name;
                dto.EmailTemplateName = created?.EMAIL_TEMPLATES?.TemplateName ?? string.Empty;

                return new ApiResponse(200, "Alert rule created successfully", dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating alert rule");
                return new ApiResponse(500, $"Error creating alert rule: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdateAsync(int id, UpdateAlertRuleDto updateDto)
        {
            try
            {
                if (updateDto == null)
                    return new ApiResponse(400, "DTO is required");

                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponse(404, "Alert rule not found");

                // Check if rule name already exists (if changed)
                if (!string.IsNullOrEmpty(updateDto.RuleName) && updateDto.RuleName != entity.RuleName)
                {
                    var nameExists = await _repository.RuleNameExistsAsync(entity.DocumentTypeId, updateDto.RuleName, id);
                    if (nameExists)
                        return new ApiResponse(400, $"Rule name '{updateDto.RuleName}' already exists for this document type");
                }

                // Validate trigger type if changed
                if (!string.IsNullOrEmpty(updateDto.TriggerType))
                {
                    var validTriggerTypes = new[] { "FormSubmitted", "ApprovalRequired", "ApprovalApproved", "ApprovalRejected", "ApprovalReturned" };
                    if (!validTriggerTypes.Contains(updateDto.TriggerType))
                        return new ApiResponse(400, $"Invalid trigger type. Must be one of: {string.Join(", ", validTriggerTypes)}");
                }

                // Validate notification type if changed
                if (!string.IsNullOrWhiteSpace(updateDto.NotificationType))
                {
                    var validNotificationTypes = new[] { "Email", "Internal", "Both" };
                    if (!validNotificationTypes.Contains(updateDto.NotificationType))
                        return new ApiResponse(400, $"Invalid notification type. Must be one of: {string.Join(", ", validNotificationTypes)}");
                }

                var effectiveNotificationType = !string.IsNullOrWhiteSpace(updateDto.NotificationType)
                    ? updateDto.NotificationType
                    : entity.NotificationType;
                var effectiveTargetUserId = updateDto.TargetUserId ?? entity.TargetUserId;

                if (effectiveNotificationType == "Email" || effectiveNotificationType == "Both")
                {
                    var invalidTargets = await FindInvalidEmailTargetsAsync(effectiveTargetUserId);
                    if (invalidTargets.Any())
                        return new ApiResponse(400, $"TargetUserId contains users without email: {string.Join(", ", invalidTargets)}");
                }

                // Validate email template exists (if provided)
                if (updateDto.EmailTemplateId.HasValue)
                {
                    var templateExists = await _unitOfWork.AppDbContext.Set<EMAIL_TEMPLATES>()
                        .AnyAsync(t => t.Id == updateDto.EmailTemplateId.Value && t.IsActive && !t.IsDeleted);
                    if (!templateExists)
                        return new ApiResponse(404, "Email template not found");
                }

                // Update properties
                if (!string.IsNullOrEmpty(updateDto.RuleName))
                    entity.RuleName = updateDto.RuleName;
                if (!string.IsNullOrEmpty(updateDto.TriggerType))
                    entity.TriggerType = updateDto.TriggerType;
                if (updateDto.ConditionJson != null)
                    entity.ConditionJson = updateDto.ConditionJson;
                if (updateDto.EmailTemplateId.HasValue)
                    entity.EmailTemplateId = updateDto.EmailTemplateId;
                if (!string.IsNullOrEmpty(updateDto.NotificationType))
                    entity.NotificationType = updateDto.NotificationType;
                if (updateDto.TargetRoleId != null)
                    entity.TargetRoleId = updateDto.TargetRoleId;
                if (updateDto.TargetUserId != null)
                    entity.TargetUserId = updateDto.TargetUserId;
                if (updateDto.IsActive.HasValue)
                    entity.IsActive = updateDto.IsActive.Value;

                entity.UpdatedDate = DateTime.UtcNow;

                _repository.Update(entity);
                await _unitOfWork.CompleteAsyn();

                // Reload to ensure updated navigation props (especially after changing EmailTemplateId)
                var updated = await _repository.GetByIdAsync(id);
                var dto = _mapper.Map<AlertRuleDto>(updated ?? entity);
                dto.DocumentTypeName = (updated ?? entity).DOCUMENT_TYPES?.Name ?? string.Empty;
                dto.EmailTemplateName = (updated ?? entity).EMAIL_TEMPLATES?.TemplateName ?? string.Empty;

                return new ApiResponse(200, "Alert rule updated successfully", dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating alert rule {Id}", id);
                return new ApiResponse(500, $"Error updating alert rule: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponse(404, "Alert rule not found");

                // Soft delete
                entity.IsDeleted = true;
                entity.DeletedDate = DateTime.UtcNow;
                entity.IsActive = false;

                _repository.Update(entity);
                await _unitOfWork.CompleteAsyn();

                return new ApiResponse(200, "Alert rule deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting alert rule {Id}", id);
                return new ApiResponse(500, $"Error deleting alert rule: {ex.Message}");
            }
        }

        public async Task<ApiResponse> ActivateAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponse(404, "Alert rule not found");

                if (entity.NotificationType == "Email" || entity.NotificationType == "Both")
                {
                    var invalidTargets = await FindInvalidEmailTargetsAsync(entity.TargetUserId);
                    if (invalidTargets.Any())
                        return new ApiResponse(400, $"TargetUserId contains users without email: {string.Join(", ", invalidTargets)}");
                }

                entity.IsActive = true;
                entity.UpdatedDate = DateTime.UtcNow;

                _repository.Update(entity);
                await _unitOfWork.CompleteAsyn();

                return new ApiResponse(200, "Alert rule activated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating alert rule {Id}", id);
                return new ApiResponse(500, $"Error activating alert rule: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeactivateAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponse(404, "Alert rule not found");

                entity.IsActive = false;
                entity.UpdatedDate = DateTime.UtcNow;

                _repository.Update(entity);
                await _unitOfWork.CompleteAsyn();

                return new ApiResponse(200, "Alert rule deactivated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating alert rule {Id}", id);
                return new ApiResponse(500, $"Error deactivating alert rule: {ex.Message}");
            }
        }

        private async Task<List<string>> FindInvalidEmailTargetsAsync(string? targetUserId)
        {
            var invalidTargets = new List<string>();
            if (string.IsNullOrWhiteSpace(targetUserId)) return invalidTargets;

            var userIds = targetUserId.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            foreach (var userId in userIds)
            {
                if (userId.Equals("public-user", StringComparison.OrdinalIgnoreCase))
                {
                    invalidTargets.Add(userId);
                    continue;
                }

                if (userId.Contains("@")) continue; // direct email is valid

                if (int.TryParse(userId, out var userIdInt))
                {
                    var user = await _identityContext.TblUsers
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => u.Id == userIdInt);

                    if (user == null || (string.IsNullOrWhiteSpace(user.Email) &&
                                         (string.IsNullOrWhiteSpace(user.Username) || !user.Username.Contains("@"))))
                    {
                        invalidTargets.Add(userId);
                    }
                }
                else
                {
                    var user = await _identityContext.TblUsers
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => u.Username.ToLower() == userId.ToLower());

                    if (user == null || (string.IsNullOrWhiteSpace(user.Email) &&
                                         (string.IsNullOrWhiteSpace(user.Username) || !user.Username.Contains("@"))))
                    {
                        invalidTargets.Add(userId);
                    }
                }
            }

            return invalidTargets;
        }
    }
}

