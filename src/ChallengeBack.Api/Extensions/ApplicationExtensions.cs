using ChallengeBack.Application.Interfaces.Services;
using ChallengeBack.Application.Services.Company;

namespace ChallengeBack.Api.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register services (use cases)
        services.AddScoped<ICreateCompanyService, CreateCompanyService>();
        
        // TODO: Add other services when they are created
        // services.AddScoped<IGetCompanyService, GetCompanyService>();
        // services.AddScoped<IUpdateCompanyService, UpdateCompanyService>();
        // services.AddScoped<IDeleteCompanyService, DeleteCompanyService>();

        return services;
    }
}
