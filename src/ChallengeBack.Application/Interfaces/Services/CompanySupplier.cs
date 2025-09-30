using ChallengeBack.Application.Dto.Base;
using ChallengeBack.Application.Dto.CompanySupplier;
using CompanySupplierEntity = ChallengeBack.Domain.Entities.CompanySupplier;

namespace ChallengeBack.Application.Interfaces.Services;

public class AddCompanySupplierDto {
    public int CompanyId { get; set; }
    public int SupplierId { get; set; }
}

public interface IDeleteCompanySupplierService {
    Task Execute(int companyId, int supplierId, CancellationToken ct);
}

public interface IAddCompanySupplierService {
    Task Execute(AddCompanySupplierDto addCompanySupplierDto, CancellationToken ct);
}

public interface IGetAllCompanySuppliersService {
    Task<PagedResultDto<CompanySupplierEntity>> Execute(GetAllCompanySupplierFilterDto filter, CancellationToken ct);
}