using formBuilder.Domian.Interfaces;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Domian.Entitys.froms;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Services.Services.Base;
using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.Common;
using FormBuilder.Services.Services.Common;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FormBuilder.API.Models;

namespace FormBuilder.Services
{
    public class FormSubmissionValuesService : BaseService<FORM_SUBMISSION_VALUES, FormSubmissionValueDto, CreateFormSubmissionValueDto, UpdateFormSubmissionValueDto>, IFormSubmissionValuesService
    {
        private readonly IunitOfwork _unitOfWork;
        private readonly ValidationMessageService _validationMessageService;

        public FormSubmissionValuesService(
            IunitOfwork unitOfWork, 
            IMapper mapper,
            ValidationMessageService validationMessageService) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _validationMessageService = validationMessageService ?? throw new ArgumentNullException(nameof(validationMessageService));
        }

        protected override IBaseRepository<FORM_SUBMISSION_VALUES> Repository => _unitOfWork.FormSubmissionValuesRepository;

        public async Task<ApiResponse> GetAllAsync()
        {
            var result = await base.GetAllAsync();
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> GetByIdAsync(int id)
        {
            var result = await base.GetByIdAsync(id);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> GetBySubmissionIdAsync(int submissionId)
        {
            var values = await _unitOfWork.FormSubmissionValuesRepository.GetBySubmissionIdAsync(submissionId);
            var valueDtos = _mapper.Map<IEnumerable<FormSubmissionValueDto>>(values);
            return new ApiResponse(200, "Form submission values retrieved successfully", valueDtos);
        }

        public async Task<ApiResponse> GetByFieldIdAsync(int fieldId)
        {
            var values = await _unitOfWork.FormSubmissionValuesRepository.GetByFieldIdAsync(fieldId);
            var valueDtos = _mapper.Map<IEnumerable<FormSubmissionValueDto>>(values);
            return new ApiResponse(200, "Form submission values by field retrieved successfully", valueDtos);
        }

        public async Task<ApiResponse> GetBySubmissionAndFieldAsync(int submissionId, int fieldId)
        {
            var value = await _unitOfWork.FormSubmissionValuesRepository.GetBySubmissionAndFieldAsync(submissionId, fieldId);
            if (value == null)
                return new ApiResponse(404, "Form submission value not found");

            var valueDto = _mapper.Map<FormSubmissionValueDto>(value);
            return new ApiResponse(200, "Form submission value retrieved successfully", valueDto);
        }

        public async Task<ApiResponse> CreateAsync(CreateFormSubmissionValueDto createDto)
        {
            if (createDto == null)
            {
                return new ApiResponse(400, "Payload is required");
            }

            var validation = await ValidateCreateAsync(createDto);
            if (!validation.IsValid)
            {
                return new ApiResponse(400, validation.ErrorMessage ?? "Validation failed");
            }

            var entity = _mapper.Map<FORM_SUBMISSION_VALUES>(createDto);
            entity.CreatedDate = entity.CreatedDate == default ? DateTime.UtcNow : entity.CreatedDate;
            entity.IsActive = true;
            entity.IsDeleted = false;

            // Ensure ValueString is not null (database constraint)
            if (string.IsNullOrEmpty(entity.ValueString))
            {
                entity.ValueString = string.Empty; // Default empty string
            }

            // Ensure ValueJson is not null (database constraint)
            if (string.IsNullOrEmpty(entity.ValueJson))
            {
                entity.ValueJson = "{}"; // Default empty JSON object
            }

            Repository.Add(entity);
            await _unitOfWork.CompleteAsyn();

            var dto = _mapper.Map<FormSubmissionValueDto>(entity);
            return new ApiResponse(200, "Form submission value created successfully", dto);
        }

        protected override async Task<ValidationResult> ValidateCreateAsync(CreateFormSubmissionValueDto dto)
        {
            // Check if value already exists for this field and submission
            var exists = await _unitOfWork.FormSubmissionValuesRepository
                .ExistsBySubmissionAndFieldAsync(dto.SubmissionId, dto.FieldId);

            if (exists)
                return ValidationResult.Failure("Form submission value already exists for this field");

            return ValidationResult.Success();
        }

        public async Task<ApiResponse> CreateBulkAsync(BulkFormSubmissionValuesDto bulkDto)
        {
            if (bulkDto == null || !bulkDto.Values.Any())
                return new ApiResponse(400, "No values provided");

            // 1) Validate all values against their field types (required, format, etc.)
            var validationErrors = await ValidateBulkValuesAsync(bulkDto);
            if (validationErrors.HasErrors)
            {
                return new ApiResponse(400, "Validation failed", new
                {
                    errors = validationErrors.Errors,
                    errorCount = validationErrors.ErrorCount
                });
            }

            // 2) Create entities (skipping duplicates by SubmissionId + FieldId)
            var entities = new List<FORM_SUBMISSION_VALUES>();

            foreach (var valueDto in bulkDto.Values)
            {
                // Skip if value already exists
                var exists = await _unitOfWork.FormSubmissionValuesRepository
                    .ExistsBySubmissionAndFieldAsync(bulkDto.SubmissionId, valueDto.FieldId);

                if (!exists)
                {
                    var entity = _mapper.Map<FORM_SUBMISSION_VALUES>(valueDto);
                    entity.SubmissionId = bulkDto.SubmissionId; // Override with bulk submission ID
                    
                    // Ensure ValueString is not null (database constraint)
                    if (string.IsNullOrEmpty(entity.ValueString))
                    {
                        entity.ValueString = string.Empty; // Default empty string
                    }
                    
                    // Ensure ValueJson is not null (database constraint)
                    if (string.IsNullOrEmpty(entity.ValueJson))
                    {
                        entity.ValueJson = "{}"; // Default empty JSON object
                    }
                    
                    entities.Add(entity);
                }
            }

            if (entities.Any())
            {
                _unitOfWork.FormSubmissionValuesRepository.AddRange(entities);
                await _unitOfWork.CompleteAsyn();
            }

            var createdDtos = _mapper.Map<IEnumerable<FormSubmissionValueDto>>(entities);
            return new ApiResponse(200, "Form submission values created successfully", createdDtos);
        }

        public async Task<ApiResponse> UpdateAsync(int id, UpdateFormSubmissionValueDto updateDto)
        {
            if (updateDto == null)
            {
                return new ApiResponse(400, "Payload is required");
            }

            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id && !e.IsDeleted, asNoTracking: false);
            if (entity == null)
            {
                return new ApiResponse(404, "Form submission value not found or has been deleted");
            }

            var validation = await ValidateUpdateAsync(id, updateDto, entity);
            if (!validation.IsValid)
            {
                return new ApiResponse(400, validation.ErrorMessage ?? "Validation failed");
            }

            _mapper.Map(updateDto, entity);
            entity.UpdatedDate = DateTime.UtcNow;

            // Ensure ValueString is not null (database constraint)
            if (string.IsNullOrEmpty(entity.ValueString))
            {
                entity.ValueString = string.Empty; // Default empty string
            }

            // Ensure ValueJson is not null (database constraint)
            if (string.IsNullOrEmpty(entity.ValueJson))
            {
                entity.ValueJson = "{}"; // Default empty JSON object
            }

            Repository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            var dto = _mapper.Map<FormSubmissionValueDto>(entity);
            return new ApiResponse(200, "Form submission value updated successfully", dto);
        }

        public async Task<ApiResponse> UpdateByFieldAsync(int submissionId, int fieldId, UpdateFormSubmissionValueDto updateDto)
        {
            if (updateDto == null)
                return new ApiResponse(400, "DTO is required");

            var entity = await _unitOfWork.FormSubmissionValuesRepository
                .GetBySubmissionAndFieldAsync(submissionId, fieldId);

            if (entity == null)
                return new ApiResponse(404, "Form submission value not found");

            _mapper.Map(updateDto, entity);
            entity.UpdatedDate = DateTime.UtcNow;

            // Ensure ValueString is not null (database constraint)
            if (string.IsNullOrEmpty(entity.ValueString))
            {
                entity.ValueString = string.Empty; // Default empty string
            }

            // Ensure ValueJson is not null (database constraint)
            if (string.IsNullOrEmpty(entity.ValueJson))
            {
                entity.ValueJson = "{}"; // Default empty JSON object
            }

            _unitOfWork.FormSubmissionValuesRepository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            var valueDto = _mapper.Map<FormSubmissionValueDto>(entity);
            return new ApiResponse(200, "Form submission value updated successfully", valueDto);
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var result = await base.DeleteAsync(id);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> DeleteBySubmissionIdAsync(int submissionId)
        {
            var deleted = await _unitOfWork.FormSubmissionValuesRepository.DeleteBySubmissionIdAsync(submissionId);
            var message = deleted ? "Form submission values deleted successfully" : "No form submission values found";

            return new ApiResponse(200, message);
        }

        public async Task<ApiResponse> ExistsAsync(int id)
        {
            var exists = await _unitOfWork.FormSubmissionValuesRepository.AnyAsync(v => v.Id == id);
            return new ApiResponse(200, "Form submission value existence checked successfully", exists);
        }

        // ================================
        // VALIDATION HELPERS
        // ================================

        /// <summary>
        /// Validates all submitted values against their field definitions and field types.
        /// Ensures that user data is appropriate for the field type (email, password, number, date, etc.).
        /// </summary>
        private async Task<ValidationErrorCollection> ValidateBulkValuesAsync(BulkFormSubmissionValuesDto bulkDto)
        {
            var errors = new ValidationErrorCollection();

            // Get submission once (outside the loop) to check if it's linked to an ApprovalStage
            var submission = await _unitOfWork.FormSubmissionsRepository.GetByIdAsync(bulkDto.SubmissionId);
            APPROVAL_STAGES stage = null;
            if (submission != null && submission.StageId.HasValue)
            {
                stage = await _unitOfWork.ApprovalStageRepository.GetByIdAsync(submission.StageId.Value);
            }

            foreach (var valueDto in bulkDto.Values)
            {
                var field = await _unitOfWork.FormFieldRepository.GetByIdAsync(valueDto.FieldId);
                if (field == null)
                {
                    errors.AddError(
                        nameof(valueDto.FieldId),
                        ValidationErrorCodes.FieldNotFound,
                        _validationMessageService.GetMessage(ValidationErrorCodes.FieldNotFound),
                        "NotFound"
                    );
                    continue;
                }

                var fieldName = field.FieldName ?? $"FieldId:{field.Id}";
                var fieldCode = field.FieldCode ?? string.Empty;
                var fieldType = field.FIELD_TYPES;

                var typeName = fieldType?.TypeName?.ToLower() ?? string.Empty;
                var dataType = fieldType?.DataType?.ToLower() ?? "string";

                // Determine if any value is provided
                bool hasValue =
                    !string.IsNullOrWhiteSpace(valueDto.ValueString) ||
                    valueDto.ValueNumber.HasValue ||
                    valueDto.ValueDate.HasValue ||
                    valueDto.ValueBool.HasValue ||
                    !string.IsNullOrWhiteSpace(valueDto.ValueJson);

                // 1) Required validation (IsMandatory)
                if (field.IsMandatory == true && !hasValue)
                {
                    errors.AddError(
                        fieldCode,
                        ValidationErrorCodes.Required,
                        _validationMessageService.GetMessage(ValidationErrorCodes.Required, fieldName),
                        "Required"
                    );
                    continue; // No need to validate format if value is missing
                }

                // If no value and not mandatory, skip other checks
                if (!hasValue)
                    continue;

                // 2) Type-specific validation
                // Normalize main string value for format checks
                var stringValue = valueDto.ValueString ?? valueDto.ValueJson ?? string.Empty;
                // Frontend checkbox/radio controls sometimes send values as JSON arrays (e.g. ["1"]).
                // For scalar field types, unwrap a single-value array into a scalar string before parsing.
                stringValue = TryUnwrapSingleJsonArrayScalar(stringValue) ?? stringValue;

                // Numeric
                if (dataType == "int" || dataType == "decimal" || dataType.Contains("number"))
                {
                    if (!valueDto.ValueNumber.HasValue)
                    {
                        if (string.IsNullOrWhiteSpace(stringValue) || !decimal.TryParse(stringValue, out _))
                        {
                            errors.AddError(
                                fieldCode,
                                ValidationErrorCodes.InvalidFormat,
                                _validationMessageService.GetMessage(ValidationErrorCodes.InvalidFormat, fieldName),
                                "Format"
                            );
                        }
                    }
                }
                // Date / DateTime
                else if (dataType == "date" || dataType == "datetime" || dataType.Contains("date"))
                {
                    if (!valueDto.ValueDate.HasValue)
                    {
                        if (string.IsNullOrWhiteSpace(stringValue) || !DateTime.TryParse(stringValue, out _))
                        {
                            errors.AddError(
                                fieldCode,
                                ValidationErrorCodes.InvalidFormat,
                                _validationMessageService.GetMessage(ValidationErrorCodes.InvalidFormat, fieldName),
                                "Format"
                            );
                        }
                    }
                }
                // Boolean
                else if (dataType == "bool" || dataType == "boolean")
                {
                    if (!valueDto.ValueBool.HasValue)
                    {
                        if (string.IsNullOrWhiteSpace(stringValue) || !bool.TryParse(stringValue, out _))
                        {
                            errors.AddError(
                                fieldCode,
                                ValidationErrorCodes.InvalidFormat,
                                _validationMessageService.GetMessage(ValidationErrorCodes.InvalidFormat, fieldName),
                                "Format"
                            );
                        }
                    }
                }
                // JSON
                else if (dataType == "json")
                {
                    if (!string.IsNullOrWhiteSpace(stringValue))
                    {
                        try
                        {
                            System.Text.Json.JsonDocument.Parse(stringValue);
                        }
                        catch
                        {
                            errors.AddError(
                                fieldCode,
                                ValidationErrorCodes.InvalidFormat,
                                _validationMessageService.GetMessage(ValidationErrorCodes.InvalidFormat, fieldName),
                                "Format"
                            );
                        }
                    }
                }

                // 3) FieldType-specific semantic validation based on TypeName
                // Email
                if (typeName.Contains("email"))
                {
                    if (!IsValidEmail(stringValue))
                    {
                        errors.AddError(
                            fieldCode,
                            ValidationErrorCodes.InvalidFormat,
                            _validationMessageService.GetMessage(ValidationErrorCodes.InvalidFormat, fieldName),
                            "Email"
                        );
                    }
                }
                // Password
                else if (typeName.Contains("password"))
                {
                    if (!IsValidPassword(stringValue))
                    {
                        errors.AddError(
                            fieldCode,
                            ValidationErrorCodes.InvalidFormat,
                            "Password is too weak. It should be at least 8 characters and contain letters and numbers.",
                            "Password"
                        );
                    }
                }
                // Phone
                else if (typeName.Contains("phone"))
                {
                    if (!IsValidPhone(stringValue))
                    {
                        errors.AddError(
                            fieldCode,
                            ValidationErrorCodes.InvalidFormat,
                            _validationMessageService.GetMessage(ValidationErrorCodes.InvalidFormat, fieldName),
                            "Phone"
                        );
                    }
                }
                // URL
                else if (typeName.Contains("url"))
                {
                    if (!IsValidUrl(stringValue))
                    {
                        errors.AddError(
                            fieldCode,
                            ValidationErrorCodes.InvalidFormat,
                            _validationMessageService.GetMessage(ValidationErrorCodes.InvalidFormat, fieldName),
                            "Url"
                        );
                    }
                }

                // 4) Validate against ApprovalStage MinAmount and MaxAmount (if submission is linked to a stage)
                if (stage != null && stage.MinAmount.HasValue && stage.MaxAmount.HasValue)
                    {
                        // Check if this field should be validated
                        // If AmountFieldCode is specified, only validate that field
                        // If AmountFieldCode is null, validate all numeric fields
                        bool shouldValidate = false;
                        if (string.IsNullOrWhiteSpace(stage.AmountFieldCode))
                        {
                            // No specific field specified - validate all numeric fields
                            shouldValidate = dataType == "int" || dataType == "decimal" || dataType.Contains("number");
                        }
                        else
                        {
                            // Specific field specified - only validate if field code matches
                            shouldValidate = fieldCode.Equals(stage.AmountFieldCode, StringComparison.OrdinalIgnoreCase) &&
                                           (dataType == "int" || dataType == "decimal" || dataType.Contains("number"));
                        }

                        if (shouldValidate)
                        {
                            decimal? fieldValue = null;
                            
                            // Get numeric value from ValueNumber or parse from ValueString
                            if (valueDto.ValueNumber.HasValue)
                            {
                                fieldValue = valueDto.ValueNumber.Value;
                            }
                            else if (!string.IsNullOrWhiteSpace(stringValue) && decimal.TryParse(stringValue, out decimal parsedValue))
                            {
                                fieldValue = parsedValue;
                            }

                            // Validate if value is provided
                            if (fieldValue.HasValue)
                            {
                                if (fieldValue.Value < stage.MinAmount.Value)
                                {
                                    errors.AddError(
                                        fieldCode,
                                        ValidationErrorCodes.InvalidRange,
                                        $"Field value ({fieldValue.Value}) must be greater than or equal to Min Amount ({stage.MinAmount.Value}) for stage '{stage.StageName}'",
                                        "Range"
                                    );
                                }
                                else if (fieldValue.Value > stage.MaxAmount.Value)
                                {
                                    errors.AddError(
                                        fieldCode,
                                        ValidationErrorCodes.InvalidRange,
                                        $"Field value ({fieldValue.Value}) must be less than or equal to Max Amount ({stage.MaxAmount.Value}) for stage '{stage.StageName}'",
                                        "Range"
                                    );
                                }
                            }
                        }
                    }
            }

            return errors;
        }

        /// <summary>
        /// If <paramref name="value"/> is a JSON array with a single primitive element (string/number/bool),
        /// returns that element as a string (e.g. ["1"] -> "1"). Otherwise returns null.
        /// </summary>
        private static string? TryUnwrapSingleJsonArrayScalar(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var trimmed = value.Trim();
            if (!trimmed.StartsWith("[") || !trimmed.EndsWith("]"))
                return null;

            try
            {
                using var doc = JsonDocument.Parse(trimmed);
                if (doc.RootElement.ValueKind != JsonValueKind.Array)
                    return null;

                var arr = doc.RootElement;
                if (arr.GetArrayLength() != 1)
                    return null;

                var el = arr[0];
                return el.ValueKind switch
                {
                    JsonValueKind.String => el.GetString(),
                    JsonValueKind.Number => el.GetRawText(),
                    JsonValueKind.True => "true",
                    JsonValueKind.False => "false",
                    _ => null
                };
            }
            catch
            {
                return null;
            }
        }

        private static bool IsValidEmail(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            try
            {
                // Simple email pattern
                var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(value, pattern, RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private static bool IsValidPassword(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            // At least 8 chars, contains letters and digits
            if (value.Length < 8)
                return false;

            bool hasLetter = value.Any(char.IsLetter);
            bool hasDigit = value.Any(char.IsDigit);

            return hasLetter && hasDigit;
        }

        private static bool IsValidPhone(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            // Allow +, digits, spaces, -, ()
            var pattern = @"^[\d\+\-\s\(\)]{6,20}$";
            return Regex.IsMatch(value, pattern);
        }

        private static bool IsValidUrl(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            return Uri.TryCreate(value, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        // ================================
        // HELPER METHODS
        // ================================
        private ApiResponse ConvertToApiResponse<T>(ServiceResult<T> result)
        {
            if (result.Success)
                return new ApiResponse(result.StatusCode, "Success", result.Data);
            else
                return new ApiResponse(result.StatusCode, result.ErrorMessage);
        }

        private ApiResponse ConvertToApiResponse(ServiceResult<bool> result)
        {
            if (result.Success)
                return new ApiResponse(result.StatusCode, "Success", result.Data);
            else
                return new ApiResponse(result.StatusCode, result.ErrorMessage);
        }
    }
}
