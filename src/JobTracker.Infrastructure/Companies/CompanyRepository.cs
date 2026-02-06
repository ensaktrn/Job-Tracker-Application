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

    public async Task<IReadOnlyList<CompanyDto>> GetAllAsync(CancellationToken ct)
    {
        return await (
            from c in _db.Companies.AsNoTracking()
            join u in _db.Users.AsNoTracking() on c.UserId equals u.Id
            orderby c.Name
            select new CompanyDto(
                c.Id,
                c.Name,
                c.Website,
                c.CreatedAt,
                u.Email ?? "(no email)"
            )
        ).ToListAsync(ct);
    }

    public Task SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);
    
    public async Task<CompanyDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await (
            from c in _db.Companies.AsNoTracking()
            join u in _db.Users.AsNoTracking() on c.UserId equals u.Id
            where c.Id == id
            select new CompanyDto(
                c.Id,
                c.Name,
                c.Website,
                c.CreatedAt,
                u.Email ?? "(no email)"
            )
        ).FirstOrDefaultAsync(ct);
    }

}
