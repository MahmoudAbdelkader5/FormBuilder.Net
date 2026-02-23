using formBuilder.Domian.Interfaces;
using FormBuilder.API.DTOs;
using FormBuilder.API.Models;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.Domain.Interfaces;
using FormBuilder.Services.Services.Base;
using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.Common;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FormBuilder.Services
{
    public class FormGridService : BaseService<FORM_GRIDS, FormGridDto, CreateFormGridDto, UpdateFormGridDto>, IFormGridService
    {
        private readonly IunitOfwork _unitOfWork;

        public FormGridService(IunitOfwork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        protected override IBaseRepository<FORM_GRIDS> Repository => _unitOfWork.FormGridRepository;

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

        public async Task<ApiResponse> GetByFormBuilderIdAsync(int formBuilderId)
        {
            var formGrids = await _unitOfWork.FormGridRepository.GetByFormBuilderIdAsync(formBuilderId);
            var formGridDtos = _mapper.Map<IEnumerable<FormGridDto>>(formGrids);
            return new ApiResponse(200, "Form grids by form builder retrieved successfully", formGridDtos);
        }

        public async Task<ApiResponse> GetByTabIdAsync(int tabId)
        {
            var formGrids = await _unitOfWork.FormGridRepository.GetByTabIdAsync(tabId);
            var formGridDtos = _mapper.Map<IEnumerable<FormGridDto>>(formGrids);
            return new ApiResponse(200, "Form grids by tab retrieved successfully", formGridDtos);
        }

        public async Task<ApiResponse> GetActiveByFormBuilderIdAsync(int formBuilderId)
        {
            var formGrids = await _unitOfWork.FormGridRepository.GetActiveByFormBuilderIdAsync(formBuilderId);
            var formGridDtos = _mapper.Map<IEnumerable<FormGridDto>>(formGrids);
            return new ApiResponse(200, "Active form grids by form builder retrieved successfully", formGridDtos);
        }

        public async Task<ApiResponse> GetByGridCodeAsync(string gridCode, int formBuilderId)
        {
            var formGrid = await _unitOfWork.FormGridRepository.GetByGridCodeAsync(gridCode, formBuilderId);
            if (formGrid == null)
                return new ApiResponse(404, "Form grid not found");

            var formGridDto = _mapper.Map<FormGridDto>(formGrid);
            return new ApiResponse(200, "Form grid retrieved successfully", formGridDto);
        }

        public async Task<ApiResponse> CreateAsync(CreateFormGridDto createDto)
        {
            // Validate before creating
            var validation = await ValidateCreateAsync(createDto);
            if (!validation.IsValid)
            {
                return new ApiResponse(400, validation.ErrorMessage ?? "Validation failed");
            }

            // Get next grid order if not specified
            var gridOrder = createDto.GridOrder ??
                await _unitOfWork.FormGridRepository.GetNextGridOrderAsync(createDto.FormBuilderId, createDto.TabId);

            var entity = _mapper.Map<FORM_GRIDS>(createDto);
            entity.GridOrder = gridOrder;
            entity.CreatedDate = DateTime.UtcNow;
            entity.IsActive = true;

            Repository.Add(entity);
            await _unitOfWork.CompleteAsyn();

            var dto = _mapper.Map<FormGridDto>(entity);
            return new ApiResponse(200, "Form grid created successfully", dto);
        }

        protected override async Task<ValidationResult> ValidateCreateAsync(CreateFormGridDto dto)
        {
            var codeExists = await _unitOfWork.FormGridRepository.GridCodeExistsAsync(dto.GridCode, dto.FormBuilderId);
            if (codeExists)
                return ValidationResult.Failure("Form grid code already exists for this form builder");

            // Validate MinRows and MaxRows
            if (dto.MinRows.HasValue && dto.MaxRows.HasValue)
            {
                if (dto.MinRows.Value > dto.MaxRows.Value)
                    return ValidationResult.Failure("MinRows cannot be greater than MaxRows");
                
                if (dto.MinRows.Value < 0)
                    return ValidationResult.Failure("MinRows cannot be negative");
                
                if (dto.MaxRows.Value < 0)
                    return ValidationResult.Failure("MaxRows cannot be negative");
            }

            if (dto.MinRows.HasValue && dto.MinRows.Value < 0)
                return ValidationResult.Failure("MinRows cannot be negative");

            if (dto.MaxRows.HasValue && dto.MaxRows.Value < 0)
                return ValidationResult.Failure("MaxRows cannot be negative");

            return ValidationResult.Success();
        }

        public async Task<ApiResponse> UpdateAsync(int id, UpdateFormGridDto updateDto)
        {
            var result = await base.UpdateAsync(id, updateDto);
            return ConvertToApiResponse(result);
        }

        protected override async Task<ValidationResult> ValidateUpdateAsync(int id, UpdateFormGridDto dto, FORM_GRIDS entity)
        {
            if (!string.IsNullOrEmpty(dto.GridCode) && dto.GridCode != entity.GridCode)
            {
                var codeExists = await _unitOfWork.FormGridRepository.GridCodeExistsAsync(dto.GridCode, entity.FormBuilderId, id);
                if (codeExists)
                    return ValidationResult.Failure("Form grid code already exists for this form builder");
            }

            // Validate MinRows and MaxRows
            var minRows = dto.MinRows ?? entity.MinRows;
            var maxRows = dto.MaxRows ?? entity.MaxRows;

            if (minRows.HasValue && maxRows.HasValue)
            {
                if (minRows.Value > maxRows.Value)
                    return ValidationResult.Failure("MinRows cannot be greater than MaxRows");
                
                if (minRows.Value < 0)
                    return ValidationResult.Failure("MinRows cannot be negative");
                
                if (maxRows.Value < 0)
                    return ValidationResult.Failure("MaxRows cannot be negative");
            }

            if (minRows.HasValue && minRows.Value < 0)
                return ValidationResult.Failure("MinRows cannot be negative");

            if (maxRows.HasValue && maxRows.Value < 0)
                return ValidationResult.Failure("MaxRows cannot be negative");

            return ValidationResult.Success();
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            // Use Repository.SingleOrDefaultAsync directly (without IsDeleted filter) to get entity even if already deleted
            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id, asNoTracking: false);
            if (entity == null)
            {
                return new ApiResponse(404, "Form grid not found");
            }

            // Check if already deleted
            if (entity.IsDeleted)
            {
                return new ApiResponse(200, "Form grid is already deleted");
            }

            // Soft Delete - Always use soft delete
            entity.IsDeleted = true;
            entity.DeletedDate = DateTime.UtcNow;
            entity.IsActive = false;
            entity.UpdatedDate = DateTime.UtcNow;
            
            // Use repository Update method directly to ensure changes are tracked
            _unitOfWork.FormGridRepository.Update(entity);
            await _unitOfWork.CompleteAsyn();
            
            return new ApiResponse(200, "Form grid deleted successfully");
        }

        public async Task<ApiResponse> ToggleActiveAsync(int id, bool isActive)
        {
            var result = await base.ToggleActiveAsync(id, isActive);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> ExistsAsync(int id)
        {
            var exists = await _unitOfWork.FormGridRepository.AnyAsync(g => g.Id == id);
            return new ApiResponse(200, "Form grid existence checked successfully", exists);
        }

        public async Task<ApiResponse> GridCodeExistsAsync(string gridCode, int formBuilderId, int? excludeId = null)
        {
            var exists = await _unitOfWork.FormGridRepository.GridCodeExistsAsync(gridCode, formBuilderId, excludeId);
            return new ApiResponse(200, "Form grid code existence checked successfully", exists);
        }

        public async Task<ApiResponse> GetNextGridOrderAsync(int formBuilderId, int? tabId = null)
        {
            var nextOrder = await _unitOfWork.FormGridRepository.GetNextGridOrderAsync(formBuilderId, tabId);
            return new ApiResponse(200, "Next grid order retrieved successfully", nextOrder);
        }

        public async Task<ApiResponse> CopyAsync(CopyFormGridDto copyDto)
        {
            try
            {
                // Load the original grid with all columns, options, and data sources
                var originalGrid = await _unitOfWork.FormGridRepository.GetByIdAsync(copyDto.SourceGridId);
                if (originalGrid == null)
                {
                    return new ApiResponse(404, "Source grid not found");
                }

                // Validate target form builder exists
                var targetFormExists = await _unitOfWork.FormBuilderRepository.AnyAsync(f => f.Id == copyDto.TargetFormBuilderId);
                if (!targetFormExists)
                {
                    return new ApiResponse(404, "Target form builder not found");
                }

                // Generate new grid code if not provided
                var newGridCode = copyDto.NewGridCode ?? $"{originalGrid.GridCode}_COPY_{DateTime.UtcNow:yyyyMMddHHmmss}";
                
                // Check if the new grid code already exists
                var codeExists = await _unitOfWork.FormGridRepository.GridCodeExistsAsync(newGridCode, copyDto.TargetFormBuilderId);
                if (codeExists)
                {
                    return new ApiResponse(400, $"Grid code '{newGridCode}' already exists in the target form builder");
                }

                // Generate new grid name if not provided
                var newGridName = copyDto.NewGridName ?? $"{originalGrid.GridName} (Copy)";

                // Get next grid order
                var gridOrder = await _unitOfWork.FormGridRepository.GetNextGridOrderAsync(copyDto.TargetFormBuilderId, copyDto.TargetTabId);

                // Create new grid
                var newGrid = new FORM_GRIDS
                {
                    FormBuilderId = copyDto.TargetFormBuilderId,
                    GridName = newGridName,
                    GridCode = newGridCode,
                    TabId = copyDto.TargetTabId,
                    GridOrder = gridOrder,
                    MinRows = originalGrid.MinRows,
                    MaxRows = originalGrid.MaxRows,
                    GridRulesJson = originalGrid.GridRulesJson,
                    IsActive = originalGrid.IsActive,
                    CreatedByUserId = originalGrid.CreatedByUserId,
                    CreatedDate = DateTime.UtcNow,
                    FORM_GRID_COLUMNS = new List<FORM_GRID_COLUMNS>()
                };

                // Copy columns if requested
                if (copyDto.CopyColumns)
                {
                    // Load columns with options and data sources
                    var originalColumns = await _unitOfWork.FormGridColumnRepository.GetByGridIdAsync(copyDto.SourceGridId);
                    
                    foreach (var originalColumn in originalColumns.OrderBy(c => c.ColumnOrder))
                    {
                        var newColumn = new FORM_GRID_COLUMNS
                        {
                            GridId = 0, // Will be set after saving
                            ColumnName = originalColumn.ColumnName,
                            ColumnCode = originalColumn.ColumnCode,
                            ColumnOrder = originalColumn.ColumnOrder,
                            FieldTypeId = originalColumn.FieldTypeId,
                            IsMandatory = originalColumn.IsMandatory,
                            DataType = originalColumn.DataType,
                            MaxLength = originalColumn.MaxLength,
                            DefaultValueJson = originalColumn.DefaultValueJson,
                            ValidationRuleJson = originalColumn.ValidationRuleJson,
                            IsReadOnly = originalColumn.IsReadOnly,
                            IsVisible = originalColumn.IsVisible,
                            VisibilityRuleJson = originalColumn.VisibilityRuleJson,
                            IsActive = originalColumn.IsActive,
                            CreatedByUserId = originalColumn.CreatedByUserId,
                            CreatedDate = DateTime.UtcNow,
                            GRID_COLUMN_OPTIONS = new List<GRID_COLUMN_OPTIONS>(),
                            GRID_COLUMN_DATA_SOURCES = new List<GRID_COLUMN_DATA_SOURCES>()
                        };

                        // Load and copy column options
                        var originalOptions = await _unitOfWork.GridColumnOptionsRepository.GetByColumnIdAsync(originalColumn.Id);
                        foreach (var originalOption in originalOptions)
                        {
                            var newOption = new GRID_COLUMN_OPTIONS
                            {
                                ColumnId = 0, // Will be set after saving
                                OptionText = originalOption.OptionText,
                                ForeignOptionText = originalOption.ForeignOptionText,
                                OptionValue = originalOption.OptionValue,
                                OptionOrder = originalOption.OptionOrder,
                                IsDefault = originalOption.IsDefault,
                                IsActive = originalOption.IsActive,
                                CreatedByUserId = originalOption.CreatedByUserId,
                                CreatedDate = DateTime.UtcNow
                            };
                            newColumn.GRID_COLUMN_OPTIONS.Add(newOption);
                        }

                        // Load and copy column data sources
                        var originalDataSources = await _unitOfWork.GridColumnDataSourcesRepository.GetByColumnIdAsync(originalColumn.Id);
                        foreach (var originalDataSource in originalDataSources)
                        {
                            var newDataSource = new GRID_COLUMN_DATA_SOURCES
                            {
                                ColumnId = 0, // Will be set after saving
                                SourceType = originalDataSource.SourceType,
                                ApiUrl = originalDataSource.ApiUrl,
                                ApiPath = originalDataSource.ApiPath,
                                HttpMethod = originalDataSource.HttpMethod,
                                RequestBodyJson = originalDataSource.RequestBodyJson,
                                ValuePath = originalDataSource.ValuePath,
                                TextPath = originalDataSource.TextPath,
                                ConfigurationJson = originalDataSource.ConfigurationJson,
                                ArrayPropertyNames = originalDataSource.ArrayPropertyNames,
                                IsActive = originalDataSource.IsActive,
                                CreatedByUserId = originalDataSource.CreatedByUserId,
                                CreatedDate = DateTime.UtcNow
                            };
                            newColumn.GRID_COLUMN_DATA_SOURCES.Add(newDataSource);
                        }

                        newGrid.FORM_GRID_COLUMNS.Add(newColumn);
                    }
                }

                // Save the new grid
                Repository.Add(newGrid);
                await _unitOfWork.CompleteAsyn();

                // Update GridId in columns and ColumnId in options/data sources
                foreach (var column in newGrid.FORM_GRID_COLUMNS)
                {
                    column.GridId = newGrid.Id;

                    foreach (var option in column.GRID_COLUMN_OPTIONS)
                    {
                        option.ColumnId = column.Id;
                    }

                    foreach (var dataSource in column.GRID_COLUMN_DATA_SOURCES)
                    {
                        dataSource.ColumnId = column.Id;
                    }
                }

                // Final save
                await _unitOfWork.CompleteAsyn();

                // Return the new grid
                var gridDto = _mapper.Map<FormGridDto>(newGrid);
                return new ApiResponse(200, "Grid copied successfully", gridDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, $"Error copying grid: {ex.Message}");
            }
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
    }
}