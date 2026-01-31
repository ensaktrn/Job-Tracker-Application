using JobTracker.Application.Companies;
using JobTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Infrastructure.Companies;

public sealed class CompanyReadRepository : ICompanyReadRepository
{
    private readonly AppDbContext _db;
    public CompanyReadRepository(AppDbContext db) => _db = db;

    public Task<bool> ExistsAsync(Guid id, CancellationToken ct)
        => _db.Companies.AsNoTracking().AnyAsync(x => x.Id == id, ct);
}