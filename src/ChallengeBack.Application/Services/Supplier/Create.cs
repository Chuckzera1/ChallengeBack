using ChallengeBack.Application.Dto.Supplier;
using ChallengeBack.Application.Interfaces.Repositories;
using ChallengeBack.Application.Interfaces.Services;
using ChallengeBack.Domain.Enums;
using SupplierEntity = ChallengeBack.Domain.Entities.Supplier;

namespace ChallengeBack.Application.Services.Supplier;

public class CreateSupplierService : ICreateSupplierService {
    private readonly ISupplierRepository _supplierRepository;
    private readonly ICepRepository _cepRepository;
    public CreateSupplierService(ISupplierRepository supplierRepository, ICepRepository cepRepository) {
        _supplierRepository = supplierRepository;
        _cepRepository = cepRepository;
    }

    public async Task<SupplierEntity> Execute(CreateSupplierDto createSupplierDto, CancellationToken ct) {
        _ = await _cepRepository.GetByZipCodeAsync(createSupplierDto.ZipCode) ?? throw new Exception("CEP não encontrado");
        
        ValidateSupplierData(createSupplierDto);
        
        var supplier = new SupplierEntity {
            Name = createSupplierDto.Name,
            Email = createSupplierDto.Email,
            ZipCode = createSupplierDto.ZipCode,
            Type = createSupplierDto.Type,
            Cpf = createSupplierDto.Cpf,
            Cnpj = createSupplierDto.Cnpj,
            Rg = createSupplierDto.Rg,
            BirthDate = createSupplierDto.BirthDate,
        };

        return await _supplierRepository.AddAsync(supplier, ct);
    }

    private static void ValidateSupplierData(CreateSupplierDto dto)
{
    if (dto is null) throw new ArgumentNullException(nameof(dto));

    switch (dto.Type)
    {
        case PersonType.Individual:
            if (string.IsNullOrWhiteSpace(dto.Cpf))
                throw new ArgumentException("CPF é obrigatório para pessoa física");

            if (!string.IsNullOrWhiteSpace(dto.Cnpj))
                throw new ArgumentException("CNPJ não deve ser informado para pessoa física");

            if (string.IsNullOrWhiteSpace(dto.Rg))
                throw new ArgumentException("RG é obrigatório para pessoa física");

            if (!dto.BirthDate.HasValue)
                throw new ArgumentException("Data de nascimento é obrigatória para pessoa física");
            break;

        case PersonType.Company:
            if (string.IsNullOrWhiteSpace(dto.Cnpj))
                throw new ArgumentException("CNPJ é obrigatório para pessoa jurídica");

            if (!string.IsNullOrWhiteSpace(dto.Cpf))
                throw new ArgumentException("CPF não deve ser informado para pessoa jurídica");

            if (!string.IsNullOrWhiteSpace(dto.Rg))
                throw new ArgumentException("RG não deve ser informado para pessoa jurídica");

            if (dto.BirthDate.HasValue)
                throw new ArgumentException("Data de nascimento não deve ser informada para pessoa jurídica");
            break;

        default:
            throw new ArgumentOutOfRangeException(nameof(PersonType), "Tipo de pessoa inválido.");
    }
}

}

