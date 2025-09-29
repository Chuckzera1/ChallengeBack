using ChallengeBack.Application.Dto.Supplier;
using ChallengeBack.Domain.Entities;

namespace ChallengeBack.Application.Interfaces.Services;

public interface ICreateSupplierService {
    Task<Supplier> Execute(CreateSupplierDto createSupplierDto, CancellationToken ct);
}
