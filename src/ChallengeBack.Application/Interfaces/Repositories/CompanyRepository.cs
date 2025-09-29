using ChallengeBack.Domain.Entities;

namespace ChallengeBack.Application.Interfaces.Repositories;

public interface ICompanyRepository
{
    Task<Company> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<Company>> GetAllAsync(CancellationToken ct);
    Task<Company> AddAsync(Company company, CancellationToken ct);
    Task<Company> UpdateAsync(Company company, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
}
