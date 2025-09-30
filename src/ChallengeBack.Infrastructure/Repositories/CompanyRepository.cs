using ChallengeBack.Application.Dto.Company;
using ChallengeBack.Application.Dto.Base;
using ChallengeBack.Application.Interfaces.Repositories;
using ChallengeBack.Domain.Entities;
using ChallengeBack.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChallengeBack.Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly ApplicationDbContext _context;

    public CompanyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Company> GetByIdAsync(int id, CancellationToken ct) => 
        await _context.Companies
            .Include(c => c.CompanySuppliers)
            .ThenInclude(cs => cs.Supplier)
            .FirstOrDefaultAsync(c => c.Id == id, ct) ?? throw new Exception("Company not found");
    
    public async Task<PagedResultDto<Company>> GetAllWithFilterAsync(GetAllCompanyFilterDto filter, CancellationToken ct)
    {
        var query = _context.Companies
            .Include(c => c.CompanySuppliers)
            .ThenInclude(cs => cs.Supplier)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(c => c.FantasyName.Contains(filter.Name));
        }

        if (!string.IsNullOrWhiteSpace(filter.Cnpj))
        {
            query = query.Where(c => c.Cnpj != null && c.Cnpj.Contains(filter.Cnpj));
        }

        var totalCount = await query.CountAsync(ct);
        
        var companies = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip(filter.Skip)
            .Take(filter.Limit)
            .ToListAsync(ct);

        return new PagedResultDto<Company>
        {
            Data = companies,
            TotalCount = totalCount,
            Page = filter.Page,
            Limit = filter.Limit
        };
    }
    public async Task<Company> AddAsync(Company company, CancellationToken ct)
    {
        _context.Companies.Add(company);
        await _context.SaveChangesAsync(ct);
        return company;
    }
    public async Task<Company> UpdateAsync(Company company, CancellationToken ct)
    {
        _context.Companies.Update(company);
        await _context.SaveChangesAsync(ct);
        return company;
    }
    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var company = await GetByIdAsync(id, ct);
        _context.Companies.Remove(company);
        await _context.SaveChangesAsync(ct);
    }
}
