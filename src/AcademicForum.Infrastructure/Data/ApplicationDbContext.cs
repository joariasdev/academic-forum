using AcademicForum.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AcademicForum.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Movie> Movies { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Venue> Venues { get; set; }
    public DbSet<Discussion> Discussions { get; set; }
    public DbSet<Response> Responses { get; set; }
    public DbSet<AttendeeRecord> AttendeeRecords { get; set; }
}
