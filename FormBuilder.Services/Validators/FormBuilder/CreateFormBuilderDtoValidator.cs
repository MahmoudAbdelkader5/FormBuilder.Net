using FluentValidation;
using FormBuilder.Core.DTOS.FormBuilder;

namespace FormBuilder.Services.Validators.FormBuilder
{
    public class CreateFormBuilderDtoValidator : AbstractValidator<CreateFormBuilderDto>
    {
        public CreateFormBuilderDtoValidator()
        {
            RuleFor(x => x.FormName)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.ForeignFormName)
                .MaximumLength(200)
                .When(x => !string.IsNullOrWhiteSpace(x.ForeignFormName));

            RuleFor(x => x.FormCode)
                .NotEmpty()
                .MaximumLength(100)
                .Matches("^[A-Za-z0-9_]+$")
                .WithMessage("Form code must be alphanumeric (underscores allowed).");

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.Description));

            RuleFor(x => x.ForeignDescription)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.ForeignDescription));

            RuleFor(x => x.SapExecutionMode)
                .Must(v => string.IsNullOrWhiteSpace(v) || v == "OnSubmit" || v == "OnFinalApproval" || v == "OnSpecificWorkflowStage")
                .WithMessage("SapExecutionMode must be OnSubmit, OnFinalApproval, or OnSpecificWorkflowStage.");
        }
    }
}
