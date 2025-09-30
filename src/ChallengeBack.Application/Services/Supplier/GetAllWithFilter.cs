using ChallengeBack.Application.Dto.Supplier;
using ChallengeBack.Application.Dto.Base;
using ChallengeBack.Application.Interfaces.Repositories;
using ChallengeBack.Application.Interfaces.Services;
using SupplierEntity = ChallengeBack.Domain.Entities.Supplier;

namespace ChallengeBack.Application.Services.Supplier;

public class GetAllSuppliersWithFilterService : IGetAllSuppliersWithFilterService
{
    private readonly ISupplierRepository _supplierRepository;

    public GetAllSuppliersWithFilterService(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<PagedResultDto<SupplierEntity>> Execute(GetAllSupplierFilterDto filter, CancellationToken ct)
    {
        return await _supplierRepository.GetAllWithFilterAsync(filter, ct);
    }
}
