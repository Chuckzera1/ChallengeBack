using ChallengeBack.Domain.Entities;
using ChallengeBack.Application.Dto.Company;

namespace ChallengeBack.Application.Interfaces.Services;

public interface ICreateCompanyService
{
    Task<Company> Execute(CreateCompanyDto createCompanyDto, CancellationToken ct);
}

public interface IDeleteCompanyService
{
    Task Execute(int id, CancellationToken ct);
}

public interface IGetAllCompaniesService
{
    Task<IEnumerable<Company>> Execute(CancellationToken ct);
}

public interface IUpdateCompanyService 
{
    Task<Company> Execute(int id, UpdateCompanyDto updateCompanyDto, CancellationToken ct);
}

