using JobTracker.Domain.Entities;

namespace JobTracker.Application.Companies;

public interface ICompanyRepository
{
    Task AddAsync(Company company, CancellationToken ct);
    Task<IReadOnlyList<Company>> GetAllAsync(CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
    Task<Company?> GetByIdAsync(Guid id, CancellationToken ct);

}