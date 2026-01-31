using JobTracker.Application.Common;

namespace JobTracker.Application.JobApplications;

public interface IJobApplicationRepository
{
    Task AddAsync(Domain.Entities.JobApplication application, CancellationToken ct);
    Task<Domain.Entities.JobApplication?> GetByIdAsync(Guid id, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);

    Task<PagedResult<JobApplicationDto>> QueryAsync(GetJobApplicationsQuery query, CancellationToken ct);
}
