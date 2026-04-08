using AcademicForum.Domain.Entities;
using AcademicForum.Infrastructure.Contracts;
using AcademicForum.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AcademicForum.Infrastructure.Repositories;

public class ResponseRepository : IGenericRepository<Response>
{
    private readonly ApplicationDbContext _context;
    public ResponseRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Response>> GetAllAsync()
    {
        return await _context.Responses.ToListAsync();
    }
    public async Task<Response?> GetByIdAsync(int id)
    {
        return await _context.Responses.FindAsync(id);
    }
    public async Task AddAsync(Response response)
    {
        await _context.Responses.AddAsync(response);
        
    }
    public async Task UpdateAsync(Response response)
    {
        _context.Responses.Update(response);
        
    }
    public async Task DeleteAsync(int id)
    {
        var response = await _context.Responses.FindAsync(id);
        if (response != null)
        {
            _context.Responses.Remove(response);
            
        }
    }


}
