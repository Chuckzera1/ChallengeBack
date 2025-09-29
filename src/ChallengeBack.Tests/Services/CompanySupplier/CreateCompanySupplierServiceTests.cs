using ChallengeBack.Application.Dto.CompanySupplier;
using ChallengeBack.Application.Interfaces.Repositories;
using ChallengeBack.Application.Services.CompanySupplier;
using ChallengeBack.Domain.Entities;
using ChallengeBack.Domain.Enums;
using CompanyEntity = ChallengeBack.Domain.Entities.Company;
using SupplierEntity = ChallengeBack.Domain.Entities.Supplier;
using CompanySupplierEntity = ChallengeBack.Domain.Entities.CompanySupplier;
using Moq;

namespace ChallengeBack.Tests.Services.CompanySupplier;

public class CreateCompanySupplierServiceTests
{
    private readonly Mock<ICompanySupplierRepository> _companySupplierRepositoryMock;
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly Mock<ISupplierRepository> _supplierRepositoryMock;
    private readonly CreateCompanySupplierService _service;

    public CreateCompanySupplierServiceTests()
    {
        _companySupplierRepositoryMock = new Mock<ICompanySupplierRepository>();
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _supplierRepositoryMock = new Mock<ISupplierRepository>();
        _service = new CreateCompanySupplierService(_companySupplierRepositoryMock.Object, _companyRepositoryMock.Object, _supplierRepositoryMock.Object);
    }

    [Fact]
    public async Task Execute_WhenValidData_ShouldCreateCompanySupplierSuccessfully()
    {
        
        var addCompanySupplierDto = new AddCompanySupplierDto
        {
            CompanyId = 1,
            SupplierId = 2
        };

        var company = new CompanyEntity
        {
            Id = 1,
            Cnpj = "12345678000195",
            FantasyName = "Test Company",
            ZipCode = "80000000",
            State = "SP"
        };

        var supplier = new SupplierEntity
        {
            Id = 2,
            Type = PersonType.Individual,
            Name = "Test Supplier",
            Email = "test@test.com",
            ZipCode = "80000000",
            BirthDate = DateTime.Today.AddYears(-25)
        };

        var expectedCompanySupplier = new CompanySupplierEntity
        {
            Id = 1,
            CompanyId = addCompanySupplierDto.CompanyId,
            SupplierId = addCompanySupplierDto.SupplierId
        };

        _companyRepositoryMock
            .Setup(x => x.GetByIdAsync(addCompanySupplierDto.CompanyId, CancellationToken.None))
            .ReturnsAsync(company);

        _supplierRepositoryMock
            .Setup(x => x.GetByIdAsync(addCompanySupplierDto.SupplierId, CancellationToken.None))
            .ReturnsAsync(supplier);

        _companySupplierRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<CompanySupplierEntity>(), CancellationToken.None))
            .ReturnsAsync(expectedCompanySupplier);

        
        await _service.Execute(addCompanySupplierDto, CancellationToken.None);

        
        Assert.Equal(expectedCompanySupplier.Id, expectedCompanySupplier.Id);
        Assert.Equal(addCompanySupplierDto.CompanyId, expectedCompanySupplier.CompanyId);
        Assert.Equal(addCompanySupplierDto.SupplierId, expectedCompanySupplier.SupplierId);  

        _companyRepositoryMock.Verify(
            x => x.GetByIdAsync(addCompanySupplierDto.CompanyId, CancellationToken.None), 
            Times.Once);
        
        _supplierRepositoryMock.Verify(
            x => x.GetByIdAsync(addCompanySupplierDto.SupplierId, CancellationToken.None), 
            Times.Once);

        _companySupplierRepositoryMock.Verify(
            x => x.AddAsync(It.Is<CompanySupplierEntity>(cs => 
                cs.CompanyId == addCompanySupplierDto.CompanyId && 
                cs.SupplierId == addCompanySupplierDto.SupplierId), 
                CancellationToken.None), 
            Times.Once);
    }

    [Fact]
    public async Task Execute_WhenCompanyNotFound_ShouldThrowException()
    {
        
        var addCompanySupplierDto = new AddCompanySupplierDto
        {
            CompanyId = 1,
            SupplierId = 2
        };

        _companyRepositoryMock
            .Setup(x => x.GetByIdAsync(addCompanySupplierDto.CompanyId, CancellationToken.None))
            .ReturnsAsync((CompanyEntity?)null);

        
        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _service.Execute(addCompanySupplierDto, CancellationToken.None));

        Assert.Equal("Company not found", exception.Message);
        _companyRepositoryMock.Verify(
            x => x.GetByIdAsync(addCompanySupplierDto.CompanyId, CancellationToken.None), 
            Times.Once);
    }

    [Fact]
    public async Task Execute_WhenSupplierNotFound_ShouldThrowException()
    {
        
        var addCompanySupplierDto = new AddCompanySupplierDto
        {
            CompanyId = 1,
            SupplierId = 2
        };

        var company = new CompanyEntity
        {
            Id = 1,
            Cnpj = "12345678000195",
            FantasyName = "Test Company",
            ZipCode = "80000000",
            State = "SP"
        };

        _companyRepositoryMock
            .Setup(x => x.GetByIdAsync(addCompanySupplierDto.CompanyId, CancellationToken.None))
            .ReturnsAsync(company);

        _supplierRepositoryMock
            .Setup(x => x.GetByIdAsync(addCompanySupplierDto.SupplierId, CancellationToken.None))
            .ReturnsAsync((SupplierEntity?)null);

        
        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _service.Execute(addCompanySupplierDto, CancellationToken.None));

        Assert.Equal("Supplier not found", exception.Message);
        _companyRepositoryMock.Verify(
            x => x.GetByIdAsync(addCompanySupplierDto.CompanyId, CancellationToken.None), 
            Times.Once);
        
        _supplierRepositoryMock.Verify(
            x => x.GetByIdAsync(addCompanySupplierDto.SupplierId, CancellationToken.None), 
            Times.Once);
    }

    [Fact]
    public async Task Execute_WhenCompanyInPRAndSupplierIsUnderage_ShouldThrowException()
    {
        
        var addCompanySupplierDto = new AddCompanySupplierDto
        {
            CompanyId = 1,
            SupplierId = 2
        };

        var company = new CompanyEntity
        {
            Id = 1,
            Cnpj = "12345678000195",
            FantasyName = "Test Company",
            ZipCode = "80000000",
            State = "PR"
        };

        var supplier = new SupplierEntity
        {
            Id = 2,
            Type = PersonType.Individual,
            Name = "Test Supplier",
            Email = "test@test.com",
            ZipCode = "80000000",
            Cpf = "12345678901",
            Rg = "123456789",
            BirthDate = DateTime.Today.AddYears(-17) 
        };

        _companyRepositoryMock
            .Setup(x => x.GetByIdAsync(addCompanySupplierDto.CompanyId, CancellationToken.None))
            .ReturnsAsync(company);

        _supplierRepositoryMock
            .Setup(x => x.GetByIdAsync(addCompanySupplierDto.SupplierId, CancellationToken.None))
            .ReturnsAsync(supplier);

        
        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _service.Execute(addCompanySupplierDto, CancellationToken.None));

        Assert.Equal("Fornecedor não pode ser menor de idade", exception.Message);
        _companyRepositoryMock.Verify(
            x => x.GetByIdAsync(addCompanySupplierDto.CompanyId, CancellationToken.None), 
            Times.Once);
        
        _supplierRepositoryMock.Verify(
            x => x.GetByIdAsync(addCompanySupplierDto.SupplierId, CancellationToken.None), 
            Times.Once);
    }


    [Fact]
    public async Task Execute_WhenSupplierIdIsZero_ShouldThrowException()
    {
        
        var addCompanySupplierDto = new AddCompanySupplierDto
        {
            CompanyId = 1,
            SupplierId = 0
        };

        var company = new CompanyEntity
        {
            Id = 1,
            Cnpj = "12345678000195",
            FantasyName = "Test Company",
            ZipCode = "80000000",
            State = "SP"
        };

        _companyRepositoryMock
            .Setup(x => x.GetByIdAsync(addCompanySupplierDto.CompanyId, CancellationToken.None))
            .ReturnsAsync(company);

        _supplierRepositoryMock
            .Setup(x => x.GetByIdAsync(addCompanySupplierDto.SupplierId, CancellationToken.None))
            .ReturnsAsync((SupplierEntity?)null);

        
        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _service.Execute(addCompanySupplierDto, CancellationToken.None));

        Assert.Equal("Supplier not found", exception.Message);
        _companyRepositoryMock.Verify(
            x => x.GetByIdAsync(addCompanySupplierDto.CompanyId, CancellationToken.None), 
            Times.Once);
        
        _supplierRepositoryMock.Verify(
            x => x.GetByIdAsync(addCompanySupplierDto.SupplierId, CancellationToken.None), 
            Times.Once);
    }
}
