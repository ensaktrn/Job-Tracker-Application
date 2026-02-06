using JobTracker.Domain.Enums;

namespace JobTracker.Application.JobApplications;

public sealed record JobApplicationDto(
    Guid Id,
    ApplicationStatus Status,
    DateTimeOffset AppliedAt,
    DateTimeOffset? LastUpdatedAt,
    Guid JobPostingId,
    string JobTitle,
    string JobUrl,
    Guid CompanyId,
    string CompanyName
);