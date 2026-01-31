namespace JobTracker.Application.Companies;

public interface ICompanyReadRepository
{
    Task<bool> ExistsAsync(Guid id, CancellationToken ct);
}