using AutoMapper;
using formBuilder.Domian.Interfaces;
using FormBuilder.API.DTOs;
using FormBuilder.API.Models;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domain.Interfaces;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Services.Services.Base;
using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.Common;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Services
{
    public class GridColumnOptionsService : BaseService<GRID_COLUMN_OPTIONS, GridColumnOptionDto, CreateGridColumnOptionDto, UpdateGridColumnOptionDto>, IGridColumnOptionsService
    {
        private readonly IunitOfwork _unitOfWork;
        private readonly IGridColumnOptionsRepository _gridColumnOptionsRepository;
        private readonly IStringLocalizer<GridColumnOptionsService>? _localizer;

        public GridColumnOptionsService(IunitOfwork unitOfWork, IMapper mapper, IStringLocalizer<GridColumnOptionsService>? localizer = null)
            : base(unitOfWork, mapper, null)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _gridColumnOptionsRepository = unitOfWork.GridColumnOptionsRepository;
            _localizer = localizer;
        }

        protected override IBaseRepository<GRID_COLUMN_OPTIONS> Repository => _unitOfWork.GridColumnOptionsRepository;

        /// <summary>
        /// Checks if column has Api or LookupTable DataSource (options should not be saved in database)
        /// </summary>
        private async Task<bool> HasExternalDataSourceAsync(int columnId)
        {
            try
            {
                var dataSources = await _unitOfWork.GridColumnDataSourcesRepository.GetActiveByColumnIdAsync(columnId);
                foreach (var dataSource in dataSources)
                {
                    if (string.Equals(dataSource.SourceType, "Api", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(dataSource.SourceType, "LookupTable", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                // If we can't check, assume it's safe to proceed
            }

            return false;
        }

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

        public async Task<ApiResponse> GetByColumnIdAsync(int columnId)
        {
            // If column has Api or LookupTable DataSource, return empty list (options are not stored in database)
            if (await HasExternalDataSourceAsync(columnId))
            {
                return new ApiResponse(200, "Column uses external data source", new List<GridColumnOptionDto>());
            }

            var options = await _gridColumnOptionsRepository.GetByColumnIdAsync(columnId);
            var dtos = _mapper.Map<IEnumerable<GridColumnOptionDto>>(options);
            return new ApiResponse(200, "Grid column options retrieved successfully", dtos);
        }

        public async Task<ApiResponse> GetActiveByColumnIdAsync(int columnId)
        {
            // If column has Api or LookupTable DataSource, return empty list (options are not stored in database)
            if (await HasExternalDataSourceAsync(columnId))
            {
                return new ApiResponse(200, "Column uses external data source", new List<GridColumnOptionDto>());
            }

            var options = await _gridColumnOptionsRepository.GetActiveByColumnIdAsync(columnId);
            var dtos = _mapper.Map<IEnumerable<GridColumnOptionDto>>(options);
            return new ApiResponse(200, "Active grid column options retrieved successfully", dtos);
        }

        public async Task<ApiResponse> CreateAsync(CreateGridColumnOptionDto createDto)
        {
            // Check if column has external data source
            if (await HasExternalDataSourceAsync(createDto.ColumnId))
            {
                return new ApiResponse(400, "Cannot add static options to a column with external data source (API or LookupTable)");
            }

            // Validate column exists
            var column = await _unitOfWork.FormGridColumnRepository.GetByIdAsync(createDto.ColumnId);
            if (column == null)
                return new ApiResponse(404, "Grid column not found");

            var result = await base.CreateAsync(createDto);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> CreateBulkAsync(List<CreateGridColumnOptionDto> createDtos)
        {
            if (createDtos == null || !createDtos.Any())
            {
                var message = _localizer?["GridColumnOptions_NoOptionsProvided"] ?? "No grid column options provided";
                return new ApiResponse(400, message);
            }

            // Validate all column IDs exist
            var columnIds = createDtos.Select(d => d.ColumnId).Distinct().ToList();
            foreach (var columnId in columnIds)
            {
                var columnExists = await _unitOfWork.FormGridColumnRepository.AnyAsync(c => c.Id == columnId);
                if (!columnExists)
                {
                    return new ApiResponse(404, $"Grid column with ID {columnId} not found");
                }

                // Check if column has external data source
                if (await HasExternalDataSourceAsync(columnId))
                {
                    return new ApiResponse(400, $"Cannot add static options to column {columnId} with external data source");
                }
            }

            var entities = _mapper.Map<List<GRID_COLUMN_OPTIONS>>(createDtos);
            foreach (var entity in entities)
            {
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsActive = true;
                Repository.Add(entity);
            }

            await _unitOfWork.CompleteAsyn();

            var dtos = _mapper.Map<IEnumerable<GridColumnOptionDto>>(entities);
            return new ApiResponse(201, "Grid column options created successfully", dtos);
        }

        public async Task<ApiResponse> UpdateAsync(int id, UpdateGridColumnOptionDto updateDto)
        {
            var result = await base.UpdateAsync(id, updateDto);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var result = await base.DeleteAsync(id);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> SoftDeleteAsync(int id)
        {
            var result = await base.SoftDeleteAsync(id);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> ToggleActiveAsync(int id, bool isActive)
        {
            var result = await base.ToggleActiveAsync(id, isActive);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> ExistsAsync(int id)
        {
            var exists = await Repository.AnyAsync(e => e.Id == id);
            return new ApiResponse(200, exists ? "Option exists" : "Option does not exist", exists);
        }

        public async Task<ApiResponse> GetDefaultOptionAsync(int columnId)
        {
            var option = await _gridColumnOptionsRepository.GetDefaultOptionAsync(columnId);
            if (option == null)
                return new ApiResponse(404, "Default option not found");

            var dto = _mapper.Map<GridColumnOptionDto>(option);
            return new ApiResponse(200, "Default option retrieved successfully", dto);
        }

        public async Task<ApiResponse> GetOptionsCountAsync(int columnId)
        {
            var count = await _gridColumnOptionsRepository.GetOptionsCountAsync(columnId);
            return new ApiResponse(200, "Options count retrieved successfully", count);
        }

        public async Task<ApiResponse> ColumnHasOptionsAsync(int columnId)
        {
            var hasOptions = await _gridColumnOptionsRepository.ColumnHasOptionsAsync(columnId);
            return new ApiResponse(200, "Check completed", hasOptions);
        }

        private ApiResponse ConvertToApiResponse<T>(ServiceResult<T> result)
        {
            if (result.Success)
            {
                return new ApiResponse(result.StatusCode, "Operation completed successfully", result.Data);
            }
            else if (result.StatusCode == 404)
            {
                return new ApiResponse(404, result.ErrorMessage ?? "Resource not found");
            }
            else
            {
                return new ApiResponse(result.StatusCode, result.ErrorMessage ?? "Operation failed");
            }
        }
    }
}

