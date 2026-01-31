namespace JobTracker.Application.JobApplications;

public interface IJobApplicationService
{
    Task<JobApplicationDto> CreateAsync(CreateJobApplicationRequest request, CancellationToken ct);
    Task<IReadOnlyList<JobApplicationDto>> GetAllAsync(CancellationToken ct);
    Task<JobApplicationDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<JobApplicationDto> ChangeStatusAsync(Guid id, ChangeApplicationStatusRequest request, CancellationToken ct);
}