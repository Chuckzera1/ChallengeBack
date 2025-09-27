using ChallengeBack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChallengeBack.Infrastructure.Mappers;

public class SupplierMapper : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Suppliers").HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();
        builder.Property(s => s.Type).IsRequired();
        builder.Property(s => s.Cpf).IsRequired().HasMaxLength(11);
        builder.Property(s => s.Cnpj).IsRequired().HasMaxLength(14);
        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Email).IsRequired().HasMaxLength(100);
        builder.Property(s => s.ZipCode).IsRequired().HasMaxLength(8);
        builder.Property(s => s.Rg).IsRequired().HasMaxLength(11);
        builder.Property(s => s.BirthDate).IsRequired();
        builder.Property(s => s.CreatedAt).IsRequired();
        builder.Property(s => s.UpdatedAt).IsRequired();
    }
}
