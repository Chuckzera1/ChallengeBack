using ChallengeBack.Domain.Enums;

namespace ChallengeBack.Domain.Entities;

public class Supplier
{
    public int Id { get; set; }
    public PersonType Type { get; set; }
    public string? Cpf { get; set; }
    public string? Cnpj { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string ZipCode { get; set; }
    public string? Rg { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<CompanySupplier> CompanySuppliers { get; set; } = new List<CompanySupplier>();

    public Supplier() { }

    public void SetCpf(string cpf)
    {
        if (Type != PersonType.Individual)
            throw new InvalidOperationException("CPF só pode ser definido para pessoa física");
        
        Cpf = cpf;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetCnpj(string cnpj)
    {
        if (Type != PersonType.Company)
            throw new InvalidOperationException("CNPJ só pode ser definido para pessoa jurídica");
        
        Cnpj = cnpj;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetRg(string rg)
    {
        if (Type != PersonType.Individual)
            throw new InvalidOperationException("RG só pode ser definido para pessoa física");
        
        Rg = rg;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetBirthDate(DateTime birthDate)
    {
        if (Type != PersonType.Individual)
            throw new InvalidOperationException("Data de nascimento só pode ser definida para pessoa física");
        
        BirthDate = birthDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsUnderage()
    {
        if (BirthDate == null) return false;
        
        var age = DateTime.Today.Year - BirthDate.Value.Year;
        if (BirthDate.Value.Date > DateTime.Today.AddYears(-age)) age--;
        
        return age < 18;
    }
}
