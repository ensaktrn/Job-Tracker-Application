using JobTracker.Domain.Entities;

namespace JobTracker.Application.JobPostings;

public interface IJobPostingRepository
{
    Task AddAsync(JobPosting posting, CancellationToken ct);
    Task<IReadOnlyList<JobPosting>> GetAllAsync(CancellationToken ct);
    Task<JobPosting?> GetByIdAsync(Guid id, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}