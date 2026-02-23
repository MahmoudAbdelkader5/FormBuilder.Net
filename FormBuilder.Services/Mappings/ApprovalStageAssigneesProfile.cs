using AutoMapper;
using FormBuilder.Application.DTOs.ApprovalWorkflow;
using FormBuilder.Domian.Entitys.FormBuilder;

namespace FormBuilder.Services.Mappings
{
    public class ApprovalStageAssigneesProfile : Profile
    {
        public ApprovalStageAssigneesProfile()
        {
            CreateMap<APPROVAL_STAGE_ASSIGNEES, ApprovalStageAssigneesDto>()
                .ForMember(dest => dest.StageName, opt => opt.MapFrom(src => src.APPROVAL_STAGES != null ? src.APPROVAL_STAGES.StageName : null))
                .ForMember(dest => dest.RoleName, opt => opt.Ignore()) // Will be populated from Identity if needed
                .ForMember(dest => dest.UserName, opt => opt.Ignore()); // Will be populated from Identity if needed

            CreateMap<ApprovalStageAssigneesCreateDto, APPROVAL_STAGE_ASSIGNEES>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.APPROVAL_STAGES, opt => opt.Ignore());

            CreateMap<ApprovalStageAssigneesUpdateDto, APPROVAL_STAGE_ASSIGNEES>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.APPROVAL_STAGES, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}

