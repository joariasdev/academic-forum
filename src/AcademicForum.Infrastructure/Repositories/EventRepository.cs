using AcademicForum.Domain.Entities;
using AcademicForum.Infrastructure.Contracts;
using AcademicForum.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AcademicForum.Infrastructure.Repositories;

public class EventRepository : IGenericRepository<Event>
{
    private readonly ApplicationDbContext _context;
    public EventRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Event>> GetAllAsync()
    {
        return await _context.Events.ToListAsync();
    }
    public async Task<Event?> GetByIdAsync(int id)
    {
        return await _context.Events.FindAsync(id);
    }
    public async Task AddAsync(Event eventEntity)
    {
        await _context.Events.AddAsync(eventEntity);
        
    }
    public async Task UpdateAsync(Event eventEntity)
    {
        _context.Events.Update(eventEntity);
        
    }
    public async Task DeleteAsync(int id)
    {
        var eventEntity = await _context.Events.FindAsync(id);
        if (eventEntity != null)
        {
            _context.Events.Remove(eventEntity);
            
        }
    }


}
