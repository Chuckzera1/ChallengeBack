namespace ChallengeBack.Domain.Entities;

public class CompanySupplier
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int SupplierId { get; set; }

    // Navigation properties
    public virtual Company Company { get; set; } = null!;
    public virtual Supplier Supplier { get; set; } = null!;

    public CompanySupplier() { }
}
