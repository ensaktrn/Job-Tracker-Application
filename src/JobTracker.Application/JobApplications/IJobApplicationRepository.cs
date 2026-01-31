using JobTracker.Domain.Entities;

namespace JobTracker.Application.JobApplications;

public interface IJobApplicationRepository
{
    Task AddAsync(JobApplication application, CancellationToken ct);
    Task<IReadOnlyList<JobApplication>> GetAllAsync(CancellationToken ct);
    Task<JobApplication?> GetByIdAsync(Guid id, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}