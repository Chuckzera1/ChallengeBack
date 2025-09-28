namespace ChallengeBack.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection HandleDependencyInjection(this IServiceCollection services)
    {
        // Add Infrastructure layer services (repositories)
        services.AddInfrastructureServices();
        
        // Add Application layer services (use cases)
        services.AddApplicationServices();

        return services;
    }
}
