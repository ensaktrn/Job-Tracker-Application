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

    public async Task<IReadOnlyList<JobPosting>> GetAllAsync(CancellationToken ct)
        => await _db.JobPostings.AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(ct);

    public async Task<JobPosting?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _db.JobPostings.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);
}