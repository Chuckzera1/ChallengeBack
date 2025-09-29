using ChallengeBack.Application.Interfaces.Repositories;
using ChallengeBack.Application.Services.Company;
using ChallengeBack.Domain.Entities;
using Moq;

namespace ChallengeBack.Tests.Services.Company;

public class DeleteCompanyServiceTests
{
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly DeleteCompanyService _service;

    public DeleteCompanyServiceTests()
    {
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _service = new DeleteCompanyService(_companyRepositoryMock.Object);
    }

    [Fact]
    public async Task Execute_WhenCompanyExists_ShouldDeleteSuccessfully()
    {
        var companyId = 1;

        _companyRepositoryMock
            .Setup(x => x.DeleteAsync(companyId, CancellationToken.None))
            .Returns(Task.CompletedTask);

        await _service.Execute(companyId, CancellationToken.None);

        _companyRepositoryMock.Verify(x => x.DeleteAsync(companyId, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Execute_WhenCompanyDoesNotExist_ShouldPropagateException()
    {
        var companyId = 999;
        var expectedException = new Exception("Company not found");

        _companyRepositoryMock
            .Setup(x => x.DeleteAsync(companyId, CancellationToken.None))
            .ThrowsAsync(expectedException);

        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _service.Execute(companyId, CancellationToken.None));

        Assert.Equal("Company not found", exception.Message);
        _companyRepositoryMock.Verify(x => x.DeleteAsync(companyId, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Execute_WhenRepositoryThrowsException_ShouldPropagateException()
    {
        var companyId = 1;
        var expectedException = new InvalidOperationException("Database error");

        _companyRepositoryMock
            .Setup(x => x.DeleteAsync(companyId, CancellationToken.None))
            .ThrowsAsync(expectedException);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _service.Execute(companyId, CancellationToken.None));

        Assert.Equal("Database error", exception.Message);
        _companyRepositoryMock.Verify(x => x.DeleteAsync(companyId, CancellationToken.None), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(999)]
    public async Task Execute_WithDifferentIds_ShouldCallRepositoryWithCorrectId(int companyId)
    {
        _companyRepositoryMock
            .Setup(x => x.DeleteAsync(companyId, CancellationToken.None))
            .Returns(Task.CompletedTask);

        await _service.Execute(companyId, CancellationToken.None);

        _companyRepositoryMock.Verify(x => x.DeleteAsync(companyId, CancellationToken.None), Times.Once);
    }
}
