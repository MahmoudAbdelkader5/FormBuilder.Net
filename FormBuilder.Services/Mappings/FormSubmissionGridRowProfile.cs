using AutoMapper;
using FormBuilder.API.DTOs;
using FormBuilder.Domian.Entitys.FormBuilder;

namespace FormBuilder.Services.Mappings
{
    public class FormSubmissionGridRowProfile : Profile
    {
        public FormSubmissionGridRowProfile()
        {
            CreateMap<FORM_SUBMISSION_GRID_ROWS, FormSubmissionGridRowDto>()
                .ForMember(dest => dest.SubmissionNumber, opt => opt.MapFrom(src => src.FORM_SUBMISSIONS != null ? src.FORM_SUBMISSIONS.DocumentNumber : null))
                .ForMember(dest => dest.GridName, opt => opt.MapFrom(src => src.FORM_GRIDS != null ? src.FORM_GRIDS.GridName : null));

            CreateMap<CreateFormSubmissionGridRowDto, FORM_SUBMISSION_GRID_ROWS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.RowIndex, opt => opt.MapFrom(src => src.RowIndex ?? 0))
                .ForMember(dest => dest.FORM_SUBMISSIONS, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_GRIDS, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_SUBMISSION_GRID_CELLS, opt => opt.Ignore());

            CreateMap<UpdateFormSubmissionGridRowDto, FORM_SUBMISSION_GRID_ROWS>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SubmissionId, opt => opt.UseDestinationValue()) // Never update SubmissionId
                .ForMember(dest => dest.GridId, opt => opt.UseDestinationValue()) // Never update GridId
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_SUBMISSIONS, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_GRIDS, opt => opt.Ignore())
                .ForMember(dest => dest.FORM_SUBMISSION_GRID_CELLS, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
