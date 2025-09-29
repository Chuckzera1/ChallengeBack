using ChallengeBack.Application.Dto.CompanySupplier;
using ChallengeBack.Application.Dto.Base;
using ChallengeBack.Application.Interfaces.Repositories;
using ChallengeBack.Application.Interfaces.Services;
using CompanySupplierEntity = ChallengeBack.Domain.Entities.CompanySupplier;

namespace ChallengeBack.Application.Services.CompanySupplier;

public class GetAllCompanySuppliersService : IGetAllCompanySuppliersService
{
    private readonly ICompanySupplierRepository _companySupplierRepository;

    public GetAllCompanySuppliersService(ICompanySupplierRepository companySupplierRepository)
    {
        _companySupplierRepository = companySupplierRepository;
    }

    public async Task<PagedResultDto<CompanySupplierEntity>> Execute(GetAllCompanySupplierFilterDto filter, CancellationToken ct)
    {
        return await _companySupplierRepository.GetAllWithFilterAsync(filter, ct);
    }
}
