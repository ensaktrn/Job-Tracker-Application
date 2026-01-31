using JobTracker.Domain.Entities;

namespace JobTracker.Application.Companies;

public sealed class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _repo;

    public CompanyService(ICompanyRepository repo) => _repo = repo;

    public async Task<CompanyDto> CreateAsync(CreateCompanyRequest request, CancellationToken ct)
    {
        var company = new Company(request.Name, request.Website);

        await _repo.AddAsync(company, ct);
        await _repo.SaveChangesAsync(ct);

        return new CompanyDto(company.Id, company.Name, company.Website, company.CreatedAt);
    }

    public async Task<IReadOnlyList<CompanyDto>> GetAllAsync(CancellationToken ct)
    {
        var companies = await _repo.GetAllAsync(ct);

        return companies
            .Select(c => new CompanyDto(c.Id, c.Name, c.Website, c.CreatedAt))
            .ToList();
    }
    
    public async Task<CompanyDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        if (id == Guid.Empty) return null;

        var company = await _repo.GetByIdAsync(id, ct);
        if (company is null) return null;

        return new CompanyDto(company.Id, company.Name, company.Website, company.CreatedAt);
    }

}