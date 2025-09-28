using ChallengeBack.Application.Interfaces.Repositories;
using ChallengeBack.Application.Services.Company;
using CompanyEntity = ChallengeBack.Domain.Entities.Company;
using Moq;

namespace ChallengeBack.Tests.Services.Company;

public class GetAllCompaniesServiceTests
{
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly GetAllCompaniesService _service;

    public GetAllCompaniesServiceTests()
    {
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _service = new GetAllCompaniesService(_companyRepositoryMock.Object);
    }

    [Fact]
    public async Task Execute_WhenNoCompanies_ShouldReturnEmptyList()
    {
        _companyRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new List<CompanyEntity>() as IEnumerable<CompanyEntity>));

        var result = await _service.Execute(CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(result);
        _companyRepositoryMock.Verify(x => x.GetAllAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Execute_WhenCompaniesExist_ShouldReturnAllCompanies()
    {
        var companies = new List<CompanyEntity>
        {
            new CompanyEntity
            {
                Id = 1,
                Cnpj = "12345678000195",
                FantasyName = "Company 1",
                ZipCode = "12345678",
                State = "SP",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new CompanyEntity
            {
                Id = 2,
                Cnpj = "98765432000123",
                FantasyName = "Company 2",
                ZipCode = "87654321",
                State = "RJ",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        _companyRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(companies as IEnumerable<CompanyEntity>));

        var result = await _service.Execute(CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, c => c.Cnpj == "12345678000195");
        Assert.Contains(result, c => c.Cnpj == "98765432000123");
        _companyRepositoryMock.Verify(x => x.GetAllAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Execute_WhenRepositoryThrowsException_ShouldPropagateException()
    {
        var expectedException = new InvalidOperationException("Database error");

        _companyRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _service.Execute(CancellationToken.None));

        Assert.Equal("Database error", exception.Message);
        _companyRepositoryMock.Verify(x => x.GetAllAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Execute_WithCancellationToken_ShouldPassTokenToRepository()
    {
        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        _companyRepositoryMock
            .Setup(x => x.GetAllAsync(token))
            .Returns(Task.FromResult(new List<CompanyEntity>() as IEnumerable<CompanyEntity>));

        await _service.Execute(token);

        _companyRepositoryMock.Verify(x => x.GetAllAsync(token), Times.Once);
    }
}
