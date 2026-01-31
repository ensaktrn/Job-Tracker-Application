using JobTracker.Application.Common;
using JobTracker.Application.JobApplications;
using JobTracker.Infrastructure.Data;
using JobTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace JobTracker.Infrastructure.JobApplications;

public sealed class JobApplicationRepository : IJobApplicationRepository
{
    private readonly AppDbContext _db;
    public JobApplicationRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(JobApplication application, CancellationToken ct)
        => await _db.Applications.AddAsync(application, ct);

    public async Task<JobApplication?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _db.Applications.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);
    public async Task<PagedResult<JobApplicationDto>> QueryAsync(GetJobApplicationsQuery q, CancellationToken ct)
    {
        var page = q.Page < 1 ? 1 : q.Page;
        var pageSize = q.PageSize is < 1 ? 20 : q.PageSize;
        pageSize = Math.Min(pageSize, 100); // hard cap

        // JobApplication -> JobPosting -> Company join (CompanyId filtresi için)
        var query = _db.Applications
            .AsNoTracking()
            .Include(x => x.JobPosting)
            .AsQueryable();

        if (q.Status is not null)
            query = query.Where(x => x.Status == q.Status);

        if (q.From is not null)
            query = query.Where(x => x.AppliedAt >= q.From);

        if (q.To is not null)
            query = query.Where(x => x.AppliedAt <= q.To);

        if (q.CompanyId is not null)
            query = query.Where(x => x.JobPosting != null && x.JobPosting.CompanyId == q.CompanyId);

        // Sorting whitelist
        query = (q.Sort ?? "appliedAt_desc").ToLowerInvariant() switch
        {
            "appliedat_asc" => query.OrderBy(x => x.AppliedAt),
            "appliedat_desc" => query.OrderByDescending(x => x.AppliedAt),
            "updatedat_asc" => query.OrderBy(x => x.LastUpdatedAt),
            "updatedat_desc" => query.OrderByDescending(x => x.LastUpdatedAt),
            _ => query.OrderByDescending(x => x.AppliedAt)
        };

        var total = await query.CountAsync(ct);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new JobApplicationDto(
                x.Id,
                x.JobPostingId,
                x.Status,
                x.AppliedAt,
                x.LastUpdatedAt
            ))
            .ToListAsync(ct);

        return new PagedResult<JobApplicationDto>(items, page, pageSize, total);
    }
}