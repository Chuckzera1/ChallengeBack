using ChallengeBack.Application.Dto.CompanySupplier;
using ChallengeBack.Application.Interfaces.Services;
using ChallengeBack.Application.Services.Company;
using ChallengeBack.Application.Services.CompanySupplier;
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
        services.AddScoped<IGetAllSuppliersWithFilterService, GetAllSuppliersWithFilterService>();
        services.AddScoped<IUpdateSupplierService, UpdateSupplierService>();
        services.AddScoped<IDeleteSupplierService, DeleteSupplierService>();

        // CompanySupplier
        services.AddScoped<IAddCompanySupplierService, CreateCompanySupplierService>();
        services.AddScoped<IDeleteCompanySupplierService, DeleteCompanySupplierService>();
        services.AddScoped<IGetAllCompanySuppliersService, GetAllCompanySuppliersService>();

        return services;
    }
}
