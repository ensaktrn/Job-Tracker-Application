namespace JobTracker.Application.Companies;

public sealed record CompanyDto(
    Guid Id,
    string Name,
    string? Website,
    DateTimeOffset CreatedAt,
    string CreatedByEmail
);