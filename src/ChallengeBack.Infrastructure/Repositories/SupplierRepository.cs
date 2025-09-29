using ChallengeBack.Application.Dto.Supplier;
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

    public async Task<Supplier> GetByIdAsync(int id, CancellationToken ct) => 
        await _context.Suppliers
            .Include(s => s.CompanySuppliers)
            .ThenInclude(cs => cs.Company)
            .FirstOrDefaultAsync(s => s.Id == id, ct) ?? throw new Exception("Supplier not found");
    
    public async Task<IEnumerable<Supplier>> GetAllWithFilterAsync(GetAllSupplierFilterDto filter, CancellationToken ct)
    {
        var query = _context.Suppliers
            .Include(s => s.CompanySuppliers)
            .ThenInclude(cs => cs.Company)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(s => s.Name.Contains(filter.Name));
        }

        if (!string.IsNullOrWhiteSpace(filter.Cpf))
        {
            query = query.Where(s => s.Cpf != null && s.Cpf.Contains(filter.Cpf));
        }

        if (!string.IsNullOrWhiteSpace(filter.Cnpj))
        {
            query = query.Where(s => s.Cnpj != null && s.Cnpj.Contains(filter.Cnpj));
        }

        return await query.ToListAsync(ct);
    }
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