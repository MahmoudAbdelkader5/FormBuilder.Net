using AutoMapper;
using FormBuilder.Application.DTOs.ApprovalWorkflow;
using FormBuilder.Domian.Entitys.FormBuilder;

namespace FormBuilder.Services.Mappings
{
    public class DocumentApprovalHistoryProfile : Profile
    {
        public DocumentApprovalHistoryProfile()
        {
            CreateMap<DOCUMENT_APPROVAL_HISTORY, DocumentApprovalHistoryDto>()
                .ForMember(dest => dest.DocumentNumber, opt => opt.MapFrom(src => src.FORM_SUBMISSIONS != null ? src.FORM_SUBMISSIONS.DocumentNumber : null))
                .ForMember(dest => dest.FormName, opt => opt.MapFrom(src => src.FORM_SUBMISSIONS != null && src.FORM_SUBMISSIONS.FORM_BUILDER != null ? src.FORM_SUBMISSIONS.FORM_BUILDER.FormName : null))
                .ForMember(dest => dest.DocumentTypeName, opt => opt.MapFrom(src => src.FORM_SUBMISSIONS != null && src.FORM_SUBMISSIONS.DOCUMENT_TYPES != null ? src.FORM_SUBMISSIONS.DOCUMENT_TYPES.Name : null))
                .ForMember(dest => dest.SubmissionStatus, opt => opt.MapFrom(src => src.FORM_SUBMISSIONS != null ? src.FORM_SUBMISSIONS.Status : null))
                .ForMember(dest => dest.StageName, opt => opt.MapFrom(src => src.APPROVAL_STAGES != null ? src.APPROVAL_STAGES.StageName : null))
                .ForMember(dest => dest.ActionByUserName, opt => opt.Ignore()); // Will be populated from Identity if needed

            CreateMap<DocumentApprovalHistoryCreateDto, DOCUMENT_APPROVAL_HISTORY>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ActionDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_SUBMISSIONS, opt => opt.Ignore())
                .ForMember(dest => dest.APPROVAL_STAGES, opt => opt.Ignore());
        }
    }
}

