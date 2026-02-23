using AutoMapper;
using FormBuilder.API.Models.DTOs;
using FormBuilder.Application.DTOS;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.Domain.Interfaces.Services;
using formBuilder.Domian.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Services.Services.Email
{
    public class SmtpConfigsService : ISmtpConfigsService
    {
        private readonly IunitOfwork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISecretProtector _protector;
        private readonly ILogger<SmtpConfigsService> _logger;

        public SmtpConfigsService(
            IunitOfwork unitOfWork,
            IMapper mapper,
            ISecretProtector protector,
            ILogger<SmtpConfigsService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _protector = protector;
            _logger = logger;
        }

        public async Task<ServiceResult<IEnumerable<SmtpConfigDto>>> GetAllAsync(bool includeInactive = true)
        {
            try
            {
                var query = _unitOfWork.AppDbContext.Set<SMTP_CONFIGS>()
                    .AsNoTracking()
                    .Where(x => !x.IsDeleted);

                if (!includeInactive)
                {
                    query = query.Where(x => x.IsActive);
                }

                var list = await query
                    .OrderByDescending(x => x.IsActive)
                    .ThenBy(x => x.Name)
                    .ToListAsync();

                return ServiceResult<IEnumerable<SmtpConfigDto>>.Ok(_mapper.Map<IEnumerable<SmtpConfigDto>>(list));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving SMTP configs");
                return ServiceResult<IEnumerable<SmtpConfigDto>>.Error("Error retrieving SMTP configs");
            }
        }

        public async Task<ServiceResult<SmtpConfigDto>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.AppDbContext.Set<SMTP_CONFIGS>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

                if (entity == null) return ServiceResult<SmtpConfigDto>.NotFound("SMTP config not found");
                return ServiceResult<SmtpConfigDto>.Ok(_mapper.Map<SmtpConfigDto>(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving SMTP config {Id}", id);
                return ServiceResult<SmtpConfigDto>.Error("Error retrieving SMTP config");
            }
        }

        public async Task<ServiceResult<SmtpConfigDto>> CreateAsync(CreateSmtpConfigDto createDto)
        {
            try
            {
                if (createDto == null) return ServiceResult<SmtpConfigDto>.BadRequest("DTO is required");

                var name = createDto.Name?.Trim();
                if (string.IsNullOrWhiteSpace(name)) return ServiceResult<SmtpConfigDto>.BadRequest("Name is required");

                var exists = await _unitOfWork.AppDbContext.Set<SMTP_CONFIGS>()
                    .AnyAsync(x => !x.IsDeleted && x.Name == name);
                if (exists) return ServiceResult<SmtpConfigDto>.BadRequest($"SMTP config name '{name}' already exists");

                var entity = new SMTP_CONFIGS
                {
                    Name = name,
                    Host = createDto.Host.Trim(),
                    Port = createDto.Port,
                    UseSsl = createDto.UseSsl,
                    UserName = createDto.UserName.Trim(),
                    PasswordEncrypted = _protector.Protect(createDto.Password),
                    FromEmail = createDto.FromEmail.Trim(),
                    FromDisplayName = createDto.FromDisplayName.Trim(),
                    IsActive = createDto.IsActive,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                _unitOfWork.Repositary<SMTP_CONFIGS>().Add(entity);
                await _unitOfWork.CompleteAsyn();

                return ServiceResult<SmtpConfigDto>.Ok(_mapper.Map<SmtpConfigDto>(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating SMTP config");
                return ServiceResult<SmtpConfigDto>.Error("Error creating SMTP config");
            }
        }

        public async Task<ServiceResult<SmtpConfigDto>> UpdateAsync(int id, UpdateSmtpConfigDto updateDto)
        {
            try
            {
                if (updateDto == null) return ServiceResult<SmtpConfigDto>.BadRequest("DTO is required");

                var entity = await _unitOfWork.AppDbContext.Set<SMTP_CONFIGS>()
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

                if (entity == null) return ServiceResult<SmtpConfigDto>.NotFound("SMTP config not found");

                if (!string.IsNullOrWhiteSpace(updateDto.Name))
                {
                    var name = updateDto.Name.Trim();
                    var exists = await _unitOfWork.AppDbContext.Set<SMTP_CONFIGS>()
                        .AnyAsync(x => !x.IsDeleted && x.Id != id && x.Name == name);
                    if (exists) return ServiceResult<SmtpConfigDto>.BadRequest($"SMTP config name '{name}' already exists");
                    entity.Name = name;
                }

                if (!string.IsNullOrWhiteSpace(updateDto.Host)) entity.Host = updateDto.Host.Trim();
                if (updateDto.Port.HasValue) entity.Port = updateDto.Port.Value;
                if (updateDto.UseSsl.HasValue) entity.UseSsl = updateDto.UseSsl.Value;
                if (!string.IsNullOrWhiteSpace(updateDto.UserName)) entity.UserName = updateDto.UserName.Trim();
                if (!string.IsNullOrWhiteSpace(updateDto.Password)) entity.PasswordEncrypted = _protector.Protect(updateDto.Password);
                if (!string.IsNullOrWhiteSpace(updateDto.FromEmail)) entity.FromEmail = updateDto.FromEmail.Trim();
                if (!string.IsNullOrWhiteSpace(updateDto.FromDisplayName)) entity.FromDisplayName = updateDto.FromDisplayName.Trim();
                if (updateDto.IsActive.HasValue) entity.IsActive = updateDto.IsActive.Value;

                entity.UpdatedDate = DateTime.UtcNow;

                _unitOfWork.Repositary<SMTP_CONFIGS>().Update(entity);
                await _unitOfWork.CompleteAsyn();

                return ServiceResult<SmtpConfigDto>.Ok(_mapper.Map<SmtpConfigDto>(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating SMTP config {Id}", id);
                return ServiceResult<SmtpConfigDto>.Error("Error updating SMTP config");
            }
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.AppDbContext.Set<SMTP_CONFIGS>()
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

                if (entity == null) return ServiceResult<bool>.NotFound("SMTP config not found");

                // Prevent delete if used by templates
                var used = await _unitOfWork.AppDbContext.Set<EMAIL_TEMPLATES>()
                    .AnyAsync(t => !t.IsDeleted && t.SmtpConfigId == id);
                if (used) return ServiceResult<bool>.BadRequest("Cannot delete SMTP config because it is used by email templates");

                entity.IsDeleted = true;
                entity.IsActive = false;
                entity.DeletedDate = DateTime.UtcNow;
                entity.UpdatedDate = DateTime.UtcNow;

                _unitOfWork.Repositary<SMTP_CONFIGS>().Update(entity);
                await _unitOfWork.CompleteAsyn();

                return ServiceResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting SMTP config {Id}", id);
                return ServiceResult<bool>.Error("Error deleting SMTP config");
            }
        }

        public async Task<ServiceResult<bool>> ToggleActiveAsync(int id, bool isActive)
        {
            try
            {
                var entity = await _unitOfWork.AppDbContext.Set<SMTP_CONFIGS>()
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

                if (entity == null) return ServiceResult<bool>.NotFound("SMTP config not found");

                entity.IsActive = isActive;
                entity.UpdatedDate = DateTime.UtcNow;

                _unitOfWork.Repositary<SMTP_CONFIGS>().Update(entity);
                await _unitOfWork.CompleteAsyn();

                return ServiceResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling SMTP config {Id} active={IsActive}", id, isActive);
                return ServiceResult<bool>.Error("Error updating SMTP config status");
            }
        }
    }
}


