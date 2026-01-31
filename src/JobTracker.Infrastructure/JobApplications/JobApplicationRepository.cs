using JobTracker.Application.JobApplications;
using JobTracker.Domain.Entities;
using JobTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Infrastructure.JobApplications;

public sealed class JobApplicationRepository : IJobApplicationRepository
{
    private readonly AppDbContext _db;
    public JobApplicationRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(JobApplication application, CancellationToken ct)
        => await _db.Applications.AddAsync(application, ct);

    public async Task<IReadOnlyList<JobApplication>> GetAllAsync(CancellationToken ct)
        => await _db.Applications.AsNoTracking()
            .OrderByDescending(x => x.AppliedAt)
            .ToListAsync(ct);

    public async Task<JobApplication?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _db.Applications.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);
}