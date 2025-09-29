using CompanySupplierEntity = ChallengeBack.Domain.Entities.CompanySupplier;

namespace ChallengeBack.Application.Dto.CompanySupplier;

public class AddCompanySupplierDto {
    public int CompanyId { get; set; }
    public int SupplierId { get; set; }
}

public interface IDeleteCompanySupplierService {
    Task Execute(int id, CancellationToken ct);
}

public interface IAddCompanySupplierService {
    Task Execute(AddCompanySupplierDto addCompanySupplierDto, CancellationToken ct);
}