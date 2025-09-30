using ChallengeBack.Application.Dto.CompanySupplier;
using ChallengeBack.Application.Dto.Base;
using ChallengeBack.Application.Interfaces.Repositories;
using ChallengeBack.Domain.Entities;
using ChallengeBack.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
    
    public async Task<CompanySupplier> DeleteAsync(int companyId, int supplierId, CancellationToken ct) {
        var companySupplier = await _context.CompanySuppliers
            .FirstOrDefaultAsync(cs => cs.CompanyId == companyId && cs.SupplierId == supplierId, ct)
             ?? throw new Exception("CompanySupplier not found");
        _context.CompanySuppliers.Remove(companySupplier);
        await _context.SaveChangesAsync(ct);
        return companySupplier;
    }
    
    public async Task<PagedResultDto<CompanySupplier>> GetAllWithFilterAsync(GetAllCompanySupplierFilterDto filter, CancellationToken ct)
    {
        var query = _context.CompanySuppliers
            .Include(cs => cs.Company)
            .Include(cs => cs.Supplier)
            .AsQueryable();

        if (filter.CompanyId.HasValue)
        {
            query = query.Where(cs => cs.CompanyId == filter.CompanyId.Value);
        }

        if (filter.SupplierId.HasValue)
        {
            query = query.Where(cs => cs.SupplierId == filter.SupplierId.Value);
        }

        var totalCount = await query.CountAsync(ct);
        
        var companySuppliers = await query
            .Skip(filter.Skip)
            .Take(filter.Limit)
            .ToListAsync(ct);

        return new PagedResultDto<CompanySupplier>
        {
            Data = companySuppliers,
            TotalCount = totalCount,
            Page = filter.Page,
            Limit = filter.Limit
        };
    }
}
