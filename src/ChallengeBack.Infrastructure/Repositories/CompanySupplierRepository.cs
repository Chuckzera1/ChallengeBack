using ChallengeBack.Application.Interfaces.Repositories;
using ChallengeBack.Domain.Entities;
using ChallengeBack.Infrastructure.Data;

namespace ChallengeBack.Infrastructure.Repositories;

public class CompanySupplierRepository : ICompanySupplierRepository {
    private readonly ApplicationDbContext _context;

    public CompanySupplierRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<CompanySupplier> AddAsync(CompanySupplier companySupplier, CancellationToken ct) {
        _context.CompanySuppliers.Add(companySupplier);
        await _context.SaveChangesAsync(ct);
        return companySupplier;
    }
    public async Task<CompanySupplier> UpdateAsync(CompanySupplier companySupplier, CancellationToken ct) {
        _context.CompanySuppliers.Update(companySupplier);
        await _context.SaveChangesAsync(ct);
        return companySupplier;
    }
    public async Task<CompanySupplier> DeleteAsync(int id, CancellationToken ct) {
        var companySupplier = await _context.CompanySuppliers.FindAsync(id, ct) ?? throw new Exception("CompanySupplier not found");
        _context.CompanySuppliers.Remove(companySupplier);
        await _context.SaveChangesAsync(ct);
        return companySupplier;
    }
}
