using AutoMapper;
using FormBuilder.API.DTOs;
using FormBuilder.Domian.Entitys.FormBuilder;

namespace FormBuilder.Services.Mappings
{
    public class FormSubmissionGridCellProfile : Profile
    {
        public FormSubmissionGridCellProfile()
        {
            CreateMap<FORM_SUBMISSION_GRID_CELLS, FormSubmissionGridCellDto>()
                .ForMember(dest => dest.ColumnCode, opt => opt.MapFrom(src => src.FORM_GRID_COLUMNS != null ? src.FORM_GRID_COLUMNS.ColumnCode : null))
                .ForMember(dest => dest.ColumnName, opt => opt.MapFrom(src => src.FORM_GRID_COLUMNS != null ? src.FORM_GRID_COLUMNS.ColumnName : null))
                .ForMember(dest => dest.FieldTypeId, opt => opt.MapFrom(src => src.FORM_GRID_COLUMNS != null ? src.FORM_GRID_COLUMNS.FieldTypeId : (int?)null))
                .ForMember(dest => dest.FieldTypeName, opt => opt.Ignore())
                .ForMember(dest => dest.DisplayValue, opt => opt.Ignore()); // Will be set manually based on value type

            CreateMap<CreateFormSubmissionGridCellDto, FORM_SUBMISSION_GRID_CELLS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_SUBMISSION_GRID_ROWS, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_GRID_COLUMNS, opt => opt.Ignore());

            CreateMap<UpdateFormSubmissionGridCellDto, FORM_SUBMISSION_GRID_CELLS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RowId, opt => opt.Ignore())
                .ForMember(dest => dest.ColumnId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_SUBMISSION_GRID_ROWS, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_GRID_COLUMNS, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
