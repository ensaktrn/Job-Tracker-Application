namespace JobTracker.Application.JobPostings;

public sealed record CreateJobPostingRequest(
    Guid CompanyId,
    string Title,
    string Url,
    string? Notes
);