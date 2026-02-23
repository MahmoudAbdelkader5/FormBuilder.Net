using formBuilder.Domian.Interfaces;
using FormBuilder.API.DTOs;
using FormBuilder.API.Models;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Services.Services.Base;
using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.Common;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Core.IServices;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormSubmissionGridCellDto = FormBuilder.API.DTOs.FormSubmissionGridCellDto;

namespace FormBuilder.Services
{
    public class FormSubmissionGridRowService : BaseService<FORM_SUBMISSION_GRID_ROWS, FormSubmissionGridRowDto, CreateFormSubmissionGridRowDto, UpdateFormSubmissionGridRowDto>, IFormSubmissionGridRowService
    {
        private readonly IunitOfwork _unitOfWork;
        private readonly IFormSubmissionGridCellService _formSubmissionGridCellService;

        public FormSubmissionGridRowService(
            IunitOfwork unitOfWork, 
            IMapper mapper,
            IFormSubmissionGridCellService formSubmissionGridCellService) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _formSubmissionGridCellService = formSubmissionGridCellService ?? throw new ArgumentNullException(nameof(formSubmissionGridCellService));
        }

        protected override IBaseRepository<FORM_SUBMISSION_GRID_ROWS> Repository => _unitOfWork.FormSubmissionGridRowRepository;

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
            var rows = await _unitOfWork.FormSubmissionGridRowRepository.GetBySubmissionIdAsync(submissionId);
            var rowDtos = _mapper.Map<IEnumerable<FormSubmissionGridRowDto>>(rows);
            return new ApiResponse(200, "Grid rows retrieved successfully", rowDtos);
        }

        public async Task<ApiResponse> GetByGridIdAsync(int gridId)
        {
            var rows = await _unitOfWork.FormSubmissionGridRowRepository.GetByGridIdAsync(gridId);
            var rowDtos = _mapper.Map<IEnumerable<FormSubmissionGridRowDto>>(rows);
            return new ApiResponse(200, "Grid rows retrieved successfully", rowDtos);
        }

        public async Task<ApiResponse> GetBySubmissionAndGridAsync(int submissionId, int gridId)
        {
            var rows = await _unitOfWork.FormSubmissionGridRowRepository.GetBySubmissionAndGridAsync(submissionId, gridId);
            var rowDtos = _mapper.Map<IEnumerable<FormSubmissionGridRowDto>>(rows);
            return new ApiResponse(200, "Grid rows retrieved successfully", rowDtos);
        }

        public async Task<ApiResponse> GetActiveRowsAsync(int submissionId, int gridId)
        {
            var rows = await _unitOfWork.FormSubmissionGridRowRepository.GetActiveRowsAsync(submissionId, gridId);
            var rowDtos = _mapper.Map<IEnumerable<FormSubmissionGridRowDto>>(rows);
            return new ApiResponse(200, "Active grid rows retrieved successfully", rowDtos);
        }

        public async Task<ApiResponse> CreateAsync(CreateFormSubmissionGridRowDto createDto)
        {
            if (createDto == null)
                return new ApiResponse(400, "DTO is required");

            // Check if submission exists
            var submission = await _unitOfWork.FormSubmissionsRepository.GetByIdAsync(createDto.SubmissionId);
            if (submission == null)
                return new ApiResponse(404, "Form submission not found");

            // Check if grid exists
            var grid = await _unitOfWork.FormGridRepository.GetByIdAsync(createDto.GridId);
            if (grid == null)
                return new ApiResponse(404, "Form grid not found");

            // Check if submission belongs to same form builder as grid
            if (submission.FormBuilderId != grid.FormBuilderId)
                return new ApiResponse(400, "Submission and grid must belong to the same form builder");

            // Get row index if not provided
            var rowIndex = createDto.RowIndex ??
                await _unitOfWork.FormSubmissionGridRowRepository.GetNextRowIndexAsync(createDto.SubmissionId, createDto.GridId);

            // Check if row already exists at this index
            var rowExists = await _unitOfWork.FormSubmissionGridRowRepository
                .RowExistsAsync(createDto.SubmissionId, createDto.GridId, rowIndex);
            if (rowExists)
                return new ApiResponse(400, $"Row already exists at index {rowIndex}");

            var entity = _mapper.Map<FORM_SUBMISSION_GRID_ROWS>(createDto);
            entity.RowIndex = rowIndex;
            entity.CreatedDate = DateTime.UtcNow;

            _unitOfWork.FormSubmissionGridRowRepository.Add(entity);
            await _unitOfWork.CompleteAsyn();

            var createdEntity = await _unitOfWork.FormSubmissionGridRowRepository.GetByIdAsync(entity.Id);
            var rowDto = _mapper.Map<FormSubmissionGridRowDto>(createdEntity);

            return new ApiResponse(201, "Grid row created successfully", rowDto);
        }

        public async Task<ApiResponse> CreateMultipleAsync(List<CreateFormSubmissionGridRowDto> createDtos)
        {
            if (createDtos == null || !createDtos.Any())
                return new ApiResponse(400, "DTOs are required");

            var createdRows = new List<FormSubmissionGridRowDto>();

            foreach (var createDto in createDtos)
            {
                var result = await CreateAsync(createDto);
                if (result.StatusCode == 201)
                {
                    createdRows.Add(result.Data as FormSubmissionGridRowDto);
                }
            }

            return new ApiResponse(201, "Multiple grid rows created successfully", createdRows);
        }

        public async Task<ApiResponse> UpdateAsync(int id, UpdateFormSubmissionGridRowDto updateDto)
        {
            if (updateDto == null)
                return new ApiResponse(400, "DTO is required");

            var entity = await _unitOfWork.FormSubmissionGridRowRepository.GetByIdAsync(id);
            if (entity == null)
                return new ApiResponse(404, "Grid row not found");

            // Prevent changing SubmissionId and GridId - remove them from DTO if present
            if (updateDto.SubmissionId.HasValue)
            {
                updateDto.SubmissionId = null; // Prevent SubmissionId changes
            }
            if (updateDto.GridId.HasValue)
            {
                updateDto.GridId = null; // Prevent GridId changes
            }

            // Validate that the existing SubmissionId and GridId still exist
            var submissionExists = await _unitOfWork.FormSubmissionsRepository.AnyAsync(s => s.Id == entity.SubmissionId);
            if (!submissionExists)
            {
                return new ApiResponse(400, $"The submission (Id: {entity.SubmissionId}) associated with this row no longer exists.");
            }

            var gridExists = await _unitOfWork.FormGridRepository.AnyAsync(g => g.Id == entity.GridId);
            if (!gridExists)
            {
                return new ApiResponse(400, $"The grid (Id: {entity.GridId}) associated with this row no longer exists.");
            }

            // Update row index if provided and different
            if (updateDto.RowIndex.HasValue && updateDto.RowIndex.Value != entity.RowIndex)
            {
                // Check if row already exists at new index
                var rowExists = await _unitOfWork.FormSubmissionGridRowRepository
                    .RowExistsAsync(entity.SubmissionId, entity.GridId, updateDto.RowIndex.Value);
                if (rowExists)
                    return new ApiResponse(400, $"Row already exists at index {updateDto.RowIndex.Value}");

                entity.RowIndex = updateDto.RowIndex.Value;
            }

            _mapper.Map(updateDto, entity);
            entity.UpdatedDate = DateTime.UtcNow;
            _unitOfWork.FormSubmissionGridRowRepository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            var updatedEntity = await _unitOfWork.FormSubmissionGridRowRepository.GetByIdAsync(id);
            var rowDto = _mapper.Map<FormSubmissionGridRowDto>(updatedEntity);

            return new ApiResponse(200, "Grid row updated successfully", rowDto);
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var result = await base.DeleteAsync(id);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> DeleteBySubmissionAndGridAsync(int submissionId, int gridId)
        {
            var rows = await _unitOfWork.FormSubmissionGridRowRepository.GetBySubmissionAndGridAsync(submissionId, gridId);
            if (!rows.Any())
                return new ApiResponse(404, "No grid rows found");

            // Soft Delete
            foreach (var row in rows)
            {
                row.IsDeleted = true;
                row.DeletedDate = DateTime.UtcNow;
                row.IsActive = false;
                _unitOfWork.FormSubmissionGridRowRepository.Update(row);
            }

            await _unitOfWork.CompleteAsyn();

            return new ApiResponse(200, $"{rows.Count()} grid rows deleted successfully");
        }

        public async Task<ApiResponse> ToggleActiveAsync(int id, bool isActive)
        {
            var result = await base.ToggleActiveAsync(id, isActive);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> ExistsAsync(int id)
        {
            var exists = await _unitOfWork.FormSubmissionGridRowRepository.AnyAsync(r => r.Id == id);
            return new ApiResponse(200, "Grid row existence checked successfully", exists);
        }

        public async Task<ApiResponse> RowExistsAsync(int submissionId, int gridId, int rowIndex)
        {
            var exists = await _unitOfWork.FormSubmissionGridRowRepository
                .RowExistsAsync(submissionId, gridId, rowIndex);
            return new ApiResponse(200, "Row existence checked successfully", exists);
        }

        public async Task<ApiResponse> GetNextRowIndexAsync(int submissionId, int gridId)
        {
            var nextIndex = await _unitOfWork.FormSubmissionGridRowRepository
                .GetNextRowIndexAsync(submissionId, gridId);
            return new ApiResponse(200, "Next row index retrieved successfully", nextIndex);
        }

        public async Task<ApiResponse> GetRowCountBySubmissionAsync(int submissionId)
        {
            var count = await _unitOfWork.FormSubmissionGridRowRepository
                .GetRowCountBySubmissionAsync(submissionId);
            return new ApiResponse(200, "Row count retrieved successfully", count);
        }

        public async Task<ApiResponse> GetRowCountByGridAsync(int gridId)
        {
            var count = await _unitOfWork.FormSubmissionGridRowRepository
                .GetRowCountByGridAsync(gridId);
            return new ApiResponse(200, "Row count retrieved successfully", count);
        }

        public async Task<ApiResponse> GetByFormBuilderIdAsync(int formBuilderId)
        {
            var rows = await _unitOfWork.FormSubmissionGridRowRepository
                .GetByFormBuilderIdAsync(formBuilderId);
            var rowDtos = _mapper.Map<IEnumerable<FormSubmissionGridRowDto>>(rows);
            return new ApiResponse(200, "Grid rows retrieved successfully", rowDtos);
        }

        public async Task<ApiResponse> ReorderRowsAsync(int submissionId, int gridId)
        {
            var rows = await _unitOfWork.FormSubmissionGridRowRepository
                .GetBySubmissionAndGridAsync(submissionId, gridId);

            var orderedRows = rows.OrderBy(r => r.RowIndex).ToList();

            for (int i = 0; i < orderedRows.Count; i++)
            {
                orderedRows[i].RowIndex = i;
                orderedRows[i].UpdatedDate = DateTime.UtcNow;
                _unitOfWork.FormSubmissionGridRowRepository.Update(orderedRows[i]);
            }

            await _unitOfWork.CompleteAsyn();

            return new ApiResponse(200, "Rows reordered successfully");
        }

        public async Task<ApiResponse> SaveBulkGridDataAsync(BulkSaveGridDataDto bulkDto)
        {
            if (bulkDto == null)
                return new ApiResponse(400, "DTO is required");

            // التحقق من Submission
            var submission = await _unitOfWork.FormSubmissionsRepository.GetByIdAsync(bulkDto.SubmissionId);
            if (submission == null)
                return new ApiResponse(404, "Submission not found");

            // التحقق من Grid
            var grid = await _unitOfWork.FormGridRepository.GetByIdAsync(bulkDto.GridId);
            if (grid == null)
                return new ApiResponse(404, "Grid not found");

            // حذف البيانات القديمة
            await DeleteBySubmissionAndGridAsync(bulkDto.SubmissionId, bulkDto.GridId);

            // حفظ البيانات الجديدة
            var savedRows = new List<FormSubmissionGridRowDto>();

            foreach (var rowDto in bulkDto.Rows)
            {
                // إنشاء Row
                var createRowDto = new CreateFormSubmissionGridRowDto
                {
                    SubmissionId = bulkDto.SubmissionId,
                    GridId = bulkDto.GridId,
                    RowIndex = rowDto.RowIndex,
                    IsActive = true
                };

                var rowResult = await CreateAsync(createRowDto);
                if (rowResult.StatusCode != 201 && rowResult.StatusCode != 200)
                    return rowResult;

                var rowData = rowResult.Data as FormSubmissionGridRowDto;
                if (rowData == null) continue;

                // حفظ Cells
                if (rowDto.Cells != null && rowDto.Cells.Any())
                {
                    foreach (var cellDto in rowDto.Cells)
                    {
                        var createCellDto = new CreateFormSubmissionGridCellDto
                        {
                            RowId = rowData.Id,
                            ColumnId = cellDto.ColumnId,
                            ValueString = cellDto.ValueString,
                            ValueNumber = cellDto.ValueNumber,
                            ValueDate = cellDto.ValueDate,
                            ValueBool = cellDto.ValueBool,
                            ValueJson = cellDto.ValueJson
                        };

                        var cellResult = await _formSubmissionGridCellService.CreateAsync(createCellDto);
                        if (cellResult.StatusCode != 201 && cellResult.StatusCode != 200)
                        {
                            // Log the error but continue - cell creation failed
                            // يمكن أن يفشل إذا كان ColumnId غير موجود
                            System.Diagnostics.Debug.WriteLine($"Failed to create cell: RowId={rowData.Id}, ColumnId={cellDto.ColumnId}, Error={cellResult.Message}");
                        }
                    }
                }

                savedRows.Add(rowData);
            }

            return new ApiResponse(200, "Grid data saved successfully", savedRows);
        }

        public async Task<ApiResponse> GetCompleteGridDataAsync(int submissionId, int gridId)
        {
            var rows = await _unitOfWork.FormSubmissionGridRowRepository
                .GetBySubmissionAndGridAsync(submissionId, gridId);

            var rowsWithCells = new List<FormSubmissionGridRowWithCellsDto>();

            foreach (var row in rows.Where(r => r.IsActive))
            {
                var rowDto = _mapper.Map<FormSubmissionGridRowDto>(row);
                var cells = await _unitOfWork.FormSubmissionGridCellRepository.GetByRowIdAsync(row.Id);
                var cellDtos = _mapper.Map<List<FormSubmissionGridCellDto>>(cells);

                rowsWithCells.Add(new FormSubmissionGridRowWithCellsDto
                {
                    Id = rowDto.Id,
                    SubmissionId = rowDto.SubmissionId,
                    GridId = rowDto.GridId,
                    RowIndex = rowDto.RowIndex,
                    IsActive = rowDto.IsActive,
                    CreatedDate = rowDto.CreatedDate,
                    UpdatedDate = rowDto.UpdatedDate,
                    SubmissionNumber = rowDto.SubmissionNumber,
                    GridName = rowDto.GridName,
                    Cells = cellDtos
                });
            }

            return new ApiResponse(200, "Complete grid data retrieved successfully", rowsWithCells);
        }

        public async Task<ApiResponse> ValidateGridDataAsync(BulkSaveGridDataDto bulkDto)
        {
            if (bulkDto == null)
                return new ApiResponse(400, "DTO is required");

            var errors = new List<GridValidationErrorDto>();
            var warnings = new List<GridValidationWarningDto>();

            // التحقق من Grid
            var grid = await _unitOfWork.FormGridRepository.GetByIdAsync(bulkDto.GridId);
            if (grid == null)
            {
                errors.Add(new GridValidationErrorDto
                {
                    Field = "GridId",
                    Message = "Grid not found"
                });
                return new ApiResponse(400, "Validation failed", new GridValidationResultDto
                {
                    IsValid = false,
                    Errors = errors,
                    Warnings = warnings
                });
            }

            // التحقق من MinRows
            if (grid.MinRows.HasValue && bulkDto.Rows.Count < grid.MinRows.Value)
            {
                errors.Add(new GridValidationErrorDto
                {
                    Field = "Rows",
                    Message = $"Minimum {grid.MinRows.Value} row(s) required. Current: {bulkDto.Rows.Count}",
                    RowIndex = -1
                });
            }

            // التحقق من MaxRows
            if (grid.MaxRows.HasValue && bulkDto.Rows.Count > grid.MaxRows.Value)
            {
                errors.Add(new GridValidationErrorDto
                {
                    Field = "Rows",
                    Message = $"Maximum {grid.MaxRows.Value} row(s) allowed. Current: {bulkDto.Rows.Count}",
                    RowIndex = -1
                });
            }

            // جلب Columns
            var columns = await _unitOfWork.FormGridColumnRepository.GetByGridIdAsync(bulkDto.GridId);
            var requiredColumns = columns.Where(c => c.IsMandatory && c.IsActive).ToList();
            var readOnlyColumns = columns.Where(c => c.IsReadOnly && c.IsActive).ToList();

            // التحقق من كل Row
            for (int rowIndex = 0; rowIndex < bulkDto.Rows.Count; rowIndex++)
            {
                var row = bulkDto.Rows[rowIndex];

                // التحقق من Required Columns
                foreach (var column in requiredColumns)
                {
                    var cell = row.Cells?.FirstOrDefault(c => c.ColumnId == column.Id);
                    if (cell == null || IsCellEmpty(cell, column))
                    {
                        errors.Add(new GridValidationErrorDto
                        {
                            Field = column.ColumnName,
                            Message = $"{column.ColumnName} is required",
                            RowIndex = rowIndex,
                            ColumnId = column.Id
                        });
                    }
                }

                // التحقق من نوع البيانات و Read-only columns
                if (row.Cells != null)
                {
                    foreach (var cell in row.Cells)
                    {
                        var column = columns.FirstOrDefault(c => c.Id == cell.ColumnId);
                        if (column != null)
                        {
                            // التحقق من Read-only columns
                            if (column.IsReadOnly)
                            {
                                // Check if this is an existing submission - if so, verify the value hasn't changed
                                // For new submissions, read-only columns can be set initially but not modified later
                                var existingRows = await _unitOfWork.FormSubmissionGridRowRepository
                                    .GetBySubmissionAndGridAsync(bulkDto.SubmissionId, bulkDto.GridId);
                                
                                if (existingRows.Any())
                                {
                                    // Try to find existing row by RowIndex
                                    var existingRow = existingRows.FirstOrDefault(r => r.RowIndex == row.RowIndex);
                                    
                                    if (existingRow != null)
                                    {
                                        // This is an update - check if read-only column value changed
                                        var existingCell = await _unitOfWork.FormSubmissionGridCellRepository
                                            .GetByRowAndColumnAsync(existingRow.Id, column.Id);
                                        
                                        if (existingCell != null)
                                        {
                                            var existingValue = GetCellValueAsString(existingCell);
                                            var newValue = GetCellValueAsString(cell, column);
                                            
                                            if (existingValue != newValue)
                                            {
                                                errors.Add(new GridValidationErrorDto
                                                {
                                                    Field = column.ColumnName,
                                                    Message = $"{column.ColumnName} is read-only and cannot be modified",
                                                    RowIndex = rowIndex,
                                                    ColumnId = column.Id
                                                });
                                            }
                                        }
                                    }
                                }
                            }

                            // التحقق من نوع البيانات
                            var validationResult = ValidateCellValue(cell, column);
                            if (!validationResult.IsValid)
                            {
                                errors.AddRange(validationResult.Errors.Select(e => new GridValidationErrorDto
                                {
                                    Field = column.ColumnName,
                                    Message = e,
                                    RowIndex = rowIndex,
                                    ColumnId = column.Id
                                }));
                            }
                        }
                    }
                }
            }

            return new ApiResponse(200, "Validation completed", new GridValidationResultDto
            {
                IsValid = errors.Count == 0,
                Errors = errors,
                Warnings = warnings
            });
        }

        private bool IsCellEmpty(SaveFormSubmissionGridCellDto cell, FORM_GRID_COLUMNS column)
        {
            if (column.FieldTypeId == null) return true;

            var dataType = column.DataType?.ToLower() ?? "";

            // التحقق حسب نوع البيانات
            if (dataType.Contains("string") || dataType.Contains("text"))
                return string.IsNullOrWhiteSpace(cell.ValueString);

            if (dataType.Contains("number") || dataType.Contains("decimal") || dataType.Contains("int"))
                return !cell.ValueNumber.HasValue;

            if (dataType.Contains("date") || dataType.Contains("datetime"))
                return !cell.ValueDate.HasValue;

            if (dataType.Contains("boolean") || dataType.Contains("bool"))
                return !cell.ValueBool.HasValue;

            return true;
        }

        private (bool IsValid, List<string> Errors) ValidateCellValue(SaveFormSubmissionGridCellDto cell, FORM_GRID_COLUMNS column)
        {
            var errors = new List<string>();
            var dataType = column.DataType?.ToLower() ?? "";

            // التحقق من نوع البيانات
            if (dataType.Contains("number") || dataType.Contains("decimal") || dataType.Contains("int"))
            {
                if (!cell.ValueNumber.HasValue && !string.IsNullOrWhiteSpace(cell.ValueString))
                {
                    if (!decimal.TryParse(cell.ValueString, out _))
                    {
                        errors.Add("Value must be a valid number");
                    }
                }
            }
            else if (dataType.Contains("date") || dataType.Contains("datetime"))
            {
                if (!cell.ValueDate.HasValue && !string.IsNullOrWhiteSpace(cell.ValueString))
                {
                    if (!DateTime.TryParse(cell.ValueString, out _))
                    {
                        errors.Add("Value must be a valid date");
                    }
                }
            }
            else if (dataType.Contains("boolean") || dataType.Contains("bool"))
            {
                if (!cell.ValueBool.HasValue && !string.IsNullOrWhiteSpace(cell.ValueString))
                {
                    if (!bool.TryParse(cell.ValueString, out _))
                    {
                        errors.Add("Value must be a valid boolean");
                    }
                }
            }

            return (errors.Count == 0, errors);
        }

        private string GetCellValueAsString(FORM_SUBMISSION_GRID_CELLS cell)
        {
            if (!string.IsNullOrWhiteSpace(cell.ValueString))
                return cell.ValueString;
            if (cell.ValueNumber.HasValue)
                return cell.ValueNumber.Value.ToString();
            if (cell.ValueDate.HasValue)
                return cell.ValueDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
            if (cell.ValueBool.HasValue)
                return cell.ValueBool.Value.ToString();
            if (!string.IsNullOrWhiteSpace(cell.ValueJson))
                return cell.ValueJson;
            return string.Empty;
        }

        private string GetCellValueAsString(SaveFormSubmissionGridCellDto cell, FORM_GRID_COLUMNS column)
        {
            if (!string.IsNullOrWhiteSpace(cell.ValueString))
                return cell.ValueString;
            if (cell.ValueNumber.HasValue)
                return cell.ValueNumber.Value.ToString();
            if (cell.ValueDate.HasValue)
                return cell.ValueDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
            if (cell.ValueBool.HasValue)
                return cell.ValueBool.Value.ToString();
            if (!string.IsNullOrWhiteSpace(cell.ValueJson))
                return cell.ValueJson;
            return string.Empty;
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
