using ChallengeBack.Application.Dto.Supplier;
using ChallengeBack.Application.Interfaces.Repositories;
using ChallengeBack.Application.Services.Supplier;
using SupplierEntity = ChallengeBack.Domain.Entities.Supplier;
using ChallengeBack.Domain.Enums;
using Moq;

namespace ChallengeBack.Tests.Services.Supplier;

public class CreateSupplierServiceTests
{
    private readonly Mock<ISupplierRepository> _supplierRepositoryMock;
    private readonly Mock<ICepRepository> _cepRepositoryMock;
    private readonly CreateSupplierService _service;

    public CreateSupplierServiceTests()
    {
        _supplierRepositoryMock = new Mock<ISupplierRepository>();
        _cepRepositoryMock = new Mock<ICepRepository>();
        _service = new CreateSupplierService(_supplierRepositoryMock.Object, _cepRepositoryMock.Object);
    }

    [Fact]
    public async Task Execute_WhenCepIsNotFound_ShouldThrowException()
    {
        var createSupplierDto = new CreateSupplierDto
        {
            Name = "Test Supplier",
            Email = "supplier@test.com",
            ZipCode = "00000000",
            Type = PersonType.Company,
            Cnpj = "12345678000195"
        };

        _cepRepositoryMock
            .Setup(x => x.GetByZipCodeAsync(createSupplierDto.ZipCode))
            .ReturnsAsync((string?)null);

        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Execute(createSupplierDto, CancellationToken.None));
        
        Assert.Equal("CEP não encontrado", exception.Message);

        _cepRepositoryMock.Verify(x => x.GetByZipCodeAsync(createSupplierDto.ZipCode), Times.Once);
        _supplierRepositoryMock.Verify(x => x.AddAsync(It.IsAny<SupplierEntity>(), CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task Execute_WhenIndividualSupplierMissingCpf_ShouldThrowArgumentException()
    {
        var createSupplierDto = new CreateSupplierDto
        {
            Name = "Test Supplier",
            Email = "supplier@test.com",
            ZipCode = "01234567",
            Type = PersonType.Individual,
            Cpf = null, // Missing CPF
            Rg = "123456789",
            BirthDate = new DateTime(1990, 1, 1)
        };

        _cepRepositoryMock
            .Setup(x => x.GetByZipCodeAsync(createSupplierDto.ZipCode))
            .ReturnsAsync("SP");

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.Execute(createSupplierDto, CancellationToken.None));
        
        Assert.Equal("CPF é obrigatório para pessoa física", exception.Message);
    }

    [Fact]
    public async Task Execute_WhenCompanySupplierMissingCnpj_ShouldThrowArgumentException()
    {
        var createSupplierDto = new CreateSupplierDto
        {
            Name = "Test Supplier",
            Email = "supplier@test.com",
            ZipCode = "01234567",
            Type = PersonType.Company,
            Cnpj = null // Missing CNPJ
        };

        _cepRepositoryMock
            .Setup(x => x.GetByZipCodeAsync(createSupplierDto.ZipCode))
            .ReturnsAsync("SP");

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.Execute(createSupplierDto, CancellationToken.None));
        
        Assert.Equal("CNPJ é obrigatório para pessoa jurídica", exception.Message);
    }

    [Fact]
    public async Task Execute_WhenValidIndividualSupplier_ShouldCreateSuccessfully()
    {
        var createSupplierDto = new CreateSupplierDto
        {
            Name = "John Doe",
            Email = "john@test.com",
            ZipCode = "01234567",
            Type = PersonType.Individual,
            Cpf = "12345678901",
            Rg = "123456789",
            BirthDate = new DateTime(1990, 1, 1)
        };

        var expectedSupplier = new SupplierEntity
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@test.com",
            ZipCode = "01234567",
            Type = PersonType.Individual,
            Cpf = "12345678901",
            Rg = "123456789",
            BirthDate = new DateTime(1990, 1, 1),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _cepRepositoryMock
            .Setup(x => x.GetByZipCodeAsync(createSupplierDto.ZipCode))
            .ReturnsAsync("SP");

        _supplierRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<SupplierEntity>(), CancellationToken.None))
            .ReturnsAsync(expectedSupplier);

        var result = await _service.Execute(createSupplierDto, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("John Doe", result.Name);
        Assert.Equal("john@test.com", result.Email);
        Assert.Equal("01234567", result.ZipCode);
        Assert.Equal(PersonType.Individual, result.Type);
        Assert.Equal("12345678901", result.Cpf);
        Assert.Equal("123456789", result.Rg);
        Assert.Equal(new DateTime(1990, 1, 1), result.BirthDate);
        Assert.Null(result.Cnpj);

        _cepRepositoryMock.Verify(x => x.GetByZipCodeAsync(createSupplierDto.ZipCode), Times.Once);
        _supplierRepositoryMock.Verify(x => x.AddAsync(It.IsAny<SupplierEntity>(), CancellationToken.None), Times.Once);
    }
}
