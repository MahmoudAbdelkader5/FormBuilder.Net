using AutoMapper;
using FormBuilder.API.DTOs;
using FormBuilder.Domian.Entitys.FormBuilder;

namespace FormBuilder.Services.Mappings
{
    public class FormGridProfile : Profile
    {
        public FormGridProfile()
        {
            CreateMap<FORM_GRIDS, FormGridDto>()
                .ForMember(dest => dest.FormBuilderName, opt => opt.MapFrom(src => src.FORM_BUILDER != null ? src.FORM_BUILDER.FormName : null))
                .ForMember(dest => dest.TabName, opt => opt.MapFrom(src => src.FORM_TABS != null ? src.FORM_TABS.TabName : null));

            CreateMap<CreateFormGridDto, FORM_GRIDS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_BUILDER, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_TABS, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_GRID_COLUMNS, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_SUBMISSION_GRID_ROWS, opt => opt.Ignore())
                .ForMember(dest => dest.GridOrder, opt => opt.Ignore()); // Will be set manually

            CreateMap<UpdateFormGridDto, FORM_GRIDS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_BUILDER, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_TABS, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_GRID_COLUMNS, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_SUBMISSION_GRID_ROWS, opt => opt.Ignore())
                .ForMember(dest => dest.FormBuilderId, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
