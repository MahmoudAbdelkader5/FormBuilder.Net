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
    public class EmailTemplatesService : IEmailTemplatesService
    {
        private readonly IunitOfwork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EmailTemplatesService> _logger;

        public EmailTemplatesService(
            IunitOfwork unitOfWork,
            IMapper mapper,
            ILogger<EmailTemplatesService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResult<IEnumerable<EmailTemplateDto>>> GetAllAsync(int? documentTypeId = null, bool includeInactive = true)
        {
            try
            {
                var query = _unitOfWork.AppDbContext.Set<EMAIL_TEMPLATES>()
                    .AsNoTracking()
                    .Where(x => !x.IsDeleted);

                if (documentTypeId.HasValue && documentTypeId.Value > 0)
                {
                    query = query.Where(x => x.DocumentTypeId == documentTypeId.Value);
                }

                if (!includeInactive)
                {
                    query = query.Where(x => x.IsActive);
                }

                var list = await query
                    .OrderByDescending(x => x.IsDefault)
                    .ThenByDescending(x => x.IsActive)
                    .ThenBy(x => x.TemplateCode)
                    .ThenBy(x => x.TemplateName)
                    .ToListAsync();

                return ServiceResult<IEnumerable<EmailTemplateDto>>.Ok(_mapper.Map<IEnumerable<EmailTemplateDto>>(list));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving email templates");
                return ServiceResult<IEnumerable<EmailTemplateDto>>.Error("Error retrieving email templates");
            }
        }

        public async Task<ServiceResult<EmailTemplateDto>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.AppDbContext.Set<EMAIL_TEMPLATES>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

                if (entity == null) return ServiceResult<EmailTemplateDto>.NotFound("Email template not found");
                return ServiceResult<EmailTemplateDto>.Ok(_mapper.Map<EmailTemplateDto>(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving email template {Id}", id);
                return ServiceResult<EmailTemplateDto>.Error("Error retrieving email template");
            }
        }

        public async Task<ServiceResult<EmailTemplateDto>> CreateAsync(CreateEmailTemplateDto createDto)
        {
            try
            {
                if (createDto == null) return ServiceResult<EmailTemplateDto>.BadRequest("DTO is required");

                // Validate document type exists
                var docTypeExists = await _unitOfWork.AppDbContext.Set<DOCUMENT_TYPES>()
                    .AnyAsync(dt => dt.Id == createDto.DocumentTypeId && !dt.IsDeleted);
                if (!docTypeExists) return ServiceResult<EmailTemplateDto>.BadRequest("Document type not found");

                // Validate smtp config exists
                var smtpExists = await _unitOfWork.AppDbContext.Set<SMTP_CONFIGS>()
                    .AnyAsync(s => s.Id == createDto.SmtpConfigId && !s.IsDeleted);
                if (!smtpExists) return ServiceResult<EmailTemplateDto>.BadRequest("SMTP config not found");

                var code = NormalizeCode(createDto.TemplateCode);
                if (string.IsNullOrWhiteSpace(code)) return ServiceResult<EmailTemplateDto>.BadRequest("TemplateCode is required");

                var entity = new EMAIL_TEMPLATES
                {
                    DocumentTypeId = createDto.DocumentTypeId,
                    TemplateName = createDto.TemplateName.Trim(),
                    TemplateCode = code,
                    SubjectTemplate = createDto.SubjectTemplate ?? string.Empty,
                    BodyTemplateHtml = createDto.BodyTemplateHtml ?? string.Empty,
                    SmtpConfigId = createDto.SmtpConfigId,
                    IsDefault = createDto.IsDefault,
                    IsActive = createDto.IsActive,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                _unitOfWork.Repositary<EMAIL_TEMPLATES>().Add(entity);
                await _unitOfWork.CompleteAsyn();

                // Ensure single default per (DocumentTypeId, TemplateCode)
                if (entity.IsDefault)
                {
                    await UnsetOtherDefaultsAsync(entity.Id, entity.DocumentTypeId, entity.TemplateCode);
                }

                return ServiceResult<EmailTemplateDto>.Ok(_mapper.Map<EmailTemplateDto>(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating email template");
                return ServiceResult<EmailTemplateDto>.Error("Error creating email template");
            }
        }

        public async Task<ServiceResult<EmailTemplateDto>> UpdateAsync(int id, UpdateEmailTemplateDto updateDto)
        {
            try
            {
                if (updateDto == null) return ServiceResult<EmailTemplateDto>.BadRequest("DTO is required");

                var entity = await _unitOfWork.AppDbContext.Set<EMAIL_TEMPLATES>()
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
                if (entity == null) return ServiceResult<EmailTemplateDto>.NotFound("Email template not found");

                if (updateDto.DocumentTypeId.HasValue && updateDto.DocumentTypeId.Value > 0 && updateDto.DocumentTypeId.Value != entity.DocumentTypeId)
                {
                    var docTypeExists = await _unitOfWork.AppDbContext.Set<DOCUMENT_TYPES>()
                        .AnyAsync(dt => dt.Id == updateDto.DocumentTypeId.Value && !dt.IsDeleted);
                    if (!docTypeExists) return ServiceResult<EmailTemplateDto>.BadRequest("Document type not found");
                    entity.DocumentTypeId = updateDto.DocumentTypeId.Value;
                }

                if (!string.IsNullOrWhiteSpace(updateDto.TemplateName))
                    entity.TemplateName = updateDto.TemplateName.Trim();

                if (!string.IsNullOrWhiteSpace(updateDto.TemplateCode))
                    entity.TemplateCode = NormalizeCode(updateDto.TemplateCode);

                if (updateDto.SubjectTemplate != null)
                    entity.SubjectTemplate = updateDto.SubjectTemplate;

                if (updateDto.BodyTemplateHtml != null)
                    entity.BodyTemplateHtml = updateDto.BodyTemplateHtml;

                if (updateDto.SmtpConfigId.HasValue && updateDto.SmtpConfigId.Value > 0 && updateDto.SmtpConfigId.Value != entity.SmtpConfigId)
                {
                    var smtpExists = await _unitOfWork.AppDbContext.Set<SMTP_CONFIGS>()
                        .AnyAsync(s => s.Id == updateDto.SmtpConfigId.Value && !s.IsDeleted);
                    if (!smtpExists) return ServiceResult<EmailTemplateDto>.BadRequest("SMTP config not found");
                    entity.SmtpConfigId = updateDto.SmtpConfigId.Value;
                }

                if (updateDto.IsDefault.HasValue)
                    entity.IsDefault = updateDto.IsDefault.Value;

                if (updateDto.IsActive.HasValue)
                    entity.IsActive = updateDto.IsActive.Value;

                entity.UpdatedDate = DateTime.UtcNow;

                _unitOfWork.Repositary<EMAIL_TEMPLATES>().Update(entity);
                await _unitOfWork.CompleteAsyn();

                if (entity.IsDefault)
                {
                    await UnsetOtherDefaultsAsync(entity.Id, entity.DocumentTypeId, entity.TemplateCode);
                }

                return ServiceResult<EmailTemplateDto>.Ok(_mapper.Map<EmailTemplateDto>(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating email template {Id}", id);
                return ServiceResult<EmailTemplateDto>.Error("Error updating email template");
            }
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.AppDbContext.Set<EMAIL_TEMPLATES>()
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
                if (entity == null) return ServiceResult<bool>.NotFound("Email template not found");

                // Prevent delete if used by alert rules
                var used = await _unitOfWork.AppDbContext.Set<ALERT_RULES>()
                    .AnyAsync(ar => !ar.IsDeleted && ar.EmailTemplateId == id);
                if (used) return ServiceResult<bool>.BadRequest("Cannot delete email template because it is used by alert rules");

                entity.IsDeleted = true;
                entity.IsActive = false;
                entity.DeletedDate = DateTime.UtcNow;
                entity.UpdatedDate = DateTime.UtcNow;

                _unitOfWork.Repositary<EMAIL_TEMPLATES>().Update(entity);
                await _unitOfWork.CompleteAsyn();

                return ServiceResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting email template {Id}", id);
                return ServiceResult<bool>.Error("Error deleting email template");
            }
        }

        public async Task<ServiceResult<bool>> ToggleActiveAsync(int id, bool isActive)
        {
            try
            {
                var entity = await _unitOfWork.AppDbContext.Set<EMAIL_TEMPLATES>()
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
                if (entity == null) return ServiceResult<bool>.NotFound("Email template not found");

                entity.IsActive = isActive;
                entity.UpdatedDate = DateTime.UtcNow;

                _unitOfWork.Repositary<EMAIL_TEMPLATES>().Update(entity);
                await _unitOfWork.CompleteAsyn();

                return ServiceResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling email template {Id} active={IsActive}", id, isActive);
                return ServiceResult<bool>.Error("Error updating email template status");
            }
        }

        private async Task UnsetOtherDefaultsAsync(int templateId, int documentTypeId, string templateCode)
        {
            // Use raw SQL for quick update (avoid tracking issues)
            await _unitOfWork.AppDbContext.Database.ExecuteSqlRawAsync(
                @"UPDATE EMAIL_TEMPLATES
                  SET IsDefault = 0, UpdatedDate = GETUTCDATE()
                  WHERE Id <> {0} AND DocumentTypeId = {1} AND TemplateCode = {2} AND IsDeleted = 0",
                templateId, documentTypeId, templateCode);
        }

        private static string NormalizeCode(string code)
        {
            return (code ?? string.Empty).Trim();
        }
    }
}


