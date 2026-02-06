using JobTracker.Domain.Entities;

namespace JobTracker.Application.Companies;

public interface ICompanyRepository
{
    Task AddAsync(Company company, CancellationToken ct);
    Task<IReadOnlyList<CompanyDto>> GetAllAsync(CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
    Task<CompanyDto?> GetByIdAsync(Guid id, CancellationToken ct);

}