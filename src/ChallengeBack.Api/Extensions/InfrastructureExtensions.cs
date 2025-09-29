using ChallengeBack.Application.Interfaces.Repositories;
using ChallengeBack.Infrastructure.Repositories;
using ChallengeBack.Infrastructure.CepApi;

namespace ChallengeBack.Api.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Register repositories
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<ICepRepository, CepRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<ICompanySupplierRepository, CompanySupplierRepository>();

        return services;
    }
}
