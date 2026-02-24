using FormBuilder.API.Attributes;
using FormBuilder.Core.DTOS.FormRules;
using FormBuilder.Core.IServices.FormBuilder;
using FormBuilder.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FormBuilder.Domian.Entitys.FormBuilder;
using formBuilder.Domian.Interfaces;

namespace FormBuilder.ApiProject.Controllers.FormBuilder
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FormRulesController : ControllerBase
    {
        private readonly IFORM_RULESService _formRulesService;
        private readonly IFormRuleEvaluationService _ruleEvaluationService;
        private readonly ILogger<FormRulesController>? _logger;
        private readonly IunitOfwork? _unitOfWork;

        public FormRulesController(
            IFORM_RULESService formRulesService,
            IFormRuleEvaluationService ruleEvaluationService,
            ILogger<FormRulesController>? logger = null,
            IunitOfwork? unitOfWork = null)
        {
            _formRulesService = formRulesService ?? throw new ArgumentNullException(nameof(formRulesService));
            _ruleEvaluationService = ruleEvaluationService ?? throw new ArgumentNullException(nameof(ruleEvaluationService));
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        // ----------------------------------------------------------------------
        // --- 1. GET Operations (Read) ---
        // ----------------------------------------------------------------------

        [HttpGet]
        [RequirePermission("FormRule_Allow_View")]
        [ProducesResponseType(typeof(IEnumerable<FormRuleDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllRules(CancellationToken cancellationToken = default)
        {
            var rules = await _formRulesService.GetAllRulesAsync();
            return Ok(rules);
        }

        [HttpGet("{id}")]
        [RequirePermission("FormRule_Allow_View")]
        [ProducesResponseType(typeof(FormRuleDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRuleById(int id, CancellationToken cancellationToken = default)
        {
            var rule = await _formRulesService.GetRuleByIdAsync(id);
            if (rule == null)
            {
                return NotFound($"Rule with ID {id} not found.");
            }

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

            var ruleDto = new FormRuleDto
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
                // Blocking Rules fields
                EvaluationPhase = rule.EvaluationPhase,
                ConditionSource = rule.ConditionSource,
                ConditionKey = rule.ConditionKey,
                BlockMessage = rule.BlockMessage,
                Priority = rule.Priority
            };

            return Ok(ruleDto);
        }

        // ----------------------------------------------------------------------
        // --- 2. POST Operation (Create) ---
        // ----------------------------------------------------------------------
        [HttpPost]
        [RequirePermission("FormRule_Allow_Create")]
        [ProducesResponseType(typeof(FormRuleDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateRule([FromBody] CreateFormRuleDto createDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdRule = await _formRulesService.CreateRuleAsync(createDto);

            // Reload rule with actions to get the complete data
            var ruleWithActions = await _formRulesService.GetRuleByIdAsync(createdRule.Id);

            // Load actions from separate table
            var actions = ruleWithActions?.FORM_RULE_ACTIONS?
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

            var elseActions = ruleWithActions?.FORM_RULE_ACTIONS?
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

            var createdRuleDto = new FormRuleDto
            {
                Id = createdRule.Id,
                FormBuilderId = createdRule.FormBuilderId,
                RuleName = createdRule.RuleName,
                RuleType = createdRule.RuleType,
                ConditionField = createdRule.ConditionField,
                ConditionOperator = createdRule.ConditionOperator,
                ConditionValue = createdRule.ConditionValue,
                ConditionValueType = createdRule.ConditionValueType,
                StoredProcedureId = createdRule.StoredProcedureId,
                StoredProcedureName = createdRule.StoredProcedureName,
                StoredProcedureDatabase = createdRule.StoredProcedureDatabase,
                ParameterMapping = createdRule.ParameterMapping,
                ResultMapping = createdRule.ResultMapping,
                Actions = actions.Any() ? actions : null, // Return as List, not JSON
                ElseActions = elseActions.Any() ? elseActions : null, // Return as List, not JSON
                IsActive = createdRule.IsActive,
                IsDeleted = createdRule.IsDeleted,
                DeletedDate = createdRule.DeletedDate,
                ExecutionOrder = createdRule.ExecutionOrder ?? 1,
                // Blocking Rules fields
                EvaluationPhase = createdRule.EvaluationPhase,
                ConditionSource = createdRule.ConditionSource,
                ConditionKey = createdRule.ConditionKey,
                BlockMessage = createdRule.BlockMessage,
                Priority = createdRule.Priority
            };

                return CreatedAtAction(nameof(GetRuleById), new { id = createdRuleDto.Id }, createdRuleDto);
            }
            catch (InvalidOperationException ex)
            {
                // Handle duplicate validation and other business logic exceptions
                _logger?.LogWarning("Invalid operation in CreateRule: {Message}", ex.Message);
                return BadRequest(new Microsoft.AspNetCore.Mvc.ProblemDetails
                {
                    Status = 400,
                    Title = "Invalid Operation",
                    Detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error creating rule");
                return StatusCode(500, new Microsoft.AspNetCore.Mvc.ProblemDetails
                {
                    Status = 500,
                    Title = "Internal Server Error",
                    Detail = "An error occurred while creating the rule."
                });
            }
        }

        // ----------------------------------------------------------------------
        // --- 3. PUT Operation (Update) ---
        // ----------------------------------------------------------------------
        [HttpPut("{id}")]
        [RequirePermission("FormRule_Allow_Edit")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateRule(int id, [FromBody] UpdateFormRuleDto updateDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var ruleExists = await _formRulesService.RuleExistsAsync(id);
                if (!ruleExists)
                {
                    return NotFound($"Rule with ID {id} not found.");
                }

                var isUpdated = await _formRulesService.UpdateRuleAsync(updateDto, id);

                if (!isUpdated)
                {
                    return BadRequest("Failed to update the rule.");
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                // Handle duplicate validation and other business logic exceptions
                _logger?.LogWarning("Invalid operation in UpdateRule: {Message}", ex.Message);
                return BadRequest(new Microsoft.AspNetCore.Mvc.ProblemDetails
                {
                    Status = 400,
                    Title = "Invalid Operation",
                    Detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error updating rule");
                return StatusCode(500, new Microsoft.AspNetCore.Mvc.ProblemDetails
                {
                    Status = 500,
                    Title = "Internal Server Error",
                    Detail = "An error occurred while updating the rule."
                });
            }
        }

        // ----------------------------------------------------------------------
        // --- 4. DELETE Operation (Soft Delete) ---
        // ----------------------------------------------------------------------
        [HttpDelete("{id}")]
        [RequirePermission("FormRule_Allow_Delete")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteRule(int id, CancellationToken cancellationToken = default)
        {
            var isDeleted = await _formRulesService.DeleteRuleAsync(id);

            if (!isDeleted)
            {
                return NotFound($"Rule with ID {id} not found.");
            }

            return NoContent();
        }

        // ----------------------------------------------------------------------
        // --- 4.1. RESTORE Operation (Restore Soft-Deleted Rule) ---
        // ----------------------------------------------------------------------
        [HttpPost("{id}/restore")]
        [RequirePermission("FormRule_Allow_Manage")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RestoreRule(int id, CancellationToken cancellationToken = default)
        {
            var isRestored = await _formRulesService.RestoreRuleAsync(id);

            if (!isRestored)
            {
                return NotFound($"Deleted rule with ID {id} not found.");
            }

            return NoContent();
        }

        // ----------------------------------------------------------------------
        // --- 5. Validation & Utility Operations ---
        // ----------------------------------------------------------------------

        [HttpGet("check-name/{ruleName}/form/{formBuilderId}")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CheckRuleNameUnique(int formBuilderId, string ruleName, [FromQuery] int? ignoreId = null, CancellationToken cancellationToken = default)
        {
            var isUnique = await _formRulesService.IsRuleNameUniqueAsync(formBuilderId, ruleName, ignoreId);
            return Ok(new
            {
                formBuilderId,
                ruleName,
                isUnique,
                message = isUnique ? "Rule name is available" : "Rule name is already in use"
            });
        }

        [HttpGet("{id}/exists")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RuleExists(int id, CancellationToken cancellationToken = default)
        {
            var exists = await _formRulesService.RuleExistsAsync(id);
            return Ok(new
            {
                id,
                exists,
                message = exists ? "Rule exists" : "Rule does not exist"
            });
        }

        // ----------------------------------------------------------------------
        // --- 6. Bulk Operations ---
        // ----------------------------------------------------------------------

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(object), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateRulesBulk([FromBody] List<CreateFormRuleDto> createDtos, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var results = new List<object>();
            var createdRules = new List<FormRuleDto>();

            foreach (var createDto in createDtos)
            {
                try
                {
                    var createdRule = await _formRulesService.CreateRuleAsync(createDto);
                    // Reload rule to get complete data with actions
                    var fullRule = await _formRulesService.GetRuleByIdAsync(createdRule.Id);
                    var ruleDto = new FormRuleDto
                    {
                        Id = fullRule.Id,
                        FormBuilderId = fullRule.FormBuilderId,
                        RuleName = fullRule.RuleName,
                        RuleType = fullRule.RuleType,
                        ConditionField = fullRule.ConditionField,
                        ConditionOperator = fullRule.ConditionOperator,
                        ConditionValue = fullRule.ConditionValue,
                        ConditionValueType = fullRule.ConditionValueType,
                        StoredProcedureId = fullRule.StoredProcedureId,
                        StoredProcedureName = fullRule.StoredProcedureName,
                        StoredProcedureDatabase = fullRule.StoredProcedureDatabase,
                        ParameterMapping = fullRule.ParameterMapping,
                        ResultMapping = fullRule.ResultMapping,
                        Actions = fullRule.FORM_RULE_ACTIONS?
                            .Where(a => !a.IsElseAction && a.IsActive)
                            .OrderBy(a => a.ActionOrder)
                            .Select(a => new ActionDataDto
                            {
                                Type = a.ActionType,
                                FieldCode = a.FieldCode,
                                Value = a.Value,
                                Expression = a.Expression
                            })
                            .ToList(),
                        ElseActions = fullRule.FORM_RULE_ACTIONS?
                            .Where(a => a.IsElseAction && a.IsActive)
                            .OrderBy(a => a.ActionOrder)
                            .Select(a => new ActionDataDto
                            {
                                Type = a.ActionType,
                                FieldCode = a.FieldCode,
                                Value = a.Value,
                                Expression = a.Expression
                            })
                            .ToList(),
                        IsActive = fullRule.IsActive,
                        IsDeleted = fullRule.IsDeleted,
                        DeletedDate = fullRule.DeletedDate,
                        ExecutionOrder = fullRule.ExecutionOrder ?? 1,
                        FormName = fullRule.FORM_BUILDER?.FormName ?? string.Empty,
                        FormCode = fullRule.FORM_BUILDER?.FormCode ?? string.Empty,
                        // Blocking Rules fields
                        EvaluationPhase = fullRule.EvaluationPhase,
                        ConditionSource = fullRule.ConditionSource,
                        ConditionKey = fullRule.ConditionKey,
                        BlockMessage = fullRule.BlockMessage,
                        Priority = fullRule.Priority
                    };
                    createdRules.Add(ruleDto);

                    results.Add(new
                    {
                        success = true,
                        ruleName = createDto.RuleName,
                        message = "Created successfully"
                    });
                }
                catch (InvalidOperationException ex)
                {
                    results.Add(new
                    {
                        success = false,
                        ruleName = createDto.RuleName,
                        message = ex.Message
                    });
                }
            }

            return Ok(new
            {
                total = createDtos.Count,
                successful = createdRules.Count,
                failed = createDtos.Count - createdRules.Count,
                results,
                createdRules
            });
        }

        // ----------------------------------------------------------------------
        // --- 7. Additional Utility Endpoints ---
        // ----------------------------------------------------------------------

        [HttpGet("form/{formBuilderId}")]
        [ProducesResponseType(typeof(IEnumerable<FormRuleDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRulesByFormId(int formBuilderId, CancellationToken cancellationToken = default)
        {
            var allRules = await _formRulesService.GetAllRulesAsync();
            var formRules = allRules.Where(r => r.FormBuilderId == formBuilderId).ToList();

            return Ok(formRules);
        }

        [HttpGet("form/{formBuilderId}/active")]
        [AllowAnonymous] // Allow anonymous access for public form viewing
        [ProducesResponseType(typeof(IEnumerable<FormRuleDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetActiveRulesByFormId(int formBuilderId, CancellationToken cancellationToken = default)
        {
            var activeRules = await _formRulesService.GetActiveRulesByFormIdAsync(formBuilderId);
            return Ok(activeRules);
        }

        [HttpGet("stats")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRulesStats(CancellationToken cancellationToken = default)
        {
            var allRules = await _formRulesService.GetAllRulesAsync();
            var totalRules = allRules.Count();
            var activeRules = allRules.Count(r => r.IsActive);
            var inactiveRules = totalRules - activeRules;

            return Ok(new
            {
                totalRules,
                activeRules,
                inactiveRules,
                rulesByForm = allRules.GroupBy(r => r.FormBuilderId)
                    .Select(g => new
                    {
                        formBuilderId = g.Key,
                        count = g.Count(),
                        activeCount = g.Count(r => r.IsActive)
                    })
            });
        }

        // ----------------------------------------------------------------------
        // --- 8. Validation Endpoint ---
        // ----------------------------------------------------------------------

        /// <summary>
        /// Validates form rules against field values (used when submitting form)
        /// POST /api/FormRules/validate
        /// </summary>
        [HttpPost("validate")]
        [AllowAnonymous] // Allow anonymous for public form submissions
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ValidateFormRules([FromBody] ValidateFormRulesRequestDto request, CancellationToken cancellationToken = default)
        {
            if (request == null || request.FormBuilderId <= 0)
            {
                return BadRequest(new { message = "Invalid request. FormBuilderId is required." });
            }

            try
            {
                // Get active rules for the form
                var allRules = await _formRulesService.GetAllRulesAsync();
                var activeRules = allRules
                    .Where(r => r.FormBuilderId == request.FormBuilderId && r.IsActive)
                    .OrderBy(r => r.ExecutionOrder ?? 1)
                    .ToList();

                // If no active rules exist, return success immediately (no validation needed)
                if (activeRules == null || !activeRules.Any())
                {
                    return Ok(new
                    {
                        isValid = true,
                        message = "No active rules found for this form. Validation passed.",
                        errors = new List<string>()
                    });
                }

                // Convert JsonElement dictionary to object dictionary for service compatibility
                // Only do this conversion if we have rules to evaluate
                var fieldValuesAsObjects = new Dictionary<string, object>();
                if (request.FieldValues != null)
                {
                    foreach (var kvp in request.FieldValues)
                    {
                        // Convert JsonElement to object (supports arrays, strings, numbers, booleans, null)
                        object? value = null;
                        if (kvp.Value.ValueKind == System.Text.Json.JsonValueKind.Array)
                        {
                            // For arrays, convert to List<object> or keep as JsonElement
                            value = kvp.Value.Deserialize<object>();
                        }
                        else if (kvp.Value.ValueKind == System.Text.Json.JsonValueKind.String)
                        {
                            value = kvp.Value.GetString();
                        }
                        else if (kvp.Value.ValueKind == System.Text.Json.JsonValueKind.Number)
                        {
                            // Try to get as decimal first, then double
                            if (kvp.Value.TryGetDecimal(out var decimalValue))
                                value = decimalValue;
                            else if (kvp.Value.TryGetInt64(out var intValue))
                                value = intValue;
                            else
                                value = kvp.Value.GetDouble();
                        }
                        else if (kvp.Value.ValueKind == System.Text.Json.JsonValueKind.True || kvp.Value.ValueKind == System.Text.Json.JsonValueKind.False)
                        {
                            value = kvp.Value.GetBoolean();
                        }
                        else if (kvp.Value.ValueKind == System.Text.Json.JsonValueKind.Null)
                        {
                            value = null;
                        }
                        else
                        {
                            // For complex objects, deserialize as object
                            value = kvp.Value.Deserialize<object>();
                        }
                        
                        fieldValuesAsObjects[kvp.Key] = value ?? string.Empty;
                    }
                }

                var validationErrors = new List<string>();

                foreach (var rule in activeRules)
                {
                    try
                    {
                        bool conditionMet = false;

                        // Handle StoredProcedure rules
                        if (rule.RuleType != null && rule.RuleType.Equals("StoredProcedure", StringComparison.OrdinalIgnoreCase))
                        {
                            // Evaluate stored procedure rule
                            if (rule.StoredProcedureId.HasValue && rule.StoredProcedureId.Value > 0)
                            {
                                // Use stored procedure ID (preferred method)
                                conditionMet = await _ruleEvaluationService.EvaluateStoredProcedureByIdAsync(
                                    rule.StoredProcedureId.Value,
                                    rule.ParameterMapping,
                                    rule.ResultMapping,
                                    fieldValuesAsObjects);
                            }
                            else if (!string.IsNullOrWhiteSpace(rule.StoredProcedureName) && 
                                     !string.IsNullOrWhiteSpace(rule.StoredProcedureDatabase))
                            {
                                // Use stored procedure name and database (backward compatibility)
                                conditionMet = await _ruleEvaluationService.EvaluateStoredProcedureAsync(
                                    rule.StoredProcedureName,
                                    rule.StoredProcedureDatabase,
                                    rule.ParameterMapping,
                                    rule.ResultMapping,
                                    fieldValuesAsObjects);
                            }
                            else
                            {
                                _logger?.LogWarning("StoredProcedure rule {RuleId} missing required stored procedure information", rule.Id);
                                continue;
                            }

                            // Validate actions based on stored procedure result
                            if (conditionMet)
                            {
                                if (rule.Actions != null && rule.Actions.Any())
                                {
                                    var actionErrors = _ruleEvaluationService.ValidateActions(
                                        rule.Actions,
                                        fieldValuesAsObjects,
                                        rule.RuleName);
                                    validationErrors.AddRange(actionErrors);
                                }
                            }
                            else
                            {
                                // Handle else actions if condition is not met
                                if (rule.ElseActions != null && rule.ElseActions.Any())
                                {
                                    var actionErrors = _ruleEvaluationService.ValidateActions(
                                        rule.ElseActions,
                                        fieldValuesAsObjects,
                                        rule.RuleName);
                                    validationErrors.AddRange(actionErrors);
                                }
                            }
                        }
                        else
                        {
                            // Handle Condition rules (existing logic)
                            // Build rule data from fields (new approach) or parse RuleJson (backward compatibility)
                            FormRuleDataDto? ruleData = null;
                            
                            if (!string.IsNullOrWhiteSpace(rule.ConditionField) && !string.IsNullOrWhiteSpace(rule.ConditionOperator))
                            {
                                // Use new approach with separate fields
                                // Convert Actions and ElseActions Lists to JSON strings
                                string? actionsJson = null;
                                string? elseActionsJson = null;

                                if (rule.Actions != null && rule.Actions.Any())
                                {
                                    actionsJson = System.Text.Json.JsonSerializer.Serialize(rule.Actions);
                                }

                                if (rule.ElseActions != null && rule.ElseActions.Any())
                                {
                                    elseActionsJson = System.Text.Json.JsonSerializer.Serialize(rule.ElseActions);
                                }

                                ruleData = _ruleEvaluationService.BuildRuleDataFromFields(
                                    rule.ConditionField,
                                    rule.ConditionOperator,
                                    rule.ConditionValue,
                                    rule.ConditionValueType,
                                    actionsJson,
                                    elseActionsJson);
                            }

                            if (ruleData == null || ruleData.Condition == null)
                            {
                                _logger?.LogWarning("Invalid rule structure for rule {RuleId}: {RuleName}", rule.Id, rule.RuleName);
                                continue;
                            }

                            // Evaluate condition
                            conditionMet = _ruleEvaluationService.EvaluateCondition(
                                ruleData.Condition,
                                fieldValuesAsObjects);

                            if (conditionMet)
                            {
                                // Validate actions (check if mandatory fields are filled)
                                if (ruleData.Actions != null && ruleData.Actions.Any())
                                {
                                    var actionErrors = _ruleEvaluationService.ValidateActions(
                                        ruleData.Actions,
                                        fieldValuesAsObjects,
                                        rule.RuleName);
                                    validationErrors.AddRange(actionErrors);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "Error evaluating rule {RuleId}: {RuleName}", rule.Id, rule.RuleName);
                        // Continue with other rules - don't fail entire validation
                    }
                }

                if (validationErrors.Any())
                {
                    return BadRequest(new
                    {
                        valid = false,
                        errors = validationErrors,
                        message = "Form validation failed based on rules"
                    });
                }

                return Ok(new
                {
                    valid = true,
                    message = "Form validation passed"
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error validating form rules");
                return StatusCode(500, new { message = "Internal server error during validation", error = ex.Message });
            }
        }

        // ----------------------------------------------------------------------
        // --- 9. Evaluate Single Rule Endpoint ---
        // ----------------------------------------------------------------------

        /// <summary>
        /// Evaluates a single rule with provided field values (for testing/debugging)
        /// POST /api/FormRules/evaluate
        /// </summary>
        [HttpPost("evaluate")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> EvaluateRule([FromBody] EvaluateRuleRequestDto request, CancellationToken cancellationToken = default)
        {
            if (request == null || request.RuleId <= 0)
            {
                return BadRequest(new { message = "Invalid request. RuleId is required." });
            }

            if (request.FieldValues == null)
            {
                request.FieldValues = new Dictionary<string, object>();
            }

            try
            {
                // Get the rule
                var rule = await _formRulesService.GetRuleByIdAsync(request.RuleId);
                if (rule == null)
                {
                    return NotFound(new { message = $"Rule with ID {request.RuleId} not found." });
                }

                // Build rule data from fields (new approach) or parse RuleJson (backward compatibility)
                FormRuleDataDto? ruleData = null;
                
                if (!string.IsNullOrWhiteSpace(rule.ConditionField) && !string.IsNullOrWhiteSpace(rule.ConditionOperator))
                {
                    // Build ActionsJson and ElseActionsJson from FORM_RULE_ACTIONS
                    string? actionsJson = null;
                    string? elseActionsJson = null;

                    if (rule.FORM_RULE_ACTIONS != null && rule.FORM_RULE_ACTIONS.Any())
                    {
                        var actions = rule.FORM_RULE_ACTIONS
                            .Where(a => !a.IsElseAction && a.IsActive)
                            .OrderBy(a => a.ActionOrder)
                            .Select(a => new
                            {
                                Type = a.ActionType,
                                FieldCode = a.FieldCode,
                                Value = a.Value,
                                Expression = a.Expression
                            })
                            .ToList();

                        var elseActionsList = rule.FORM_RULE_ACTIONS
                            .Where(a => a.IsElseAction && a.IsActive)
                            .OrderBy(a => a.ActionOrder)
                            .Select(a => new
                            {
                                Type = a.ActionType,
                                FieldCode = a.FieldCode,
                                Value = a.Value,
                                Expression = a.Expression
                            })
                            .ToList();

                        if (actions.Any())
                            actionsJson = System.Text.Json.JsonSerializer.Serialize(actions);
                        
                        if (elseActionsList.Any())
                            elseActionsJson = System.Text.Json.JsonSerializer.Serialize(elseActionsList);
                    }

                    // Use new approach with separate fields
                    ruleData = _ruleEvaluationService.BuildRuleDataFromFields(
                        rule.ConditionField,
                        rule.ConditionOperator,
                        rule.ConditionValue,
                        rule.ConditionValueType,
                        actionsJson,
                        elseActionsJson);
                }
                else if (!string.IsNullOrWhiteSpace(rule.RuleJson))
                {
                    // Fallback to old RuleJson approach for backward compatibility
                    ruleData = _ruleEvaluationService.ParseRuleJson(rule.RuleJson);
                }

                if (ruleData == null || ruleData.Condition == null)
                {
                    return BadRequest(new { message = "Invalid rule structure. Condition fields are required." });
                }

                // Evaluate condition
                bool conditionMet = _ruleEvaluationService.EvaluateCondition(
                    ruleData.Condition,
                    request.FieldValues);

                // Get actions that would be applied
                var appliedActions = new List<object>();
                var elseActions = new List<object>();

                if (conditionMet)
                {
                    if (ruleData.Actions != null && ruleData.Actions.Any())
                    {
                        foreach (var action in ruleData.Actions)
                        {
                            appliedActions.Add(new
                            {
                                type = action.Type,
                                fieldCode = action.FieldCode,
                                value = action.Value,
                                expression = action.Expression
                            });
                        }
                    }
                }
                else
                {
                    if (ruleData.ElseActions != null && ruleData.ElseActions.Any())
                    {
                        foreach (var action in ruleData.ElseActions)
                        {
                            elseActions.Add(new
                            {
                                type = action.Type,
                                fieldCode = action.FieldCode,
                                value = action.Value,
                                expression = action.Expression
                            });
                        }
                    }
                }

                // Simulate field state after applying actions
                var simulatedFieldStates = new Dictionary<string, object>(request.FieldValues);
                
                if (conditionMet && ruleData.Actions != null)
                {
                    foreach (var action in ruleData.Actions)
                    {
                        switch (action.Type)
                        {
                            case "SetDefault":
                                if (!simulatedFieldStates.ContainsKey(action.FieldCode))
                                {
                                    simulatedFieldStates[action.FieldCode] = action.Value ?? "";
                                }
                                break;
                            case "ClearValue":
                                if (simulatedFieldStates.ContainsKey(action.FieldCode))
                                {
                                    simulatedFieldStates[action.FieldCode] = "";
                                }
                                break;
                            case "Compute":
                                if (!string.IsNullOrEmpty(action.Expression))
                                {
                                    try
                                    {
                                        var computedValue = _ruleEvaluationService.EvaluateExpression(
                                            action.Expression,
                                            request.FieldValues);
                                        simulatedFieldStates[action.FieldCode] = computedValue;
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger?.LogWarning(ex, "Error computing expression for field {FieldCode}", action.FieldCode);
                                    }
                                }
                                break;
                        }
                    }
                }

                return Ok(new
                {
                    ruleId = rule.Id,
                    ruleName = rule.RuleName,
                    conditionMet,
                    condition = new
                    {
                        field = ruleData.Condition.Field,
                        @operator = ruleData.Condition.Operator,
                        value = ruleData.Condition.Value,
                        valueType = ruleData.Condition.ValueType
                    },
                    appliedActions,
                    elseActions,
                    simulatedFieldStates,
                    message = conditionMet ? "Condition is met - THEN actions would be applied" : "Condition is not met - ELSE actions would be applied (if any)"
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error evaluating rule {RuleId}", request.RuleId);
                return StatusCode(500, new { message = "Internal server error during rule evaluation", error = ex.Message });
            }
        }

        // ----------------------------------------------------------------------
        // --- 10. Get Form Fields After Applying Rules ---
        // ----------------------------------------------------------------------

        /// <summary>
        /// Get form fields with rules applied (for frontend to display fields based on rules)
        /// POST /api/FormRules/apply-rules
        /// </summary>
        [HttpPost("apply-rules")]
        [AllowAnonymous] // Allow anonymous for public form viewing
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ApplyRulesToForm([FromBody] ApplyRulesRequestDto request, CancellationToken cancellationToken = default)
        {
            if (request == null || request.FormBuilderId <= 0)
            {
                return BadRequest(new { message = "Invalid request. FormBuilderId is required." });
            }

            try
            {
                // Get form with fields (you'll need to inject IFormBuilderService or IFormFieldService)
                // For now, we'll return the field states after applying rules
                var fieldStates = new Dictionary<string, FieldStateDto>();

                // Get active rules for the form
                var allRules = await _formRulesService.GetAllRulesAsync();
                var activeRules = allRules
                    .Where(r => r.FormBuilderId == request.FormBuilderId && r.IsActive)
                    .OrderBy(r => r.ExecutionOrder ?? 1)
                    .ToList();

                // Initialize field states from request (if provided) or empty
                if (request.FieldValues != null)
                {
                    foreach (var field in request.FieldValues)
                    {
                        fieldStates[field.Key] = new FieldStateDto
                        {
                            FieldCode = field.Key,
                            Value = field.Value,
                            IsVisible = true, // Default visible
                            IsReadOnly = false, // Default editable
                            IsMandatory = false // Default not mandatory
                        };
                    }
                }

                // Apply rules to field states
                foreach (var rule in activeRules)
                {
                    try
                    {
                        bool conditionMet = false;

                        // Handle StoredProcedure rules
                        if (rule.RuleType != null && rule.RuleType.Equals("StoredProcedure", StringComparison.OrdinalIgnoreCase))
                        {
                            // Convert field values to dictionary
                            var fieldValuesDict = request.FieldValues?.ToDictionary(
                                kvp => kvp.Key,
                                kvp => kvp.Value) ?? new Dictionary<string, object>();

                            // Evaluate stored procedure rule
                            if (rule.StoredProcedureId.HasValue && rule.StoredProcedureId.Value > 0)
                            {
                                conditionMet = await _ruleEvaluationService.EvaluateStoredProcedureByIdAsync(
                                    rule.StoredProcedureId.Value,
                                    rule.ParameterMapping,
                                    rule.ResultMapping,
                                    fieldValuesDict);
                            }
                            else if (!string.IsNullOrWhiteSpace(rule.StoredProcedureName) && 
                                     !string.IsNullOrWhiteSpace(rule.StoredProcedureDatabase))
                            {
                                conditionMet = await _ruleEvaluationService.EvaluateStoredProcedureAsync(
                                    rule.StoredProcedureName,
                                    rule.StoredProcedureDatabase,
                                    rule.ParameterMapping,
                                    rule.ResultMapping,
                                    fieldValuesDict);
                            }
                        }
                        else
                        {
                            // Handle Condition rules
                            if (!string.IsNullOrWhiteSpace(rule.ConditionField) && 
                                !string.IsNullOrWhiteSpace(rule.ConditionOperator))
                            {
                                var fieldValuesDict = request.FieldValues?.ToDictionary(
                                    kvp => kvp.Key,
                                    kvp => kvp.Value) ?? new Dictionary<string, object>();

                                var ruleData = _ruleEvaluationService.BuildRuleDataFromFields(
                                    rule.ConditionField,
                                    rule.ConditionOperator,
                                    rule.ConditionValue,
                                    rule.ConditionValueType,
                                    rule.Actions != null ? System.Text.Json.JsonSerializer.Serialize(rule.Actions) : null,
                                    rule.ElseActions != null ? System.Text.Json.JsonSerializer.Serialize(rule.ElseActions) : null);

                                if (ruleData?.Condition != null)
                                {
                                    conditionMet = _ruleEvaluationService.EvaluateCondition(
                                        ruleData.Condition,
                                        fieldValuesDict);
                                }
                            }
                        }

                        // Apply actions based on condition result
                        var actionsToApply = conditionMet ? rule.Actions : rule.ElseActions;

                        if (actionsToApply != null && actionsToApply.Any())
                        {
                            foreach (var action in actionsToApply)
                            {
                                if (!fieldStates.ContainsKey(action.FieldCode))
                                {
                                    fieldStates[action.FieldCode] = new FieldStateDto
                                    {
                                        FieldCode = action.FieldCode,
                                        IsVisible = true,
                                        IsReadOnly = false,
                                        IsMandatory = false
                                    };
                                }

                                var fieldState = fieldStates[action.FieldCode];

                                switch (action.Type)
                                {
                                    case "SetVisible":
                                        fieldState.IsVisible = action.Value?.ToString() == "true" || 
                                                               action.Value?.ToString() == "True" ||
                                                               action.Value is bool boolValue && boolValue;
                                        break;
                                    case "SetReadOnly":
                                        fieldState.IsReadOnly = action.Value?.ToString() == "true" || 
                                                                action.Value?.ToString() == "True" ||
                                                                action.Value is bool boolVal && boolVal;
                                        break;
                                    case "SetMandatory":
                                        fieldState.IsMandatory = action.Value?.ToString() == "true" || 
                                                                 action.Value?.ToString() == "True" ||
                                                                 action.Value is bool boolMandatory && boolMandatory;
                                        break;
                                    case "SetDefault":
                                        if (string.IsNullOrEmpty(fieldState.Value?.ToString()))
                                        {
                                            fieldState.Value = action.Value;
                                        }
                                        break;
                                    case "ClearValue":
                                        fieldState.Value = null;
                                        break;
                                    case "Compute":
                                        if (!string.IsNullOrEmpty(action.Expression))
                                        {
                                            try
                                            {
                                                var fieldValuesDict = request.FieldValues?.ToDictionary(
                                                    kvp => kvp.Key,
                                                    kvp => kvp.Value) ?? new Dictionary<string, object>();
                                                fieldState.Value = _ruleEvaluationService.EvaluateExpression(
                                                    action.Expression,
                                                    fieldValuesDict);
                                            }
                                            catch (Exception ex)
                                            {
                                                _logger?.LogWarning(ex, "Error computing expression for field {FieldCode}", action.FieldCode);
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "Error applying rule {RuleId}: {RuleName}", rule.Id, rule.RuleName);
                        // Continue with other rules
                    }
                }

                return Ok(new
                {
                    formBuilderId = request.FormBuilderId,
                    fieldStates = fieldStates.Values.ToList(),
                    message = "Rules applied successfully"
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error applying rules to form");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // ----------------------------------------------------------------------
        // --- 11. Blocking Rules Evaluation Endpoint ---
        // ----------------------------------------------------------------------

        /// <summary>
        /// Evaluates blocking rules for a form at a specific evaluation phase
        /// POST /api/FormRules/evaluate-blocking
        /// </summary>
        [HttpPost("evaluate-blocking")]
        [AllowAnonymous] // Allow anonymous for testing
        [ProducesResponseType(typeof(BlockingRuleResultDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> EvaluateBlockingRules([FromBody] EvaluateBlockingRulesRequestDto request, CancellationToken cancellationToken = default)
        {
            if (request == null || request.FormBuilderId <= 0)
            {
                return BadRequest(new { message = "Invalid request. FormBuilderId is required." });
            }

            if (string.IsNullOrWhiteSpace(request.EvaluationPhase) ||
                (request.EvaluationPhase != "PreOpen" && request.EvaluationPhase != "PreSubmit"))
            {
                return BadRequest(new { message = "EvaluationPhase must be either 'PreOpen' or 'PreSubmit'." });
            }

            try
            {
                _logger?.LogInformation("Evaluating blocking rules: FormId={FormId}, Phase={Phase}, SubmissionId={SubmissionId}, FieldValues={FieldValues}",
                    request.FormBuilderId, request.EvaluationPhase, request.SubmissionId,
                    request.FieldValues != null ? string.Join(", ", request.FieldValues.Select(kvp => $"{kvp.Key}={kvp.Value}")) : "null");

                // Include debug info in development environment
                var includeDebugInfo = HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment() ||
                                      HttpContext.Request.Query.ContainsKey("debug");

                var result = await _ruleEvaluationService.EvaluateBlockingRulesAsync(
                    request.FormBuilderId,
                    request.EvaluationPhase,
                    request.SubmissionId,
                    request.FieldValues,
                    includeDebugInfo);

                _logger?.LogInformation("Blocking rules evaluation result: IsBlocked={IsBlocked}, Message={Message}, RuleId={RuleId}",
                    result.IsBlocked, result.BlockMessage, result.MatchedRuleId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error evaluating blocking rules for FormId={FormId}, Phase={Phase}",
                    request.FormBuilderId, request.EvaluationPhase);
                return StatusCode(500, new { message = "Internal server error during blocking rules evaluation", error = ex.Message });
            }
        }

        // ----------------------------------------------------------------------
        // --- 11.5. Get Blocking Rules for Form ---
        // ----------------------------------------------------------------------

        /// <summary>
        /// Get blocking rules for a form (for debugging/testing)
        /// GET /api/FormRules/blocking-rules/{formBuilderId}
        /// </summary>
        [HttpGet("blocking-rules/{formBuilderId}")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBlockingRules(int formBuilderId, [FromQuery] string? evaluationPhase = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var allRules = await _formRulesService.GetAllRulesAsync();
                var blockingRules = allRules
                    .Where(r => r.FormBuilderId == formBuilderId && 
                               r.IsActive &&
                               !string.IsNullOrWhiteSpace(r.EvaluationPhase))
                    .ToList();

                if (!string.IsNullOrWhiteSpace(evaluationPhase))
                {
                    blockingRules = blockingRules
                        .Where(r => r.EvaluationPhase.Equals(evaluationPhase, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                var result = blockingRules.Select(r => new
                {
                    r.Id,
                    r.RuleName,
                    r.EvaluationPhase,
                    r.ConditionSource,
                    r.ConditionKey,
                    r.ConditionField,
                    r.ConditionOperator,
                    r.ConditionValue,
                    r.BlockMessage,
                    r.Priority,
                    r.IsActive,
                    r.ExecutionOrder
                }).ToList();

                return Ok(new
                {
                    formBuilderId,
                    evaluationPhase,
                    count = result.Count,
                    rules = result
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error retrieving blocking rules for FormId={FormId}", formBuilderId);
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // ----------------------------------------------------------------------
        // --- 12. Blocking Rules Audit Log Endpoint ---
        // ----------------------------------------------------------------------

        /// <summary>
        /// Get blocking rules audit logs
        /// GET /api/FormRules/blocking-audit-logs
        /// </summary>
        [HttpGet("blocking-audit-logs")]
        [ProducesResponseType(typeof(IEnumerable<object>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBlockingAuditLogs([FromQuery] int? formBuilderId = null,
            [FromQuery] int? submissionId = null,
            [FromQuery] string? evaluationPhase = null,
            [FromQuery] int? ruleId = null,
            [FromQuery] bool? isBlocked = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50, CancellationToken cancellationToken = default)
        {
            try
            {
                if (_unitOfWork == null)
                {
                    return StatusCode(500, new { message = "UnitOfWork is not available" });
                }

                var repository = _unitOfWork.Repositary<BLOCKING_RULE_AUDIT_LOG>();
                var query = repository.GetAll();

                // Apply filters
                if (formBuilderId.HasValue)
                    query = query.Where(log => log.FormBuilderId == formBuilderId.Value);
                
                if (submissionId.HasValue)
                    query = query.Where(log => log.SubmissionId == submissionId.Value);
                
                if (!string.IsNullOrWhiteSpace(evaluationPhase))
                    query = query.Where(log => log.EvaluationPhase == evaluationPhase);
                
                if (ruleId.HasValue)
                    query = query.Where(log => log.RuleId == ruleId.Value);
                
                if (isBlocked.HasValue)
                    query = query.Where(log => log.IsBlocked == isBlocked.Value);

                // Order by created date descending
                query = query.OrderByDescending(log => log.CreatedDate);

                // Pagination
                var totalCount = await query.CountAsync();
                var logs = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var result = logs.Select(log => new
                {
                    log.Id,
                    log.FormBuilderId,
                    log.SubmissionId,
                    log.EvaluationPhase,
                    log.RuleId,
                    log.RuleName,
                    log.IsBlocked,
                    log.BlockMessage,
                    log.UserId,
                    log.ContextJson,
                    log.CreatedDate
                });

                return Ok(new
                {
                    data = result,
                    totalCount,
                    page,
                    pageSize,
                    totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error retrieving blocking rules audit logs");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }

    /// <summary>
    /// Request DTO for applying rules to form
    /// </summary>
    public class ApplyRulesRequestDto
    {
        public int FormBuilderId { get; set; }
        public Dictionary<string, object>? FieldValues { get; set; }
    }

    /// <summary>
    /// Field state after applying rules
    /// </summary>
    public class FieldStateDto
    {
        public string FieldCode { get; set; } = string.Empty;
        public object? Value { get; set; }
        public bool IsVisible { get; set; } = true;
        public bool IsReadOnly { get; set; } = false;
        public bool IsMandatory { get; set; } = false;
    }
}