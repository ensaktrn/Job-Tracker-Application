using JobTracker.Application.Companies;
using JobTracker.Domain.Entities;

namespace JobTracker.Application.JobPostings;

public sealed class JobPostingService : IJobPostingService
{
    private readonly IJobPostingRepository _repo;
    private readonly ICompanyReadRepository _companyRead;

    public JobPostingService(IJobPostingRepository repo, ICompanyReadRepository companyRead)
    {
        _repo = repo;
        _companyRead = companyRead;
    }

    public async Task<JobPostingDto> CreateAsync(CreateJobPostingRequest request, CancellationToken ct)
    {
        if (request.CompanyId == Guid.Empty)
            throw new ArgumentException("CompanyId is required.", nameof(request.CompanyId));

        var companyExists = await _companyRead.ExistsAsync(request.CompanyId, ct);
        if (!companyExists)
            throw new InvalidOperationException("Company does not exist.");

        var posting = new JobPosting(request.CompanyId, request.Title, request.Url, request.Notes);

        await _repo.AddAsync(posting, ct);
        await _repo.SaveChangesAsync(ct);

        return new JobPostingDto(posting.Id, posting.CompanyId, posting.Title, posting.Url, posting.Notes, posting.CreatedAt);
    }

    public async Task<IReadOnlyList<JobPostingDto>> GetAllAsync(CancellationToken ct)
    {
        var postings = await _repo.GetAllAsync(ct);

        return postings.Select(p =>
            new JobPostingDto(p.Id, p.CompanyId, p.Title, p.Url, p.Notes, p.CreatedAt)
        ).ToList();
    }

    public async Task<JobPostingDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        if (id == Guid.Empty) return null;

        var posting = await _repo.GetByIdAsync(id, ct);
        if (posting is null) return null;

        return new JobPostingDto(posting.Id, posting.CompanyId, posting.Title, posting.Url, posting.Notes, posting.CreatedAt);
    }
}