using ChallengeBack.Application.Dto.Company;
using ChallengeBack.Application.Dto.Base;
using ChallengeBack.Domain.Entities;
using ChallengeBack.Domain.Enums;
using ChallengeBack.Infrastructure.Data;
using ChallengeBack.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChallengeBack.Tests.Repositories;

public class CompanyRepositoryTests : IClassFixture<PostgresFixture>
{
    private readonly PostgresFixture _postgresFixture;
    private readonly ApplicationDbContext _context;
    private readonly CompanyRepository _repository;

    public CompanyRepositoryTests(PostgresFixture postgresFixture)
    {
        _postgresFixture = postgresFixture;
        
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(_postgresFixture.ConnectionString)
            .Options;
            
        _context = new ApplicationDbContext(options);
        _repository = new CompanyRepository(_context);
        
        _context.Database.EnsureCreated();
    }

    private async Task CleanDatabaseAsync()
    {
        _context.Companies.RemoveRange(_context.Companies);
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetByIdAsync_WhenCompanyExists_ShouldReturnCompany()
    {
        await CleanDatabaseAsync();
        
        var company = new Company
        {
            Cnpj = "12345678000195",
            FantasyName = "Test Company",
            ZipCode = "12345678",
            State = "SP"
        };

        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(company.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(company.Id, result.Id);
        Assert.Equal(company.Cnpj, result.Cnpj);
        Assert.Equal(company.FantasyName, result.FantasyName);
        Assert.Equal(company.ZipCode, result.ZipCode);
        Assert.Equal(company.State, result.State);
        Assert.True(result.CreatedAt != DateTime.MinValue);
        Assert.True(result.UpdatedAt != DateTime.MinValue);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCompanyDoesNotExist_ShouldThrowException()
    {
        await CleanDatabaseAsync();
        
        var nonExistentId = 999;

        var exception = await Assert.ThrowsAsync<Exception>(() => _repository.GetByIdAsync(nonExistentId, CancellationToken.None));
        Assert.Equal("Company not found", exception.Message);
    }

    [Fact]
    public async Task InsertAsync_ShouldAddCompany()
    {
        await CleanDatabaseAsync();
        
        var company = new Company
        {
            Cnpj = "12345678000195",
            FantasyName = "Test Company",
            ZipCode = "12345678",
            State = "SP",
        };

        var result = await _repository.AddAsync(company, CancellationToken.None);

        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal(company.Cnpj, result.Cnpj);
        Assert.Equal(company.FantasyName, result.FantasyName);
        Assert.Equal(company.ZipCode, result.ZipCode);
        Assert.Equal(company.State, result.State);
        Assert.True(result.CreatedAt != DateTime.MinValue);
        Assert.True(result.UpdatedAt != DateTime.MinValue);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCompany_WhenCompanyExists()
    {
        await CleanDatabaseAsync();
        
       var company = new Company
        {
            Cnpj = "12345678000195",
            FantasyName = "Test Company",
            ZipCode = "12345678",
            State = "SP",
        };

        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();

        company.FantasyName = "Updated Company";

        var result = await _repository.UpdateAsync(company, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(company.Id, result.Id);
        Assert.Equal(company.Cnpj, result.Cnpj);
        Assert.Equal(company.FantasyName, result.FantasyName);
        Assert.Equal(company.ZipCode, result.ZipCode);
        Assert.Equal(company.State, result.State);
        Assert.True(result.UpdatedAt != DateTime.MinValue);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteCompany_WhenCompanyExists()
    {
        await CleanDatabaseAsync();
        
        var company = new Company
        {
            Cnpj = "12345678000195",
            FantasyName = "Test Company",
            ZipCode = "12345678",
            State = "SP",
        };

        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();

        await _repository.DeleteAsync(company.Id, CancellationToken.None);

        var result = await _context.Companies.FindAsync(company.Id);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllWithFilterAsync_ShouldReturnEmpty_WhenNoCompaniesExist()
    {
        await CleanDatabaseAsync();
        
        var filter = new GetAllCompanyFilterDto { Page = 1, Limit = 10 };
        var result = await _repository.GetAllWithFilterAsync(filter, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(result.Data);
        Assert.Equal(0, result.TotalCount);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.Limit);
    }

    [Fact]
    public async Task GetAllWithFilterAsync_ShouldReturnAllCompanies_WhenCompaniesExist()
    {
        await CleanDatabaseAsync();
        
        var companies = new List<Company>
        {
            new Company
            {
                Cnpj = "12345678000195",
                FantasyName = "Company 1",
                ZipCode = "12345678",
                State = "SP"
            },
            new Company
            {
                Cnpj = "98765432000123",
                FantasyName = "Company 2",
                ZipCode = "87654321",
                State = "RJ"
            }
        };

        _context.Companies.AddRange(companies);
        await _context.SaveChangesAsync();

        var filter = new GetAllCompanyFilterDto { Page = 1, Limit = 10 };
        var result = await _repository.GetAllWithFilterAsync(filter, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result.Data.Count());
        Assert.Equal(2, result.TotalCount);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.Limit);
        Assert.Contains(result.Data, c => c.Cnpj == "12345678000195");
        Assert.Contains(result.Data, c => c.Cnpj == "98765432000123");
    }

    [Fact]
    public async Task AddAsync_WithCancellationToken_ShouldRespectCancellation()
    {
        await CleanDatabaseAsync();
        
        var company = new Company
        {
            Cnpj = "12345678000195",
            FantasyName = "Test Company",
            ZipCode = "12345678",
            State = "SP",
        };

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() => 
            _repository.AddAsync(company, cts.Token));
    }

    [Fact]
    public async Task UpdateAsync_WhenCompanyDoesNotExist_ShouldThrowException()
    {
        await CleanDatabaseAsync();
        
        var company = new Company
        {
            Id = 999,
            Cnpj = "12345678000195",
            FantasyName = "Test Company",
            ZipCode = "12345678",
            State = "SP",
        };

        var exception = await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => 
            _repository.UpdateAsync(company, CancellationToken.None));
        
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task DeleteAsync_WhenCompanyDoesNotExist_ShouldThrowException()
    {
        await CleanDatabaseAsync();
        
        var nonExistentId = 999;

        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _repository.DeleteAsync(nonExistentId, CancellationToken.None));
        
        Assert.Equal("Company not found", exception.Message);
    }

    [Fact]
    public async Task AddAsync_WithDuplicateCnpj_ShouldThrowException()
    {
        await CleanDatabaseAsync();
        
        var company1 = new Company
        {
            Cnpj = "12345678000195",
            FantasyName = "Company 1",
            ZipCode = "12345678",
            State = "SP"
        };

        var company2 = new Company
        {
            Cnpj = "12345678000195",
            FantasyName = "Company 2",
            ZipCode = "87654321",
            State = "RJ"
        };

        await _repository.AddAsync(company1, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<DbUpdateException>(() => 
            _repository.AddAsync(company2, CancellationToken.None));
        
        Assert.Contains("duplicate key", exception.InnerException?.Message?.ToLower() ?? "");
    }

    [Fact]
    public async Task GetAllWithFilterAsync_ShouldIncludeCompanySuppliers_WhenCompaniesHaveSuppliers()
    {
        await CleanDatabaseAsync();
        
        var supplier = new Supplier
        {
            Type = PersonType.Company,
            Cnpj = "98765432000123",
            Name = "Test Supplier",
            Email = "supplier@test.com",
            ZipCode = "87654321"
        };
        
        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();
        
        var company = new Company
        {
            Cnpj = "12345678000195",
            FantasyName = "Test Company",
            ZipCode = "12345678",
            State = "SP"
        };
        
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();
        
        var companySupplier = new CompanySupplier
        {
            CompanyId = company.Id,
            SupplierId = supplier.Id
        };
        
        _context.CompanySuppliers.Add(companySupplier);
        await _context.SaveChangesAsync();
        
        var filter = new GetAllCompanyFilterDto { Page = 1, Limit = 10 };
        var result = await _repository.GetAllWithFilterAsync(filter, CancellationToken.None);
        
        Assert.NotNull(result);
        Assert.Single(result.Data);
        
        var companyResult = result.Data.First();
        Assert.NotNull(companyResult.CompanySuppliers);
        Assert.Single(companyResult.CompanySuppliers);
        
        var companySupplierResult = companyResult.CompanySuppliers.First();
        Assert.NotNull(companySupplierResult.Supplier);
        Assert.Equal(supplier.Id, companySupplierResult.Supplier.Id);
        Assert.Equal(supplier.Cnpj, companySupplierResult.Supplier.Cnpj);
    }

    [Fact]
    public async Task GetAllWithFilterAsync_WithPagination_ShouldReturnCorrectPage()
    {
        await CleanDatabaseAsync();
        
        var companies = new List<Company>();
        for (int i = 1; i <= 15; i++)
        {
            companies.Add(new Company
            {
                Cnpj = $"{i:D14}0001{i:D2}",
                FantasyName = $"Company {i}",
                ZipCode = $"{i:D8}",
                State = "SP"
            });
        }

        _context.Companies.AddRange(companies);
        await _context.SaveChangesAsync();

        var filter = new GetAllCompanyFilterDto { Page = 2, Limit = 5 };
        var result = await _repository.GetAllWithFilterAsync(filter, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(5, result.Data.Count());
        Assert.Equal(15, result.TotalCount);
        Assert.Equal(2, result.Page);
        Assert.Equal(5, result.Limit);
        Assert.Equal(3, result.TotalPages);
        Assert.True(result.HasNextPage);
        Assert.True(result.HasPreviousPage);
    }

    [Fact]
    public async Task GetAllWithFilterAsync_WithNameFilter_ShouldReturnFilteredCompanies()
    {
        await CleanDatabaseAsync();
        
        var companies = new List<Company>
        {
            new Company
            {
                Cnpj = "12345678000195",
                FantasyName = "Tech Company",
                ZipCode = "12345678",
                State = "SP"
            },
            new Company
            {
                Cnpj = "98765432000123",
                FantasyName = "Business Corp",
                ZipCode = "87654321",
                State = "RJ"
            },
            new Company
            {
                Cnpj = "11111111000111",
                FantasyName = "Tech Solutions",
                ZipCode = "11111111",
                State = "MG"
            }
        };

        _context.Companies.AddRange(companies);
        await _context.SaveChangesAsync();

        var filter = new GetAllCompanyFilterDto { Name = "Tech", Page = 1, Limit = 10 };
        var result = await _repository.GetAllWithFilterAsync(filter, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result.Data.Count());
        Assert.Equal(2, result.TotalCount);
        Assert.Contains(result.Data, c => c.FantasyName == "Tech Company");
        Assert.Contains(result.Data, c => c.FantasyName == "Tech Solutions");
    }

    [Fact]
    public async Task GetAllWithFilterAsync_WithCnpjFilter_ShouldReturnFilteredCompanies()
    {
        await CleanDatabaseAsync();
        
        var companies = new List<Company>
        {
            new Company
            {
                Cnpj = "12345678000195",
                FantasyName = "Company 1",
                ZipCode = "12345678",
                State = "SP"
            },
            new Company
            {
                Cnpj = "98765432000123",
                FantasyName = "Company 2",
                ZipCode = "87654321",
                State = "RJ"
            }
        };

        _context.Companies.AddRange(companies);
        await _context.SaveChangesAsync();

        var filter = new GetAllCompanyFilterDto { Cnpj = "123456", Page = 1, Limit = 10 };
        var result = await _repository.GetAllWithFilterAsync(filter, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result.Data);
        Assert.Equal(1, result.TotalCount);
        Assert.Equal("12345678000195", result.Data.First().Cnpj);
    }
}
