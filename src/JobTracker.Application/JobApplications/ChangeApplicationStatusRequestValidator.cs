using FluentValidation;

namespace JobTracker.Application.JobApplications;

public sealed class ChangeApplicationStatusRequestValidator : AbstractValidator<ChangeApplicationStatusRequest>
{
    public ChangeApplicationStatusRequestValidator()
    {
        RuleFor(x => x.Status).IsInEnum();
    }
}