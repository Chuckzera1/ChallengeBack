using ChallengeBack.Application.Interfaces.Repositories;
using ChallengeBack.Application.Services.CompanySupplier;
using Moq;

namespace ChallengeBack.Tests.Services.CompanySupplier;

public class DeleteCompanySupplierServiceTests
{
    private readonly Mock<ICompanySupplierRepository> _companySupplierRepositoryMock;
    private readonly DeleteCompanySupplierService _service;

    public DeleteCompanySupplierServiceTests()
    {
        _companySupplierRepositoryMock = new Mock<ICompanySupplierRepository>();
        _service = new DeleteCompanySupplierService(_companySupplierRepositoryMock.Object);
    }

    [Fact]
    public async Task Execute_WhenCancellationTokenIsCancelled_ShouldHandleCancellation()
    {
        
        var companySupplierId = 1;
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        _companySupplierRepositoryMock
            .Setup(x => x.DeleteAsync(companySupplierId, companySupplierId, cancellationTokenSource.Token))
            .ThrowsAsync(new OperationCanceledException());

        
        await Assert.ThrowsAsync<OperationCanceledException>(() => 
            _service.Execute(companySupplierId, companySupplierId, cancellationTokenSource.Token));

        _companySupplierRepositoryMock.Verify(
            x => x.DeleteAsync(companySupplierId, companySupplierId, cancellationTokenSource.Token), 
            Times.Once);
    }

    [Fact]
    public async Task Execute_WhenRepositoryThrowsArgumentException_ShouldPropagateException()
    {
        
        var companySupplierId = 1;
        var expectedException = new ArgumentException("Invalid CompanySupplier ID");

        _companySupplierRepositoryMock
            .Setup(x => x.DeleteAsync(companySupplierId, companySupplierId, CancellationToken.None))
            .ThrowsAsync(expectedException);

        
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
            _service.Execute(companySupplierId, companySupplierId, CancellationToken.None));

        Assert.Equal(expectedException.Message, exception.Message);
        _companySupplierRepositoryMock.Verify(
            x => x.DeleteAsync(companySupplierId, companySupplierId, CancellationToken.None), 
            Times.Once);
    }
}
