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
        builder.Property(s => s.Cpf).HasMaxLength(11);
        builder.HasIndex(s => s.Cpf).IsUnique();
        builder.Property(s => s.Cnpj).HasMaxLength(14);
        builder.HasIndex(s => s.Cnpj).IsUnique();
        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Email).IsRequired().HasMaxLength(100);
        builder.Property(s => s.ZipCode).IsRequired().HasMaxLength(8);
        builder.Property(s => s.Rg).HasMaxLength(11);
        builder.HasIndex(s => s.Rg).IsUnique();
        builder.Property(s => s.BirthDate);
        builder.Property(s => s.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAdd();
        builder.Property(s => s.UpdatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
    }
}
