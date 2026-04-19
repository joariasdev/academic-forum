namespace AcademicForum.Application.DTOs;
public class AttendeeRecordDto
{
    public int Id { get; set; }
    public bool HasAttended { get; set; }
    public int EventId { get; set; }
    public int MemberId { get; set; }
}
