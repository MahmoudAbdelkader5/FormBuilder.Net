using AutoMapper;
using FormBuilder.API.Models.DTOs;
using FormBuilder.Domian.Entitys.FromBuilder;

namespace FormBuilder.Services.Mappings
{
    public class FormAttachmentTypeProfile : Profile
    {
        public FormAttachmentTypeProfile()
        {
            CreateMap<FORM_ATTACHMENT_TYPES, FormAttachmentTypeDto>()
                .ForMember(dest => dest.FormBuilderName, opt => opt.MapFrom(src => src.FORM_BUILDER != null ? src.FORM_BUILDER.FormName : null))
                .ForMember(dest => dest.AttachmentTypeName, opt => opt.MapFrom(src => src.ATTACHMENT_TYPES != null ? src.ATTACHMENT_TYPES.Name : null))
                .ForMember(dest => dest.AttachmentTypeCode, opt => opt.MapFrom(src => src.ATTACHMENT_TYPES != null ? src.ATTACHMENT_TYPES.Code : null))
                .ForMember(dest => dest.AttachmentTypeMaxSizeMB, opt => opt.MapFrom(src => src.ATTACHMENT_TYPES != null ? src.ATTACHMENT_TYPES.MaxSizeMB : 0));

            CreateMap<CreateFormAttachmentTypeDto, FORM_ATTACHMENT_TYPES>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_BUILDER, opt => opt.Ignore())
                .ForMember(dest => dest.ATTACHMENT_TYPES, opt => opt.Ignore());

            CreateMap<UpdateFormAttachmentTypeDto, FORM_ATTACHMENT_TYPES>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FormBuilderId, opt => opt.Ignore())
                .ForMember(dest => dest.AttachmentTypeId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_BUILDER, opt => opt.Ignore())
                .ForMember(dest => dest.ATTACHMENT_TYPES, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
