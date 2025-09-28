using ChallengeBack.Domain.Entities;

namespace ChallengeBack.Application.Interfaces.Repositories;

public interface ICompanyRepository
{
    Task<Company> GetByIdAsync(int id);
    Task<IEnumerable<Company>> GetAllAsync();
    Task<Company> AddAsync(Company company, CancellationToken ct);
    Task<Company> UpdateAsync(Company company);
    Task DeleteAsync(int id);
}
