using AcademicForum.Domain.Entities;
using AcademicForum.Infrastructure.Contracts;
using AcademicForum.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AcademicForum.Infrastructure.Repositories;

public class DiscussionRepository : IGenericRepository<Discussion>
{
    private readonly ApplicationDbContext _context;
    public DiscussionRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Discussion>> GetAllAsync()
    {
        return await _context.Discussions.ToListAsync();
    }
    public async Task<Discussion?> GetByIdAsync(int id)
    {
        return await _context.Discussions.FindAsync(id);
    }
    public async Task AddAsync(Discussion discussion)
    {
        await _context.Discussions.AddAsync(discussion);
        
    }
    public async Task UpdateAsync(Discussion discussion)
    {
        _context.Discussions.Update(discussion);
        
    }
    public async Task DeleteAsync(int id)
    {
        var discussion = await _context.Discussions.FindAsync(id);
        if (discussion != null)
        {
            _context.Discussions.Remove(discussion);
            
        }
    }


}
