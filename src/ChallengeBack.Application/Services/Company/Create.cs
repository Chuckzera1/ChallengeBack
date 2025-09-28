namespace ChallengeBack.Application.Services.Company;

using ChallengeBack.Application.Interfaces.Repositories;
using ChallengeBack.Application.Interfaces.Services;
using ChallengeBack.Domain.Entities;
using ChallengeBack.Application.Dto.Company;

public class CreateCompanyService : ICreateCompanyService
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ICepRepository _cepRepository;

    public CreateCompanyService(ICompanyRepository companyRepository, ICepRepository cepRepository)
    {
        _companyRepository = companyRepository;
        _cepRepository = cepRepository;
    }

    public async Task<Company> Execute(CreateCompanyDto createCompanyDto, CancellationToken ct)
    {
        var state = await _cepRepository.GetByZipCodeAsync(createCompanyDto.ZipCode) ?? throw new Exception("CEP não encontrado");
        var company = new Company
        {
            Cnpj = createCompanyDto.Cnpj,
            FantasyName = createCompanyDto.FantasyName,
            ZipCode = createCompanyDto.ZipCode,
            State = state,
        };

        return await _companyRepository.AddAsync(company, ct);
    }
}
