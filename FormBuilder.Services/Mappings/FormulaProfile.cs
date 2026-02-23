using AutoMapper;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Domian.Entitys.FormBuilder;

namespace FormBuilder.Services.Mappings
{
    public class FormulaProfile : Profile
    {
        public FormulaProfile()
        {
            CreateMap<FORMULAS, FormulaDto>()
                .ForMember(dest => dest.FormBuilderName, opt => opt.MapFrom(src => src.FORM_BUILDER != null ? src.FORM_BUILDER.FormName : null))
                .ForMember(dest => dest.ResultFieldName, opt => opt.MapFrom(src => src.RESULT_FIELD != null ? src.RESULT_FIELD.FieldName : null))
                .ForMember(dest => dest.ResultFieldCode, opt => opt.MapFrom(src => src.RESULT_FIELD != null ? src.RESULT_FIELD.FieldCode : null))
                .ForMember(dest => dest.VariableCount, opt => opt.MapFrom(src => src.FORMULA_VARIABLES != null ? src.FORMULA_VARIABLES.Count : 0))
                .ForMember(dest => dest.Variables, opt => opt.Ignore()); // Will be mapped separately if needed

            CreateMap<CreateFormulaDto, FORMULAS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_BUILDER, opt => opt.Ignore())
                .ForMember(dest => dest.RESULT_FIELD, opt => opt.Ignore())
                .ForMember(dest => dest.FORMULA_VARIABLES, opt => opt.Ignore());

            CreateMap<UpdateFormulaDto, FORMULAS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FormBuilderId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_BUILDER, opt => opt.Ignore())
                .ForMember(dest => dest.RESULT_FIELD, opt => opt.Ignore())
                .ForMember(dest => dest.FORMULA_VARIABLES, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
