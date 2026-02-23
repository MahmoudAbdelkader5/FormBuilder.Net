using AutoMapper;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Domian.Entitys.FormBuilder;

namespace FormBuilder.Services.Mappings
{
    public class FormBuilderProfile : Profile
    {
        public FormBuilderProfile()
        {
            CreateMap<FORM_BUILDER, FormBuilderDto>();

            CreateMap<CreateFormBuilderDto, FORM_BUILDER>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Version, opt => opt.MapFrom(_ => 1))
                .ForMember(dest => dest.IsPublished, opt => opt.MapFrom(src => src.IsPublished))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<UpdateFormBuilderDto, FORM_BUILDER>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Alert Rules Mappings
            CreateMap<ALERT_RULES, AlertRuleDto>()
                .ForMember(dest => dest.DocumentTypeName, opt => opt.Ignore())
                .ForMember(dest => dest.EmailTemplateName, opt => opt.Ignore());

            CreateMap<CreateAlertRuleDto, ALERT_RULES>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.DOCUMENT_TYPES, opt => opt.Ignore())
                .ForMember(dest => dest.EMAIL_TEMPLATES, opt => opt.Ignore());

            CreateMap<UpdateAlertRuleDto, ALERT_RULES>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentTypeId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.DOCUMENT_TYPES, opt => opt.Ignore())
                .ForMember(dest => dest.EMAIL_TEMPLATES, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}

