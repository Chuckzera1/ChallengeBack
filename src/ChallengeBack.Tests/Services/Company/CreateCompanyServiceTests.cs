namespace ChallengeBack.Tests.Services.Company;

using ChallengeBack.Application.Dto.Company;
using ChallengeBack.Application.Interfaces.Repositories;
using ChallengeBack.Application.Services.Company;
using ChallengeBack.Domain.Entities;
using Moq;

public class CreateCompanyServiceTests
{
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly Mock<ICepRepository> _cepRepositoryMock;
    private readonly CreateCompanyService _service;

    public CreateCompanyServiceTests()
    {
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _cepRepositoryMock = new Mock<ICepRepository>();
        _service = new CreateCompanyService(_companyRepositoryMock.Object, _cepRepositoryMock.Object);
    }

    [Fact]
    public async Task Execute_WhenCepIsNotFound_ShouldThrowException()
    {
        var createCompanyDto = new CreateCompanyDto
        {
            Cnpj = "12345678000195",
            FantasyName = "Empresa Teste",
            ZipCode = "00000000"
        };

        _cepRepositoryMock
            .Setup(x => x.GetByZipCodeAsync(createCompanyDto.ZipCode))
            .ReturnsAsync((string?)null);

        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Execute(createCompanyDto, CancellationToken.None));
        
        Assert.Equal("CEP não encontrado", exception.Message);

        _cepRepositoryMock.Verify(x => x.GetByZipCodeAsync(createCompanyDto.ZipCode), Times.Once);
        _companyRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Company>(), CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task Execute_WhenCepRepositoryThrowsException_ShouldPropagateException()
    {
        var createCompanyDto = new CreateCompanyDto
        {
            Cnpj = "12345678000195",
            FantasyName = "Empresa Teste",
            ZipCode = "01234567"
        };

        var expectedException = new InvalidOperationException("Erro na API de CEP");

        _cepRepositoryMock
            .Setup(x => x.GetByZipCodeAsync(createCompanyDto.ZipCode))
            .ThrowsAsync(expectedException);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.Execute(createCompanyDto, CancellationToken.None));
        
        Assert.Equal("Erro na API de CEP", exception.Message);

        _companyRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Company>(), CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task Execute_WhenCompanyRepositoryThrowsException_ShouldPropagateException()
    {
        var createCompanyDto = new CreateCompanyDto
        {
            Cnpj = "12345678000195",
            FantasyName = "Empresa Teste",
            ZipCode = "01234567"
        };

        var expectedState = "SP";
        var expectedException = new InvalidOperationException("Erro ao salvar empresa");

        _cepRepositoryMock
            .Setup(x => x.GetByZipCodeAsync(createCompanyDto.ZipCode))
            .ReturnsAsync((string?)expectedState);

        _companyRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Company>(), CancellationToken.None))
            .ThrowsAsync(expectedException);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.Execute(createCompanyDto, CancellationToken.None));
        
        Assert.Equal("Erro ao salvar empresa", exception.Message);

        _cepRepositoryMock.Verify(x => x.GetByZipCodeAsync(createCompanyDto.ZipCode), Times.Once);
    }

    [Theory]
    [InlineData("12345678000195", "Empresa Teste 1", "01234567", "SP")]
    [InlineData("98765432000123", "Empresa Teste 2", "20000000", "RJ")]
    [InlineData("11111111000111", "Empresa Teste 3", "40000000", "BA")]
    public async Task Execute_WithDifferentValidInputs_ShouldCreateCompanySuccessfully(
        string cnpj, string fantasyName, string zipCode, string expectedState)
    {
        var createCompanyDto = new CreateCompanyDto
        {
            Cnpj = cnpj,
            FantasyName = fantasyName,
            ZipCode = zipCode
        };

        var expectedCompany = new Company
        {
            Id = 1,
            Cnpj = cnpj,
            FantasyName = fantasyName,
            ZipCode = zipCode,
            State = expectedState,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _cepRepositoryMock
            .Setup(x => x.GetByZipCodeAsync(zipCode))
            .ReturnsAsync((string?)expectedState);

        _companyRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Company>(), CancellationToken.None))
            .ReturnsAsync(expectedCompany);

        var result = await _service.Execute(createCompanyDto, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(cnpj, result.Cnpj);
        Assert.Equal(fantasyName, result.FantasyName);
        Assert.Equal(zipCode, result.ZipCode);
        Assert.Equal(expectedState, result.State);
    }
}
