using JobTracker.Domain.Entities;

namespace JobTracker.Application.JobPostings;

public interface IJobPostingRepository
{
    Task AddAsync(JobPosting posting, CancellationToken ct);
    Task<IReadOnlyList<JobPostingDto>> GetAllAsync(CancellationToken ct);
    Task<JobPostingDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}