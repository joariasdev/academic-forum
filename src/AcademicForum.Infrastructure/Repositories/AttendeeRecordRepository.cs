using AcademicForum.Domain.Entities;
using AcademicForum.Infrastructure.Contracts;
using AcademicForum.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AcademicForum.Infrastructure.Repositories;

public class AttendeeRecordRepository : IGenericRepository<AttendeeRecord>
{
    private readonly ApplicationDbContext _context;
    public AttendeeRecordRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<AttendeeRecord>> GetAllAsync()
    {
        return await _context.AttendeeRecords.ToListAsync();
    }
    public async Task<AttendeeRecord?> GetByIdAsync(int id)
    {
        return await _context.AttendeeRecords.FindAsync(id);
    }
    public async Task AddAsync(AttendeeRecord attendeeRecord)
    {
        await _context.AttendeeRecords.AddAsync(attendeeRecord);
    }
    public async Task UpdateAsync(AttendeeRecord attendeeRecord)
    {
        _context.AttendeeRecords.Update(attendeeRecord);
    }
    public async Task DeleteAsync(int id)
    {
        var attendeeRecord = await _context.AttendeeRecords.FindAsync(id);
        if (attendeeRecord != null)
        {
            _context.AttendeeRecords.Remove(attendeeRecord);
        }
    }


}
