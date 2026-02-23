using AutoMapper;
using FormBuilder.API.Models.DTOs;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domian.Entitys.FromBuilder;

namespace FormBuilder.Services.Mappings
{
    public class EmailConfigProfile : Profile
    {
        public EmailConfigProfile()
        {
            CreateMap<SMTP_CONFIGS, SmtpConfigDto>()
                .ForMember(dest => dest.HasPassword, opt => opt.MapFrom(src => !string.IsNullOrWhiteSpace(src.PasswordEncrypted)));

            CreateMap<EMAIL_TEMPLATES, EmailTemplateDto>();
        }
    }
}


