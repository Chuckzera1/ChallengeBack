using ChallengeBack.Application.Dto.Supplier;
using ChallengeBack.Application.Interfaces.Repositories;
using ChallengeBack.Application.Services.Supplier;
using SupplierEntity = ChallengeBack.Domain.Entities.Supplier;
using ChallengeBack.Domain.Enums;
using Moq;

namespace ChallengeBack.Tests.Services.Supplier;

public class UpdateSupplierServiceTests
{
    private readonly Mock<ISupplierRepository> _supplierRepositoryMock;
    private readonly UpdateSupplierService _service;

    public UpdateSupplierServiceTests()
    {
        _supplierRepositoryMock = new Mock<ISupplierRepository>();
        _service = new UpdateSupplierService(_supplierRepositoryMock.Object);
    }

    [Fact]
    public async Task Execute_WhenSupplierExists_ShouldUpdateSuccessfully()
    {
        var supplierId = 1;
        var updateSupplierDto = new UpdateSupplierDto
        {
            Name = "Updated Name",
            Email = "updated@test.com",
            ZipCode = "12345678",
            BirthDate = new DateTime(1990, 5, 15)
        };

        var existingSupplier = new SupplierEntity
        {
            Id = supplierId,
            Name = "Original Name",
            Email = "original@test.com",
            ZipCode = "87654321",
            Type = PersonType.Individual,
            Cpf = "12345678901",
            BirthDate = new DateTime(1985, 1, 1)
        };

        var updatedSupplier = new SupplierEntity
        {
            Id = supplierId,
            Name = updateSupplierDto.Name,
            Email = updateSupplierDto.Email,
            ZipCode = updateSupplierDto.ZipCode,
            Type = PersonType.Individual,
            Cpf = "12345678901",
            BirthDate = updateSupplierDto.BirthDate
        };

        _supplierRepositoryMock
            .Setup(x => x.GetByIdAsync(supplierId, CancellationToken.None))
            .ReturnsAsync(existingSupplier);

        _supplierRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<SupplierEntity>(), CancellationToken.None))
            .ReturnsAsync(updatedSupplier);

        var result = await _service.Execute(supplierId, updateSupplierDto, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(updateSupplierDto.Name, result.Name);
        Assert.Equal(updateSupplierDto.Email, result.Email);
        Assert.Equal(updateSupplierDto.ZipCode, result.ZipCode);
        Assert.Equal(updateSupplierDto.BirthDate, result.BirthDate);

        _supplierRepositoryMock.Verify(x => x.GetByIdAsync(supplierId, CancellationToken.None), Times.Once);
        _supplierRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<SupplierEntity>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Execute_WhenSupplierNotFound_ShouldThrowException()
    {
        var supplierId = 999;
        var updateSupplierDto = new UpdateSupplierDto
        {
            Name = "Updated Name",
            Email = "updated@test.com",
            ZipCode = "12345678",
            BirthDate = new DateTime(1990, 5, 15)
        };

        _supplierRepositoryMock
            .Setup(x => x.GetByIdAsync(supplierId, CancellationToken.None))
            .ThrowsAsync(new Exception("Supplier not found"));

        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _service.Execute(supplierId, updateSupplierDto, CancellationToken.None));

        Assert.Equal("Supplier not found", exception.Message);
        _supplierRepositoryMock.Verify(x => x.GetByIdAsync(supplierId, CancellationToken.None), Times.Once);
        _supplierRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<SupplierEntity>(), It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task Execute_WhenUpdatingWithNullBirthDate_ShouldUpdateSuccessfully()
    {
        var supplierId = 1;
        var updateSupplierDto = new UpdateSupplierDto
        {
            Name = "Updated Name",
            Email = "updated@test.com",
            ZipCode = "12345678",
            BirthDate = null
        };

        var existingSupplier = new SupplierEntity
        {
            Id = supplierId,
            Name = "Original Name",
            Email = "original@test.com",
            ZipCode = "87654321",
            Type = PersonType.Company,
            Cnpj = "12345678000195",
            BirthDate = new DateTime(1985, 1, 1)
        };

        var updatedSupplier = new SupplierEntity
        {
            Id = supplierId,
            Name = updateSupplierDto.Name,
            Email = updateSupplierDto.Email,
            ZipCode = updateSupplierDto.ZipCode,
            Type = PersonType.Company,
            Cnpj = "12345678000195",
            BirthDate = null
        };

        _supplierRepositoryMock
            .Setup(x => x.GetByIdAsync(supplierId, CancellationToken.None))
            .ReturnsAsync(existingSupplier);

        _supplierRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<SupplierEntity>(), CancellationToken.None))
            .ReturnsAsync(updatedSupplier);

        var result = await _service.Execute(supplierId, updateSupplierDto, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(updateSupplierDto.Name, result.Name);
        Assert.Equal(updateSupplierDto.Email, result.Email);
        Assert.Equal(updateSupplierDto.ZipCode, result.ZipCode);
        Assert.Null(result.BirthDate);

        _supplierRepositoryMock.Verify(x => x.GetByIdAsync(supplierId, CancellationToken.None), Times.Once);
        _supplierRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<SupplierEntity>(), CancellationToken.None), Times.Once);
    }
}
