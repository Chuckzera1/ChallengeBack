using ChallengeBack.Domain.Entities;

namespace ChallengeBack.Application.Interfaces;

public interface ISupplierRepository
{
    Task<Supplier> GetByIdAsync(int id);
    Task<IEnumerable<Supplier>> GetAllAsync();
    Task<Supplier> AddAsync(Supplier supplier);
    Task<Supplier> UpdateAsync(Supplier supplier);
    Task<Supplier> DeleteAsync(int id);
}
