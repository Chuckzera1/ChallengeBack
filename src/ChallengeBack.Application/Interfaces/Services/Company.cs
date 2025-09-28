using ChallengeBack.Domain.Entities;
using ChallengeBack.Application.Dto.Company;

namespace ChallengeBack.Application.Interfaces.Services;

public interface ICreateCompanyService
{
    Task<Company> Execute(CreateCompanyDto createCompanyDto, CancellationToken ct);
}
