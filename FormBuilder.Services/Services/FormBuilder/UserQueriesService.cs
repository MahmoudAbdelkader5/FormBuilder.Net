using AutoMapper;
using formBuilder.Domian.Interfaces;
using FormBuilder.API.Models;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Core.IServices.FormBuilder;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domian.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Services.Services.FormBuilder
{
    public class UserQueriesService : IUserQueriesService
    {
        private readonly IUserQueriesRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserQueriesService> _logger;
        private readonly IunitOfwork _unitOfWork;

        public UserQueriesService(
            IUserQueriesRepository repository,
            IMapper mapper,
            ILogger<UserQueriesService> logger,
            IunitOfwork unitOfWork)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ApiResponse> GetAllAsync(string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new ApiResponse(400, "User ID is required");
                }

                var queries = await _repository.GetByUserIdAsync(userId);
                var queriesDto = _mapper.Map<List<UserQueryDto>>(queries);
                return new ApiResponse(200, "Queries retrieved successfully", queriesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving queries for user {UserId}", userId);
                return new ApiResponse(500, $"Error retrieving queries: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetByDatabaseAsync(string userId, string databaseName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new ApiResponse(400, "User ID is required");
                }

                if (string.IsNullOrWhiteSpace(databaseName))
                {
                    return new ApiResponse(400, "Database name is required");
                }

                var queries = await _repository.GetByUserIdAndDatabaseAsync(userId, databaseName);
                var queriesDto = _mapper.Map<List<UserQueryDto>>(queries);
                return new ApiResponse(200, "Queries retrieved successfully", queriesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving queries for user {UserId} and database {DatabaseName}", userId, databaseName);
                return new ApiResponse(500, $"Error retrieving queries: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetByIdAsync(int id, string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new ApiResponse(400, "User ID is required");
                }

                var query = await _repository.SingleOrDefaultAsync(q => q.Id == id && !q.IsDeleted);
                if (query == null)
                {
                    return new ApiResponse(404, "Query not found");
                }

                // Verify that the query belongs to the user
                if (query.UserId != userId)
                {
                    return new ApiResponse(403, "You don't have permission to access this query");
                }

                var queryDto = _mapper.Map<UserQueryDto>(query);
                return new ApiResponse(200, "Query retrieved successfully", queryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving query {QueryId} for user {UserId}", id, userId);
                return new ApiResponse(500, $"Error retrieving query: {ex.Message}");
            }
        }

        public async Task<ApiResponse> CreateAsync(CreateUserQueryDto createDto, string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new ApiResponse(400, "User ID is required");
                }

                if (createDto == null)
                {
                    return new ApiResponse(400, "Query data is required");
                }

                var entity = _mapper.Map<USER_QUERIES>(createDto);
                entity.UserId = userId;
                entity.CreatedByUserId = userId;
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsActive = true;
                entity.IsDeleted = false;

                _repository.Add(entity);
                await _unitOfWork.CompleteAsyn();

                var queryDto = _mapper.Map<UserQueryDto>(entity);
                return new ApiResponse(201, "Query created successfully", queryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating query for user {UserId}", userId);
                return new ApiResponse(500, $"Error creating query: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdateAsync(int id, UpdateUserQueryDto updateDto, string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new ApiResponse(400, "User ID is required");
                }

                var entity = await _repository.SingleOrDefaultAsync(q => q.Id == id && !q.IsDeleted);
                if (entity == null)
                {
                    return new ApiResponse(404, "Query not found");
                }

                // Verify that the query belongs to the user
                if (entity.UserId != userId)
                {
                    return new ApiResponse(403, "You don't have permission to update this query");
                }

                // Update only provided fields
                if (!string.IsNullOrWhiteSpace(updateDto.QueryName))
                {
                    entity.QueryName = updateDto.QueryName;
                }

                if (!string.IsNullOrWhiteSpace(updateDto.DatabaseName))
                {
                    entity.DatabaseName = updateDto.DatabaseName;
                }

                if (!string.IsNullOrWhiteSpace(updateDto.Query))
                {
                    entity.Query = updateDto.Query;
                }

                entity.UpdatedDate = DateTime.UtcNow;

                _repository.Update(entity);
                await _unitOfWork.CompleteAsyn();

                var queryDto = _mapper.Map<UserQueryDto>(entity);
                return new ApiResponse(200, "Query updated successfully", queryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating query {QueryId} for user {UserId}", id, userId);
                return new ApiResponse(500, $"Error updating query: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeleteAsync(int id, string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new ApiResponse(400, "User ID is required");
                }

                var entity = await _repository.SingleOrDefaultAsync(q => q.Id == id && !q.IsDeleted);
                if (entity == null)
                {
                    return new ApiResponse(404, "Query not found");
                }

                // Verify that the query belongs to the user
                if (entity.UserId != userId)
                {
                    return new ApiResponse(403, "You don't have permission to delete this query");
                }

                _repository.Delete(entity);
                await _unitOfWork.CompleteAsyn();

                return new ApiResponse(200, "Query deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting query {QueryId} for user {UserId}", id, userId);
                return new ApiResponse(500, $"Error deleting query: {ex.Message}");
            }
        }

        public async Task<ApiResponse> SoftDeleteAsync(int id, string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new ApiResponse(400, "User ID is required");
                }

                var entity = await _repository.SingleOrDefaultAsync(q => q.Id == id && !q.IsDeleted);
                if (entity == null)
                {
                    return new ApiResponse(404, "Query not found");
                }

                // Verify that the query belongs to the user
                if (entity.UserId != userId)
                {
                    return new ApiResponse(403, "You don't have permission to delete this query");
                }

                entity.IsDeleted = true;
                entity.DeletedDate = DateTime.UtcNow;
                entity.DeletedByUserId = userId;

                _repository.Update(entity);
                await _unitOfWork.CompleteAsyn();

                return new ApiResponse(200, "Query deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error soft deleting query {QueryId} for user {UserId}", id, userId);
                return new ApiResponse(500, $"Error deleting query: {ex.Message}");
            }
        }
    }
}

