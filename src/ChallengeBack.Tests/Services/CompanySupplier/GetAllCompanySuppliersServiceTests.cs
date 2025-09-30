using ChallengeBack.Application.Dto.CompanySupplier;
using ChallengeBack.Application.Dto.Base;
using ChallengeBack.Application.Interfaces.Repositories;
using ChallengeBack.Application.Services.CompanySupplier;
using CompanySupplierEntity = ChallengeBack.Domain.Entities.CompanySupplier;
using Moq;

namespace ChallengeBack.Tests.Services.CompanySupplier;

public class GetAllCompanySuppliersServiceTests
{
    private readonly Mock<ICompanySupplierRepository> _companySupplierRepositoryMock;
    private readonly GetAllCompanySuppliersService _service;

    public GetAllCompanySuppliersServiceTests()
    {
        _companySupplierRepositoryMock = new Mock<ICompanySupplierRepository>();
        _service = new GetAllCompanySuppliersService(_companySupplierRepositoryMock.Object);
    }

    [Fact]
    public async Task Execute_WhenNoCompanySuppliers_ShouldReturnEmptyPagedResult()
    {
        var filter = new GetAllCompanySupplierFilterDto { Page = 1, Limit = 10 };
        var expectedResult = new PagedResultDto<CompanySupplierEntity>
        {
            Data = new List<CompanySupplierEntity>(),
            TotalCount = 0,
            Page = 1,
            Limit = 10
        };

        _companySupplierRepositoryMock
            .Setup(x => x.GetAllWithFilterAsync(filter, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _service.Execute(filter, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(result.Data);
        Assert.Equal(0, result.TotalCount);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.Limit);
        _companySupplierRepositoryMock.Verify(x => x.GetAllWithFilterAsync(filter, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Execute_WhenCompanySuppliersExist_ShouldReturnPagedCompanySuppliers()
    {
        var filter = new GetAllCompanySupplierFilterDto { Page = 1, Limit = 10 };
        var companySuppliers = new List<CompanySupplierEntity>
        {
            new CompanySupplierEntity
            {
                Id = 1,
                CompanyId = 1,
                SupplierId = 1
            },
            new CompanySupplierEntity
            {
                Id = 2,
                CompanyId = 2,
                SupplierId = 2
            }
        };

        var expectedResult = new PagedResultDto<CompanySupplierEntity>
        {
            Data = companySuppliers,
            TotalCount = 2,
            Page = 1,
            Limit = 10
        };

        _companySupplierRepositoryMock
            .Setup(x => x.GetAllWithFilterAsync(filter, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _service.Execute(filter, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result.Data.Count());
        Assert.Equal(2, result.TotalCount);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.Limit);
        Assert.Contains(result.Data, cs => cs.CompanyId == 1);
        Assert.Contains(result.Data, cs => cs.CompanyId == 2);
        _companySupplierRepositoryMock.Verify(x => x.GetAllWithFilterAsync(filter, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Execute_WhenRepositoryThrowsException_ShouldPropagateException()
    {
        var filter = new GetAllCompanySupplierFilterDto { Page = 1, Limit = 10 };
        var expectedException = new InvalidOperationException("Database error");

        _companySupplierRepositoryMock
            .Setup(x => x.GetAllWithFilterAsync(filter, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _service.Execute(filter, CancellationToken.None));

        Assert.Equal("Database error", exception.Message);
        _companySupplierRepositoryMock.Verify(x => x.GetAllWithFilterAsync(filter, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Execute_WithCancellationToken_ShouldPassTokenToRepository()
    {
        using var cts = new CancellationTokenSource();
        var token = cts.Token;
        var filter = new GetAllCompanySupplierFilterDto { Page = 1, Limit = 10 };
        var expectedResult = new PagedResultDto<CompanySupplierEntity>
        {
            Data = new List<CompanySupplierEntity>(),
            TotalCount = 0,
            Page = 1,
            Limit = 10
        };

        _companySupplierRepositoryMock
            .Setup(x => x.GetAllWithFilterAsync(filter, token))
            .ReturnsAsync(expectedResult);

        await _service.Execute(filter, token);

        _companySupplierRepositoryMock.Verify(x => x.GetAllWithFilterAsync(filter, token), Times.Once);
    }

    [Fact]
    public async Task Execute_WithPagination_ShouldReturnCorrectPagedResult()
    {
        var filter = new GetAllCompanySupplierFilterDto { Page = 2, Limit = 5 };
        var companySuppliers = new List<CompanySupplierEntity>
        {
            new CompanySupplierEntity
            {
                Id = 6,
                CompanyId = 6,
                SupplierId = 6
            }
        };

        var expectedResult = new PagedResultDto<CompanySupplierEntity>
        {
            Data = companySuppliers,
            TotalCount = 15,
            Page = 2,
            Limit = 5
        };

        _companySupplierRepositoryMock
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
        _companySupplierRepositoryMock.Verify(x => x.GetAllWithFilterAsync(filter, CancellationToken.None), Times.Once);
    }
}
