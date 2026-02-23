using AutoMapper;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Domian.Entitys.FromBuilder;

namespace FormBuilder.Services.Mappings
{
    public class FormSubmissionAttachmentsProfile : Profile
    {
        public FormSubmissionAttachmentsProfile()
        {
            CreateMap<FORM_SUBMISSION_ATTACHMENTS, FormSubmissionAttachmentDto>()
                .ForMember(dest => dest.SubmissionDocumentNumber, opt => opt.MapFrom(src => src.FORM_SUBMISSIONS != null ? src.FORM_SUBMISSIONS.DocumentNumber : null))
                .ForMember(dest => dest.FieldCode, opt => opt.MapFrom(src => src.FORM_FIELDS != null ? src.FORM_FIELDS.FieldCode : null))
                .ForMember(dest => dest.FieldName, opt => opt.MapFrom(src => src.FORM_FIELDS != null ? src.FORM_FIELDS.FieldName : null))
                .ForMember(dest => dest.FileSizeFormatted, opt => opt.Ignore()) // Will be formatted manually
                .ForMember(dest => dest.DownloadUrl, opt => opt.Ignore()); // Will be set manually

            CreateMap<CreateFormSubmissionAttachmentDto, FORM_SUBMISSION_ATTACHMENTS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.UploadedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.FORM_SUBMISSIONS, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_FIELDS, opt => opt.Ignore())
                // Explicit mapping for required fields to ensure they are set
                .ForMember(dest => dest.SubmissionId, opt => opt.MapFrom(src => src.SubmissionId))
                .ForMember(dest => dest.FieldId, opt => opt.MapFrom(src => src.FieldId))
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => src.FilePath))
                .ForMember(dest => dest.FileSize, opt => opt.MapFrom(src => src.FileSize))
                .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.ContentType));

            CreateMap<UpdateFormSubmissionAttachmentDto, FORM_SUBMISSION_ATTACHMENTS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SubmissionId, opt => opt.Ignore())
                .ForMember(dest => dest.FieldId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UploadedDate, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_SUBMISSIONS, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_FIELDS, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
