using JobTracker.Application.Companies;
using JobTracker.Domain.Entities;
using JobTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Infrastructure.Companies;

public sealed class CompanyRepository : ICompanyRepository
{
    private readonly AppDbContext _db;

    public CompanyRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(Company company, CancellationToken ct)
        => await _db.Companies.AddAsync(company, ct);

    public async Task<IReadOnlyList<Company>> GetAllAsync(CancellationToken ct)
        => await _db.Companies.AsNoTracking().OrderBy(x => x.Name).ToListAsync(ct);

    public async Task SaveChangesAsync(CancellationToken ct)
        => await _db.SaveChangesAsync(ct);
}