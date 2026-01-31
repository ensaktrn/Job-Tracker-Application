using FluentValidation;

namespace JobTracker.Application.JobApplications;

public sealed class CreateJobApplicationRequestValidator : AbstractValidator<CreateJobApplicationRequest>
{
    public CreateJobApplicationRequestValidator()
    {
        RuleFor(x => x.JobPostingId).NotEmpty();
    }
}