using AutoMapper;
using FormBuilder.API.DTOs;
using FormBuilder.Domian.Entitys.FormBuilder;
using System.Linq;

namespace FormBuilder.Services.Mappings
{
    public class GridColumnDataSourceProfile : Profile
    {
        public GridColumnDataSourceProfile()
        {
            CreateMap<GRID_COLUMN_DATA_SOURCES, GridColumnDataSourceDto>()
                .ForMember(dest => dest.ColumnName, opt => opt.MapFrom(src => src.FORM_GRID_COLUMNS != null ? src.FORM_GRID_COLUMNS.ColumnName : null))
                .ForMember(dest => dest.ColumnCode, opt => opt.MapFrom(src => src.FORM_GRID_COLUMNS != null ? src.FORM_GRID_COLUMNS.ColumnCode : null))
                .ForMember(dest => dest.GridName, opt => opt.MapFrom(src => src.FORM_GRID_COLUMNS != null && src.FORM_GRID_COLUMNS.FORM_GRIDS != null ? src.FORM_GRID_COLUMNS.FORM_GRIDS.GridName : null))
                .ForMember(dest => dest.FormBuilderName, opt => opt.MapFrom(src => src.FORM_GRID_COLUMNS != null && src.FORM_GRID_COLUMNS.FORM_GRIDS != null && src.FORM_GRID_COLUMNS.FORM_GRIDS.FORM_BUILDER != null ? src.FORM_GRID_COLUMNS.FORM_GRIDS.FORM_BUILDER.FormName : null))
                .ForMember(dest => dest.ArrayPropertyNames, opt => opt.Ignore())
                .AfterMap((src, dest) => 
                {
                    if (src.ArrayPropertyNames != null && src.ArrayPropertyNames.Length > 0)
                    {
                        var parts = src.ArrayPropertyNames.Split(new[] { ',' });
                        dest.ArrayPropertyNames = parts.Select(s => s.Trim()).Where(s => s.Length > 0).ToList();
                    }
                    else
                    {
                        dest.ArrayPropertyNames = null;
                    }
                });

            CreateMap<CreateGridColumnDataSourceDto, GRID_COLUMN_DATA_SOURCES>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_GRID_COLUMNS, opt => opt.Ignore())
                .ForMember(dest => dest.ArrayPropertyNames, opt => opt.MapFrom(src => 
                    src.ArrayPropertyNames != null && src.ArrayPropertyNames.Any() 
                        ? string.Join(",", src.ArrayPropertyNames) 
                        : null));

            CreateMap<UpdateGridColumnDataSourceDto, GRID_COLUMN_DATA_SOURCES>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ColumnId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_GRID_COLUMNS, opt => opt.Ignore())
                .ForMember(dest => dest.ArrayPropertyNames, opt => opt.MapFrom(src => 
                    src.ArrayPropertyNames != null && src.ArrayPropertyNames.Any() 
                        ? string.Join(",", src.ArrayPropertyNames) 
                        : null))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}

