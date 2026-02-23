using AutoMapper;
using FormBuilder.Application.DTOs.ApprovalWorkflow;
using FormBuilder.Domian.Entitys.FormBuilder;

namespace FormBuilder.Services.Mappings
{
    public class ApprovalDelegationProfile : Profile
    {
        public ApprovalDelegationProfile()
        {
            CreateMap<APPROVAL_DELEGATIONS, ApprovalDelegationDto>()
                .ForMember(dest => dest.FromUserName, opt => opt.Ignore()) // Will be populated from Identity if needed
                .ForMember(dest => dest.ToUserName, opt => opt.Ignore()); // Will be populated from Identity if needed

            CreateMap<ApprovalDelegationCreateDto, APPROVAL_DELEGATIONS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore());

            CreateMap<ApprovalDelegationUpdateDto, APPROVAL_DELEGATIONS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FromUserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}

