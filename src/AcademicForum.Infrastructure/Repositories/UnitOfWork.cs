using AcademicForum.Infrastructure.Data;

namespace AcademicForum.Infrastructure.Repositories;

public class UnitOfWork
{
    private readonly ApplicationDbContext _context;
    public MovieRepository Movies { get; }
    public MemberRepository Members { get; }
    public VenueRepository Venues { get; }
    public DiscussionRepository Discussions { get; }
    public ResponseRepository Responses { get; }
    public AttendeeRecordRepository AttendeeRecords { get; }
    public EventRepository Events { get; }

    public UnitOfWork(
            ApplicationDbContext context,
            MovieRepository movies,
            MemberRepository members,
            VenueRepository venues,
            DiscussionRepository discussions,
            ResponseRepository responses,
            AttendeeRecordRepository attendeeRecords,
            EventRepository events)
    {
        _context = context;
        Movies = movies;
        Members = members;
        Venues = venues;
        Discussions = discussions;
        Responses = responses;
        AttendeeRecords = attendeeRecords;
        Events = events;
    }

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }
}
