using ChallengeBack.Application.Interfaces;
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
    public async Task<IEnumerable<Company>> GetAllAsync() => await _context.Companies.ToListAsync();
    public async Task<Company> AddAsync(Company company)
    {
        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();
        return company;
    }
    public async Task<Company> UpdateAsync(Company company)
    {
        _context.Companies.Update(company);
        await _context.SaveChangesAsync();
        return company;
    }
    public async Task<Company> DeleteAsync(int id)
    {
        var company = await GetByIdAsync(id);
        _context.Companies.Remove(company);
        await _context.SaveChangesAsync();
        return company;
    }
}
