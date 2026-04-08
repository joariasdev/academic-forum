namespace AcademicForum.Domain.Entities;

public class Member
{
    int Id { get; set; }
    string Name { get; set; } = String.Empty;
    string Email { get; set; } = String.Empty;
    string Role { get; set; } = String.Empty;
    public ICollection<Discussion> Discussions {get; set;} = new List<Discussion>();
    public ICollection<Response> Responses {get; set;} = new List<Response>();
    public ICollection<AttendeeRecord> AttendeeRecords {get; set;} = new List<AttendeeRecord>();
}
