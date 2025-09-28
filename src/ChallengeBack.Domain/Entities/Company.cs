namespace ChallengeBack.Domain.Entities;

public class Company
{
    public int Id { get; set; }
    public string Cnpj { get; set; }
    public string FantasyName { get; set; }
    public string ZipCode { get; set; }
    public string State { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<CompanySupplier> CompanySuppliers { get; set; } = new List<CompanySupplier>();

    public Company() { }
}

