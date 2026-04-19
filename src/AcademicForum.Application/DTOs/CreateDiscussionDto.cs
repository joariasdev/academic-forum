namespace AcademicForum.Application.DTOs;
public class CreateDiscussionDto
{
    public string Comment { get; set; } = String.Empty;
    public DateTime Date { get; set; }
    public int MovieId { get; set; }
    public int EventId { get; set; }
    public int MemberId { get; set; }
}

