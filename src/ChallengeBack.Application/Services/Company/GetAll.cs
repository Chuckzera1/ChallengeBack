using ChallengeBack.Application.Dto.Company;
using ChallengeBack.Application.Dto.Base;
using ChallengeBack.Application.Interfaces.Repositories;
using ChallengeBack.Application.Interfaces.Services;
using ChallengeBack.Domain.Entities;
using CompanyEntity = ChallengeBack.Domain.Entities.Company;

namespace ChallengeBack.Application.Services.Company;

public class GetAllCompaniesService : IGetAllCompaniesService {
    private readonly ICompanyRepository _companyRepository;

    public GetAllCompaniesService(ICompanyRepository companyRepository) {
        _companyRepository = companyRepository;
    }

    public async Task<PagedResultDto<CompanyEntity>> Execute(GetAllCompanyFilterDto filter, CancellationToken ct) {
        return await _companyRepository.GetAllWithFilterAsync(filter, ct);
    }
}