namespace AcademicForum.Application.DTOs;
public class ResponseDto
{
    public int Id { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public int DiscussionId { get; set; }
}

