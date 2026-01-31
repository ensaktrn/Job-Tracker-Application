using JobTracker.Application.JobPostings;
using JobTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Infrastructure.JobPostings;

public sealed class JobPostingReadRepository : IJobPostingReadRepository
{
    private readonly AppDbContext _db;
    public JobPostingReadRepository(AppDbContext db) => _db = db;

    public Task<bool> ExistsAsync(Guid id, CancellationToken ct)
        => _db.JobPostings.AsNoTracking().AnyAsync(x => x.Id == id, ct);
}