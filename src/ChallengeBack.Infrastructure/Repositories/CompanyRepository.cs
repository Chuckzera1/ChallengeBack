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

    public async Task<Company> GetByIdAsync(int id) => await _context.Companies.FindAsync(id) ?? throw new Exception("Company not found");
    public async Task<IEnumerable<Company>> GetAllAsync(CancellationToken ct) => await _context.Companies.ToListAsync(ct);
    public async Task<Company> AddAsync(Company company, CancellationToken ct)
    {
        await _context.Companies.AddAsync(company);
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
        var company = await GetByIdAsync(id);
        _context.Companies.Remove(company);
        await _context.SaveChangesAsync(ct);
    }
}
