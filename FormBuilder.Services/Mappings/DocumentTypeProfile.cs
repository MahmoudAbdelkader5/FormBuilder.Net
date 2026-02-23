using AutoMapper;
using FormBuilder.API.Models.DTOs;
using FormBuilder.Domian.Entitys.FromBuilder;

namespace FormBuilder.Services.Mappings
{
    public class DocumentTypeProfile : Profile
    {
        public DocumentTypeProfile()
        {
            CreateMap<DOCUMENT_TYPES, DocumentTypeDto>()
                .ForMember(dest => dest.FormBuilderName, opt => opt.MapFrom(src => src.FORM_BUILDER != null ? src.FORM_BUILDER.FormName : null))
                .ForMember(dest => dest.ParentMenuName, opt => opt.MapFrom(src => src.ParentMenu != null ? src.ParentMenu.Name : null))
                .ForMember(dest => dest.ApprovalWorkflowId, opt => opt.MapFrom(src => src.ApprovalWorkflowId))
                .ForMember(dest => dest.ApprovalWorkflowName, opt => opt.MapFrom(src => src.ApprovalWorkflow != null ? src.ApprovalWorkflow.Name : null));

            CreateMap<CreateDocumentTypeDto, DOCUMENT_TYPES>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<UpdateDocumentTypeDto, DOCUMENT_TYPES>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_BUILDER, opt => opt.Ignore())
                .ForMember(dest => dest.ParentMenu, opt => opt.Ignore())
                .ForMember(dest => dest.Children, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
