using ChallengeBack.Domain.Entities;
using ChallengeBack.Domain.Enums;
using ChallengeBack.Infrastructure.Data;
using ChallengeBack.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using ChallengeBack.Application.Dto.Supplier;

namespace ChallengeBack.Tests.Repositories;

public class SupplierRepositoryTests : IClassFixture<PostgresFixture>
{
    private readonly PostgresFixture _postgresFixture;
    private readonly ApplicationDbContext _context;
    private readonly SupplierRepository _repository;

    public SupplierRepositoryTests(PostgresFixture postgresFixture)
    {
        _postgresFixture = postgresFixture;
        
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(_postgresFixture.ConnectionString)
            .Options;
            
        _context = new ApplicationDbContext(options);
        _repository = new SupplierRepository(_context);
        
        _context.Database.EnsureCreated();
    }

    private async Task CleanDatabaseAsync()
    { 
        _context.CompanySuppliers.RemoveRange(_context.CompanySuppliers);
        _context.Suppliers.RemoveRange(_context.Suppliers);
        _context.Companies.RemoveRange(_context.Companies);
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetByIdAsync_WhenSupplierExists_ShouldReturnSupplier()
    {
        await CleanDatabaseAsync();
        
        var supplier = new Supplier
        {
            Type = PersonType.Company,
            Cnpj = "12345678000195",
            Name = "Test Supplier",
            Email = "supplier@test.com",
            ZipCode = "12345678"
        };

        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(supplier.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(supplier.Id, result.Id);
        Assert.Equal(supplier.Type, result.Type);
        Assert.Equal(supplier.Cnpj, result.Cnpj);
        Assert.Equal(supplier.Name, result.Name);
        Assert.Equal(supplier.Email, result.Email);
        Assert.Equal(supplier.ZipCode, result.ZipCode);
        Assert.True(result.CreatedAt != DateTime.MinValue);
        Assert.True(result.UpdatedAt != DateTime.MinValue);
    }

    [Fact]
    public async Task GetByIdAsync_WhenSupplierDoesNotExist_ShouldThrowException()
    {
        await CleanDatabaseAsync();
        
        var nonExistentId = 999;

        var exception = await Assert.ThrowsAsync<Exception>(() => _repository.GetByIdAsync(nonExistentId, CancellationToken.None));
        Assert.Equal("Supplier not found", exception.Message);
    }

    [Fact]
    public async Task InsertAsync_ShouldAddSupplier()
    {
        await CleanDatabaseAsync();
        
        var supplier = new Supplier
        {
            Type = PersonType.Individual,
            Cpf = "12345678901",
            Name = "Test Supplier",
            Email = "supplier@test.com",
            ZipCode = "12345678",
            Rg = "123456789",
            BirthDate = new DateTime(1990, 1, 1).ToUniversalTime()
        };

        var result = await _repository.AddAsync(supplier, CancellationToken.None);

        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal(supplier.Type, result.Type);
        Assert.Equal(supplier.Cpf, result.Cpf);
        Assert.Equal(supplier.Name, result.Name);
        Assert.Equal(supplier.Email, result.Email);
        Assert.Equal(supplier.ZipCode, result.ZipCode);
        Assert.Equal(supplier.Rg, result.Rg);
        Assert.Equal(supplier.BirthDate.Value.ToUniversalTime(), result.BirthDate);
        Assert.True(result.CreatedAt != DateTime.MinValue);
        Assert.True(result.UpdatedAt != DateTime.MinValue);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateSupplier_WhenSupplierExists()
    {
        await CleanDatabaseAsync();
        
        var supplier = new Supplier
        {
            Type = PersonType.Company,
            Cnpj = "12345678000195",
            Name = "Test Supplier",
            Email = "supplier@test.com",
            ZipCode = "12345678"
        };

        await _context.Suppliers.AddAsync(supplier);
        await _context.SaveChangesAsync();

        supplier.Name = "Updated Supplier";
        supplier.Email = "updated@test.com";

        var result = await _repository.UpdateAsync(supplier, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(supplier.Id, result.Id);
        Assert.Equal(supplier.Type, result.Type);
        Assert.Equal(supplier.Cnpj, result.Cnpj);
        Assert.Equal(supplier.Name, result.Name);
        Assert.Equal(supplier.Email, result.Email);
        Assert.Equal(supplier.ZipCode, result.ZipCode);
        Assert.True(result.UpdatedAt != DateTime.MinValue);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteSupplier_WhenSupplierExists()
    {
        await CleanDatabaseAsync();
        
        var supplier = new Supplier
        {
            Type = PersonType.Company,
            Cnpj = "12345678000195",
            Name = "Test Supplier",
            Email = "supplier@test.com",
            ZipCode = "12345678"
        };

        await _context.Suppliers.AddAsync(supplier);
        await _context.SaveChangesAsync();

        await _repository.DeleteAsync(supplier.Id, CancellationToken.None);

        var result = await _context.Suppliers.FindAsync(supplier.Id);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllWithFilterAsync_ShouldReturnAllSuppliers_WhenSuppliersExist()
    {
        await CleanDatabaseAsync();
        
        var suppliers = new List<Supplier>
        {
            new Supplier
            {
                Type = PersonType.Company,
                Cnpj = "12345678000195",
                Name = "Supplier 1",
                Email = "supplier1@test.com",
                ZipCode = "12345678"
            },
            new Supplier
            {
                Type = PersonType.Individual,
                Cpf = "98765432100",
                Name = "Supplier 2",
                Email = "supplier2@test.com",
                ZipCode = "87654321",
                Rg = "987654321",
                BirthDate = new DateTime(1985, 5, 15).ToUniversalTime()
            }
        };

        _context.Suppliers.AddRange(suppliers);
        await _context.SaveChangesAsync();

        var filter = new GetAllSupplierFilterDto { Page = 1, Limit = 10 };
        var result = await _repository.GetAllWithFilterAsync(filter, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result.Data.Count());
        Assert.Equal(2, result.TotalCount);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.Limit);
        Assert.Contains(result.Data, s => s.Cnpj == "12345678000195");
        Assert.Contains(result.Data, s => s.Cpf == "98765432100");
    }

    [Fact]
    public async Task AddAsync_WithCancellationToken_ShouldRespectCancellation()
    {
        await CleanDatabaseAsync();
        
        var supplier = new Supplier
        {
            Type = PersonType.Company,
            Cnpj = "12345678000195",
            Name = "Test Supplier",
            Email = "supplier@test.com",
            ZipCode = "12345678"
        };

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() => 
            _repository.AddAsync(supplier, cts.Token));
    }

    [Fact]
    public async Task UpdateAsync_WhenSupplierDoesNotExist_ShouldThrowException()
    {
        await CleanDatabaseAsync();
        
        var supplier = new Supplier
        {
            Id = 999,
            Type = PersonType.Company,
            Cnpj = "12345678000195",
            Name = "Test Supplier",
            Email = "supplier@test.com",
            ZipCode = "12345678"
        };

        var exception = await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => 
            _repository.UpdateAsync(supplier, CancellationToken.None));
        
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task DeleteAsync_WhenSupplierDoesNotExist_ShouldThrowException()
    {
        await CleanDatabaseAsync();
        
        var nonExistentId = 999;

        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _repository.DeleteAsync(nonExistentId, CancellationToken.None));
        
        Assert.Equal("Supplier not found", exception.Message);
    }

    [Fact]
    public async Task AddAsync_WithDuplicateCnpj_ShouldThrowException()
    {
        await CleanDatabaseAsync();
        
        var supplier1 = new Supplier
        {
            Type = PersonType.Company,
            Cnpj = "12345678000195",
            Name = "Supplier 1",
            Email = "supplier1@test.com",
            ZipCode = "12345678"
        };

        var supplier2 = new Supplier
        {
            Type = PersonType.Company,
            Cnpj = "12345678000195",
            Name = "Supplier 2",
            Email = "supplier2@test.com",
            ZipCode = "87654321"
        };

        await _repository.AddAsync(supplier1, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<DbUpdateException>(() => 
            _repository.AddAsync(supplier2, CancellationToken.None));
        
        Assert.Contains("duplicate key", exception.InnerException?.Message?.ToLower() ?? "");
    }

    [Fact]
    public async Task AddAsync_WithDuplicateCpf_ShouldThrowException()
    {
        await CleanDatabaseAsync();
        
        var supplier1 = new Supplier
        {
            Type = PersonType.Individual,
            Cpf = "12345678901",
            Name = "Supplier 1",
            Email = "supplier1@test.com",
            ZipCode = "12345678"
        };

        var supplier2 = new Supplier
        {
            Type = PersonType.Individual,
            Cpf = "12345678901",
            Name = "Supplier 2",
            Email = "supplier2@test.com",
            ZipCode = "87654321"
        };

        await _repository.AddAsync(supplier1, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<DbUpdateException>(() => 
            _repository.AddAsync(supplier2, CancellationToken.None));
        
        Assert.Contains("duplicate key", exception.InnerException?.Message?.ToLower() ?? "");
    }

    [Fact]
    public async Task GetAllAsync_ShouldIncludeCompanySuppliers_WhenSuppliersHaveCompanies()
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
            CompanyId = company.Id,
            SupplierId = supplier.Id
        };
        
        _context.CompanySuppliers.Add(companySupplier);
        await _context.SaveChangesAsync();
        
        var filter = new GetAllSupplierFilterDto { Page = 1, Limit = 10 };
        var result = await _repository.GetAllWithFilterAsync(filter, CancellationToken.None);
        
        Assert.NotNull(result);
        Assert.Single(result.Data);
        
        var supplierResult = result.Data.First();
        Assert.NotNull(supplierResult.CompanySuppliers);
        Assert.Single(supplierResult.CompanySuppliers);
        
        var companySupplierResult = supplierResult.CompanySuppliers.First();
        Assert.NotNull(companySupplierResult.Company);
        Assert.Equal(company.Id, companySupplierResult.Company.Id);
        Assert.Equal(company.Cnpj, companySupplierResult.Company.Cnpj);
    }

    [Fact]
    public async Task AddAsync_WithIndividualSupplier_ShouldSetCorrectProperties()
    {
        await CleanDatabaseAsync();
        
        var supplier = new Supplier
        {
            Type = PersonType.Individual,
            Cpf = "12345678901",
            Name = "John Doe",
            Email = "john@test.com",
            ZipCode = "12345678",
            Rg = "123456789",
            BirthDate = new DateTime(1990, 1, 1).ToUniversalTime()
        };

        var result = await _repository.AddAsync(supplier, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(PersonType.Individual, result.Type);
        Assert.Equal("12345678901", result.Cpf);
        Assert.Null(result.Cnpj);
        Assert.Equal("John Doe", result.Name);
        Assert.Equal("john@test.com", result.Email);
        Assert.Equal("123456789", result.Rg);
        Assert.Equal(new DateTime(1990, 1, 1).ToUniversalTime(), result.BirthDate);
    }

    [Fact]
    public async Task AddAsync_WithCompanySupplier_ShouldSetCorrectProperties()
    {
        await CleanDatabaseAsync();
        
        var supplier = new Supplier
        {
            Type = PersonType.Company,
            Cnpj = "12345678000195",
            Name = "Test Company Supplier",
            Email = "company@test.com",
            ZipCode = "12345678"
        };

        var result = await _repository.AddAsync(supplier, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(PersonType.Company, result.Type);
        Assert.Equal("12345678000195", result.Cnpj);
        Assert.Null(result.Cpf);
        Assert.Null(result.Rg);
        Assert.Null(result.BirthDate);
        Assert.Equal("Test Company Supplier", result.Name);
        Assert.Equal("company@test.com", result.Email);
    }

    [Fact]
    public async Task GetAllWithFilterAsync_WithPagination_ShouldReturnCorrectPage()
    {
        await CleanDatabaseAsync();
        
        var suppliers = new List<Supplier>();
        for (int i = 1; i <= 15; i++)
        {
            suppliers.Add(new Supplier
            {
                Type = PersonType.Individual,
                Cpf = $"{i:D11}",
                Name = $"Supplier {i}",
                Email = $"supplier{i}@test.com",
                ZipCode = $"{i:D8}"
            });
        }

        _context.Suppliers.AddRange(suppliers);
        await _context.SaveChangesAsync();

        var filter = new GetAllSupplierFilterDto { Page = 2, Limit = 5 };
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
    public async Task GetAllWithFilterAsync_WithNameFilter_ShouldReturnFilteredSuppliers()
    {
        await CleanDatabaseAsync();
        
        var suppliers = new List<Supplier>
        {
            new Supplier
            {
                Type = PersonType.Individual,
                Cpf = "12345678901",
                Name = "John Doe",
                Email = "john@test.com",
                ZipCode = "12345678"
            },
            new Supplier
            {
                Type = PersonType.Individual,
                Cpf = "98765432100",
                Name = "Jane Smith",
                Email = "jane@test.com",
                ZipCode = "87654321"
            },
            new Supplier
            {
                Type = PersonType.Individual,
                Cpf = "11111111111",
                Name = "Johnny Walker",
                Email = "johnny@test.com",
                ZipCode = "11111111"
            }
        };

        _context.Suppliers.AddRange(suppliers);
        await _context.SaveChangesAsync();

        var filter = new GetAllSupplierFilterDto { Name = "John", Page = 1, Limit = 10 };
        var result = await _repository.GetAllWithFilterAsync(filter, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result.Data.Count());
        Assert.Equal(2, result.TotalCount);
        Assert.Contains(result.Data, s => s.Name == "John Doe");
        Assert.Contains(result.Data, s => s.Name == "Johnny Walker");
    }

    [Fact]
    public async Task GetAllWithFilterAsync_WithCpfFilter_ShouldReturnFilteredSuppliers()
    {
        await CleanDatabaseAsync();
        
        var suppliers = new List<Supplier>
        {
            new Supplier
            {
                Type = PersonType.Individual,
                Cpf = "12345678901",
                Name = "Supplier 1",
                Email = "supplier1@test.com",
                ZipCode = "12345678"
            },
            new Supplier
            {
                Type = PersonType.Individual,
                Cpf = "98765432100",
                Name = "Supplier 2",
                Email = "supplier2@test.com",
                ZipCode = "87654321"
            }
        };

        _context.Suppliers.AddRange(suppliers);
        await _context.SaveChangesAsync();

        var filter = new GetAllSupplierFilterDto { Cpf = "123456", Page = 1, Limit = 10 };
        var result = await _repository.GetAllWithFilterAsync(filter, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result.Data);
        Assert.Equal(1, result.TotalCount);
        Assert.Equal("12345678901", result.Data.First().Cpf);
    }

    [Fact]
    public async Task GetAllWithFilterAsync_WithCnpjFilter_ShouldReturnFilteredSuppliers()
    {
        await CleanDatabaseAsync();
        
        var suppliers = new List<Supplier>
        {
            new Supplier
            {
                Type = PersonType.Company,
                Cnpj = "12345678000195",
                Name = "Company Supplier 1",
                Email = "company1@test.com",
                ZipCode = "12345678"
            },
            new Supplier
            {
                Type = PersonType.Company,
                Cnpj = "98765432000123",
                Name = "Company Supplier 2",
                Email = "company2@test.com",
                ZipCode = "87654321"
            }
        };

        _context.Suppliers.AddRange(suppliers);
        await _context.SaveChangesAsync();

        var filter = new GetAllSupplierFilterDto { Cnpj = "123456", Page = 1, Limit = 10 };
        var result = await _repository.GetAllWithFilterAsync(filter, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result.Data);
        Assert.Equal(1, result.TotalCount);
        Assert.Equal("12345678000195", result.Data.First().Cnpj);
    }
}
