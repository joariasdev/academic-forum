namespace AcademicForum.Application.DTOs;
public class ResponseDto
{
    public int Id { get; set; }
    public string Comment { get; set; } = String.Empty;
    public DateTime Date { get; set; }
    public int DiscussionId { get; set; }
    public int MemberId { get; set; }
}

