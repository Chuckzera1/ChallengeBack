namespace ChallengeBack.Application.Dto.Supplier;

public class SupplierListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string? Cpf { get; set; }
    public string? Cnpj { get; set; }
    public string? Rg { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<SupplierCompanyListDto> CompanySuppliers { get; set; } = new();
}

public class SupplierCompanyListDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int SupplierId { get; set; }
    public CompanyBasicDto Company { get; set; } = null!;
}

public class CompanyBasicDto
{
    public int Id { get; set; }
    public string Cnpj { get; set; } = string.Empty;
    public string FantasyName { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
