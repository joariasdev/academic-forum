using AcademicForum.Domain.Entities;
using AcademicForum.Infrastructure.Contracts;
using AcademicForum.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AcademicForum.Infrastructure.Repositories;

public class MovieRepository : IGenericRepository<Movie>
{
    private readonly ApplicationDbContext _context;
    public MovieRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        return await _context.Movies.ToListAsync();
    }
    public async Task<Movie?> GetByIdAsync(int id)
    {
        return await _context.Movies.FindAsync(id);
    }
    public async Task AddAsync(Movie movie)
    {
        await _context.Movies.AddAsync(movie);
        
    }
    public async Task UpdateAsync(Movie movie)
    {
        _context.Movies.Update(movie);
        
    }
    public async Task DeleteAsync(int id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie != null)
        {
            _context.Movies.Remove(movie);
            
        }
    }


}
