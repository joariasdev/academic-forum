namespace AcademicForum.Application.DTOs;
public class CreateAttendeeRecordDto
{
    public bool HasAttended { get; set; }
    public int EventId { get; set; }
    public int MemberId { get; set; }
}
