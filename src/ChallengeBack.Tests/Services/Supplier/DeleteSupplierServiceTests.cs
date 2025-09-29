using ChallengeBack.Application.Interfaces.Repositories;
using ChallengeBack.Application.Services.Supplier;
using SupplierEntity = ChallengeBack.Domain.Entities.Supplier;
using ChallengeBack.Domain.Enums;
using Moq;

namespace ChallengeBack.Tests.Services.Supplier;

public class DeleteSupplierServiceTests
{
    private readonly Mock<ISupplierRepository> _supplierRepositoryMock;
    private readonly DeleteSupplierService _service;

    public DeleteSupplierServiceTests()
    {
        _supplierRepositoryMock = new Mock<ISupplierRepository>();
        _service = new DeleteSupplierService(_supplierRepositoryMock.Object);
    }

    [Fact]
    public async Task Execute_WhenSupplierExists_ShouldDeleteSuccessfully()
    {
        var supplierId = 1;
        var expectedSupplier = new SupplierEntity
        {
            Id = supplierId,
            Name = "John Doe",
            Email = "john@test.com",
            Type = PersonType.Individual,
            Cpf = "12345678901"
        };

        _supplierRepositoryMock
            .Setup(x => x.DeleteAsync(supplierId, CancellationToken.None))
            .ReturnsAsync(expectedSupplier);

        await _service.Execute(supplierId, CancellationToken.None);

        _supplierRepositoryMock.Verify(x => x.DeleteAsync(supplierId, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Execute_WhenSupplierNotFound_ShouldThrowException()
    {
        var supplierId = 999;

        _supplierRepositoryMock
            .Setup(x => x.DeleteAsync(supplierId, CancellationToken.None))
            .ThrowsAsync(new Exception("Supplier not found"));

        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _service.Execute(supplierId, CancellationToken.None));

        Assert.Equal("Supplier not found", exception.Message);
        _supplierRepositoryMock.Verify(x => x.DeleteAsync(supplierId, CancellationToken.None), Times.Once);
    }
}
