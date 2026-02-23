using FormBuilder.Infrastructure.Data;
using FormBuilder.core;
using FormBuilder.Domain.Interfaces.Repositories;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.Domian.Entitys.froms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FormBuilder.Infrastructure.Repositories
{
    public class FormAttachmentTypeRepository : BaseRepository<FORM_ATTACHMENT_TYPES>, IFormAttachmentTypeRepository
    {
        public FormBuilderDbContext _context { get; }

        public FormAttachmentTypeRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<FORM_ATTACHMENT_TYPES> GetByIdAsync(int id)
        {
            return await _context.FORM_ATTACHMENT_TYPES
                .Include(fat => fat.FORM_BUILDER)
                .Include(fat => fat.ATTACHMENT_TYPES)
                .FirstOrDefaultAsync(fat => fat.Id == id);
        }

        public async Task<IEnumerable<FORM_ATTACHMENT_TYPES>> GetAllAsync(Expression<Func<FORM_ATTACHMENT_TYPES, object>> include = null)
        {
            var query = _context.FORM_ATTACHMENT_TYPES.AsQueryable();

            // Apply includes if provided
            if (include != null)
            {
                query = query.Include(include);
            }
            else
            {
                // Default includes
                query = query
                    .Include(fat => fat.FORM_BUILDER)
                    .Include(fat => fat.ATTACHMENT_TYPES);
            }

            return await query
                .OrderBy(fat => fat.FormBuilderId)
                .ThenBy(fat => fat.ATTACHMENT_TYPES.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORM_ATTACHMENT_TYPES>> GetByFormBuilderIdAsync(int formBuilderId)
        {
            return await _context.FORM_ATTACHMENT_TYPES
                .Include(fat => fat.ATTACHMENT_TYPES)
                .Include(fat => fat.FORM_BUILDER)
                .Where(fat => fat.FormBuilderId == formBuilderId)
                .OrderBy(fat => fat.ATTACHMENT_TYPES.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORM_ATTACHMENT_TYPES>> GetByAttachmentTypeIdAsync(int attachmentTypeId)
        {
            return await _context.FORM_ATTACHMENT_TYPES
                .Include(fat => fat.FORM_BUILDER)
                .Include(fat => fat.ATTACHMENT_TYPES)
                .Where(fat => fat.AttachmentTypeId == attachmentTypeId)
                .OrderBy(fat => fat.FORM_BUILDER.FormName)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORM_ATTACHMENT_TYPES>> GetActiveAsync()
        {
            return await _context.FORM_ATTACHMENT_TYPES
                .Include(fat => fat.FORM_BUILDER)
                .Include(fat => fat.ATTACHMENT_TYPES)
                .Where(fat => fat.IsActive)
                .OrderBy(fat => fat.FormBuilderId)
                .ThenBy(fat => fat.ATTACHMENT_TYPES.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORM_ATTACHMENT_TYPES>> GetActiveByFormBuilderIdAsync(int formBuilderId)
        {
            return await _context.FORM_ATTACHMENT_TYPES
                .Include(fat => fat.ATTACHMENT_TYPES)
                .Include(fat => fat.FORM_BUILDER)
                .Where(fat => fat.FormBuilderId == formBuilderId && fat.IsActive)
                .OrderBy(fat => fat.ATTACHMENT_TYPES.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORM_ATTACHMENT_TYPES>> GetMandatoryByFormBuilderIdAsync(int formBuilderId)
        {
            return await _context.FORM_ATTACHMENT_TYPES
                .Include(fat => fat.ATTACHMENT_TYPES)
                .Include(fat => fat.FORM_BUILDER)
                .Where(fat => fat.FormBuilderId == formBuilderId && fat.IsMandatory && fat.IsActive)
                .OrderBy(fat => fat.ATTACHMENT_TYPES.Name)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int formBuilderId, int attachmentTypeId)
        {
            return await _context.FORM_ATTACHMENT_TYPES
                .AnyAsync(fat => fat.FormBuilderId == formBuilderId && fat.AttachmentTypeId == attachmentTypeId);
        }

        public async Task<bool> IsActiveAsync(int id)
        {
            return await _context.FORM_ATTACHMENT_TYPES
                .AnyAsync(fat => fat.Id == id && fat.IsActive);
        }

        public async Task<int> DeleteByFormBuilderIdAsync(int formBuilderId)
        {
            return await _context.FORM_ATTACHMENT_TYPES
                .Where(fat => fat.FormBuilderId == formBuilderId)
                .ExecuteDeleteAsync();
        }

        public async Task<bool> HasMandatoryAttachmentsAsync(int formBuilderId)
        {
            return await _context.FORM_ATTACHMENT_TYPES
                .AnyAsync(fat => fat.FormBuilderId == formBuilderId && fat.IsMandatory && fat.IsActive);
        }
    }
}