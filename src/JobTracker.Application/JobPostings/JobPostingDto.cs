namespace JobTracker.Application.JobPostings;

public sealed record JobPostingDto(
    Guid Id,
    Guid CompanyId,
    string Title,
    string Url,
    string? Notes,
    DateTimeOffset CreatedAt
);