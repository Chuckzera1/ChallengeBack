using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChallengeBack.Domain.Entities;

namespace ChallengeBack.Infrastructure.Mappers;

public class CompanyMapper : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies").HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        builder.Property(c => c.Cnpj).IsRequired().HasMaxLength(14);
        builder.Property(c => c.FantasyName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.ZipCode).IsRequired().HasMaxLength(8);
        builder.Property(c => c.State).IsRequired();
        builder.Property(c => c.CreatedAt).IsRequired();
        builder.Property(c => c.UpdatedAt).IsRequired();
    }
}
