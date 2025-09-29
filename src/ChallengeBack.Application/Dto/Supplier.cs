using ChallengeBack.Domain.Enums;

namespace ChallengeBack.Application.Dto.Supplier;

public class CreateSupplierDto {
    public string Name { get; set; }
    public string Email { get; set; }
    public string ZipCode { get; set; }
    public PersonType Type { get; set; }
    public string? Cpf { get; set; }
    public string? Cnpj { get; set; }
    public string? Rg { get; set; }
    public DateTime? BirthDate { get; set; }
}

public class UpdateSupplierDto {
    public string Name { get; set; }
    public string Email { get; set; }
    public string ZipCode { get; set; }
    public DateTime? BirthDate { get; set; }
}