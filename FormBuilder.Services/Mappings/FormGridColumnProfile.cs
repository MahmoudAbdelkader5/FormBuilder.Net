using AutoMapper;
using FormBuilder.API.DTOs;
using FormBuilder.Domian.Entitys.FormBuilder;

namespace FormBuilder.Services.Mappings
{
    public class FormGridColumnProfile : Profile
    {
        public FormGridColumnProfile()
        {
            CreateMap<FORM_GRID_COLUMNS, FormGridColumnDto>()
                .ForMember(dest => dest.GridName, opt => opt.MapFrom(src => src.FORM_GRIDS != null ? src.FORM_GRIDS.GridName : null))
                .ForMember(dest => dest.FormBuilderName, opt => opt.MapFrom(src => src.FORM_GRIDS != null && src.FORM_GRIDS.FORM_BUILDER != null ? src.FORM_GRIDS.FORM_BUILDER.FormName : null))
                .ForMember(dest => dest.FieldTypeName, opt => opt.Ignore());

            CreateMap<CreateFormGridColumnDto, FORM_GRID_COLUMNS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.ColumnOrder, opt => opt.MapFrom(src => src.ColumnOrder ?? 0))
                .ForMember(dest => dest.ColumnName, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.ColumnName) ? string.Empty : src.ColumnName))
                .ForMember(dest => dest.ColumnCode, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.ColumnCode) ? string.Empty : src.ColumnCode))
                .ForMember(dest => dest.DataType, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.DataType) ? string.Empty : src.DataType))
                .ForMember(dest => dest.IsReadOnly, opt => opt.MapFrom(src => src.IsReadOnly))
                .ForMember(dest => dest.IsVisible, opt => opt.MapFrom(src => src.IsVisible))
                .ForMember(dest => dest.FORM_GRIDS, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_SUBMISSION_GRID_CELLS, opt => opt.Ignore())
                .ForMember(dest => dest.GRID_COLUMN_DATA_SOURCES, opt => opt.Ignore())
                .ForMember(dest => dest.GRID_COLUMN_OPTIONS, opt => opt.Ignore());

            CreateMap<UpdateFormGridColumnDto, FORM_GRID_COLUMNS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.GridId, opt => opt.UseDestinationValue()) // Always use existing GridId value, never update it
                .ForMember(dest => dest.FORM_GRIDS, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_SUBMISSION_GRID_CELLS, opt => opt.Ignore())
                .ForMember(dest => dest.GRID_COLUMN_DATA_SOURCES, opt => opt.Ignore())
                .ForMember(dest => dest.GRID_COLUMN_OPTIONS, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
