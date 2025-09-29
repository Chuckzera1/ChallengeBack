using ChallengeBack.Application.Dto.Supplier;
using ChallengeBack.Domain.Entities;

namespace ChallengeBack.Application.Interfaces.Repositories;

public interface ISupplierRepository
{
    Task<Supplier> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<Supplier>> GetAllWithFilterAsync(GetAllSupplierFilterDto filter, CancellationToken ct);
    Task<Supplier> AddAsync(Supplier supplier, CancellationToken ct);
    Task<Supplier> UpdateAsync(Supplier supplier, CancellationToken ct);
    Task<Supplier> DeleteAsync(int id, CancellationToken ct);
}
