using AutoMapper;
using FormBuilder.API.DTOs;
using FormBuilder.Domian.Entitys.FormBuilder;

namespace FormBuilder.Services.Mappings
{
    public class GridColumnOptionProfile : Profile
    {
        public GridColumnOptionProfile()
        {
            CreateMap<GRID_COLUMN_OPTIONS, GridColumnOptionDto>()
                .ForMember(dest => dest.ColumnName, opt => opt.MapFrom(src => src.FORM_GRID_COLUMNS != null ? src.FORM_GRID_COLUMNS.ColumnName : null))
                .ForMember(dest => dest.ColumnCode, opt => opt.MapFrom(src => src.FORM_GRID_COLUMNS != null ? src.FORM_GRID_COLUMNS.ColumnCode : null))
                .ForMember(dest => dest.GridName, opt => opt.MapFrom(src => src.FORM_GRID_COLUMNS != null && src.FORM_GRID_COLUMNS.FORM_GRIDS != null ? src.FORM_GRID_COLUMNS.FORM_GRIDS.GridName : null));

            CreateMap<CreateGridColumnOptionDto, GRID_COLUMN_OPTIONS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_GRID_COLUMNS, opt => opt.Ignore());

            CreateMap<UpdateGridColumnOptionDto, GRID_COLUMN_OPTIONS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ColumnId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_GRID_COLUMNS, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}

