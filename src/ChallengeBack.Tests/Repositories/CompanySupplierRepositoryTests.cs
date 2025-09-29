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

        var result = await _repository.DeleteAsync(companySupplier.Id, CancellationToken.None);

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
            _repository.DeleteAsync(nonExistentId, CancellationToken.None));
        
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
}
