using ChallengeBack.Application.Dto.Supplier;
using ChallengeBack.Domain.Entities;

namespace ChallengeBack.Application.Interfaces.Services;

public interface ICreateSupplierService {
    Task<Supplier> Execute(CreateSupplierDto createSupplierDto, CancellationToken ct);
}

public interface IGetAllSuppliersWithFilterService {
    Task<IEnumerable<Supplier>> Execute(GetAllSupplierFilterDto filter, CancellationToken ct);
}

public interface IGetSupplierByIdService {
    Task<Supplier> Execute(int id, CancellationToken ct);
}

public interface IUpdateSupplierService {
    Task<Supplier> Execute(int id, UpdateSupplierDto updateSupplierDto, CancellationToken ct);
}

public interface IDeleteSupplierService {
    Task Execute(int id, CancellationToken ct);
}
