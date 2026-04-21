using AcademicForum.Domain.Entities;
using AcademicForum.Infrastructure.Contracts;
using AcademicForum.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AcademicForum.Infrastructure.Repositories;

public class VenueRepository : IGenericRepository<Venue>
{
    private readonly ApplicationDbContext _context;
    public VenueRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Venue>> GetAllAsync()
    {
        return await _context.Venues.ToListAsync();
    }
    public async Task<Venue?> GetByIdAsync(int id)
    {
        return await _context.Venues.Include(v => v.Events).FirstOrDefaultAsync(v => v.Id == id);
    }
    public async Task AddAsync(Venue venue)
    {
        await _context.Venues.AddAsync(venue);
        
    }
    public async Task UpdateAsync(Venue venue)
    {
        _context.Venues.Update(venue);
        
    }
    public async Task DeleteAsync(int id)
    {
        var venue = await _context.Venues.FindAsync(id);
        if (venue != null)
        {
            _context.Venues.Remove(venue);
            
        }
    }


}
