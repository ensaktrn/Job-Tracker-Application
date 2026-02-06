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

    public async Task<JobApplicationDto?> GetByIdAsync(string userId, Guid id, CancellationToken ct)
    {
        return await (
            from a in _db.Applications.AsNoTracking()
            join p in _db.JobPostings.AsNoTracking() on a.JobPostingId equals p.Id
            join c in _db.Companies.AsNoTracking() on p.CompanyId equals c.Id
            where a.Id == id && a.UserId == userId
            select new JobApplicationDto(
                a.Id,
                a.Status,
                a.AppliedAt,
                a.LastUpdatedAt,
                p.Id,
                p.Title,
                p.Url,
                c.Id,
                c.Name
            )
        ).FirstOrDefaultAsync(ct);
    }
    public async Task<Domain.Entities.JobApplication?> GetEntityByIdAsync(string userId, Guid id, CancellationToken ct)
    {
        return await _db.Applications
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, ct);
    }
    public Task SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);
    public async Task<PagedResult<JobApplicationDto>> QueryAsync(
        string userId,
        GetJobApplicationsQuery q,
        CancellationToken ct)
    {
        var page = q.Page < 1 ? 1 : q.Page;
        var pageSize = Math.Min(q.PageSize <= 0 ? 20 : q.PageSize, 100);

        var query =
            from a in _db.Applications.AsNoTracking()
            join p in _db.JobPostings.AsNoTracking() on a.JobPostingId equals p.Id
            join c in _db.Companies.AsNoTracking() on p.CompanyId equals c.Id
            where a.UserId == userId
            select new { a, p, c };

        if (q.Status is not null)
            query = query.Where(x => x.a.Status == q.Status);

        if (q.From is not null)
            query = query.Where(x => x.a.AppliedAt >= q.From);

        if (q.To is not null)
            query = query.Where(x => x.a.AppliedAt <= q.To);

        query = (q.Sort ?? "appliedAt_desc").ToLowerInvariant() switch
        {
            "appliedat_asc" => query.OrderBy(x => x.a.AppliedAt),
            "appliedat_desc" => query.OrderByDescending(x => x.a.AppliedAt),
            _ => query.OrderByDescending(x => x.a.AppliedAt)
        };

        var total = await query.CountAsync(ct);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new JobApplicationDto(
                x.a.Id,
                x.a.Status,
                x.a.AppliedAt,
                x.a.LastUpdatedAt,
                x.p.Id,
                x.p.Title,
                x.p.Url,
                x.c.Id,
                x.c.Name
            ))
            .ToListAsync(ct);

        return new PagedResult<JobApplicationDto>(items, page, pageSize, total);
    }
}