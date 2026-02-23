using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Application.DTOs.ApprovalWorkflow;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.API.Models;
using FormBuilder.Services.Services.Base;
using FormBuilder.Application.DTOS;
using FormBuilder.Core.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Services
{
    public class ApprovalStageAssigneesService : BaseService<APPROVAL_STAGE_ASSIGNEES, ApprovalStageAssigneesDto, ApprovalStageAssigneesCreateDto, ApprovalStageAssigneesUpdateDto>, IApprovalStageAssigneesService
    {
        private readonly AkhmanageItContext _identityContext;
        private readonly IConfiguration _configuration;

        public ApprovalStageAssigneesService(IunitOfwork unitOfWork, IMapper mapper, AkhmanageItContext identityContext, IConfiguration configuration) : base(unitOfWork, mapper)
        {
            _identityContext = identityContext;
            _configuration = configuration;
        }

        protected override IBaseRepository<APPROVAL_STAGE_ASSIGNEES> Repository => _unitOfWork.ApprovalStageAssigneesRepository;

        public async Task<ApiResponse> GetByStageIdAsync(int stageId)
        {
            var assignees = await _unitOfWork.ApprovalStageAssigneesRepository.GetByStageIdAsync(stageId);
            var dtos = _mapper.Map<IEnumerable<ApprovalStageAssigneesDto>>(assignees);
            
            // Populate RoleName and UserName for each assignee
            foreach (var dto in dtos)
            {
                await PopulateNamesAsync(dto);
            }
            
            return new ApiResponse(200, "Success", dtos);
        }

        public async Task<ApiResponse> GetByIdAsync(int id)
        {
            var result = await base.GetByIdAsync(id);
            if (result.Success && result.Data != null)
            {
                await PopulateNamesAsync(result.Data);
            }
            return ConvertToApiResponse(result);
        }

        public new async Task<ApiResponse> CreateAsync(ApprovalStageAssigneesCreateDto dto)
        {
            // Validation: StageId must be valid
            var stage = await _unitOfWork.ApprovalStageRepository.GetByIdAsync(dto.StageId);
            if (stage == null)
            {
                return new ApiResponse(404, $"Stage with Id {dto.StageId} not found");
            }

            // Validation: Either RoleId or UserId must be provided
            if (string.IsNullOrWhiteSpace(dto.RoleId) && string.IsNullOrWhiteSpace(dto.UserId))
            {
                return new ApiResponse(400, "Either RoleId or UserId must be provided");
            }

            // Validation: Check for duplicate UserId in the same Stage (only check non-deleted records)
            if (!string.IsNullOrWhiteSpace(dto.UserId))
            {
                var duplicateExists = await Repository.AnyAsync(a => 
                    a.StageId == dto.StageId && 
                    a.UserId == dto.UserId && 
                    !a.IsDeleted);
                    
                if (duplicateExists)
                {
                    return new ApiResponse(400, "This user is already assigned to this stage");
                }
            }

            APPROVAL_STAGE_ASSIGNEES entity;
            string roleName = null;
            string userName = null;

            // Handle User assignment
            if (!string.IsNullOrWhiteSpace(dto.UserId))
            {
                if (!int.TryParse(dto.UserId, out int userIdInt))
                {
                    return new ApiResponse(400, "Invalid UserId format");
                }

                var user = await _identityContext.TblUsers
                    .Include(u => u.TblUserGroupUsers)
                        .ThenInclude(ugu => ugu.IdUserGroupNavigation)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == userIdInt);

                if (user == null)
                {
                    return new ApiResponse(404, $"User with Id {userIdInt} not found");
                }

                if (!user.IsActive)
                {
                    return new ApiResponse(400, $"User with Id {userIdInt} is inactive");
                }

                // Extract RoleId from user's role if not provided
                string roleIdToUse = dto.RoleId;
                if (string.IsNullOrWhiteSpace(roleIdToUse))
                {
                    // Find active role or use default role
                    TblUserGroup userGroup = null;

                    if (user.TblUserGroupUsers != null && user.TblUserGroupUsers.Any())
                    {
                        userGroup = user.TblUserGroupUsers
                            .Where(ugu => ugu.IdUserGroupNavigation != null && ugu.IdUserGroupNavigation.IsActive)
                            .Select(ugu => ugu.IdUserGroupNavigation)
                            .FirstOrDefault();
                    }

                    // If no active role found, use default role
                    if (userGroup == null)
                    {
                        var defaultRoleName = _configuration["ApprovalWorkflow:DefaultRoleName"];
                        var defaultRoleId = _configuration["ApprovalWorkflow:DefaultRoleId"];
                        
                        if (!string.IsNullOrWhiteSpace(defaultRoleName) || !string.IsNullOrWhiteSpace(defaultRoleId))
                        {
                            userGroup = await GetDefaultRoleAsync();
                        }

                        if (userGroup == null)
                        {
                            userGroup = await GetDefaultRoleAsync();
                            if (userGroup == null)
                            {
                                return new ApiResponse(400, $"User '{user.Name ?? user.Username}' (Id: {userIdInt}) does not have an active role and no default role configured.");
                            }
                        }
                    }

                    roleIdToUse = userGroup.Id.ToString();
                }

                // Create user assignment
                entity = new APPROVAL_STAGE_ASSIGNEES
                {
                    StageId = dto.StageId,
                    UserId = dto.UserId,
                    RoleId = roleIdToUse, // Extract RoleId from User's role or use provided RoleId
                    IsActive = dto.IsActive,
                    CreatedDate = DateTime.UtcNow
                };

                userName = user.Name;
            }
            // Handle Role assignment
            else if (!string.IsNullOrWhiteSpace(dto.RoleId))
            {
                if (!int.TryParse(dto.RoleId, out int roleIdInt))
                {
                    return new ApiResponse(400, "Invalid RoleId format");
                }

                var role = await _identityContext.TblUserGroups
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Id == roleIdInt);

                if (role == null)
                {
                    return new ApiResponse(404, $"Role with Id {roleIdInt} not found");
                }

                if (!role.IsActive)
                {
                    return new ApiResponse(400, $"Role with Id {roleIdInt} is inactive");
                }

                // Create role-based assignment
                entity = new APPROVAL_STAGE_ASSIGNEES
                {
                    StageId = dto.StageId,
                    RoleId = dto.RoleId,
                    UserId = null, // NULL for role-based assignment
                    IsActive = dto.IsActive,
                    CreatedDate = DateTime.UtcNow
                };

                roleName = role.Name;
            }
            else
            {
                return new ApiResponse(400, "Either RoleId or UserId must be provided");
            }

            Repository.Add(entity);
            await _unitOfWork.CompleteAsyn();

            // Map to DTO and populate names
            var dtoResult = _mapper.Map<ApprovalStageAssigneesDto>(entity);
            dtoResult.RoleName = roleName;
            dtoResult.UserName = userName;
            dtoResult.StageName = stage.StageName;

            return new ApiResponse(200, "Success", dtoResult);
        }

        public new async Task<ApiResponse> UpdateAsync(int id, ApprovalStageAssigneesUpdateDto dto)
        {
            // Get existing entity
            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id, asNoTracking: false);
            if (entity == null)
            {
                return new ApiResponse(404, $"Assignee with Id {id} not found");
            }

            // Update StageId if provided
            if (dto.StageId.HasValue)
            {
                var stage = await _unitOfWork.ApprovalStageRepository.GetByIdAsync(dto.StageId.Value);
                if (stage == null)
                {
                    return new ApiResponse(404, $"Stage with Id {dto.StageId.Value} not found");
                }
                entity.StageId = dto.StageId.Value;
            }

            // Update UserId and extract RoleId if UserId is provided
            if (!string.IsNullOrWhiteSpace(dto.UserId))
            {
                // Validation: Check for duplicate UserId if it's being changed (only check non-deleted records)
                if (dto.UserId != entity.UserId)
                {
                    var duplicateExists = await Repository.AnyAsync(a => 
                        a.StageId == entity.StageId && 
                        a.UserId == dto.UserId && 
                        a.Id != id && 
                        !a.IsDeleted);
                        
                    if (duplicateExists)
                    {
                        return new ApiResponse(400, "This user is already assigned to this stage");
                    }
                }

                if (!int.TryParse(dto.UserId, out int userIdInt))
                {
                    return new ApiResponse(400, "Invalid UserId format");
                }

                var user = await _identityContext.TblUsers
                    .Include(u => u.TblUserGroupUsers)
                        .ThenInclude(ugu => ugu.IdUserGroupNavigation)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == userIdInt);

                if (user == null)
                {
                    return new ApiResponse(404, $"User with Id {userIdInt} not found");
                }

                if (!user.IsActive)
                {
                    return new ApiResponse(400, $"User with Id {userIdInt} is inactive");
                }

                // Find active role or use default role
                TblUserGroup userGroup = null;

                if (user.TblUserGroupUsers != null && user.TblUserGroupUsers.Any())
                {
                    userGroup = user.TblUserGroupUsers
                        .Where(ugu => ugu.IdUserGroupNavigation != null && ugu.IdUserGroupNavigation.IsActive)
                        .Select(ugu => ugu.IdUserGroupNavigation)
                        .FirstOrDefault();
                }

                // If no active role found, use default role
                if (userGroup == null)
                {
                    var defaultRoleName = _configuration["ApprovalWorkflow:DefaultRoleName"];
                    var defaultRoleId = _configuration["ApprovalWorkflow:DefaultRoleId"];
                    
                    if (!string.IsNullOrWhiteSpace(defaultRoleName) || !string.IsNullOrWhiteSpace(defaultRoleId))
                    {
                        userGroup = await GetDefaultRoleAsync();
                    }

                    if (userGroup == null)
                    {
                        userGroup = await GetDefaultRoleAsync();
                        if (userGroup == null)
                        {
                            return new ApiResponse(400, $"User '{user.Name ?? user.Username}' (Id: {userIdInt}) does not have an active role and no default role configured.");
                        }
                    }
                }

                entity.UserId = dto.UserId;
                entity.RoleId = userGroup.Id.ToString(); // Extract RoleId from User's role or default role
            }

            // Update IsActive if provided
            if (dto.IsActive.HasValue && dto.IsActive.Value != entity.IsActive)
            {
                // If deactivating, validate minimum required assignees
                if (!dto.IsActive.Value && entity.IsActive)
                {
                    var stage = await _unitOfWork.ApprovalStageRepository.GetByIdAsync(entity.StageId);
                    if (stage != null && stage.MinimumRequiredAssignees.HasValue)
                    {
                        var activeAssignees = await _unitOfWork.ApprovalStageAssigneesRepository.GetByStageIdAsync(entity.StageId);
                        var activeCount = activeAssignees.Count();
                        
                        // If deactivating this assignee would violate the minimum requirement
                        if (activeCount <= stage.MinimumRequiredAssignees.Value)
                        {
                            return new ApiResponse(400, 
                                $"Cannot deactivate assignee. Stage requires at least {stage.MinimumRequiredAssignees.Value} active assignee(s). Currently has {activeCount} active assignee(s).");
                        }
                    }
                }
                
                entity.IsActive = dto.IsActive.Value;
            }

            entity.UpdatedDate = DateTime.UtcNow;

            Repository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            // Map to DTO and populate RoleName, UserName, and StageName
            var dtoResult = _mapper.Map<ApprovalStageAssigneesDto>(entity);
            await PopulateNamesAsync(dtoResult);

            return new ApiResponse(200, "Assignee updated successfully", dtoResult);
        }

        public new async Task<ApiResponse> DeleteAsync(int id)
        {
            // Use Repository.SingleOrDefaultAsync directly (without IsDeleted filter) to get entity even if already deleted
            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id, asNoTracking: false);
            if (entity == null)
            {
                return new ApiResponse(404, "Stage assignee not found");
            }

            // Check if already deleted
            if (entity.IsDeleted)
            {
                return new ApiResponse(200, "Stage assignee is already deleted");
            }

            // Validate minimum required assignees
            var stage = await _unitOfWork.ApprovalStageRepository.GetByIdAsync(entity.StageId);
            if (stage != null && stage.MinimumRequiredAssignees.HasValue)
            {
                var activeAssignees = await _unitOfWork.ApprovalStageAssigneesRepository.GetByStageIdAsync(entity.StageId);
                var activeCount = activeAssignees.Count();
                
                // If deleting this assignee would violate the minimum requirement
                if (activeCount <= stage.MinimumRequiredAssignees.Value)
                {
                    return new ApiResponse(400, 
                        $"Cannot delete assignee. Stage requires at least {stage.MinimumRequiredAssignees.Value} active assignee(s). Currently has {activeCount} active assignee(s).");
                }
            }

            // Soft Delete - Always use soft delete
            entity.IsDeleted = true;
            entity.DeletedDate = DateTime.UtcNow;
            entity.IsActive = false;
            entity.UpdatedDate = DateTime.UtcNow;
            
            // Use repository Update method directly to ensure changes are tracked
            _unitOfWork.ApprovalStageAssigneesRepository.Update(entity);
            await _unitOfWork.CompleteAsyn();
            
            return new ApiResponse(200, "Stage assignee deleted successfully");
        }

        public async Task<ApiResponse> BulkUpdateAsync(StageAssigneesBulkDto dto)
        {
            // Validation: StageId must be valid
            var stage = await _unitOfWork.ApprovalStageRepository.GetByIdAsync(dto.StageId);
            if (stage == null)
            {
                return new ApiResponse(404, $"Stage with Id {dto.StageId} not found");
            }

            // Get existing assignees for the stage
            var existingAssignees = await _unitOfWork.ApprovalStageAssigneesRepository.GetByStageIdAsync(dto.StageId);

            // Soft Delete existing assignees
            foreach (var assignee in existingAssignees)
            {
                assignee.IsDeleted = true;
                assignee.DeletedDate = DateTime.UtcNow;
                assignee.IsActive = false;
                _unitOfWork.ApprovalStageAssigneesRepository.Update(assignee);
            }

            // Add new role-based assignees
            foreach (var roleId in dto.RoleIds.Where(r => !string.IsNullOrWhiteSpace(r)))
            {
                var assignee = new APPROVAL_STAGE_ASSIGNEES
                {
                    StageId = dto.StageId,
                    RoleId = roleId,
                    UserId = null,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };
                _unitOfWork.ApprovalStageAssigneesRepository.Add(assignee);
            }

            // Add new user-based assignees (extract RoleId from UserId)
            var failedUsers = new List<string>();
            foreach (var userId in dto.UserIds.Where(u => !string.IsNullOrWhiteSpace(u)))
            {
                if (int.TryParse(userId, out int userIdInt))
                {
                    var user = await _identityContext.TblUsers
                        .Include(u => u.TblUserGroupUsers)
                            .ThenInclude(ugu => ugu.IdUserGroupNavigation)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => u.Id == userIdInt);

                    if (user == null)
                    {
                        failedUsers.Add($"User Id {userIdInt} not found");
                        continue;
                    }

                    if (!user.IsActive)
                    {
                        failedUsers.Add($"User '{user.Name ?? user.Username}' (Id: {userIdInt}) is inactive");
                        continue;
                    }

                    // Find active role or use default role
                    TblUserGroup userGroup = null;

                    if (user.TblUserGroupUsers != null && user.TblUserGroupUsers.Any())
                    {
                        // Try to find active role
                        userGroup = user.TblUserGroupUsers
                            .Where(ugu => ugu.IdUserGroupNavigation != null && ugu.IdUserGroupNavigation.IsActive)
                            .Select(ugu => ugu.IdUserGroupNavigation)
                            .FirstOrDefault();
                    }

                    // If no active role found, use default role
                    if (userGroup == null)
                    {
                        userGroup = await GetDefaultRoleAsync();
                        if (userGroup == null)
                        {
                            failedUsers.Add($"User '{user.Name ?? user.Username}' (Id: {userIdInt}) has no active roles and no default role configured");
                            continue;
                        }
                    }

                    var assignee = new APPROVAL_STAGE_ASSIGNEES
                    {
                        StageId = dto.StageId,
                        RoleId = userGroup.Id.ToString(), // Extract RoleId from User's role or default role
                        UserId = userId,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow
                    };
                    _unitOfWork.ApprovalStageAssigneesRepository.Add(assignee);
                }
                else
                {
                    failedUsers.Add($"Invalid UserId format: {userId}");
                }
            }

            if (failedUsers.Any())
            {
                return new ApiResponse(400, $"Some users could not be added: {string.Join("; ", failedUsers)}");
            }

            await _unitOfWork.CompleteAsyn();
            return new ApiResponse(200, "Assignees updated successfully");
        }

        private async Task PopulateNamesAsync(ApprovalStageAssigneesDto dto)
        {
            // Populate UserName
            if (!string.IsNullOrWhiteSpace(dto.UserId) && int.TryParse(dto.UserId, out int userIdInt))
            {
                var user = await _identityContext.TblUsers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == userIdInt);
                
                if (user != null)
                {
                    dto.UserName = user.Name;
                }
            }

            // Populate RoleName
            if (!string.IsNullOrWhiteSpace(dto.RoleId) && int.TryParse(dto.RoleId, out int roleIdInt))
            {
                var role = await _identityContext.TblUserGroups
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Id == roleIdInt);
                
                if (role != null)
                {
                    dto.RoleName = role.Name;
                }
            }

            // Populate StageName if not already set
            if (string.IsNullOrWhiteSpace(dto.StageName))
            {
                var stage = await _unitOfWork.ApprovalStageRepository.GetByIdAsync(dto.StageId);
                if (stage != null)
                {
                    dto.StageName = stage.StageName;
                }
            }
        }

        /// <summary>
        /// Gets the default role from configuration or finds the first active role
        /// </summary>
        private async Task<TblUserGroup> GetDefaultRoleAsync()
        {
            // Try to get default role from configuration
            var defaultRoleId = _configuration["ApprovalWorkflow:DefaultRoleId"];
            if (!string.IsNullOrWhiteSpace(defaultRoleId) && int.TryParse(defaultRoleId, out int roleIdInt))
            {
                var role = await _identityContext.TblUserGroups
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Id == roleIdInt && r.IsActive);
                
                if (role != null)
                {
                    return role;
                }
            }

            // Try to get default role by name (case-insensitive)
            var defaultRoleName = _configuration["ApprovalWorkflow:DefaultRoleName"];
            if (!string.IsNullOrWhiteSpace(defaultRoleName))
            {
                // Try exact match first
                var role = await _identityContext.TblUserGroups
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Name == defaultRoleName && r.IsActive);
                
                if (role != null)
                {
                    return role;
                }

                // Try case-insensitive match
                var allActiveRoles = await _identityContext.TblUserGroups
                    .AsNoTracking()
                    .Where(r => r.IsActive)
                    .ToListAsync();

                role = allActiveRoles
                    .FirstOrDefault(r => r.Name.Equals(defaultRoleName, StringComparison.OrdinalIgnoreCase));

                if (role != null)
                {
                    return role;
                }
            }

            // If no default role configured, return the first active role as fallback
            var firstActiveRole = await _identityContext.TblUserGroups
                .AsNoTracking()
                .Where(r => r.IsActive)
                .OrderBy(r => r.Id)
                .FirstOrDefaultAsync();

            return firstActiveRole;
        }

        /// <summary>
        /// Updates all assignees that have null or empty RoleId by extracting RoleId from their User's role
        /// </summary>
        public async Task<ApiResponse> UpdateMissingRoleIdsAsync()
        {
            // Use GetAll() instead of GetAllAsync() to allow tracking changes
            var assigneesWithoutRoleId = await Repository.GetAll(a => 
                !string.IsNullOrWhiteSpace(a.UserId) && 
                (string.IsNullOrWhiteSpace(a.RoleId) || a.RoleId == "null") &&
                !a.IsDeleted).ToListAsync();

            var updatedCount = 0;
            var failedCount = 0;
            var errors = new List<string>();

            foreach (var assignee in assigneesWithoutRoleId)
            {
                try
                {
                    if (!int.TryParse(assignee.UserId, out int userIdInt))
                    {
                        errors.Add($"Invalid UserId format for assignee Id {assignee.Id}: {assignee.UserId}");
                        failedCount++;
                        continue;
                    }

                    var user = await _identityContext.TblUsers
                        .Include(u => u.TblUserGroupUsers)
                            .ThenInclude(ugu => ugu.IdUserGroupNavigation)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => u.Id == userIdInt);

                    if (user == null)
                    {
                        errors.Add($"User with Id {userIdInt} not found for assignee Id {assignee.Id}");
                        failedCount++;
                        continue;
                    }

                    // Find active role or use default role
                    TblUserGroup userGroup = null;

                    if (user.TblUserGroupUsers != null && user.TblUserGroupUsers.Any())
                    {
                        userGroup = user.TblUserGroupUsers
                            .Where(ugu => ugu.IdUserGroupNavigation != null && ugu.IdUserGroupNavigation.IsActive)
                            .Select(ugu => ugu.IdUserGroupNavigation)
                            .FirstOrDefault();
                    }

                    // If no active role found, use default role
                    if (userGroup == null)
                    {
                        userGroup = await GetDefaultRoleAsync();
                        if (userGroup == null)
                        {
                            errors.Add($"User '{user.Name ?? user.Username}' (Id: {userIdInt}) does not have an active role and no default role configured for assignee Id {assignee.Id}");
                            failedCount++;
                            continue;
                        }
                    }

                    // Update the assignee's RoleId
                    assignee.RoleId = userGroup.Id.ToString();
                    assignee.UpdatedDate = DateTime.UtcNow;
                    Repository.Update(assignee);
                    updatedCount++;
                }
                catch (Exception ex)
                {
                    errors.Add($"Error updating assignee Id {assignee.Id}: {ex.Message}");
                    failedCount++;
                }
            }

            if (updatedCount > 0)
            {
                await _unitOfWork.CompleteAsyn();
            }

            return new ApiResponse(200, $"Updated {updatedCount} assignees. {failedCount} failed.", new
            {
                UpdatedCount = updatedCount,
                FailedCount = failedCount,
                Errors = errors
            });
        }

        private ApiResponse ConvertToApiResponse<T>(ServiceResult<T> result)
        {
            if (result.Success)
                return new ApiResponse(result.StatusCode, "Success", result.Data);
            else
                return new ApiResponse(result.StatusCode, result.ErrorMessage);
        }
    }
}

