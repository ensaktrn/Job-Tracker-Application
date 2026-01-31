using JobTracker.Domain.Enums;

namespace JobTracker.Application.JobApplications;

public sealed record GetJobApplicationsQuery(
    ApplicationStatus? Status,
    Guid? CompanyId,
    DateTimeOffset? From,
    DateTimeOffset? To,
    int Page = 1,
    int PageSize = 20,
    string? Sort = "appliedAt_desc"
);