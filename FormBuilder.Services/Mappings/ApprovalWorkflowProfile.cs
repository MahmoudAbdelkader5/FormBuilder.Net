using AutoMapper;
using FormBuilder.Application.DTOs.ApprovalWorkflow;
using FormBuilder.Domian.Entitys.FromBuilder;

namespace FormBuilder.Services.Mappings
{
    public class ApprovalWorkflowProfile : Profile
    {
        public ApprovalWorkflowProfile()
        {
            CreateMap<APPROVAL_WORKFLOWS, ApprovalWorkflowDto>()
                .ForMember(dest => dest.DocumentTypeName, opt => opt.MapFrom(src => src.DOCUMENT_TYPES != null ? src.DOCUMENT_TYPES.Name : null))
                .ForMember(dest => dest.Stages, opt => opt.MapFrom(src => src.APPROVAL_STAGES));

            CreateMap<ApprovalWorkflowCreateDto, APPROVAL_WORKFLOWS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.DOCUMENT_TYPES, opt => opt.Ignore())
                .ForMember(dest => dest.APPROVAL_STAGES, opt => opt.Ignore());

            CreateMap<ApprovalWorkflowUpdateDto, APPROVAL_WORKFLOWS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.DOCUMENT_TYPES, opt => opt.Ignore())
                .ForMember(dest => dest.APPROVAL_STAGES, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentTypeId, opt => opt.Condition(src => src.DocumentTypeId.HasValue && src.DocumentTypeId.Value > 0))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
