namespace ChallengeBack.Domain.Entities;

public class CompanySupplier
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int SupplierId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    // Navigation properties
    public virtual Company Company { get; set; } = null!;
    public virtual Supplier Supplier { get; set; } = null!;

    public CompanySupplier() { }
}
