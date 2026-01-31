using JobTracker.Domain.Enums;

namespace JobTracker.Application.JobApplications;

public sealed record JobApplicationDto(
    Guid Id,
    Guid JobPostingId,
    ApplicationStatus Status,
    DateTimeOffset AppliedAt,
    DateTimeOffset LastUpdatedAt
);