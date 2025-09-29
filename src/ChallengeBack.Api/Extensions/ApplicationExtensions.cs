using ChallengeBack.Application.Interfaces.Services;
using ChallengeBack.Application.Services.Company;
using ChallengeBack.Application.Services.Supplier;

namespace ChallengeBack.Api.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register services (use cases)

        // Company
        services.AddScoped<ICreateCompanyService, CreateCompanyService>();
        services.AddScoped<IDeleteCompanyService, DeleteCompanyService>();
        services.AddScoped<IGetAllCompaniesService, GetAllCompaniesService>();
        services.AddScoped<IUpdateCompanyService, UpdateCompanyService>();

        // Supplier
        services.AddScoped<ICreateSupplierService, CreateSupplierService>();
        // TODO: Add other services when they are created
        // services.AddScoped<IGetCompanyService, GetCompanyService>();
        // services.AddScoped<IUpdateCompanyService, UpdateCompanyService>();
        // services.AddScoped<IDeleteCompanyService, DeleteCompanyService>();

        return services;
    }
}
