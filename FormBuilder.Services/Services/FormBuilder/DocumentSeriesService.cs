using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.Domian.Interfaces;
using FormBuilder.API.Models;
using FormBuilder.Services.Services.Base;
using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.Common;
using AutoMapper;
using FormBuilder.Services.Services.FormBuilder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FormBuilder.Services
{
    public class DocumentSeriesService : BaseService<DOCUMENT_SERIES, DocumentSeriesDto, CreateDocumentSeriesDto, UpdateDocumentSeriesDto>, IDocumentSeriesService
    {
        private readonly IunitOfwork _unitOfWork;
        private readonly IFormSubmissionsService _formSubmissionsService;

        public DocumentSeriesService(
            IunitOfwork unitOfWork,
            IMapper mapper,
            IFormSubmissionsService formSubmissionsService) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _formSubmissionsService = formSubmissionsService ?? throw new ArgumentNullException(nameof(formSubmissionsService));
        }

        protected override IBaseRepository<DOCUMENT_SERIES> Repository => _unitOfWork.DocumentSeriesRepository;

        public async Task<ApiResponse> GetAllAsync()
        {
            var result = await base.GetAllAsync();
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> GetByIdAsync(int id)
        {
            var documentSeries = await _unitOfWork.DocumentSeriesRepository.GetByIdAsync(id);
            if (documentSeries == null)
                return new ApiResponse(404, "Document series not found");

            var documentSeriesDto = _mapper.Map<DocumentSeriesDto>(documentSeries);
            return new ApiResponse(200, "Document series retrieved successfully", documentSeriesDto);
        }

        public async Task<ApiResponse> GetBySeriesCodeAsync(string seriesCode)
        {
            var documentSeries = await _unitOfWork.DocumentSeriesRepository.GetBySeriesCodeAsync(seriesCode);
            if (documentSeries == null)
                return new ApiResponse(404, "Document series not found");

            var documentSeriesDto = _mapper.Map<DocumentSeriesDto>(documentSeries);
            return new ApiResponse(200, "Document series retrieved successfully", documentSeriesDto);
        }

        public async Task<ApiResponse> GetByDocumentTypeIdAsync(int documentTypeId)
        {
            var documentSeries = await _unitOfWork.DocumentSeriesRepository.GetByDocumentTypeIdAsync(documentTypeId);
            var documentSeriesDtos = _mapper.Map<IEnumerable<DocumentSeriesDto>>(documentSeries);
            return new ApiResponse(200, "Document series retrieved successfully", documentSeriesDtos);
        }

        public async Task<ApiResponse> GetByProjectIdAsync(int projectId)
        {
            var documentSeries = await _unitOfWork.DocumentSeriesRepository.GetByProjectIdAsync(projectId);
            var documentSeriesDtos = _mapper.Map<IEnumerable<DocumentSeriesDto>>(documentSeries);
            return new ApiResponse(200, "Document series retrieved successfully", documentSeriesDtos);
        }

        public async Task<ApiResponse> GetActiveAsync()
        {
            var result = await base.GetActiveAsync();
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> GetDefaultSeriesAsync(int documentTypeId, int projectId)
        {
            var documentSeries = await _unitOfWork.DocumentSeriesRepository.GetDefaultSeriesAsync(documentTypeId, projectId);
            if (documentSeries == null)
                return new ApiResponse(404, "Default document series not found");

            var documentSeriesDto = _mapper.Map<DocumentSeriesDto>(documentSeries);
            return new ApiResponse(200, "Default document series retrieved successfully", documentSeriesDto);
        }

        public async Task<ApiResponse> CreateAsync(CreateDocumentSeriesDto createDto)
        {
            NormalizeCreateDto(createDto);

            // If setting as default, remove default from other series with same document type and project
            if (createDto.IsDefault)
            {
                await RemoveDefaultFromOtherSeriesAsync(createDto.ProjectId);
            }

            var result = await base.CreateAsync(createDto);
            return ConvertToApiResponse(result);
        }

        protected override async Task<ValidationResult> ValidateCreateAsync(CreateDocumentSeriesDto dto)
        {
            var codeExists = await _unitOfWork.DocumentSeriesRepository.SeriesCodeExistsAsync(dto.SeriesCode);
            if (codeExists)
                return ValidationResult.Failure("Document series code already exists");

            // Validate ProjectId
            var projectExists = await _unitOfWork.ProjectRepository.AnyAsync(p => p.Id == dto.ProjectId);
            if (!projectExists)
                return ValidationResult.Failure("Invalid project ID");

            if (!DocumentSeriesEngineRules.TryNormalizeResetPolicy(dto.ResetPolicy, out var normalizedResetPolicy))
                return ValidationResult.Failure("ResetPolicy must be one of: None, Yearly, Monthly, Daily.");

            if (!DocumentSeriesEngineRules.TryNormalizeGenerateOn(dto.GenerateOn, out var normalizedGenerateOn))
                return ValidationResult.Failure("GenerateOn must be one of: Submit, Approval.");

            if (dto.SequenceStart <= 0)
                return ValidationResult.Failure("SequenceStart must be greater than 0.");

            if (dto.SequencePadding <= 0 || dto.SequencePadding > 12)
                return ValidationResult.Failure("SequencePadding must be between 1 and 12.");

            if (!DocumentSeriesEngineRules.TryValidateTemplate(dto.Template, out var templateError))
                return ValidationResult.Failure(templateError);

            dto.ResetPolicy = normalizedResetPolicy;
            dto.GenerateOn = normalizedGenerateOn;

            return ValidationResult.Success();
        }

        public async Task<ApiResponse> UpdateAsync(int id, UpdateDocumentSeriesDto updateDto)
        {
            // Load entity with no tracking to avoid navigation property update conflicts
            var dbContext = _unitOfWork.AppDbContext;
            var entity = await dbContext.Set<DOCUMENT_SERIES>()
                .AsNoTracking()
                .FirstOrDefaultAsync(ds => ds.Id == id);
            
            if (entity == null)
                return new ApiResponse(404, "Document series not found");

            NormalizeUpdateDto(updateDto, entity);

            // If setting as default, remove default from other series with same document type and project
            if (updateDto.IsDefault.HasValue && updateDto.IsDefault.Value)
            {
                var projectId = updateDto.ProjectId ?? entity.ProjectId;
                await RemoveDefaultFromOtherSeriesAsync(projectId, id);
            }

            // Use raw SQL to update the entity directly - this bypasses EF tracking issues
            // and prevents conflicts with Foreign Key constraints
            var dbContext1 = _unitOfWork.AppDbContext;
            var sqlParams = new List<object>();
            var updateFields = new List<string>();
            int paramIndex = 0;

            if (updateDto.ProjectId.HasValue)
            {
                updateFields.Add($"ProjectId = {{{paramIndex}}}");
                sqlParams.Add(updateDto.ProjectId.Value);
                paramIndex++;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.SeriesCode))
            {
                updateFields.Add($"SeriesCode = {{{paramIndex}}}");
                sqlParams.Add(updateDto.SeriesCode);
                paramIndex++;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.SeriesName))
            {
                updateFields.Add($"SeriesName = {{{paramIndex}}}");
                sqlParams.Add(updateDto.SeriesName);
                paramIndex++;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Template))
            {
                updateFields.Add($"Template = {{{paramIndex}}}");
                sqlParams.Add(updateDto.Template);
                paramIndex++;
            }

            if (updateDto.SequenceStart.HasValue)
            {
                updateFields.Add($"SequenceStart = {{{paramIndex}}}");
                sqlParams.Add(updateDto.SequenceStart.Value);
                paramIndex++;
            }

            if (updateDto.SequencePadding.HasValue)
            {
                updateFields.Add($"SequencePadding = {{{paramIndex}}}");
                sqlParams.Add(updateDto.SequencePadding.Value);
                paramIndex++;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.ResetPolicy))
            {
                updateFields.Add($"ResetPolicy = {{{paramIndex}}}");
                sqlParams.Add(updateDto.ResetPolicy);
                paramIndex++;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.GenerateOn))
            {
                updateFields.Add($"GenerateOn = {{{paramIndex}}}");
                sqlParams.Add(updateDto.GenerateOn);
                paramIndex++;
            }

            if (updateDto.NextNumber.HasValue)
            {
                updateFields.Add($"NextNumber = {{{paramIndex}}}");
                sqlParams.Add(updateDto.NextNumber.Value);
                paramIndex++;
            }

            if (updateDto.IsDefault.HasValue)
            {
                updateFields.Add($"IsDefault = {{{paramIndex}}}");
                sqlParams.Add(updateDto.IsDefault.Value);
                paramIndex++;
            }

            if (updateDto.IsActive.HasValue)
            {
                updateFields.Add($"IsActive = {{{paramIndex}}}");
                sqlParams.Add(updateDto.IsActive.Value);
                paramIndex++;
            }

            updateFields.Add("UpdatedDate = GETUTCDATE()");

            if (updateFields.Any())
            {
                // Add id as the last parameter for WHERE clause
                sqlParams.Add(id);
                var sql = $"UPDATE DOCUMENT_SERIES SET {string.Join(", ", updateFields)} WHERE Id = {{{paramIndex}}}";
                await dbContext.Database.ExecuteSqlRawAsync(sql, sqlParams.ToArray());
            }

            // Reload the entity to return updated data
            entity = await _unitOfWork.DocumentSeriesRepository.GetByIdAsync(id);
            if (entity == null)
                return new ApiResponse(404, "Document series not found");

            var dto = _mapper.Map<DocumentSeriesDto>(entity);
            return new ApiResponse(200, "Document series updated successfully", dto);
        }

        protected override async Task<ValidationResult> ValidateUpdateAsync(int id, UpdateDocumentSeriesDto dto, DOCUMENT_SERIES entity)
        {
            // Check if series code already exists (excluding current record)
            if (!string.IsNullOrEmpty(dto.SeriesCode) && dto.SeriesCode != entity.SeriesCode)
            {
                var codeExists = await _unitOfWork.DocumentSeriesRepository.SeriesCodeExistsAsync(dto.SeriesCode, id);
                if (codeExists)
                    return ValidationResult.Failure("Document series code already exists");
            }

            // Validate ProjectId if provided
            if (dto.ProjectId.HasValue)
            {
                var projectExists = await _unitOfWork.ProjectRepository.AnyAsync(p => p.Id == dto.ProjectId.Value);
                if (!projectExists)
                    return ValidationResult.Failure("Invalid project ID");
            }

            if (dto.SequenceStart.HasValue && dto.SequenceStart.Value <= 0)
                return ValidationResult.Failure("SequenceStart must be greater than 0.");

            if (dto.SequencePadding.HasValue && (dto.SequencePadding.Value <= 0 || dto.SequencePadding.Value > 12))
                return ValidationResult.Failure("SequencePadding must be between 1 and 12.");

            var effectiveResetPolicy = dto.ResetPolicy ?? entity.ResetPolicy;
            if (!DocumentSeriesEngineRules.TryNormalizeResetPolicy(effectiveResetPolicy, out var normalizedResetPolicy))
                return ValidationResult.Failure("ResetPolicy must be one of: None, Yearly, Monthly, Daily.");

            var effectiveGenerateOn = dto.GenerateOn ?? entity.GenerateOn;
            if (!DocumentSeriesEngineRules.TryNormalizeGenerateOn(effectiveGenerateOn, out var normalizedGenerateOn))
                return ValidationResult.Failure("GenerateOn must be one of: Submit, Approval.");

            var effectiveTemplate = dto.Template ?? entity.Template;
            if (!DocumentSeriesEngineRules.TryValidateTemplate(effectiveTemplate, out var templateError))
                return ValidationResult.Failure(templateError);

            if (dto.ResetPolicy != null)
                dto.ResetPolicy = normalizedResetPolicy;
            if (dto.GenerateOn != null)
                dto.GenerateOn = normalizedGenerateOn;

            return ValidationResult.Success();
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.DocumentSeriesRepository.GetByIdAsync(id);
            if (entity == null)
                return new ApiResponse(404, "Document series not found");

            try
            {
                // Soft-delete all active submissions linked to this series first.
                // This prevents FK conflicts while preserving data audit history.
                var submissionIds = await _unitOfWork.FormSubmissionsRepository
                    .GetAll()
                    .Where(fs => fs.SeriesId == id && !fs.IsDeleted)
                    .Select(fs => fs.Id)
                    .ToListAsync();

                foreach (var submissionId in submissionIds)
                {
                    var deleteSubmissionResult = await _formSubmissionsService.DeleteAsync(submissionId);
                    if (deleteSubmissionResult.StatusCode >= 400)
                    {
                        return new ApiResponse(
                            deleteSubmissionResult.StatusCode,
                            $"Failed to delete related form submission {submissionId}: {deleteSubmissionResult.Message}");
                    }
                }

                var result = await base.DeleteAsync(id);
                return ConvertToApiResponse(result);
            }
            catch (Exception ex)
            {
                // Check if it's a foreign key constraint violation
                if (ex.Message.Contains("REFERENCE constraint") || 
                    ex.Message.Contains("FK_FORM_SUBMISSIONS_DOCUMENT_SERIES") ||
                    ex.InnerException?.Message?.Contains("REFERENCE constraint") == true ||
                    ex.InnerException?.Message?.Contains("FK_FORM_SUBMISSIONS_DOCUMENT_SERIES") == true)
                {
                    // Re-check count in case submissions were added between checks
                    var finalCount = await _unitOfWork.FormSubmissionsRepository.CountAsync(fs => fs.SeriesId == id && !fs.IsDeleted);
                    
                    return new ApiResponse(400, 
                        $"Cannot delete document series: There are {finalCount} active form submission(s) associated with this series.");
                }
                
                return new ApiResponse(500, $"Error deleting document series: {ex.Message}");
            }
        }

        public async Task<ApiResponse> ToggleActiveAsync(int id, bool isActive)
        {
            var result = await base.ToggleActiveAsync(id, isActive);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> SetAsDefaultAsync(int id)
        {
            var entity = await _unitOfWork.DocumentSeriesRepository.GetByIdAsync(id);
            if (entity == null)
                return new ApiResponse(404, "Document series not found");

            // Remove default from other series with same document type and project
            await RemoveDefaultFromOtherSeriesAsync(entity.ProjectId, id);

            entity.IsDefault = true;
            _unitOfWork.DocumentSeriesRepository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            var dto = _mapper.Map<DocumentSeriesDto>(entity);
            return new ApiResponse(200, "Document series set as default successfully", dto);
        }

        public async Task<ApiResponse> GetNextNumberAsync(int seriesId)
        {
            var nextNumber = await _unitOfWork.DocumentSeriesRepository.GetNextNumberAsync(seriesId);
            if (nextNumber == -1)
                return new ApiResponse(404, "Document series not found");

            var series = await _unitOfWork.DocumentSeriesRepository.GetByIdAsync(seriesId);
            var result = new DocumentSeriesNumberDto
            {
                SeriesId = seriesId,
                SeriesCode = series.SeriesCode,
                NextNumber = nextNumber,
                FullNumber = $"{series.SeriesCode}-{nextNumber:D6}"
            };

            return new ApiResponse(200, "Next number retrieved successfully", result);
        }

        public async Task<ApiResponse> ExistsAsync(int id)
        {
            var exists = await _unitOfWork.DocumentSeriesRepository.AnyAsync(s => s.Id == id);
            return new ApiResponse(200, "Document series existence checked successfully", exists);
        }

        // ================================
        // PRIVATE HELPER METHODS
        // ================================
        private async Task RemoveDefaultFromOtherSeriesAsync(int projectId, int? excludeId = null)
        {
            var existingDefaultSeries = await _unitOfWork.DocumentSeriesRepository
                .GetDefaultSeriesAsync(0, projectId);

            if (existingDefaultSeries != null && existingDefaultSeries.Id != excludeId)
            {
                existingDefaultSeries.IsDefault = false;
                _unitOfWork.DocumentSeriesRepository.Update(existingDefaultSeries);
            }
        }

        private static void NormalizeCreateDto(CreateDocumentSeriesDto dto)
        {
            dto.SeriesName = string.IsNullOrWhiteSpace(dto.SeriesName) ? dto.SeriesCode : dto.SeriesName.Trim();
            dto.Template = string.IsNullOrWhiteSpace(dto.Template) ? $"{dto.SeriesCode}-{{SEQ}}" : dto.Template.Trim();
            dto.ResetPolicy = string.IsNullOrWhiteSpace(dto.ResetPolicy) ? "None" : dto.ResetPolicy.Trim();
            dto.GenerateOn = string.IsNullOrWhiteSpace(dto.GenerateOn) ? "Submit" : dto.GenerateOn.Trim();
            dto.SequenceStart = dto.SequenceStart <= 0 ? 1 : dto.SequenceStart;
            dto.SequencePadding = dto.SequencePadding <= 0 ? 3 : dto.SequencePadding;
            dto.NextNumber = dto.NextNumber <= 0 ? dto.SequenceStart : dto.NextNumber;
        }

        private static void NormalizeUpdateDto(UpdateDocumentSeriesDto dto, DOCUMENT_SERIES entity)
        {
            if (dto.SeriesName != null)
                dto.SeriesName = string.IsNullOrWhiteSpace(dto.SeriesName) ? entity.SeriesName : dto.SeriesName.Trim();

            if (dto.Template != null)
                dto.Template = string.IsNullOrWhiteSpace(dto.Template) ? entity.Template : dto.Template.Trim();

            if (dto.ResetPolicy != null)
                dto.ResetPolicy = dto.ResetPolicy.Trim();

            if (dto.GenerateOn != null)
                dto.GenerateOn = dto.GenerateOn.Trim();
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
