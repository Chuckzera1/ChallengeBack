using ChallengeBack.Domain.Entities;
using ChallengeBack.Application.Dto.Company;
using ChallengeBack.Application.Dto.Base;

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
    Task<PagedResultDto<Company>> Execute(GetAllCompanyFilterDto filter, CancellationToken ct);
}

public interface IUpdateCompanyService 
{
    Task<Company> Execute(int id, UpdateCompanyDto updateCompanyDto, CancellationToken ct);
}

