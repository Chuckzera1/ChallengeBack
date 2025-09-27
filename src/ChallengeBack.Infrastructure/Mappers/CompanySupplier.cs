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
        builder.Property(cs => cs.StartDate).IsRequired();
        builder.Property(cs => cs.EndDate).IsRequired();
    }
}
