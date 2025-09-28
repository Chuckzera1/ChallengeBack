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
        
        // Ensure database is created and migrated
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task GetByIdAsync_WhenCompanyExists_ShouldReturnCompany()
    {
        // Arrange
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

        // Act
        var result = await _repository.GetByIdAsync(company.Id);

        // Assert
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
        // Arrange
        var nonExistentId = 999;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _repository.GetByIdAsync(nonExistentId));
        Assert.Equal("Company not found", exception.Message);
    }
}
