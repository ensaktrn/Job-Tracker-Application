namespace JobTracker.Application.Companies;

public interface ICompanyService
{
    Task<CompanyDto> CreateAsync(CreateCompanyRequest request, CancellationToken ct);
    Task<IReadOnlyList<CompanyDto>> GetAllAsync(CancellationToken ct);
}