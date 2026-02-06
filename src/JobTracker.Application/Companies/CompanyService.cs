using JobTracker.Domain.Entities;
using JobTracker.Application.Common;

namespace JobTracker.Application.Companies;

public sealed class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _repo;
    private readonly ICurrentUser _currentUser;

    public CompanyService(ICompanyRepository repo, ICurrentUser currentUser)
    {
      _repo = repo;
      _currentUser = currentUser;
    } 
    public async Task<IReadOnlyList<CompanyDto>> GetAllAsync(CancellationToken ct)
        => await _repo.GetAllAsync(ct);

    public async Task<CompanyDto?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _repo.GetByIdAsync(id, ct);
    public async Task<CompanyDto> CreateAsync(CreateCompanyRequest request, CancellationToken ct)
    {
        var company = new Company( _currentUser.UserId, request.Name, request.Website);

        await _repo.AddAsync(company, ct);
        await _repo.SaveChangesAsync(ct);

        return new CompanyDto(company.Id, company.Name, company.Website, company.CreatedAt, _currentUser.UserEmail);
    }

}