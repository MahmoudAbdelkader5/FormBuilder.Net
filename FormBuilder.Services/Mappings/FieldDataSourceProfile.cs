using AutoMapper;
using FormBuilder.API.Models;
using FormBuilder.Domian.Entitys.froms;

namespace FormBuilder.Services.Mappings
{
    public class FieldDataSourceProfile : Profile
    {
        public FieldDataSourceProfile()
        {
            CreateMap<FIELD_DATA_SOURCES, FieldDataSourceDto>();

            CreateMap<CreateFieldDataSourceDto, FIELD_DATA_SOURCES>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_FIELDS, opt => opt.Ignore());

            CreateMap<UpdateFieldDataSourceDto, FIELD_DATA_SOURCES>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FieldId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_FIELDS, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
