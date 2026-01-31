using JobTracker.Application.Common;
using JobTracker.Application.JobPostings;
using JobTracker.Domain.Entities;

namespace JobTracker.Application.JobApplications;

public sealed class JobApplicationService : IJobApplicationService
{
    private readonly IJobApplicationRepository _repo;
    private readonly IJobPostingReadRepository _jobPostingRead;

    public JobApplicationService(IJobApplicationRepository repo, IJobPostingReadRepository jobPostingRead)
    {
        _repo = repo;
        _jobPostingRead = jobPostingRead;
    }

    public async Task<JobApplicationDto> CreateAsync(CreateJobApplicationRequest request, CancellationToken ct)
    {
        if (request.JobPostingId == Guid.Empty)
            throw new ArgumentException("JobPostingId is required.", nameof(request.JobPostingId));

        var exists = await _jobPostingRead.ExistsAsync(request.JobPostingId, ct);
        if (!exists)
            throw new InvalidOperationException("Job posting does not exist.");

        var app = new JobApplication(request.JobPostingId);

        await _repo.AddAsync(app, ct);
        await _repo.SaveChangesAsync(ct);

        return ToDto(app);
    }

    public Task<PagedResult<JobApplicationDto>> GetAsync(GetJobApplicationsQuery query, CancellationToken ct)
        => _repo.QueryAsync(query, ct);

    public async Task<JobApplicationDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        if (id == Guid.Empty) return null;

        var app = await _repo.GetByIdAsync(id, ct);
        return app is null ? null : ToDto(app);
    }

    public async Task<JobApplicationDto> ChangeStatusAsync(Guid id, ChangeApplicationStatusRequest request, CancellationToken ct)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id is required.", nameof(id));

        var app = await _repo.GetByIdAsync(id, ct);
        if (app is null)
            throw new InvalidOperationException("Application not found.");

        app.ChangeStatus(request.Status); // domain rule burada

        await _repo.SaveChangesAsync(ct);

        return ToDto(app);
    }

    private static JobApplicationDto ToDto(JobApplication app)
        => new(app.Id, app.JobPostingId, app.Status, app.AppliedAt, app.LastUpdatedAt);
}
