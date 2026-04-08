namespace AcademicForum.Domain.Entities;

public class Event
{

    public int Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public int MovieId { get; set; }
    public int VenueId { get; set; }
    public required Movie Movie { get; set; }
    public required Venue Venue { get; set; }
    public ICollection<Discussion> Discussions {get; set;} = new List<Discussion>();
    public ICollection<AttendeeRecord> AttendeeRecords {get; set;} = new List<AttendeeRecord>();

}
