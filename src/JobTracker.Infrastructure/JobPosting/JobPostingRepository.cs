using JobTracker.Application.JobPostings;
using JobTracker.Domain.Entities;
using JobTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Infrastructure.JobPostings;

public sealed class JobPostingRepository : IJobPostingRepository
{
    private readonly AppDbContext _db;
    public JobPostingRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(JobPosting posting, CancellationToken ct)
        => await _db.JobPostings.AddAsync(posting, ct);

    public async Task<IReadOnlyList<JobPostingDto>> GetAllAsync(CancellationToken ct)
    {
        return await (
            from p in _db.JobPostings.AsNoTracking()
            join u in _db.Users.AsNoTracking() on p.UserId equals u.Id
            orderby p.CreatedAt descending
            select new JobPostingDto(
                p.Id,
                p.CompanyId,
                p.Title,
                p.Url,
                p.Notes,
                p.CreatedAt,
                u.Email ?? "(no email)"
            )
        ).ToListAsync(ct);
    }

    public async Task<JobPostingDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await (
            from p in _db.JobPostings.AsNoTracking()
            join u in _db.Users.AsNoTracking() on p.UserId equals u.Id
            where p.Id == id
            select new JobPostingDto(
                p.Id,
                p.CompanyId,
                p.Title,
                p.Url,
                p.Notes,
                p.CreatedAt,
                u.Email ?? "(no email)"
            )
        ).FirstOrDefaultAsync(ct);
    }

    public Task SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);
}