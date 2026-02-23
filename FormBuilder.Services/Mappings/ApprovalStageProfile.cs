using AutoMapper;
using FormBuilder.Application.DTOs.ApprovalWorkflow;
using FormBuilder.Domian.Entitys.FormBuilder;

namespace FormBuilder.Services.Mappings
{
    public class ApprovalStageProfile : Profile
    {
        public ApprovalStageProfile()
        {
            CreateMap<APPROVAL_STAGES, ApprovalStageDto>()
                .ForMember(dest => dest.WorkflowName, opt => opt.MapFrom(src => src.APPROVAL_WORKFLOWS != null ? src.APPROVAL_WORKFLOWS.Name : null));

            CreateMap<ApprovalStageCreateDto, APPROVAL_STAGES>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.APPROVAL_WORKFLOWS, opt => opt.Ignore())
                .ForMember(dest => dest.APPROVAL_STAGE_ASSIGNEES, opt => opt.Ignore())
                .ForMember(dest => dest.DOCUMENT_APPROVAL_HISTORY, opt => opt.Ignore());

            CreateMap<ApprovalStageUpdateDto, APPROVAL_STAGES>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.WorkflowId, opt => opt.Ignore()) // Don't allow updating WorkflowId
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.APPROVAL_WORKFLOWS, opt => opt.Ignore())
                .ForMember(dest => dest.APPROVAL_STAGE_ASSIGNEES, opt => opt.Ignore())
                .ForMember(dest => dest.DOCUMENT_APPROVAL_HISTORY, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
