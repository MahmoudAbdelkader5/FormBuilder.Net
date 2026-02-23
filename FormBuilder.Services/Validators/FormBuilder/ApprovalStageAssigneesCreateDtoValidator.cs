using FluentValidation;
using FormBuilder.Application.DTOs.ApprovalWorkflow;

namespace FormBuilder.Services.Validators.FormBuilder
{
    public class ApprovalStageAssigneesCreateDtoValidator : AbstractValidator<ApprovalStageAssigneesCreateDto>
    {
        public ApprovalStageAssigneesCreateDtoValidator()
        {
            RuleFor(x => x.StageId)
                .GreaterThan(0)
                .WithMessage("StageId must be greater than 0.");

            // Either RoleId or UserId must be provided
            RuleFor(x => x)
                .Must(dto => !string.IsNullOrWhiteSpace(dto.RoleId) || !string.IsNullOrWhiteSpace(dto.UserId))
                .WithMessage("Either RoleId or UserId must be provided.");
        }
    }
}

