using JobTracker.Application.Companies;
using JobTracker.Domain.Entities;
using JobTracker.Application.Common;

namespace JobTracker.Application.JobPostings;

public sealed class JobPostingService : IJobPostingService
{
    private readonly IJobPostingRepository _repo;
    private readonly ICompanyReadRepository _companyRead;
    private readonly ICurrentUser _currentUser;

    public JobPostingService(ICurrentUser currentUser, IJobPostingRepository repo, ICompanyReadRepository companyRead)
    {
        _repo = repo;
        _companyRead = companyRead;
        _currentUser = currentUser;
    }

    public async Task<JobPostingDto> CreateAsync(CreateJobPostingRequest request, CancellationToken ct)
    {
        if (request.CompanyId == Guid.Empty)
            throw new ArgumentException("CompanyId is required.", nameof(request.CompanyId));

        var companyExists = await _companyRead.ExistsAsync(request.CompanyId, ct);
        if (!companyExists)
            throw new InvalidOperationException("Company does not exist.");

        var posting = new JobPosting(_currentUser.UserId, request.CompanyId, request.Title, request.Url, request.Notes);

        await _repo.AddAsync(posting, ct);
        await _repo.SaveChangesAsync(ct);

        return new JobPostingDto(posting.Id, posting.CompanyId, posting.Title, posting.Url, posting.Notes, posting.CreatedAt, _currentUser.UserEmail);
    }

    public Task<IReadOnlyList<JobPostingDto>> GetAllAsync(CancellationToken ct)
        => _repo.GetAllAsync(ct);

    public Task<JobPostingDto?> GetByIdAsync(Guid id, CancellationToken ct)
        => _repo.GetByIdAsync(id, ct);
}