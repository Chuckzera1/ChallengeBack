namespace ChallengeBack.Application.Dto.Company;

public class CompanyListDto
{
    public int Id { get; set; }
    public string Cnpj { get; set; } = string.Empty;
    public string FantasyName { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<CompanySupplierBasicDto> CompanySuppliers { get; set; } = new();
}

public class CompanySupplierBasicDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int SupplierId { get; set; }
    public SupplierBasicDto Supplier { get; set; } = null!;
}

public class SupplierBasicDto
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
}
