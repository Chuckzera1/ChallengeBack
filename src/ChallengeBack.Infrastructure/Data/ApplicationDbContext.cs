using Microsoft.EntityFrameworkCore;
using ChallengeBack.Domain.Entities;
using ChallengeBack.Infrastructure.Mappers;

namespace ChallengeBack.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Company> Companies { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<CompanySupplier> CompanySuppliers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply entity configurations
        modelBuilder.ApplyConfiguration(new CompanyMapper());
        modelBuilder.ApplyConfiguration(new SupplierMapper());
        modelBuilder.ApplyConfiguration(new CompanySupplierMapper());
    }
}
