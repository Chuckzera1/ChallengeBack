using ChallengeBack.Application.Dto.CompanySupplier;
using ChallengeBack.Application.Dto.Base;
using ChallengeBack.Domain.Entities;

namespace ChallengeBack.Application.Interfaces.Repositories;

public interface ICompanySupplierRepository
{
    Task<CompanySupplier> AddAsync(CompanySupplier companySupplier, CancellationToken ct);
    Task<CompanySupplier> UpdateAsync(CompanySupplier companySupplier, CancellationToken ct);
    Task<CompanySupplier> DeleteAsync(int id, CancellationToken ct);
    Task<PagedResultDto<CompanySupplier>> GetAllWithFilterAsync(GetAllCompanySupplierFilterDto filter, CancellationToken ct);
}
