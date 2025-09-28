using ChallengeBack.Domain.Entities;

namespace ChallengeBack.Application.Interfaces;

public interface ICompanySupplierRepository
{
    Task<CompanySupplier> GetByIdAsync(int id);
    Task<IEnumerable<CompanySupplier>> GetAllAsync();
    Task<CompanySupplier> AddAsync(CompanySupplier companySupplier);
    Task<CompanySupplier> UpdateAsync(CompanySupplier companySupplier);
    Task<CompanySupplier> DeleteAsync(int id);
}
