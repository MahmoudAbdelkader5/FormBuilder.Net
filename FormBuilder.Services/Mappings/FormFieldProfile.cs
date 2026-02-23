using AutoMapper;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.API.Models;
using FormBuilder.API.DTOs;
using FormBuilder.Core.DTOS.FormFields;
using CreateFormFieldDto = FormBuilder.Core.DTOS.FormFields.CreateFormFieldDto;
using UpdateFormFieldDto = FormBuilder.API.Models.UpdateFormFieldDto;

namespace FormBuilder.Services.Mappings
{
    public class FormFieldProfile : Profile
    {
        public FormFieldProfile()
        {
            CreateMap<FORM_FIELDS, FormFieldDto>()
                .ForMember(dest => dest.FieldTypeName, opt => opt.Ignore())
                .ForMember(dest => dest.FieldOptions, opt => opt.MapFrom(src => src.FIELD_OPTIONS != null ? src.FIELD_OPTIONS.Where(fo => fo.IsActive) : new List<FormBuilder.Domian.Entitys.froms.FIELD_OPTIONS>()))
                .ForMember(dest => dest.FieldDataSource, opt => opt.MapFrom(src => src.FIELD_DATA_SOURCES != null ? src.FIELD_DATA_SOURCES.FirstOrDefault(ds => ds.IsActive) : null))
                .ForMember(dest => dest.Grid, opt => opt.MapFrom(src => src.Grid != null ? src.Grid : null))
                .ForMember(dest => dest.ExpressionText, opt => opt.MapFrom(src => src.ExpressionText))
                .ForMember(dest => dest.CalculationMode, opt => opt.MapFrom(src => src.CalculationMode))
                .ForMember(dest => dest.RecalculateOn, opt => opt.MapFrom(src => src.RecalculateOn))
                .ForMember(dest => dest.ResultType, opt => opt.MapFrom(src => src.ResultType));

            CreateMap<CreateFormFieldDto, FORM_FIELDS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_TABS, opt => opt.Ignore())
                .ForMember(dest => dest.FIELD_OPTIONS, opt => opt.Ignore())
                .ForMember(dest => dest.FIELD_DATA_SOURCES, opt => opt.Ignore())
                .ForMember(dest => dest.Grid, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<UpdateFormFieldDto, FORM_FIELDS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TabId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_TABS, opt => opt.Ignore())
                .ForMember(dest => dest.FIELD_OPTIONS, opt => opt.Ignore())
                .ForMember(dest => dest.FIELD_DATA_SOURCES, opt => opt.Ignore())
                .ForMember(dest => dest.Grid, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
