using ChallengeBack.Application.Interfaces.Repositories;
using ChallengeBack.Application.Interfaces.Services;

namespace ChallengeBack.Application.Services.Supplier;

public class DeleteSupplierService : IDeleteSupplierService {
    private readonly ISupplierRepository _supplierRepository;

    public DeleteSupplierService(ISupplierRepository supplierRepository) {
        _supplierRepository = supplierRepository;
    }
    
    public async Task Execute(int id, CancellationToken ct) {
        await _supplierRepository.DeleteAsync(id, ct);
    }
}