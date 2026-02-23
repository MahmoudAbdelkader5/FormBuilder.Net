using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Application.DTOs.ApprovalWorkflow;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.API.Models;
using FormBuilder.Services.Services.Base;
using FormBuilder.Application.DTOS;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Services
{
    public class DocumentApprovalHistoryService : BaseService<DOCUMENT_APPROVAL_HISTORY, DocumentApprovalHistoryDto, DocumentApprovalHistoryCreateDto, object>, IDocumentApprovalHistoryService
    {
        public DocumentApprovalHistoryService(IunitOfwork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override IBaseRepository<DOCUMENT_APPROVAL_HISTORY> Repository => _unitOfWork.DocumentApprovalHistoryRepository;

        public async Task<ApiResponse> GetBySubmissionIdAsync(int submissionId)
        {
            var history = await _unitOfWork.DocumentApprovalHistoryRepository.GetBySubmissionIdAsync(submissionId);
            var dtos = _mapper.Map<IEnumerable<DocumentApprovalHistoryDto>>(history);
            return new ApiResponse(200, "Success", dtos);
        }

        public async Task<ApiResponse> GetByStageIdAsync(int stageId)
        {
            var history = await _unitOfWork.DocumentApprovalHistoryRepository.GetByStageIdAsync(stageId);
            var dtos = _mapper.Map<IEnumerable<DocumentApprovalHistoryDto>>(history);
            return new ApiResponse(200, "Success", dtos);
        }

        public async Task<ApiResponse> GetByUserIdAsync(string userId)
        {
            var history = await _unitOfWork.DocumentApprovalHistoryRepository.GetByUserIdAsync(userId);
            var dtos = _mapper.Map<IEnumerable<DocumentApprovalHistoryDto>>(history);
            return new ApiResponse(200, "Success", dtos);
        }

        public async Task<ApiResponse> GetAllApprovalHistoryAsync()
        {
            var history = await _unitOfWork.DocumentApprovalHistoryRepository.GetAllApprovalHistoryAsync();
            var dtos = _mapper.Map<IEnumerable<DocumentApprovalHistoryDto>>(history);
            return new ApiResponse(200, "All approval history retrieved successfully", dtos);
        }

        public async Task<ApiResponse> CreateAsync(DocumentApprovalHistoryCreateDto dto)
        {
            var entity = _mapper.Map<DOCUMENT_APPROVAL_HISTORY>(dto);
            entity.ActionDate = DateTime.UtcNow;
            entity.CreatedDate = DateTime.UtcNow;
            entity.IsActive = true;
            // Comments is optional at the API level, but DB column is NOT NULL.
            // Always persist empty string instead of NULL to avoid SqlException.
            entity.Comments ??= string.Empty;

            Repository.Add(entity);
            await _unitOfWork.CompleteAsyn();

            var resultDto = _mapper.Map<DocumentApprovalHistoryDto>(entity);
            return new ApiResponse(200, "History record created successfully", resultDto);
        }

        public async Task<ApiResponse> DeleteBySubmissionIdAsync(int submissionId)
        {
            await _unitOfWork.DocumentApprovalHistoryRepository.DeleteBySubmissionIdAsync(submissionId);
            return new ApiResponse(200, "Approval history records deleted successfully");
        }
    }
}

