namespace ChallengeBack.Application.Dto.Company;

public class CompanyResponseDto
{
    public int Id { get; set; }
    public string Cnpj { get; set; } = string.Empty;
    public string FantasyName { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<CompanySupplierResponseDto> CompanySuppliers { get; set; } = new();
}

public class CompanySupplierResponseDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int SupplierId { get; set; }
    public SupplierBasicDto Supplier { get; set; } = null!;
}
