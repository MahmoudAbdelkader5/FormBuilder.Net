using FluentValidation;
using FormBuilder.Application.DTOs.ApprovalWorkflow;

namespace FormBuilder.Services.Validators.FormBuilder
{
    public class ApprovalStageCreateDtoValidator : AbstractValidator<ApprovalStageCreateDto>
    {
        public ApprovalStageCreateDtoValidator()
        {
            RuleFor(x => x.StageName)
                .NotEmpty()
                .WithMessage("Stage name is required")
                .Length(2, 200)
                .WithMessage("Stage name must be between 2 and 200 characters");

            RuleFor(x => x.StageOrder)
                .GreaterThan(0)
                .WithMessage("Stage order must be greater than 0");

            RuleFor(x => x)
                .Must(dto => !dto.MinAmount.HasValue || !dto.MaxAmount.HasValue || dto.MinAmount.Value < dto.MaxAmount.Value)
                .WithMessage("Min Amount must be less than Max Amount")
                .When(dto => dto.MinAmount.HasValue && dto.MaxAmount.HasValue);

            RuleFor(x => x.MinimumRequiredAssignees)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Minimum required assignees must be greater than or equal to 0")
                .When(x => x.MinimumRequiredAssignees.HasValue);
        }
    }
}

