using FormBuilder.API.Models;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Core.DTOS.FormBuilder;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IFormSubmissionAttachmentsService
    {
        Task<ApiResponse<List<FormSubmissionAttachmentDto>>> GetAllAsync();
        Task<ApiResponse<FormSubmissionAttachmentDto>> GetByIdAsync(int id);
        Task<ApiResponse<List<FormSubmissionAttachmentDto>>> GetBySubmissionIdAsync(int submissionId);
        Task<ApiResponse<List<FormSubmissionAttachmentDto>>> GetByFieldIdAsync(int fieldId);
        Task<ApiResponse<List<FormSubmissionAttachmentDto>>> GetBySubmissionAndFieldAsync(int submissionId, int fieldId);
        Task<ApiResponse<FormSubmissionAttachmentDto>> CreateAsync(CreateFormSubmissionAttachmentDto createDto);
        Task<ApiResponse<List<FormSubmissionAttachmentDto>>> CreateBulkAsync(BulkAttachmentsDto bulkDto);
        Task<ApiResponse<FormSubmissionAttachmentDto>> UpdateAsync(int id, UpdateFormSubmissionAttachmentDto updateDto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<bool>> DeleteBySubmissionIdAsync(int submissionId);
        Task<ApiResponse<bool>> DeleteBySubmissionAndFieldAsync(int submissionId, int fieldId);
        Task<ApiResponse<FormSubmissionAttachmentDto>> UploadAttachmentAsync(IFormFile file, UploadAttachmentDto uploadDto);
        Task<ApiResponse<List<AttachmentUploadResultDto>>> UploadMultipleAttachmentsAsync(List<IFormFile> files, UploadAttachmentDto uploadDto);
        Task<ApiResponse<AttachmentStatsDto>> GetAttachmentStatsAsync(int submissionId);
        Task<ApiResponse<bool>> ExistsAsync(int id);
    }
}