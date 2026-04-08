namespace AcademicForum.Domain.Entities;

public class AttendeeRecord
{
    public int Id { get; set; }
    public bool HasAttented { get; set; }
    public int EventId { get; set; }
    public int MemberId { get; set; }
    public required Event Event { get; set; }
    public required Member Member { get; set; }
}
