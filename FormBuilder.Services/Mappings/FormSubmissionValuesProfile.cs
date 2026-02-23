using AutoMapper;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Domian.Entitys.froms;

namespace FormBuilder.Services.Mappings
{
    public class FormSubmissionValuesProfile : Profile
    {
        public FormSubmissionValuesProfile()
        {
            CreateMap<FORM_SUBMISSION_VALUES, FormSubmissionValueDto>()
                .ForMember(dest => dest.FieldCode, opt => opt.MapFrom(src => src.FieldCode))
                .ForMember(dest => dest.FieldName, opt => opt.MapFrom(src => src.FORM_FIELDS != null ? src.FORM_FIELDS.FieldName : null));

            CreateMap<CreateFormSubmissionValueDto, FORM_SUBMISSION_VALUES>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_SUBMISSIONS, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_FIELDS, opt => opt.Ignore());

            CreateMap<UpdateFormSubmissionValueDto, FORM_SUBMISSION_VALUES>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SubmissionId, opt => opt.Ignore())
                .ForMember(dest => dest.FieldId, opt => opt.Ignore())
                .ForMember(dest => dest.FieldCode, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_SUBMISSIONS, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_FIELDS, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
