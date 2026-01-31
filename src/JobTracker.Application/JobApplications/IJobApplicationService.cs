using JobTracker.Application.Common;
namespace JobTracker.Application.JobApplications;

public interface IJobApplicationService
{
    Task<JobApplicationDto> CreateAsync(CreateJobApplicationRequest request, CancellationToken ct);
    Task<PagedResult<JobApplicationDto>> GetAsync(GetJobApplicationsQuery query, CancellationToken ct);
    Task<JobApplicationDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<JobApplicationDto> ChangeStatusAsync(Guid id, ChangeApplicationStatusRequest request, CancellationToken ct);
}