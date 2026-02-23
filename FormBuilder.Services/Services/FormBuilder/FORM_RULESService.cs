using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Core.DTOS.FormRules;
using FormBuilder.Core.IServices.FormBuilder;
using FormBuilder.Domian.Entitys.froms;
using FormBuilder.Infrastructure.Data;
using FormBuilder.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FormBuilder.Domian.Interfaces;

namespace FormBuilder.Services.Services
{
    public class FORM_RULESService : IFORM_RULESService
    {
        private readonly IunitOfwork _unitOfWork;
        private readonly FormBuilderDbContext _dbContext;
        private readonly IFormRuleEvaluationService? _ruleEvaluationService;
        private readonly ILogger<FORM_RULESService>? _logger;
        private readonly IFormStoredProceduresRepository? _storedProceduresRepository;

        public FORM_RULESService(IunitOfwork unitOfWork, FormBuilderDbContext dbContext, IFormRuleEvaluationService? ruleEvaluationService = null, ILogger<FORM_RULESService>? logger = null, IFormStoredProceduresRepository? storedProceduresRepository = null)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _ruleEvaluationService = ruleEvaluationService;
            _logger = logger;
            _storedProceduresRepository = storedProceduresRepository;
        }

        public async Task<FORM_RULES> CreateRuleAsync(CreateFormRuleDto ruleDto)
        {
            if (ruleDto == null)
                throw new ArgumentNullException(nameof(ruleDto));

            // Validate form exists
            var formExists = await _unitOfWork.Repositary<FORM_BUILDER>()
                .SingleOrDefaultAsync(f => f.Id == ruleDto.FormBuilderId);
            if (formExists == null)
                throw new InvalidOperationException($"Form with ID '{ruleDto.FormBuilderId}' does not exist.");

            // Validate rule name uniqueness
            if (!await IsRuleNameUniqueAsync(ruleDto.FormBuilderId, ruleDto.RuleName))
            {
                // Log duplicate warning
                var existingRule = await _unitOfWork.Repositary<FORM_RULES>()
                    .SingleOrDefaultAsync(r => r.FormBuilderId == ruleDto.FormBuilderId &&
                                              r.RuleName == ruleDto.RuleName.Trim() &&
                                              (!r.IsDeleted || r.IsDeleted)); // Check both deleted and non-deleted
                
                if (existingRule != null)
                {
                    DuplicateValidationHelper.LogDuplicateDetection(
                        _logger,
                        "FormRule",
                        "RuleName",
                        ruleDto.RuleName,
                        existingRule.Id,
                        $"FormBuilderId: {ruleDto.FormBuilderId}",
                        existingRule.IsDeleted
                    );
                }
                else
                {
                    DuplicateValidationHelper.LogDuplicateWarning(
                        _logger,
                        "FormRule",
                        "RuleName",
                        ruleDto.RuleName,
                        $"FormBuilderId: {ruleDto.FormBuilderId}"
                    );
                }

                var errorMessage = DuplicateValidationHelper.FormatDuplicateErrorMessage(
                    "Rule",
                    "name",
                    ruleDto.RuleName,
                    $"for this form (FormBuilderId: {ruleDto.FormBuilderId})"
                );
                throw new InvalidOperationException(errorMessage);
            }

            // Determine rule type (default to Condition for backward compatibility)
            var ruleType = string.IsNullOrWhiteSpace(ruleDto.RuleType) ? "Condition" : ruleDto.RuleType;

            // Validate based on rule type
            if (ruleType.Equals("StoredProcedure", StringComparison.OrdinalIgnoreCase))
            {
                // If StoredProcedureId is provided, look up the stored procedure from whitelist
                if (ruleDto.StoredProcedureId.HasValue && ruleDto.StoredProcedureId.Value > 0)
                {
                    if (_storedProceduresRepository == null)
                    {
                        throw new InvalidOperationException("StoredProceduresRepository is not configured. Please register IFormStoredProceduresRepository in DI container.");
                    }

                    var storedProcedure = await _storedProceduresRepository.SingleOrDefaultAsync(
                        sp => sp.Id == ruleDto.StoredProcedureId.Value && sp.IsActive && !sp.IsDeleted);

                    if (storedProcedure == null)
                    {
                        throw new InvalidOperationException($"Stored procedure with ID {ruleDto.StoredProcedureId.Value} not found in whitelist or is not active.");
                    }

                    // Populate name and database from the whitelist entry
                    ruleDto.StoredProcedureName = storedProcedure.ProcedureName;
                    if (string.IsNullOrWhiteSpace(ruleDto.StoredProcedureName) && !string.IsNullOrWhiteSpace(storedProcedure.ProcedureCode))
                    {
                        // Extract procedure name from ProcedureCode if ProcedureName is not set
                        ruleDto.StoredProcedureName = ExtractProcedureNameFromCode(storedProcedure.ProcedureCode);
                    }

                    ruleDto.StoredProcedureDatabase = storedProcedure.DatabaseName;

                    if (string.IsNullOrWhiteSpace(ruleDto.StoredProcedureName))
                    {
                        throw new InvalidOperationException($"Cannot determine procedure name for stored procedure ID {ruleDto.StoredProcedureId.Value}. Either ProcedureName or ProcedureCode must be provided in the whitelist entry.");
                    }
                }
                else
                {
                    // Validate Stored Procedure fields (when ID is not provided)
                    if (string.IsNullOrWhiteSpace(ruleDto.StoredProcedureName))
                        throw new InvalidOperationException("Stored Procedure Name is required for StoredProcedure rule type. Either provide StoredProcedureId or StoredProcedureName.");

                    if (string.IsNullOrWhiteSpace(ruleDto.StoredProcedureDatabase))
                        throw new InvalidOperationException("Stored Procedure Database is required for StoredProcedure rule type. Either provide StoredProcedureId or StoredProcedureDatabase.");

                    if (!ruleDto.StoredProcedureDatabase.Equals("FormBuilder", StringComparison.OrdinalIgnoreCase) &&
                        !ruleDto.StoredProcedureDatabase.Equals("AKHManageIT", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new InvalidOperationException("Stored Procedure Database must be either 'FormBuilder' or 'AKHManageIT'.");
                    }
                }
            }
            else
            {
                // Validate condition fields (required for Condition type)
                // For Blocking Rules, ConditionKey can be used instead of ConditionField
                bool isBlockingRule = !string.IsNullOrWhiteSpace(ruleDto.EvaluationPhase);
                bool hasConditionField = !string.IsNullOrWhiteSpace(ruleDto.ConditionField);
                bool hasConditionKey = !string.IsNullOrWhiteSpace(ruleDto.ConditionKey);
                
                if (!hasConditionField && !hasConditionKey)
                {
                    if (isBlockingRule)
                    {
                        throw new InvalidOperationException("Condition Field or Condition Key is required for Condition rule type. For Blocking Rules, use ConditionKey.");
                    }
                    else
                    {
                        throw new InvalidOperationException("Condition Field is required for Condition rule type.");
                    }
                }

                if (string.IsNullOrWhiteSpace(ruleDto.ConditionOperator))
                    throw new InvalidOperationException("Condition Operator is required for Condition rule type.");
            }

            // Get actions directly from DTO (no JSON parsing needed)
            List<ActionDataDto>? actions = ruleDto.Actions;
            List<ActionDataDto>? elseActions = ruleDto.ElseActions;

            // Validate actions
            if (actions != null && actions.Any())
            {
                var validActionTypes = new[] { "SetVisible", "SetReadOnly", "SetMandatory", "SetDefault", "ClearValue", "Compute", "Block", "CopyToDocument" };
                foreach (var action in actions)
                {
                    if (string.IsNullOrEmpty(action.Type))
                        throw new InvalidOperationException("Action Type is required.");
                    // FieldCode is optional for Block and CopyToDocument actions
                    if (action.Type != "Block" && action.Type != "CopyToDocument" && string.IsNullOrEmpty(action.FieldCode))
                        throw new InvalidOperationException("Action FieldCode is required (except for Block and CopyToDocument actions).");
                    if (!validActionTypes.Contains(action.Type))
                        throw new InvalidOperationException($"Invalid action type: {action.Type}");
                    if (action.Type == "Compute" && string.IsNullOrEmpty(action.Expression))
                        throw new InvalidOperationException("Expression is required for Compute action.");
                    if (action.Type == "CopyToDocument" && action.Value == null)
                        throw new InvalidOperationException("Value (CopyToDocument configuration) is required for CopyToDocument action.");
                }
            }

            // Validate else actions
            if (elseActions != null && elseActions.Any())
            {
                var validActionTypes = new[] { "SetVisible", "SetReadOnly", "SetMandatory", "SetDefault", "ClearValue", "Compute", "Block", "CopyToDocument" };
                foreach (var action in elseActions)
                {
                    if (string.IsNullOrEmpty(action.Type))
                        throw new InvalidOperationException("Else Action Type is required.");
                    // FieldCode is optional for Block action
                    if (action.Type != "Block" && string.IsNullOrEmpty(action.FieldCode))
                        throw new InvalidOperationException("Else Action FieldCode is required (except for Block action).");
                    if (!validActionTypes.Contains(action.Type))
                        throw new InvalidOperationException($"Invalid else action type: {action.Type}");
                    if (action.Type == "Compute" && string.IsNullOrEmpty(action.Expression))
                        throw new InvalidOperationException("Expression is required for Compute action.");
                    if (action.Type == "CopyToDocument" && action.Value == null)
                        throw new InvalidOperationException("Value (CopyToDocument configuration) is required for CopyToDocument action.");
                }
            }

            // For Blocking Rules, use ConditionKey if provided, otherwise use ConditionField
            var conditionField = !string.IsNullOrWhiteSpace(ruleDto.ConditionKey) 
                ? ruleDto.ConditionKey 
                : ruleDto.ConditionField;

            var ruleEntity = new FORM_RULES
            {
                FormBuilderId = ruleDto.FormBuilderId,
                RuleName = ruleDto.RuleName,
                RuleType = ruleType,
                ConditionField = conditionField,
                ConditionOperator = ruleDto.ConditionOperator,
                ConditionValue = ruleDto.ConditionValue,
                ConditionValueType = ruleDto.ConditionValueType ?? "constant",
                StoredProcedureId = ruleDto.StoredProcedureId,
                StoredProcedureName = ruleDto.StoredProcedureName,
                StoredProcedureDatabase = ruleDto.StoredProcedureDatabase,
                ParameterMapping = ruleDto.ParameterMapping,
                ResultMapping = ruleDto.ResultMapping,
                IsActive = ruleDto.IsActive,
                ExecutionOrder = ruleDto.ExecutionOrder ?? 1,
                // Blocking Rules fields
                EvaluationPhase = ruleDto.EvaluationPhase,
                ConditionSource = ruleDto.ConditionSource,
                ConditionKey = ruleDto.ConditionKey,
                BlockMessage = ruleDto.BlockMessage,
                Priority = ruleDto.Priority
            };

            _unitOfWork.Repositary<FORM_RULES>().Add(ruleEntity);
            await _unitOfWork.CompleteAsyn();

            // Save Actions to separate table
            if (actions != null && actions.Any())
            {
                int actionOrder = 1;
                foreach (var action in actions)
                {
                    var ruleAction = new FORM_RULE_ACTIONS
                    {
                        RuleId = ruleEntity.Id,
                        ActionType = action.Type,
                        FieldCode = action.FieldCode,
                        Value = action.Value?.ToString(),
                        Expression = action.Expression,
                        IsElseAction = false,
                        ActionOrder = actionOrder++,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow
                    };
                    _unitOfWork.Repositary<FORM_RULE_ACTIONS>().Add(ruleAction);
                }
            }

            // Save Else Actions to separate table
            if (elseActions != null && elseActions.Any())
            {
                int actionOrder = 1;
                foreach (var action in elseActions)
                {
                    var ruleAction = new FORM_RULE_ACTIONS
                    {
                        RuleId = ruleEntity.Id,
                        ActionType = action.Type,
                        FieldCode = action.FieldCode,
                        Value = action.Value?.ToString(),
                        Expression = action.Expression,
                        IsElseAction = true,
                        ActionOrder = actionOrder++,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow
                    };
                    _unitOfWork.Repositary<FORM_RULE_ACTIONS>().Add(ruleAction);
                }
            }

            await _unitOfWork.CompleteAsyn();

            return ruleEntity;
        }

        public async Task<FORM_RULES> GetRuleByIdAsync(int id)
        {
            var rules = await _unitOfWork.Repositary<FORM_RULES>()
                .GetAllAsync(
                    filter: r => r.Id == id && !r.IsDeleted,
                    r => r.FORM_RULE_ACTIONS, r => r.FORM_BUILDER
                );
            return rules.FirstOrDefault();
        }

        public async Task<IEnumerable<FormRuleDto>> GetAllRulesAsync()
        {
            // 1. Update GetAllAsync to include the related FORM_BUILDERS entity.
            var rules = await _unitOfWork.Repositary<FORM_RULES>()
                .GetAllAsync(
                    filter: r => !r.IsDeleted, // Exclude soft-deleted rules
                    rule => rule.FORM_BUILDER, rule => rule.FORM_RULE_ACTIONS
                );

            // 2. Update the Select to map the new fields from the included entity.
            return rules.Select(rule => 
            {
                // Load actions from separate table
                var actions = rule.FORM_RULE_ACTIONS?
                    .Where(a => !a.IsElseAction && a.IsActive)
                    .OrderBy(a => a.ActionOrder)
                    .Select(a => new ActionDataDto
                    {
                        Type = a.ActionType,
                        FieldCode = a.FieldCode,
                        Value = a.Value,
                        Expression = a.Expression
                    })
                    .ToList() ?? new List<ActionDataDto>();

                var elseActions = rule.FORM_RULE_ACTIONS?
                    .Where(a => a.IsElseAction && a.IsActive)
                    .OrderBy(a => a.ActionOrder)
                    .Select(a => new ActionDataDto
                    {
                        Type = a.ActionType,
                        FieldCode = a.FieldCode,
                        Value = a.Value,
                        Expression = a.Expression
                    })
                    .ToList() ?? new List<ActionDataDto>();

                return new FormRuleDto
                {
                    Id = rule.Id,
                    FormBuilderId = rule.FormBuilderId,
                    RuleName = rule.RuleName,
                    RuleType = rule.RuleType,
                    ConditionField = rule.ConditionField,
                    ConditionOperator = rule.ConditionOperator,
                    ConditionValue = rule.ConditionValue,
                    ConditionValueType = rule.ConditionValueType,
                    StoredProcedureId = rule.StoredProcedureId,
                    StoredProcedureName = rule.StoredProcedureName,
                    StoredProcedureDatabase = rule.StoredProcedureDatabase,
                    ParameterMapping = rule.ParameterMapping,
                    ResultMapping = rule.ResultMapping,
                    Actions = actions.Any() ? actions : null, // Return as List, not JSON
                    ElseActions = elseActions.Any() ? elseActions : null, // Return as List, not JSON
                    IsActive = rule.IsActive,
                    IsDeleted = rule.IsDeleted,
                    DeletedDate = rule.DeletedDate,
                    ExecutionOrder = rule.ExecutionOrder ?? 1,
                    FormName = rule.FORM_BUILDER?.FormName ?? string.Empty,
                    FormCode = rule.FORM_BUILDER?.FormCode ?? string.Empty,
                    // Blocking Rules fields
                    EvaluationPhase = rule.EvaluationPhase,
                    ConditionSource = rule.ConditionSource,
                    ConditionKey = rule.ConditionKey,
                    BlockMessage = rule.BlockMessage,
                    Priority = rule.Priority
                };
            }).ToList();
        }

        public async Task<bool> UpdateRuleAsync(UpdateFormRuleDto ruleDto,int id)
        {
            if (ruleDto == null)
                throw new ArgumentNullException(nameof(ruleDto));

            var existingRule = await GetRuleByIdAsync(id);
            if (existingRule == null)
                throw new InvalidOperationException($"Rule with ID '{id}' does not exist.");

            // Validate rule name uniqueness (excluding current rule)
            if (!await IsRuleNameUniqueAsync(ruleDto.FormBuilderId, ruleDto.RuleName, id))
            {
                // Log duplicate warning
                var conflictingRule = await _unitOfWork.Repositary<FORM_RULES>()
                    .SingleOrDefaultAsync(r => r.FormBuilderId == ruleDto.FormBuilderId &&
                                              r.RuleName == ruleDto.RuleName.Trim() &&
                                              r.Id != id);
                
                if (conflictingRule != null)
                {
                    DuplicateValidationHelper.LogDuplicateDetection(
                        _logger,
                        "FormRule",
                        "RuleName",
                        ruleDto.RuleName,
                        conflictingRule.Id,
                        $"FormBuilderId: {ruleDto.FormBuilderId}",
                        conflictingRule.IsDeleted
                    );
                }

                var errorMessage = DuplicateValidationHelper.FormatDuplicateErrorMessage(
                    "Rule",
                    "name",
                    ruleDto.RuleName,
                    $"for this form (FormBuilderId: {ruleDto.FormBuilderId})"
                );
                throw new InvalidOperationException(errorMessage);
            }

            // Determine rule type (default to existing or Condition)
            var ruleType = string.IsNullOrWhiteSpace(ruleDto.RuleType) 
                ? (existingRule.RuleType ?? "Condition") 
                : ruleDto.RuleType;

            // Validate based on rule type
            if (ruleType.Equals("StoredProcedure", StringComparison.OrdinalIgnoreCase))
            {
                // If StoredProcedureId is provided, look up the stored procedure from whitelist
                if (ruleDto.StoredProcedureId.HasValue && ruleDto.StoredProcedureId.Value > 0)
                {
                    if (_storedProceduresRepository == null)
                    {
                        throw new InvalidOperationException("StoredProceduresRepository is not configured. Please register IFormStoredProceduresRepository in DI container.");
                    }

                    var storedProcedure = await _storedProceduresRepository.SingleOrDefaultAsync(
                        sp => sp.Id == ruleDto.StoredProcedureId.Value && sp.IsActive && !sp.IsDeleted);

                    if (storedProcedure == null)
                    {
                        throw new InvalidOperationException($"Stored procedure with ID {ruleDto.StoredProcedureId.Value} not found in whitelist or is not active.");
                    }

                    // Populate name and database from the whitelist entry
                    ruleDto.StoredProcedureName = storedProcedure.ProcedureName;
                    if (string.IsNullOrWhiteSpace(ruleDto.StoredProcedureName) && !string.IsNullOrWhiteSpace(storedProcedure.ProcedureCode))
                    {
                        // Extract procedure name from ProcedureCode if ProcedureName is not set
                        ruleDto.StoredProcedureName = ExtractProcedureNameFromCode(storedProcedure.ProcedureCode);
                    }

                    ruleDto.StoredProcedureDatabase = storedProcedure.DatabaseName;

                    if (string.IsNullOrWhiteSpace(ruleDto.StoredProcedureName))
                    {
                        throw new InvalidOperationException($"Cannot determine procedure name for stored procedure ID {ruleDto.StoredProcedureId.Value}. Either ProcedureName or ProcedureCode must be provided in the whitelist entry.");
                    }
                }
                else
                {
                    // Validate Stored Procedure fields (when ID is not provided)
                    if (string.IsNullOrWhiteSpace(ruleDto.StoredProcedureName))
                        throw new InvalidOperationException("Stored Procedure Name is required for StoredProcedure rule type. Either provide StoredProcedureId or StoredProcedureName.");

                    if (string.IsNullOrWhiteSpace(ruleDto.StoredProcedureDatabase))
                        throw new InvalidOperationException("Stored Procedure Database is required for StoredProcedure rule type. Either provide StoredProcedureId or StoredProcedureDatabase.");

                    if (!ruleDto.StoredProcedureDatabase.Equals("FormBuilder", StringComparison.OrdinalIgnoreCase) &&
                        !ruleDto.StoredProcedureDatabase.Equals("AKHManageIT", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new InvalidOperationException("Stored Procedure Database must be either 'FormBuilder' or 'AKHManageIT'.");
                    }
                }
            }
            else
            {
                // Validate condition fields (required for Condition type)
                // For Blocking Rules, ConditionKey can be used instead of ConditionField
                bool isBlockingRule = !string.IsNullOrWhiteSpace(ruleDto.EvaluationPhase);
                bool hasConditionField = !string.IsNullOrWhiteSpace(ruleDto.ConditionField);
                bool hasConditionKey = !string.IsNullOrWhiteSpace(ruleDto.ConditionKey);
                
                if (!hasConditionField && !hasConditionKey)
                {
                    if (isBlockingRule)
                    {
                        throw new InvalidOperationException("Condition Field or Condition Key is required for Condition rule type. For Blocking Rules, use ConditionKey.");
                    }
                    else
                    {
                        throw new InvalidOperationException("Condition Field is required for Condition rule type.");
                    }
                }

                if (string.IsNullOrWhiteSpace(ruleDto.ConditionOperator))
                    throw new InvalidOperationException("Condition Operator is required for Condition rule type.");
            }

            // Get actions directly from DTO (no JSON parsing needed)
            List<ActionDataDto>? actions = ruleDto.Actions;
            List<ActionDataDto>? elseActions = ruleDto.ElseActions;

            // Validate actions
            if (actions != null && actions.Any())
            {
                var validActionTypes = new[] { "SetVisible", "SetReadOnly", "SetMandatory", "SetDefault", "ClearValue", "Compute", "Block", "CopyToDocument" };
                foreach (var action in actions)
                {
                    if (string.IsNullOrEmpty(action.Type))
                        throw new InvalidOperationException("Action Type is required.");
                    // FieldCode is optional for Block and CopyToDocument actions
                    if (action.Type != "Block" && action.Type != "CopyToDocument" && string.IsNullOrEmpty(action.FieldCode))
                        throw new InvalidOperationException("Action FieldCode is required (except for Block and CopyToDocument actions).");
                    if (!validActionTypes.Contains(action.Type))
                        throw new InvalidOperationException($"Invalid action type: {action.Type}");
                    if (action.Type == "Compute" && string.IsNullOrEmpty(action.Expression))
                        throw new InvalidOperationException("Expression is required for Compute action.");
                    if (action.Type == "CopyToDocument" && action.Value == null)
                        throw new InvalidOperationException("Value (CopyToDocument configuration) is required for CopyToDocument action.");
                }
            }

            // Validate else actions
            if (elseActions != null && elseActions.Any())
            {
                var validActionTypes = new[] { "SetVisible", "SetReadOnly", "SetMandatory", "SetDefault", "ClearValue", "Compute", "Block", "CopyToDocument" };
                foreach (var action in elseActions)
                {
                    if (string.IsNullOrEmpty(action.Type))
                        throw new InvalidOperationException("Else Action Type is required.");
                    // FieldCode is optional for Block action
                    if (action.Type != "Block" && string.IsNullOrEmpty(action.FieldCode))
                        throw new InvalidOperationException("Else Action FieldCode is required (except for Block action).");
                    if (!validActionTypes.Contains(action.Type))
                        throw new InvalidOperationException($"Invalid else action type: {action.Type}");
                    if (action.Type == "Compute" && string.IsNullOrEmpty(action.Expression))
                        throw new InvalidOperationException("Expression is required for Compute action.");
                    if (action.Type == "CopyToDocument" && action.Value == null)
                        throw new InvalidOperationException("Value (CopyToDocument configuration) is required for CopyToDocument action.");
                }
            }

            // For Blocking Rules, use ConditionKey if provided, otherwise use ConditionField
            var conditionField = !string.IsNullOrWhiteSpace(ruleDto.ConditionKey) 
                ? ruleDto.ConditionKey 
                : ruleDto.ConditionField;

            // Update properties
            existingRule.FormBuilderId = ruleDto.FormBuilderId;
            existingRule.RuleName = ruleDto.RuleName;
            existingRule.RuleType = ruleType;
            existingRule.ConditionField = conditionField;
            existingRule.ConditionOperator = ruleDto.ConditionOperator;
            existingRule.ConditionValue = ruleDto.ConditionValue;
            existingRule.ConditionValueType = ruleDto.ConditionValueType ?? existingRule.ConditionValueType ?? "constant";
            
            // Update Blocking Rules fields
            if (!string.IsNullOrWhiteSpace(ruleDto.EvaluationPhase))
                existingRule.EvaluationPhase = ruleDto.EvaluationPhase;
            if (!string.IsNullOrWhiteSpace(ruleDto.ConditionSource))
                existingRule.ConditionSource = ruleDto.ConditionSource;
            if (!string.IsNullOrWhiteSpace(ruleDto.ConditionKey))
                existingRule.ConditionKey = ruleDto.ConditionKey;
            if (!string.IsNullOrWhiteSpace(ruleDto.BlockMessage))
                existingRule.BlockMessage = ruleDto.BlockMessage;
            if (ruleDto.Priority.HasValue)
                existingRule.Priority = ruleDto.Priority;
            
            // Update StoredProcedureId if provided
            if (ruleDto.StoredProcedureId.HasValue)
            {
                existingRule.StoredProcedureId = ruleDto.StoredProcedureId.Value;
            }
            
            existingRule.StoredProcedureId = ruleDto.StoredProcedureId;
            existingRule.StoredProcedureName = ruleDto.StoredProcedureName;
            existingRule.StoredProcedureDatabase = ruleDto.StoredProcedureDatabase;
            existingRule.ParameterMapping = ruleDto.ParameterMapping;
            existingRule.ResultMapping = ruleDto.ResultMapping;
            existingRule.IsActive = ruleDto.IsActive;
            existingRule.ExecutionOrder = ruleDto.ExecutionOrder ?? existingRule.ExecutionOrder ?? 1;

            _unitOfWork.Repositary<FORM_RULES>().Update(existingRule);

            // Soft Delete existing actions using DbContext directly to avoid tracking conflicts
            var actionsToDelete = await _dbContext.FORM_RULE_ACTIONS
                .Where(a => a.RuleId == existingRule.Id && !a.IsDeleted)
                .ToListAsync();
            
            if (actionsToDelete.Any())
            {
                foreach (var action in actionsToDelete)
                {
                    action.IsDeleted = true;
                    action.DeletedDate = DateTime.UtcNow;
                    action.IsActive = false;
                }
            }

            // Save new Actions to separate table
            if (actions != null && actions.Any())
            {
                int actionOrder = 1;
                foreach (var action in actions)
                {
                    var ruleAction = new FORM_RULE_ACTIONS
                    {
                        RuleId = existingRule.Id,
                        ActionType = action.Type,
                        FieldCode = action.FieldCode,
                        Value = action.Value?.ToString(),
                        Expression = action.Expression,
                        IsElseAction = false,
                        ActionOrder = actionOrder++,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow
                    };
                    _unitOfWork.Repositary<FORM_RULE_ACTIONS>().Add(ruleAction);
                }
            }

            // Save new Else Actions to separate table
            if (elseActions != null && elseActions.Any())
            {
                int actionOrder = 1;
                foreach (var action in elseActions)
                {
                    var ruleAction = new FORM_RULE_ACTIONS
                    {
                        RuleId = existingRule.Id,
                        ActionType = action.Type,
                        FieldCode = action.FieldCode,
                        Value = action.Value?.ToString(),
                        Expression = action.Expression,
                        IsElseAction = true,
                        ActionOrder = actionOrder++,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow
                    };
                    _unitOfWork.Repositary<FORM_RULE_ACTIONS>().Add(ruleAction);
                }
            }

            var result = await _unitOfWork.CompleteAsyn();

            return result > 0;
        }

        public async Task<bool> DeleteRuleAsync(int id)
        {
            var ruleToDelete = await GetRuleByIdAsync(id);
            if (ruleToDelete == null || ruleToDelete.IsDeleted)
                return false;

            // Use soft delete with IsDeleted flag
            ruleToDelete.IsDeleted = true;
            ruleToDelete.DeletedDate = DateTime.UtcNow;
            ruleToDelete.UpdatedDate = DateTime.UtcNow;
            _unitOfWork.Repositary<FORM_RULES>().Update(ruleToDelete);
            var result = await _unitOfWork.CompleteAsyn();
            return result > 0;
        }

        public async Task<bool> RestoreRuleAsync(int id)
        {
            var ruleToRestore = await _unitOfWork.Repositary<FORM_RULES>()
                .SingleOrDefaultAsync(r => r.Id == id && r.IsDeleted);
            
            if (ruleToRestore == null)
                return false;

            ruleToRestore.IsDeleted = false;
            ruleToRestore.DeletedDate = null;
            ruleToRestore.DeletedByUserId = null;
            ruleToRestore.UpdatedDate = DateTime.UtcNow;
            _unitOfWork.Repositary<FORM_RULES>().Update(ruleToRestore);
            var result = await _unitOfWork.CompleteAsyn();
            return result > 0;
        }

        public async Task<bool> IsRuleNameUniqueAsync(int formBuilderId, string ruleName, int? ignoreId = null)
        {
            if (string.IsNullOrWhiteSpace(ruleName))
                return false;

            // Use the specialized repository method which already handles IsDeleted filtering
            return await _unitOfWork.FORM_RULESRepository.IsRuleNameUniqueAsync(formBuilderId, ruleName, ignoreId);
        }

        public async Task<bool> RuleExistsAsync(int id)
        {
            return await _unitOfWork.Repositary<FORM_RULES>()
                .AnyAsync(r => r.Id == id && !r.IsDeleted);
        }

        public async Task<IEnumerable<FormRuleDto>> GetActiveRulesByFormIdAsync(int formBuilderId)
        {
            // Use filter directly instead of Where after GetAllAsync
            var activeRules = await _unitOfWork.Repositary<FORM_RULES>()
                .GetAllAsync(
                    filter: r => r.FormBuilderId == formBuilderId && r.IsActive && !r.IsDeleted,
                    rule => rule.FORM_BUILDER, rule => rule.FORM_RULE_ACTIONS
                );

            return activeRules
                .OrderBy(r => r.ExecutionOrder ?? 1)
                .Select(rule => 
                {
                    // Load actions from separate table
                    var actions = rule.FORM_RULE_ACTIONS?
                        .Where(a => !a.IsElseAction && a.IsActive)
                        .OrderBy(a => a.ActionOrder)
                        .Select(a => new ActionDataDto
                        {
                            Type = a.ActionType,
                            FieldCode = a.FieldCode,
                            Value = a.Value,
                            Expression = a.Expression
                        })
                        .ToList() ?? new List<ActionDataDto>();

                    var elseActions = rule.FORM_RULE_ACTIONS?
                        .Where(a => a.IsElseAction && a.IsActive)
                        .OrderBy(a => a.ActionOrder)
                        .Select(a => new ActionDataDto
                        {
                            Type = a.ActionType,
                            FieldCode = a.FieldCode,
                            Value = a.Value,
                            Expression = a.Expression
                        })
                        .ToList() ?? new List<ActionDataDto>();

                return new FormRuleDto
                {
                    Id = rule.Id,
                    FormBuilderId = rule.FormBuilderId,
                    RuleName = rule.RuleName,
                    RuleType = rule.RuleType ?? "Condition",
                    ConditionField = rule.ConditionField,
                    ConditionOperator = rule.ConditionOperator,
                    ConditionValue = rule.ConditionValue,
                    ConditionValueType = rule.ConditionValueType,
                    StoredProcedureId = rule.StoredProcedureId,
                    StoredProcedureName = rule.StoredProcedureName,
                    StoredProcedureDatabase = rule.StoredProcedureDatabase,
                    ParameterMapping = rule.ParameterMapping,
                    ResultMapping = rule.ResultMapping,
                    Actions = actions.Any() ? actions : null, // Return as List, not JSON
                    ElseActions = elseActions.Any() ? elseActions : null, // Return as List, not JSON
                    IsActive = rule.IsActive,
                    IsDeleted = rule.IsDeleted,
                    DeletedDate = rule.DeletedDate,
                    ExecutionOrder = rule.ExecutionOrder ?? 1,
                    FormName = rule.FORM_BUILDER?.FormName ?? string.Empty,
                    FormCode = rule.FORM_BUILDER?.FormCode ?? string.Empty
                };
                })
                .ToList();
        }

        // Private helper method - Validate ActionsJson structure
        private bool IsValidActionsJson(string actionsJson)
        {
            if (string.IsNullOrWhiteSpace(actionsJson))
                return false;

            try
            {
                var actions = JsonSerializer.Deserialize<List<ActionDataDto>>(
                    actionsJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (actions == null || !actions.Any())
                    return false;

                var validActionTypes = new[] { "SetVisible", "SetReadOnly", "SetMandatory", "SetDefault", "ClearValue", "Compute" };
                foreach (var action in actions)
                {
                    if (string.IsNullOrEmpty(action.Type) || string.IsNullOrEmpty(action.FieldCode))
                        return false;

                    if (!validActionTypes.Contains(action.Type))
                        return false;

                    // For Compute action, Expression is required
                    if (action.Type == "Compute" && string.IsNullOrEmpty(action.Expression))
                        return false;
                }

                return true;
            }
            catch (JsonException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Private helper method - Enhanced validation for RuleJson structure (for backward compatibility)
        private bool IsValidJson(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
                return false;

            try
            {
                // First, check if it's valid JSON
                var ruleData = JsonSerializer.Deserialize<FormRuleDataDto>(
                    jsonString,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (ruleData == null)
                    return false;

                // Validate Condition exists and has required fields
                if (ruleData.Condition == null || string.IsNullOrEmpty(ruleData.Condition.Field))
                    return false;

                // Validate Operator is valid
                var validOperators = new[] { "==", "!=", ">", "<", ">=", "<=", "contains", "isEmpty", "isNotEmpty" };
                if (!validOperators.Contains(ruleData.Condition.Operator))
                    return false;

                // Validate ValueType
                if (ruleData.Condition.ValueType != "constant" && ruleData.Condition.ValueType != "field")
                    return false;

                // Validate Actions exist and are valid
                if (ruleData.Actions == null || !ruleData.Actions.Any())
                    return false;

                var validActionTypes = new[] { "SetVisible", "SetReadOnly", "SetMandatory", "SetDefault", "ClearValue", "Compute" };
                foreach (var action in ruleData.Actions)
                {
                    if (string.IsNullOrEmpty(action.Type) || string.IsNullOrEmpty(action.FieldCode))
                        return false;

                    if (!validActionTypes.Contains(action.Type))
                        return false;

                    // For Compute action, Expression is required
                    if (action.Type == "Compute" && string.IsNullOrEmpty(action.Expression))
                        return false;
                }

                // Validate ElseActions if present (optional)
                if (ruleData.ElseActions != null)
                {
                    foreach (var action in ruleData.ElseActions)
                    {
                        if (string.IsNullOrEmpty(action.Type) || string.IsNullOrEmpty(action.FieldCode))
                            return false;

                        if (!validActionTypes.Contains(action.Type))
                            return false;

                        if (action.Type == "Compute" && string.IsNullOrEmpty(action.Expression))
                            return false;
                    }
                }

                return true;
            }
            catch (JsonException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Extract procedure name from ProcedureCode
        /// </summary>
        private string? ExtractProcedureNameFromCode(string procedureCode)
        {
            if (string.IsNullOrWhiteSpace(procedureCode))
                return null;

            // Try to extract procedure name from CREATE PROCEDURE statement
            var patterns = new[]
            {
                @"CREATE\s+PROCEDURE\s+(?:\[?(\w+)\]?\.)?\[?(\w+)\]?",
                @"CREATE\s+PROC\s+(?:\[?(\w+)\]?\.)?\[?(\w+)\]?",
                @"PROCEDURE\s+(?:\[?(\w+)\]?\.)?\[?(\w+)\]?"
            };

            foreach (var pattern in patterns)
            {
                var match = System.Text.RegularExpressions.Regex.Match(
                    procedureCode,
                    pattern,
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    // Return the procedure name (group 2 if schema exists, group 1 otherwise)
                    if (match.Groups.Count > 2 && !string.IsNullOrWhiteSpace(match.Groups[2].Value))
                    {
                        return match.Groups[2].Value;
                    }
                    else if (match.Groups.Count > 1 && !string.IsNullOrWhiteSpace(match.Groups[1].Value))
                    {
                        return match.Groups[1].Value;
                    }
                }
            }

            return null;
        }
    }
}