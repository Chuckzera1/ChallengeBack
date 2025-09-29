using ChallengeBack.Application.Dto.CompanySupplier;
using ChallengeBack.Application.Interfaces.Repositories;
using CompanySupplierEntity = ChallengeBack.Domain.Entities.CompanySupplier;
using SupplierEntity = ChallengeBack.Domain.Entities.Supplier;

namespace ChallengeBack.Application.Services.CompanySupplier;

public class CreateCompanySupplierService : IAddCompanySupplierService {
    private readonly ICompanySupplierRepository _companySupplierRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly ISupplierRepository _supplierRepository;
    public CreateCompanySupplierService(ICompanySupplierRepository companySupplierRepository, ICompanyRepository companyRepository, ISupplierRepository supplierRepository) {
        _companySupplierRepository = companySupplierRepository;
        _companyRepository = companyRepository;
        _supplierRepository = supplierRepository;
    }

    public async Task Execute(AddCompanySupplierDto addCompanySupplierDto, CancellationToken ct) {

        var company = await _companyRepository.GetByIdAsync(addCompanySupplierDto.CompanyId, ct) ?? throw new Exception("Company not found");
        var supplier = await _supplierRepository.GetByIdAsync(addCompanySupplierDto.SupplierId, ct) ?? throw new Exception("Supplier not found");

        if (company.State == "PR"){
            ValidateCompanySupplierRules(supplier);
        }
        
        var companySupplier = new CompanySupplierEntity {
            CompanyId = addCompanySupplierDto.CompanyId,
            SupplierId = addCompanySupplierDto.SupplierId,
        };
        
        await _companySupplierRepository.AddAsync(companySupplier, ct);
    }

    private static void ValidateCompanySupplierRules(SupplierEntity supplier) {
        if(supplier.IsUnderage()){
            throw new Exception("Fornecedor não pode ser menor de idade");
        }
    }
}