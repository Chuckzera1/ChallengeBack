namespace ChallengeBack.Application.Dto.CompanySupplier;

public class CompanySupplierResponseDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int SupplierId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyCnpj { get; set; } = string.Empty;
    public string SupplierName { get; set; } = string.Empty;
    public string SupplierEmail { get; set; } = string.Empty;
    public string SupplierType { get; set; } = string.Empty;
    public string SupplierCpf { get; set; } = string.Empty;
    public string SupplierCnpj { get; set; } = string.Empty;
}
