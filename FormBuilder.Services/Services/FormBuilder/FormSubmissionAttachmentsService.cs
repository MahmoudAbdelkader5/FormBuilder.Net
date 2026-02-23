using formBuilder.Domian.Interfaces;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.Services.Services.Base;
using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.Common;
using FormBuilder.API.Models;
using FormBuilder.Core.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Services
{
    public class FormSubmissionAttachmentsService : BaseService<FORM_SUBMISSION_ATTACHMENTS, FormSubmissionAttachmentDto, CreateFormSubmissionAttachmentDto, UpdateFormSubmissionAttachmentDto>, IFormSubmissionAttachmentsService
    {
        private readonly IunitOfwork _unitOfWork;
        private readonly IFileStorageService _fileStorageService;

        public FormSubmissionAttachmentsService(
            IunitOfwork unitOfWork, 
            IMapper mapper,
            IFileStorageService fileStorageService) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _fileStorageService = fileStorageService ?? throw new ArgumentNullException(nameof(fileStorageService));
        }

        protected override IBaseRepository<FORM_SUBMISSION_ATTACHMENTS> Repository => _unitOfWork.FormSubmissionAttachmentsRepository;

        public async Task<ApiResponse<List<FormSubmissionAttachmentDto>>> GetAllAsync()
        {
            var result = await base.GetAllAsync();
            if (result.Success && result.Data != null)
            {
                var dtos = result.Data.ToList();
                foreach (var dto in dtos)
                {
                    SetComputedProperties(dto);
                }
                return new ApiResponse<List<FormSubmissionAttachmentDto>>(result.StatusCode, "Success", dtos);
            }
            return new ApiResponse<List<FormSubmissionAttachmentDto>>(result.StatusCode, result.ErrorMessage);
        }

        public async Task<ApiResponse<FormSubmissionAttachmentDto>> GetByIdAsync(int id)
        {
            var result = await base.GetByIdAsync(id);
            if (result.Success && result.Data != null)
            {
                SetComputedProperties(result.Data);
                return new ApiResponse<FormSubmissionAttachmentDto>(result.StatusCode, "Success", result.Data);
            }
            return new ApiResponse<FormSubmissionAttachmentDto>(result.StatusCode, result.ErrorMessage);
        }

        public async Task<ApiResponse<List<FormSubmissionAttachmentDto>>> GetBySubmissionIdAsync(int submissionId)
        {
            var attachments = await _unitOfWork.FormSubmissionAttachmentsRepository.GetBySubmissionIdAsync(submissionId);
            var attachmentDtos = _mapper.Map<IEnumerable<FormSubmissionAttachmentDto>>(attachments).ToList();
            foreach (var dto in attachmentDtos)
            {
                SetComputedProperties(dto);
            }
            return new ApiResponse<List<FormSubmissionAttachmentDto>>(200, "Form submission attachments retrieved successfully", attachmentDtos);
        }

        public async Task<ApiResponse<List<FormSubmissionAttachmentDto>>> GetByFieldIdAsync(int fieldId)
        {
            var attachments = await _unitOfWork.FormSubmissionAttachmentsRepository.GetByFieldIdAsync(fieldId);
            var attachmentDtos = _mapper.Map<IEnumerable<FormSubmissionAttachmentDto>>(attachments).ToList();
            foreach (var dto in attachmentDtos)
            {
                SetComputedProperties(dto);
            }
            return new ApiResponse<List<FormSubmissionAttachmentDto>>(200, "Form submission attachments by field retrieved successfully", attachmentDtos);
        }

        public async Task<ApiResponse<List<FormSubmissionAttachmentDto>>> GetBySubmissionAndFieldAsync(int submissionId, int fieldId)
        {
            var attachments = await _unitOfWork.FormSubmissionAttachmentsRepository.GetBySubmissionAndFieldAsync(submissionId, fieldId);
            var attachmentDtos = _mapper.Map<IEnumerable<FormSubmissionAttachmentDto>>(attachments).ToList();
            foreach (var dto in attachmentDtos)
            {
                SetComputedProperties(dto);
            }
            return new ApiResponse<List<FormSubmissionAttachmentDto>>(200, "Form submission attachments retrieved successfully", attachmentDtos);
        }

        public async Task<ApiResponse<FormSubmissionAttachmentDto>> CreateAsync(CreateFormSubmissionAttachmentDto createDto)
        {
            var result = await base.CreateAsync(createDto);
            if (result.Success && result.Data != null)
            {
                SetComputedProperties(result.Data);
                return new ApiResponse<FormSubmissionAttachmentDto>(result.StatusCode, "Success", result.Data);
            }
            return new ApiResponse<FormSubmissionAttachmentDto>(result.StatusCode, result.ErrorMessage);
        }

        protected override async Task<ValidationResult> ValidateCreateAsync(CreateFormSubmissionAttachmentDto dto)
        {
            // Validate that the submission exists
            var submissionExists = await _unitOfWork.FormSubmissionsRepository.AnyAsync(s => s.Id == dto.SubmissionId);
            if (!submissionExists)
                return ValidationResult.Failure($"Submission with ID {dto.SubmissionId} not found");

            // Check if file name already exists for this submission
            var fileNameExists = await _unitOfWork.FormSubmissionAttachmentsRepository
                .FileNameExistsAsync(dto.SubmissionId, dto.FileName);

            if (fileNameExists)
                return ValidationResult.Failure("File name already exists for this submission");

            return ValidationResult.Success();
        }

        public async Task<ApiResponse<List<FormSubmissionAttachmentDto>>> CreateBulkAsync(BulkAttachmentsDto bulkDto)
        {
            if (bulkDto == null || !bulkDto.Attachments.Any())
                return new ApiResponse<List<FormSubmissionAttachmentDto>>(400, "No attachments provided");

            // Validate that the submission exists
            var submissionExists = await _unitOfWork.FormSubmissionsRepository.AnyAsync(s => s.Id == bulkDto.SubmissionId);
            if (!submissionExists)
                return new ApiResponse<List<FormSubmissionAttachmentDto>>(404, $"Submission with ID {bulkDto.SubmissionId} not found");

            var entities = bulkDto.Attachments.Select(attachmentDto =>
            {
                var entity = _mapper.Map<FORM_SUBMISSION_ATTACHMENTS>(attachmentDto);
                entity.SubmissionId = bulkDto.SubmissionId; // Override with bulk submission ID
                entity.UploadedDate = DateTime.UtcNow;
                return entity;
            }).ToList();

            _unitOfWork.FormSubmissionAttachmentsRepository.AddRange(entities);
            await _unitOfWork.CompleteAsyn();

            var createdDtos = _mapper.Map<IEnumerable<FormSubmissionAttachmentDto>>(entities).ToList();
            foreach (var dto in createdDtos)
            {
                SetComputedProperties(dto);
            }
            return new ApiResponse<List<FormSubmissionAttachmentDto>>(200, "Form submission attachments created successfully", createdDtos);
        }

        public async Task<ApiResponse<FormSubmissionAttachmentDto>> UpdateAsync(int id, UpdateFormSubmissionAttachmentDto updateDto)
        {
            var result = await base.UpdateAsync(id, updateDto);
            if (result.Success && result.Data != null)
            {
                SetComputedProperties(result.Data);
                return new ApiResponse<FormSubmissionAttachmentDto>(result.StatusCode, "Success", result.Data);
            }
            return new ApiResponse<FormSubmissionAttachmentDto>(result.StatusCode, result.ErrorMessage);
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var result = await base.DeleteAsync(id);
            return new ApiResponse<bool>(result.StatusCode, result.Success ? "Success" : result.ErrorMessage, result.Success);
        }

        public async Task<ApiResponse<bool>> DeleteBySubmissionIdAsync(int submissionId)
        {
            var deleted = await _unitOfWork.FormSubmissionAttachmentsRepository.DeleteBySubmissionIdAsync(submissionId);
            var message = deleted ? "Form submission attachments deleted successfully" : "No form submission attachments found";
            return new ApiResponse<bool>(200, message, deleted);
        }

        public async Task<ApiResponse<bool>> DeleteBySubmissionAndFieldAsync(int submissionId, int fieldId)
        {
            var deleted = await _unitOfWork.FormSubmissionAttachmentsRepository.DeleteBySubmissionAndFieldAsync(submissionId, fieldId);
            var message = deleted ? "Form submission attachments deleted successfully" : "No form submission attachments found";
            return new ApiResponse<bool>(200, message, deleted);
        }

        public async Task<ApiResponse<FormSubmissionAttachmentDto>> UploadAttachmentAsync(IFormFile file, UploadAttachmentDto uploadDto)
        {
            if (file == null || file.Length == 0)
                return new ApiResponse<FormSubmissionAttachmentDto>(400, "No file provided");

            // Validate file size (e.g., 10MB max)
            if (file.Length > 10 * 1024 * 1024)
                return new ApiResponse<FormSubmissionAttachmentDto>(400, "File size exceeds maximum allowed size (10MB)");

            // Validate file type (PDF, Images, Excel, Word)
            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".xls", ".xlsx", ".doc", ".docx" };
            var fileExtension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (string.IsNullOrEmpty(fileExtension) || !allowedExtensions.Contains(fileExtension))
            {
                return new ApiResponse<FormSubmissionAttachmentDto>(400, 
                    $"File type not allowed. Allowed types: PDF, Images (JPG, PNG), Excel (XLS, XLSX), Word (DOC, DOCX)");
            }

            // Save file to storage
            string filePath;
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var subFolder = $"submissions/{uploadDto.SubmissionId}";
                    filePath = await _fileStorageService.SaveFileAsync(stream, file.FileName, subFolder);
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<FormSubmissionAttachmentDto>(500, $"Error saving file: {ex.Message}");
            }

            var createDto = new CreateFormSubmissionAttachmentDto
            {
                SubmissionId = uploadDto.SubmissionId,
                FieldId = uploadDto.FieldId,
                FieldCode = uploadDto.FieldCode,
                FileName = file.FileName,
                FilePath = filePath,
                FileSize = file.Length,
                ContentType = file.ContentType
            };

            return await CreateAsync(createDto);
        }

        public async Task<ApiResponse<List<AttachmentUploadResultDto>>> UploadMultipleAttachmentsAsync(List<IFormFile> files, UploadAttachmentDto uploadDto)
        {
            if (files == null || !files.Any())
                return new ApiResponse<List<AttachmentUploadResultDto>>(400, "No files provided");

            // Validate file types for all files
            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".xls", ".xlsx", ".doc", ".docx" };
            foreach (var file in files)
            {
                var fileExtension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
                if (string.IsNullOrEmpty(fileExtension) || !allowedExtensions.Contains(fileExtension))
                {
                    return new ApiResponse<List<AttachmentUploadResultDto>>(400,
                        $"File '{file.FileName}' type not allowed. Allowed types: PDF, Images (JPG, PNG), Excel (XLS, XLSX), Word (DOC, DOCX)");
                }

                // Validate file size
                if (file.Length > 10 * 1024 * 1024)
                {
                    return new ApiResponse<List<AttachmentUploadResultDto>>(400,
                        $"File '{file.FileName}' size exceeds maximum allowed size (10MB)");
                }
            }

            var results = new List<AttachmentUploadResultDto>();

            foreach (var file in files)
            {
                var uploadResult = await UploadAttachmentAsync(file, uploadDto);
                if (uploadResult.StatusCode == 200 && uploadResult.Data != null)
                {
                    var attachment = uploadResult.Data;
                    results.Add(new AttachmentUploadResultDto
                    {
                        AttachmentId = attachment.Id,
                        FileName = attachment.FileName,
                        FilePath = attachment.FilePath,
                        FileSize = attachment.FileSize,
                        ContentType = attachment.ContentType,
                        UploadedDate = attachment.UploadedDate,
                        Success = true,
                        Message = "Upload successful"
                    });
                }
                else
                {
                    results.Add(new AttachmentUploadResultDto
                    {
                        FileName = file.FileName,
                        Success = false,
                        Message = uploadResult.Message
                    });
                }
            }

            return new ApiResponse<List<AttachmentUploadResultDto>>(200, "Multiple attachments upload completed", results);
        }

        public async Task<ApiResponse<AttachmentStatsDto>> GetAttachmentStatsAsync(int submissionId)
        {
            var totalSize = await _unitOfWork.FormSubmissionAttachmentsRepository.GetTotalSizeBySubmissionAsync(submissionId);
            var count = await _unitOfWork.FormSubmissionAttachmentsRepository.GetCountBySubmissionAsync(submissionId);
            var attachments = await _unitOfWork.FormSubmissionAttachmentsRepository.GetBySubmissionIdAsync(submissionId);

            var stats = new AttachmentStatsDto
            {
                SubmissionId = submissionId,
                TotalAttachments = count,
                TotalSize = totalSize,
                TotalSizeFormatted = FormatFileSize(totalSize),
                AttachmentsByType = attachments
                    .GroupBy(a => a.ContentType)
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            return new ApiResponse<AttachmentStatsDto>(200, "Attachment statistics retrieved successfully", stats);
        }

        public async Task<ApiResponse<bool>> ExistsAsync(int id)
        {
            var exists = await _unitOfWork.FormSubmissionAttachmentsRepository.AnyAsync(a => a.Id == id);
            return new ApiResponse<bool>(200, "Form submission attachment existence checked successfully", exists);
        }

        // ================================
        // HELPER METHODS
        // ================================
        private void SetComputedProperties(FormSubmissionAttachmentDto dto)
        {
            if (dto == null) return;

            dto.FileSizeFormatted = FormatFileSize(dto.FileSize);
            dto.DownloadUrl = $"/api/attachments/download/{dto.Id}";
        }

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            double len = bytes;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }
}
