using AutoMapper;
using FormBuilder.Domian.Entitys.froms;
using FormBuilder.API.Models;

namespace FormBuilder.Services.Mappings
{
    public class FieldOptionsProfile : Profile
    {
        public FieldOptionsProfile()
        {
            CreateMap<FIELD_OPTIONS, FieldOptionDto>();
            
            CreateMap<CreateFieldOptionDto, FIELD_OPTIONS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_FIELDS, opt => opt.Ignore());

            CreateMap<UpdateFieldOptionDto, FIELD_OPTIONS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FieldId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_FIELDS, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
