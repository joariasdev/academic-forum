namespace AcademicForum.Domain.Entities;

public class Member
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string Role { get; set; } = String.Empty;
    public ICollection<Discussion> Discussions {get; set;} = new List<Discussion>();
    public ICollection<Response> Responses {get; set;} = new List<Response>();
    public ICollection<AttendeeRecord> AttendeeRecords {get; set;} = new List<AttendeeRecord>();
}
