using ChallengeBack.Application.Dto.CompanySupplier;
using ChallengeBack.Application.Dto.Base;
using ChallengeBack.Domain.Entities;
using ChallengeBack.Domain.Enums;
using ChallengeBack.Infrastructure.Data;
using ChallengeBack.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChallengeBack.Tests.Repositories;

public class CompanySupplierRepositoryTests : IClassFixture<PostgresFixture>
{
    private readonly PostgresFixture _postgresFixture;
    private readonly ApplicationDbContext _context;
    private readonly CompanySupplierRepository _repository;

    public CompanySupplierRepositoryTests(PostgresFixture postgresFixture)
    {
        _postgresFixture = postgresFixture;
        
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(_postgresFixture.ConnectionString)
            .Options;
            
        _context = new ApplicationDbContext(options);
        _repository = new CompanySupplierRepository(_context);
        
        _context.Database.EnsureCreated();
    }

    private async Task CleanDatabaseAsync()
    {
        _context.CompanySuppliers.RemoveRange(_context.CompanySuppliers);
        _context.Suppliers.RemoveRange(_context.Suppliers);
        _context.Companies.RemoveRange(_context.Companies);
        await _context.SaveChangesAsync();
    }

    private async Task<(Company company, Supplier supplier)> CreateTestEntitiesAsync()
    {
        var company = new Company
        {
            Cnpj = "12345678000195",
            FantasyName = "Test Company",
            ZipCode = "12345678",
            State = "SP"
        };

        var supplier = new Supplier
        {
            Type = PersonType.Company,
            Cnpj = "98765432000123",
            Name = "Test Supplier",
            Email = "supplier@test.com",
            ZipCode = "87654321"
        };

        _context.Companies.Add(company);
        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();

        return (company, supplier);
    }

    [Fact]
    public async Task AddAsync_ShouldAddCompanySupplier()
    {
        await CleanDatabaseAsync();
        
        var (company, supplier) = await CreateTestEntitiesAsync();
        
        var companySupplier = new CompanySupplier
        {
            CompanyId = company.Id,
            SupplierId = supplier.Id
        };

        var result = await _repository.AddAsync(companySupplier, CancellationToken.None);

        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal(company.Id, result.CompanyId);
        Assert.Equal(supplier.Id, result.SupplierId);
    }

    [Fact]
    public async Task AddAsync_WithCancellationToken_ShouldRespectCancellation()
    {
        await CleanDatabaseAsync();
        
        var (company, supplier) = await CreateTestEntitiesAsync();
        
        var companySupplier = new CompanySupplier
        {
            CompanyId = company.Id,
            SupplierId = supplier.Id
        };

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() => 
            _repository.AddAsync(companySupplier, cts.Token));
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCompanySupplier_WhenCompanySupplierExists()
    {
        await CleanDatabaseAsync();
        
        var (company, supplier) = await CreateTestEntitiesAsync();
        
        var companySupplier = new CompanySupplier
        {
            CompanyId = company.Id,
            SupplierId = supplier.Id
        };

        await _context.CompanySuppliers.AddAsync(companySupplier);
        await _context.SaveChangesAsync();

        var newSupplier = new Supplier
        {
            Type = PersonType.Individual,
            Cpf = "11122233344",
            Name = "New Supplier",
            Email = "newsupplier@test.com",
            ZipCode = "11111111"
        };

        _context.Suppliers.Add(newSupplier);
        await _context.SaveChangesAsync();

        companySupplier.SupplierId = newSupplier.Id;

        var result = await _repository.UpdateAsync(companySupplier, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(companySupplier.Id, result.Id);
        Assert.Equal(company.Id, result.CompanyId);
        Assert.Equal(newSupplier.Id, result.SupplierId);
    }

    [Fact]
    public async Task UpdateAsync_WhenCompanySupplierDoesNotExist_ShouldThrowException()
    {
        await CleanDatabaseAsync();
        
        var companySupplier = new CompanySupplier
        {
            Id = 999,
            CompanyId = 1,
            SupplierId = 1
        };

        var exception = await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => 
            _repository.UpdateAsync(companySupplier, CancellationToken.None));
        
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteCompanySupplier_WhenCompanySupplierExists()
    {
        await CleanDatabaseAsync();
        
        var (company, supplier) = await CreateTestEntitiesAsync();
        
        var companySupplier = new CompanySupplier
        {
            CompanyId = company.Id,
            SupplierId = supplier.Id
        };

        await _context.CompanySuppliers.AddAsync(companySupplier);
        await _context.SaveChangesAsync();

        var result = await _repository.DeleteAsync(companySupplier.CompanyId, companySupplier.SupplierId, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(companySupplier.Id, result.Id);
        Assert.Equal(company.Id, result.CompanyId);
        Assert.Equal(supplier.Id, result.SupplierId);

        var deletedEntity = await _context.CompanySuppliers.FindAsync(companySupplier.Id);
        Assert.Null(deletedEntity);
    }

    [Fact]
    public async Task DeleteAsync_WhenCompanySupplierDoesNotExist_ShouldThrowException()
    {
        await CleanDatabaseAsync();
        
        var nonExistentId = 999;

        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _repository.DeleteAsync(nonExistentId, nonExistentId, CancellationToken.None));
        
        Assert.Equal("CompanySupplier not found", exception.Message);
    }

    [Fact]
    public async Task AddAsync_WithDuplicateRelationship_ShouldThrowException()
    {
        await CleanDatabaseAsync();
        
        var (company, supplier) = await CreateTestEntitiesAsync();
        
        var companySupplier1 = new CompanySupplier
        {
            CompanyId = company.Id,
            SupplierId = supplier.Id
        };

        var companySupplier2 = new CompanySupplier
        {
            CompanyId = company.Id,
            SupplierId = supplier.Id
        };

        await _repository.AddAsync(companySupplier1, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<DbUpdateException>(() => 
            _repository.AddAsync(companySupplier2, CancellationToken.None));
        
        Assert.Contains("duplicate key", exception.InnerException?.Message?.ToLower() ?? "");
    }

    [Fact]
    public async Task AddAsync_WithNonExistentCompany_ShouldThrowException()
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
        
        var companySupplier = new CompanySupplier
        {
            CompanyId = 999,
            SupplierId = supplier.Id
        };

        var exception = await Assert.ThrowsAsync<DbUpdateException>(() => 
            _repository.AddAsync(companySupplier, CancellationToken.None));
        
        Assert.Contains("foreign key", exception.InnerException?.Message?.ToLower() ?? "");
    }

    [Fact]
    public async Task AddAsync_WithNonExistentSupplier_ShouldThrowException()
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
        
        var companySupplier = new CompanySupplier
        {
            CompanyId = company.Id,
            SupplierId = 999
        };

        var exception = await Assert.ThrowsAsync<DbUpdateException>(() => 
            _repository.AddAsync(companySupplier, CancellationToken.None));
        
        Assert.Contains("foreign key", exception.InnerException?.Message?.ToLower() ?? "");
    }

    [Fact]
    public async Task AddAsync_WithValidRelationship_ShouldIncludeNavigationProperties()
    {
        await CleanDatabaseAsync();
        
        var (company, supplier) = await CreateTestEntitiesAsync();
        
        var companySupplier = new CompanySupplier
        {
            CompanyId = company.Id,
            SupplierId = supplier.Id
        };

        var result = await _repository.AddAsync(companySupplier, CancellationToken.None);

        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal(company.Id, result.CompanyId);
        Assert.Equal(supplier.Id, result.SupplierId);

        var savedRelationship = await _context.CompanySuppliers
            .Include(cs => cs.Company)
            .Include(cs => cs.Supplier)
            .FirstOrDefaultAsync(cs => cs.Id == result.Id);

        Assert.NotNull(savedRelationship);
        Assert.NotNull(savedRelationship.Company);
        Assert.NotNull(savedRelationship.Supplier);
        Assert.Equal(company.Cnpj, savedRelationship.Company.Cnpj);
        Assert.Equal(supplier.Name, savedRelationship.Supplier.Name);
    }

    [Fact]
    public async Task UpdateAsync_WithValidData_ShouldUpdateSuccessfully()
    {
        await CleanDatabaseAsync();
        
        var (company, supplier) = await CreateTestEntitiesAsync();
        
        var newSupplier = new Supplier
        {
            Type = PersonType.Individual,
            Cpf = "11122233344",
            Name = "New Supplier",
            Email = "newsupplier@test.com",
            ZipCode = "11111111"
        };

        _context.Suppliers.Add(newSupplier);
        await _context.SaveChangesAsync();
        
        var companySupplier = new CompanySupplier
        {
            CompanyId = company.Id,
            SupplierId = supplier.Id
        };

        await _context.CompanySuppliers.AddAsync(companySupplier);
        await _context.SaveChangesAsync();

        companySupplier.SupplierId = newSupplier.Id;
        var result = await _repository.UpdateAsync(companySupplier, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(companySupplier.Id, result.Id);
        Assert.Equal(company.Id, result.CompanyId);
        Assert.Equal(newSupplier.Id, result.SupplierId);
    }

    [Fact]
    public async Task GetAllWithFilterAsync_ShouldReturnEmpty_WhenNoCompanySuppliersExist()
    {
        await CleanDatabaseAsync();
        
        var filter = new GetAllCompanySupplierFilterDto { Page = 1, Limit = 10 };
        var result = await _repository.GetAllWithFilterAsync(filter, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(result.Data);
        Assert.Equal(0, result.TotalCount);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.Limit);
    }

    [Fact]
    public async Task GetAllWithFilterAsync_ShouldReturnAllCompanySuppliers_WhenCompanySuppliersExist()
    {
        await CleanDatabaseAsync();
        
        var (company, supplier) = await CreateTestEntitiesAsync();
        
        var companySuppliers = new List<CompanySupplier>
        {
            new CompanySupplier
            {
                CompanyId = company.Id,
                SupplierId = supplier.Id
            }
        };

        _context.CompanySuppliers.AddRange(companySuppliers);
        await _context.SaveChangesAsync();

        var filter = new GetAllCompanySupplierFilterDto { Page = 1, Limit = 10 };
        var result = await _repository.GetAllWithFilterAsync(filter, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(1, result.Data.Count());
        Assert.Equal(1, result.TotalCount);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.Limit);
        Assert.Contains(result.Data, cs => cs.CompanyId == company.Id);
    }

    [Fact]
    public async Task GetAllWithFilterAsync_WithPagination_ShouldReturnCorrectPage()
    {
        await CleanDatabaseAsync();
        
        var (company, supplier) = await CreateTestEntitiesAsync();
        
        var companySuppliers = new List<CompanySupplier>();
        for (int i = 1; i <= 15; i++)
        {
            var newSupplier = new Supplier
            {
                Type = PersonType.Individual,
                Cpf = $"{i:D11}",
                Name = $"Supplier {i}",
                Email = $"supplier{i}@test.com",
                ZipCode = $"{i:D8}"
            };
            _context.Suppliers.Add(newSupplier);
            await _context.SaveChangesAsync();
            
            companySuppliers.Add(new CompanySupplier
            {
                CompanyId = company.Id,
                SupplierId = newSupplier.Id
            });
        }

        _context.CompanySuppliers.AddRange(companySuppliers);
        await _context.SaveChangesAsync();

        var filter = new GetAllCompanySupplierFilterDto { Page = 2, Limit = 5 };
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
    public async Task GetAllWithFilterAsync_WithCompanyIdFilter_ShouldReturnFilteredCompanySuppliers()
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
            Cnpj = "98765432000123",
            FantasyName = "Company 2",
            ZipCode = "87654321",
            State = "RJ"
        };

        _context.Companies.AddRange(company1, company2);
        await _context.SaveChangesAsync();

        var supplier = new Supplier
        {
            Type = PersonType.Company,
            Cnpj = "11111111000111",
            Name = "Test Supplier",
            Email = "supplier@test.com",
            ZipCode = "11111111"
        };

        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();

        var companySuppliers = new List<CompanySupplier>
        {
            new CompanySupplier { CompanyId = company1.Id, SupplierId = supplier.Id },
            new CompanySupplier { CompanyId = company2.Id, SupplierId = supplier.Id }
        };

        _context.CompanySuppliers.AddRange(companySuppliers);
        await _context.SaveChangesAsync();

        var filter = new GetAllCompanySupplierFilterDto { CompanyId = company1.Id, Page = 1, Limit = 10 };
        var result = await _repository.GetAllWithFilterAsync(filter, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result.Data);
        Assert.Equal(1, result.TotalCount);
        Assert.Equal(company1.Id, result.Data.First().CompanyId);
    }

    [Fact]
    public async Task GetAllWithFilterAsync_WithSupplierIdFilter_ShouldReturnFilteredCompanySuppliers()
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

        var supplier1 = new Supplier
        {
            Type = PersonType.Company,
            Cnpj = "11111111000111",
            Name = "Supplier 1",
            Email = "supplier1@test.com",
            ZipCode = "11111111"
        };

        var supplier2 = new Supplier
        {
            Type = PersonType.Individual,
            Cpf = "22222222222",
            Name = "Supplier 2",
            Email = "supplier2@test.com",
            ZipCode = "22222222"
        };

        _context.Suppliers.AddRange(supplier1, supplier2);
        await _context.SaveChangesAsync();

        var companySuppliers = new List<CompanySupplier>
        {
            new CompanySupplier { CompanyId = company.Id, SupplierId = supplier1.Id },
            new CompanySupplier { CompanyId = company.Id, SupplierId = supplier2.Id }
        };

        _context.CompanySuppliers.AddRange(companySuppliers);
        await _context.SaveChangesAsync();

        var filter = new GetAllCompanySupplierFilterDto { SupplierId = supplier1.Id, Page = 1, Limit = 10 };
        var result = await _repository.GetAllWithFilterAsync(filter, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result.Data);
        Assert.Equal(1, result.TotalCount);
        Assert.Equal(supplier1.Id, result.Data.First().SupplierId);
    }

    [Fact]
    public async Task GetAllWithFilterAsync_ShouldIncludeNavigationProperties()
    {
        await CleanDatabaseAsync();
        
        var (company, supplier) = await CreateTestEntitiesAsync();
        
        var companySupplier = new CompanySupplier
        {
            CompanyId = company.Id,
            SupplierId = supplier.Id
        };

        _context.CompanySuppliers.Add(companySupplier);
        await _context.SaveChangesAsync();

        var filter = new GetAllCompanySupplierFilterDto { Page = 1, Limit = 10 };
        var result = await _repository.GetAllWithFilterAsync(filter, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result.Data);
        
        var companySupplierResult = result.Data.First();
        Assert.NotNull(companySupplierResult.Company);
        Assert.NotNull(companySupplierResult.Supplier);
        Assert.Equal(company.Cnpj, companySupplierResult.Company.Cnpj);
        Assert.Equal(supplier.Name, companySupplierResult.Supplier.Name);
    }
}
