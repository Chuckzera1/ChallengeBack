using ChallengeBack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChallengeBack.Infrastructure.Mappers;

public class CompanySupplierMapper : IEntityTypeConfiguration<CompanySupplier>
{
    public void Configure(EntityTypeBuilder<CompanySupplier> builder)
    {
        builder.ToTable("CompanySuppliers").HasKey(cs => cs.Id);
        builder.Property(cs => cs.Id).ValueGeneratedOnAdd();
        builder.Property(cs => cs.CompanyId).IsRequired();
        builder.Property(cs => cs.SupplierId).IsRequired();

        builder.HasIndex(cs => new { cs.CompanyId, cs.SupplierId })
               .IsUnique();

        
        builder.HasOne(cs => cs.Company)
            .WithMany(c => c.CompanySuppliers)
            .HasForeignKey(cs => cs.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(cs => cs.Supplier)
            .WithMany(s => s.CompanySuppliers)
            .HasForeignKey(cs => cs.SupplierId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
