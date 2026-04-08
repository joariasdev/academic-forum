using AcademicForum.Domain.Entities;
using AcademicForum.Infrastructure.Contracts;
using AcademicForum.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AcademicForum.Infrastructure.Repositories;

public class MemberRepository : IGenericRepository<Member>
{
    private readonly ApplicationDbContext _context;
    public MemberRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Member>> GetAllAsync()
    {
        return await _context.Members.ToListAsync();
    }
    public async Task<Member?> GetByIdAsync(int id)
    {
        return await _context.Members.FindAsync(id);
    }
    public async Task AddAsync(Member member)
    {
        await _context.Members.AddAsync(member);
        
    }
    public async Task UpdateAsync(Member member)
    {
        _context.Members.Update(member);
        
    }
    public async Task DeleteAsync(int id)
    {
        var member = await _context.Members.FindAsync(id);
        if (member != null)
        {
            _context.Members.Remove(member);
            
        }
    }


}
