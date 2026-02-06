using JobTracker.Application.Common;

namespace JobTracker.Application.JobApplications;

public interface IJobApplicationRepository
{
    Task AddAsync(Domain.Entities.JobApplication application, CancellationToken ct);
    Task<JobApplicationDto?> GetByIdAsync(string userId, Guid id, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
    Task<Domain.Entities.JobApplication?> GetEntityByIdAsync(string userId, Guid id, CancellationToken ct);
    Task<PagedResult<JobApplicationDto>> QueryAsync(string userId, GetJobApplicationsQuery query, CancellationToken ct);
}
