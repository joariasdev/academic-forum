namespace AcademicForum.Domain.Entities;

public class Event
{

    public int Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public DateTime Date { get; set; }
    public int MovieId { get; set; }
    public int VenueId { get; set; }
    public Movie? Movie { get; set; }
    public Venue? Venue { get; set; }
    public ICollection<Discussion> Discussions {get; set;} = new List<Discussion>();
    public ICollection<AttendeeRecord> AttendeeRecords {get; set;} = new List<AttendeeRecord>();

}
