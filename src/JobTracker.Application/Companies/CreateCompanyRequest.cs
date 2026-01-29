namespace JobTracker.Application.Companies;

public sealed record CreateCompanyRequest(
    string Name,
    string? Website
);