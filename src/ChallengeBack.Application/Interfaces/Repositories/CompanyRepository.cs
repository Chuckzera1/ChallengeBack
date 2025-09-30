using ChallengeBack.Application.Dto.Company;
using ChallengeBack.Application.Dto.Base;
using ChallengeBack.Domain.Entities;

namespace ChallengeBack.Application.Interfaces.Repositories;

public interface ICompanyRepository
{
    Task<Company> GetByIdAsync(int id, CancellationToken ct);
    Task<PagedResultDto<Company>> GetAllWithFilterAsync(GetAllCompanyFilterDto filter, CancellationToken ct);
    Task<Company> AddAsync(Company company, CancellationToken ct);
    Task<Company> UpdateAsync(Company company, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
}
