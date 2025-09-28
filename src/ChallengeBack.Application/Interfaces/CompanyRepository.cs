using ChallengeBack.Domain.Entities;

namespace ChallengeBack.Application.Interfaces;

public interface ICompanyRepository
{
    Task<Company> GetByIdAsync(int id);
    Task<IEnumerable<Company>> GetAllAsync();
    Task<Company> AddAsync(Company company);
    Task<Company> UpdateAsync(Company company);
    Task<Company> DeleteAsync(int id);
}
