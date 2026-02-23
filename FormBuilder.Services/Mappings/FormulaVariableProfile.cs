using AutoMapper;
using FormBuilder.Application.DTOs.Formula;
using FormBuilder.Domian.Entitys.froms;

namespace FormBuilder.Services.Mappings
{
    public class FormulaVariableProfile : Profile
    {
        public FormulaVariableProfile()
        {
            CreateMap<FORMULA_VARIABLES, FormulaVariableDto>()
                .ForMember(dest => dest.Formulaname, opt => opt.MapFrom(src => src.FORMULAS != null ? src.FORMULAS.Name : null))
                .ForMember(dest => dest.SourceFieldName, opt => opt.MapFrom(src => src.FORM_FIELDS != null ? src.FORM_FIELDS.FieldName : null));

            CreateMap<FormulaVariableCreateDto, FORMULA_VARIABLES>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.FORMULAS, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_FIELDS, opt => opt.Ignore());

            CreateMap<FormulaVariableUpdateDto, FORMULA_VARIABLES>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.FORMULAS, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_FIELDS, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
