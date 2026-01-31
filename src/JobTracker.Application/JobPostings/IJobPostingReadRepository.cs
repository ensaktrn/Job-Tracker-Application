namespace JobTracker.Application.JobPostings;

public interface IJobPostingReadRepository
{
    Task<bool> ExistsAsync(Guid id, CancellationToken ct);
}