using ChallengeBack.Application.Dto.Supplier;
using ChallengeBack.Application.Interfaces.Repositories;
using ChallengeBack.Application.Interfaces.Services;
using SupplierEntity = ChallengeBack.Domain.Entities.Supplier;

namespace ChallengeBack.Application.Services.Supplier;

public class UpdateSupplierService : IUpdateSupplierService {
    private readonly ISupplierRepository _supplierRepository;

    public UpdateSupplierService(ISupplierRepository supplierRepository) {
        _supplierRepository = supplierRepository;
    }

    public async Task<SupplierEntity> Execute(int id, UpdateSupplierDto updateSupplierDto, CancellationToken ct) {
        var supplier = await _supplierRepository.GetByIdAsync(id, ct);
        supplier.Name = updateSupplierDto.Name;
        supplier.Email = updateSupplierDto.Email;
        supplier.BirthDate = updateSupplierDto.BirthDate;
        supplier.ZipCode = updateSupplierDto.ZipCode;

        return await _supplierRepository.UpdateAsync(supplier, ct);
    }
}