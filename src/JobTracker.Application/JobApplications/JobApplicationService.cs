using JobTracker.Application.Common;
using JobTracker.Application.JobPostings;
using JobTracker.Domain.Entities;
using JobTracker.Application.Common;

namespace JobTracker.Application.JobApplications;

public sealed class JobApplicationService : IJobApplicationService
{
    private readonly IJobApplicationRepository _repo;
    private readonly IJobPostingReadRepository _jobPostingRead;
    private readonly ICurrentUser _currentUser;

    public JobApplicationService(ICurrentUser currentUser, IJobApplicationRepository repo, IJobPostingReadRepository jobPostingRead)
    {
        _repo = repo;
        _jobPostingRead = jobPostingRead;
        _currentUser = currentUser;
    }

    public async Task<JobApplicationDto> CreateAsync(CreateJobApplicationRequest request, CancellationToken ct)
    {
        if (request.JobPostingId == Guid.Empty)
            throw new ArgumentException("JobPostingId is required.", nameof(request.JobPostingId));

        var exists = await _jobPostingRead.ExistsAsync(request.JobPostingId, ct);
        if (!exists)
            throw new InvalidOperationException("Job posting does not exist.");

        var app = new JobApplication(_currentUser.UserId, request.JobPostingId);

        await _repo.AddAsync(app, ct);
        await _repo.SaveChangesAsync(ct);

        // ✅ join’li “rich DTO” döndür
        var dto = await _repo.GetByIdAsync(_currentUser.UserId, app.Id, ct);
        if (dto is null)
            throw new InvalidOperationException("Application not found.");

        return dto;
    }

    public async Task<PagedResult<JobApplicationDto>> GetAsync(
        GetJobApplicationsQuery query,
        CancellationToken ct)
    {
        return await _repo.QueryAsync(_currentUser.UserId, query, ct);
    }

    public async Task<JobApplicationDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _repo.GetByIdAsync(_currentUser.UserId, id, ct);
    }

    public async Task<JobApplicationDto> ChangeStatusAsync(Guid id, ChangeApplicationStatusRequest request, CancellationToken ct)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id is required.", nameof(id));

        // ✅ user-scoped + tracked entity
        var app = await _repo.GetEntityByIdAsync(_currentUser.UserId, id, ct);
        if (app is null)
            throw new InvalidOperationException("Application not found.");

        app.ChangeStatus(request.Status);

        await _repo.SaveChangesAsync(ct);

        // ✅ response için rich DTO (join)
        var dto = await _repo.GetByIdAsync(_currentUser.UserId, id, ct);
        if (dto is null)
            throw new InvalidOperationException("Application not found.");

        return dto;
    }
}
