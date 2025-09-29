using ChallengeBack.Application.Interfaces.Repositories;
using ChallengeBack.Domain.Entities;
using ChallengeBack.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChallengeBack.Infrastructure.Repositories;

public class SupplierRepository : ISupplierRepository {
    private readonly ApplicationDbContext _context;

    public SupplierRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Supplier> GetByIdAsync(int id, CancellationToken ct) => await _context.Suppliers.FindAsync(id, ct) ?? throw new Exception("Supplier not found");
    public async Task<IEnumerable<Supplier>> GetAllAsync(CancellationToken ct) => await _context.Suppliers.ToListAsync(ct);
    public async Task<Supplier> AddAsync(Supplier supplier, CancellationToken ct) {
         _context.Suppliers.Add(supplier);
         await _context.SaveChangesAsync(ct);
         return supplier;
    }
    public async Task<Supplier> UpdateAsync(Supplier supplier, CancellationToken ct) {
        _context.Suppliers.Update(supplier);
        await _context.SaveChangesAsync(ct);
        return supplier;
    }
    public async Task<Supplier> DeleteAsync(int id, CancellationToken ct){
        var supplier = await _context.Suppliers.FindAsync(id, ct) ?? throw new Exception("Supplier not found");
        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync(ct);
        return supplier;
    }
}