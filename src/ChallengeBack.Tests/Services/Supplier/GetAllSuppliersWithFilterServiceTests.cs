using ChallengeBack.Application.Dto.Supplier;
using ChallengeBack.Application.Interfaces.Repositories;
using ChallengeBack.Application.Services.Supplier;
using SupplierEntity = ChallengeBack.Domain.Entities.Supplier;
using ChallengeBack.Domain.Enums;
using Moq;

namespace ChallengeBack.Tests.Services.Supplier;

public class GetAllSuppliersWithFilterServiceTests
{
    private readonly Mock<ISupplierRepository> _supplierRepositoryMock;
    private readonly GetAllSuppliersWithFilterService _service;

    public GetAllSuppliersWithFilterServiceTests()
    {
        _supplierRepositoryMock = new Mock<ISupplierRepository>();
        _service = new GetAllSuppliersWithFilterService(_supplierRepositoryMock.Object);
    }

    [Fact]
    public async Task Execute_WhenFilterByName_ShouldReturnFilteredSuppliers()
    {
        var filter = new GetAllSupplierFilterDto
        {
            Name = "John"
        };

        var expectedSuppliers = new List<SupplierEntity>
        {
            new SupplierEntity
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@test.com",
                Type = PersonType.Individual,
                Cpf = "12345678901"
            }
        };

        _supplierRepositoryMock
            .Setup(x => x.GetAllWithFilterAsync(filter, CancellationToken.None))
            .ReturnsAsync(expectedSuppliers);

        var result = await _service.Execute(filter, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("John Doe", result.First().Name);

        _supplierRepositoryMock.Verify(x => x.GetAllWithFilterAsync(filter, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Execute_WhenFilterByCpf_ShouldReturnFilteredSuppliers()
    {
        var filter = new GetAllSupplierFilterDto
        {
            Cpf = "123456"
        };

        var expectedSuppliers = new List<SupplierEntity>
        {
            new SupplierEntity
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@test.com",
                Type = PersonType.Individual,
                Cpf = "12345678901"
            }
        };

        _supplierRepositoryMock
            .Setup(x => x.GetAllWithFilterAsync(filter, CancellationToken.None))
            .ReturnsAsync(expectedSuppliers);

        var result = await _service.Execute(filter, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("12345678901", result.First().Cpf);

        _supplierRepositoryMock.Verify(x => x.GetAllWithFilterAsync(filter, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Execute_WhenFilterByCnpj_ShouldReturnFilteredSuppliers()
    {
        var filter = new GetAllSupplierFilterDto
        {
            Cnpj = "12345678"
        };

        var expectedSuppliers = new List<SupplierEntity>
        {
            new SupplierEntity
            {
                Id = 1,
                Name = "Company Supplier",
                Email = "company@test.com",
                Type = PersonType.Company,
                Cnpj = "12345678000195"
            }
        };

        _supplierRepositoryMock
            .Setup(x => x.GetAllWithFilterAsync(filter, CancellationToken.None))
            .ReturnsAsync(expectedSuppliers);

        var result = await _service.Execute(filter, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("12345678000195", result.First().Cnpj);

        _supplierRepositoryMock.Verify(x => x.GetAllWithFilterAsync(filter, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Execute_WhenNoFilters_ShouldReturnAllSuppliers()
    {
        var filter = new GetAllSupplierFilterDto();

        var expectedSuppliers = new List<SupplierEntity>
        {
            new SupplierEntity
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@test.com",
                Type = PersonType.Individual,
                Cpf = "12345678901"
            },
            new SupplierEntity
            {
                Id = 2,
                Name = "Company Supplier",
                Email = "company@test.com",
                Type = PersonType.Company,
                Cnpj = "12345678000195"
            }
        };

        _supplierRepositoryMock
            .Setup(x => x.GetAllWithFilterAsync(filter, CancellationToken.None))
            .ReturnsAsync(expectedSuppliers);

        var result = await _service.Execute(filter, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        _supplierRepositoryMock.Verify(x => x.GetAllWithFilterAsync(filter, CancellationToken.None), Times.Once);
    }
}
