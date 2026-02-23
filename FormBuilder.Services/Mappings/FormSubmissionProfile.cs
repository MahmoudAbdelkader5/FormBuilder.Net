using AutoMapper;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Domian.Entitys.froms;

namespace FormBuilder.Services.Mappings
{
    public class FormSubmissionProfile : Profile
    {
        public FormSubmissionProfile()
        {
            CreateMap<FORM_SUBMISSIONS, FormSubmissionDto>()
                .ForMember(dest => dest.FormName, opt => opt.MapFrom(src => src.FORM_BUILDER != null ? src.FORM_BUILDER.FormName : null))
                .ForMember(dest => dest.DocumentTypeName, opt => opt.MapFrom(src => src.DOCUMENT_TYPES != null ? src.DOCUMENT_TYPES.Name : null))
                .ForMember(dest => dest.SeriesCode, opt => opt.MapFrom(src => src.DOCUMENT_SERIES != null ? src.DOCUMENT_SERIES.SeriesCode : null))
                .ForMember(dest => dest.SubmittedByUserName, opt => opt.Ignore()) // Will be set manually if needed
                .ForMember(dest => dest.LastUpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate));

            // Mapping for FormSubmissionDetailDto (inherits from FormSubmissionDto)
            CreateMap<FORM_SUBMISSIONS, FormSubmissionDetailDto>()
                .IncludeBase<FORM_SUBMISSIONS, FormSubmissionDto>()
                .ForMember(dest => dest.FieldValues, opt => opt.Ignore()) // Will be mapped manually
                .ForMember(dest => dest.Attachments, opt => opt.Ignore()) // Will be mapped manually
                .ForMember(dest => dest.GridData, opt => opt.Ignore()); // Will be mapped manually

            CreateMap<CreateFormSubmissionDto, FORM_SUBMISSIONS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentNumber, opt => opt.Ignore()) // Will be generated
                .ForMember(dest => dest.Version, opt => opt.Ignore()) // Will be set manually
                .ForMember(dest => dest.SubmittedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.StageId, opt => opt.MapFrom(src => (int?)null)) // Set to null for new submissions
                .ForMember(dest => dest.FORM_BUILDER, opt => opt.Ignore())
                .ForMember(dest => dest.DOCUMENT_TYPES, opt => opt.Ignore())
                .ForMember(dest => dest.DOCUMENT_SERIES, opt => opt.Ignore())
                .ForMember(dest => dest.APPROVAL_STAGES, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_SUBMISSION_VALUES, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_SUBMISSION_ATTACHMENTS, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_SUBMISSION_GRID_ROWS, opt => opt.Ignore())
                .ForMember(dest => dest.DOCUMENT_APPROVAL_HISTORY, opt => opt.Ignore());

            CreateMap<UpdateFormSubmissionDto, FORM_SUBMISSIONS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FormBuilderId, opt => opt.Ignore())
                .ForMember(dest => dest.Version, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentTypeId, opt => opt.Ignore())
                .ForMember(dest => dest.SeriesId, opt => opt.Ignore())
                .ForMember(dest => dest.SubmittedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                // StageId يتم تحديثه من UpdateDto إذا كان له قيمة
                .ForMember(dest => dest.StageId, opt => opt.Condition(src => src.StageId.HasValue))
                .ForMember(dest => dest.FORM_BUILDER, opt => opt.Ignore())
                .ForMember(dest => dest.DOCUMENT_TYPES, opt => opt.Ignore())
                .ForMember(dest => dest.DOCUMENT_SERIES, opt => opt.Ignore())
                .ForMember(dest => dest.APPROVAL_STAGES, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_SUBMISSION_VALUES, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_SUBMISSION_ATTACHMENTS, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_SUBMISSION_GRID_ROWS, opt => opt.Ignore())
                .ForMember(dest => dest.DOCUMENT_APPROVAL_HISTORY, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
