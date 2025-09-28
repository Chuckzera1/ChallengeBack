using ChallengeBack.Domain.Entities;
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

    [Fact]
    public async Task GetByIdAsync_WhenCompanyExists_ShouldReturnCompany()
    {
        var company = new Company
        {
            Cnpj = "12345678000195",
            FantasyName = "Test Company",
            ZipCode = "12345678",
            State = "SP",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(company.Id);

        Assert.NotNull(result);
        Assert.Equal(company.Id, result.Id);
        Assert.Equal(company.Cnpj, result.Cnpj);
        Assert.Equal(company.FantasyName, result.FantasyName);
        Assert.Equal(company.ZipCode, result.ZipCode);
        Assert.Equal(company.State, result.State);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCompanyDoesNotExist_ShouldThrowException()
    {
        var nonExistentId = 999;

        var exception = await Assert.ThrowsAsync<Exception>(() => _repository.GetByIdAsync(nonExistentId));
        Assert.Equal("Company not found", exception.Message);
    }

    [Fact]
    public async Task InsertAsync_ShouldReturnCompany()
    {
        var company = new Company
        {
            Cnpj = "12345678000195",
            FantasyName = "Test Company",
            ZipCode = "12345678",
            State = "SP",
        };

        var result = await _repository.AddAsync(company, CancellationToken.None);

        Assert.NotNull(result);
        Assert.NotNull(result.Id);
        Assert.Equal(company.Cnpj, result.Cnpj);
        Assert.Equal(company.FantasyName, result.FantasyName);
        Assert.Equal(company.ZipCode, result.ZipCode);
        Assert.Equal(company.State, result.State);
        Assert.NotNull(result.CreatedAt);
        Assert.NotNull(result.UpdatedAt);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnCompany()
    {
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

        var result = await _repository.UpdateAsync(company);

        Assert.NotNull(result);
        Assert.Equal(company.Id, result.Id);
        Assert.Equal(company.Cnpj, result.Cnpj);
        Assert.Equal(company.FantasyName, result.FantasyName);
        Assert.Equal(company.ZipCode, result.ZipCode);
        Assert.Equal(company.State, result.State);
        Assert.NotNull(result.UpdatedAt);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnCompany()
    {
        var company = new Company
        {
            Cnpj = "12345678000195",
            FantasyName = "Test Company",
            ZipCode = "12345678",
            State = "SP",
        };

        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();

        await _repository.DeleteAsync(company.Id);

        var result = await _context.Companies.FindAsync(company.Id);

        Assert.Null(result);
    }
}
