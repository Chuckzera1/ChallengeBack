namespace ChallengeBack.Application.Services.Company;

using ChallengeBack.Application.Interfaces.Repositories;
using ChallengeBack.Application.Interfaces.Services;

public class DeleteCompanyService : IDeleteCompanyService
{
    private readonly ICompanyRepository _companyRepository;

    public DeleteCompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task Execute(int id, CancellationToken ct)
    {
        await _companyRepository.DeleteAsync(id, ct);
    }
}