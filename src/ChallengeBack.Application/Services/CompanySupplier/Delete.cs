using ChallengeBack.Application.Interfaces.Repositories;
using ChallengeBack.Application.Interfaces.Services;

namespace ChallengeBack.Application.Services.CompanySupplier;

public class DeleteCompanySupplierService : IDeleteCompanySupplierService {
    private readonly ICompanySupplierRepository _companySupplierRepository;

    public DeleteCompanySupplierService(ICompanySupplierRepository companySupplierRepository) {
        _companySupplierRepository = companySupplierRepository;
    }

    public async Task Execute(int id, CancellationToken ct) {
        await _companySupplierRepository.DeleteAsync(id, ct);
    }
}