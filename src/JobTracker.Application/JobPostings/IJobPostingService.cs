namespace JobTracker.Application.JobPostings;

public interface IJobPostingService
{
    Task<JobPostingDto> CreateAsync(CreateJobPostingRequest request, CancellationToken ct);
    Task<IReadOnlyList<JobPostingDto>> GetAllAsync(CancellationToken ct);
    Task<JobPostingDto?> GetByIdAsync(Guid id, CancellationToken ct);
}