using ChallengeBack.Application.Dto.Company;
using ChallengeBack.Application.Dto.Base;
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
    public async Task Execute_WhenNoCompanies_ShouldReturnEmptyPagedResult()
    {
        var filter = new GetAllCompanyFilterDto { Page = 1, Limit = 10 };
        var expectedResult = new PagedResultDto<CompanyEntity>
        {
            Data = new List<CompanyEntity>(),
            TotalCount = 0,
            Page = 1,
            Limit = 10
        };

        _companyRepositoryMock
            .Setup(x => x.GetAllWithFilterAsync(filter, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _service.Execute(filter, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(result.Data);
        Assert.Equal(0, result.TotalCount);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.Limit);
        _companyRepositoryMock.Verify(x => x.GetAllWithFilterAsync(filter, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Execute_WhenCompaniesExist_ShouldReturnPagedCompanies()
    {
        var filter = new GetAllCompanyFilterDto { Page = 1, Limit = 10 };
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

        var expectedResult = new PagedResultDto<CompanyEntity>
        {
            Data = companies,
            TotalCount = 2,
            Page = 1,
            Limit = 10
        };

        _companyRepositoryMock
            .Setup(x => x.GetAllWithFilterAsync(filter, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _service.Execute(filter, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result.Data.Count());
        Assert.Equal(2, result.TotalCount);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.Limit);
        Assert.Contains(result.Data, c => c.Cnpj == "12345678000195");
        Assert.Contains(result.Data, c => c.Cnpj == "98765432000123");
        _companyRepositoryMock.Verify(x => x.GetAllWithFilterAsync(filter, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Execute_WhenRepositoryThrowsException_ShouldPropagateException()
    {
        var filter = new GetAllCompanyFilterDto { Page = 1, Limit = 10 };
        var expectedException = new InvalidOperationException("Database error");

        _companyRepositoryMock
            .Setup(x => x.GetAllWithFilterAsync(filter, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _service.Execute(filter, CancellationToken.None));

        Assert.Equal("Database error", exception.Message);
        _companyRepositoryMock.Verify(x => x.GetAllWithFilterAsync(filter, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Execute_WithCancellationToken_ShouldPassTokenToRepository()
    {
        using var cts = new CancellationTokenSource();
        var token = cts.Token;
        var filter = new GetAllCompanyFilterDto { Page = 1, Limit = 10 };
        var expectedResult = new PagedResultDto<CompanyEntity>
        {
            Data = new List<CompanyEntity>(),
            TotalCount = 0,
            Page = 1,
            Limit = 10
        };

        _companyRepositoryMock
            .Setup(x => x.GetAllWithFilterAsync(filter, token))
            .ReturnsAsync(expectedResult);

        await _service.Execute(filter, token);

        _companyRepositoryMock.Verify(x => x.GetAllWithFilterAsync(filter, token), Times.Once);
    }

    [Fact]
    public async Task Execute_WithPagination_ShouldReturnCorrectPagedResult()
    {
        var filter = new GetAllCompanyFilterDto { Page = 2, Limit = 5 };
        var companies = new List<CompanyEntity>
        {
            new CompanyEntity
            {
                Id = 6,
                Cnpj = "11111111000111",
                FantasyName = "Company 6",
                ZipCode = "11111111",
                State = "MG",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        var expectedResult = new PagedResultDto<CompanyEntity>
        {
            Data = companies,
            TotalCount = 15,
            Page = 2,
            Limit = 5
        };

        _companyRepositoryMock
            .Setup(x => x.GetAllWithFilterAsync(filter, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _service.Execute(filter, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result.Data);
        Assert.Equal(15, result.TotalCount);
        Assert.Equal(2, result.Page);
        Assert.Equal(5, result.Limit);
        Assert.Equal(3, result.TotalPages);
        Assert.True(result.HasNextPage);
        Assert.True(result.HasPreviousPage);
        _companyRepositoryMock.Verify(x => x.GetAllWithFilterAsync(filter, CancellationToken.None), Times.Once);
    }
}
