using ChallengeBack.Application.Dto.Company;
using ChallengeBack.Application.Interfaces.Repositories;
using ChallengeBack.Application.Interfaces.Services;
using CompanyEntity = ChallengeBack.Domain.Entities.Company;

namespace ChallengeBack.Application.Services.Company;

public class UpdateCompanyService : IUpdateCompanyService {
    private readonly ICompanyRepository _companyRepository;

    public UpdateCompanyService(ICompanyRepository companyRepository) {
        _companyRepository = companyRepository;
    }

    public async Task<CompanyEntity> Execute(int id, UpdateCompanyDto updateCompanyDto, CancellationToken ct) {
        var company = await _companyRepository.GetByIdAsync(id, ct);
        company.FantasyName = updateCompanyDto.FantasyName;
        company.UpdatedAt = DateTime.UtcNow;
        
        return await _companyRepository.UpdateAsync(company, ct);
    }
}