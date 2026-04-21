namespace AcademicForum.Application.DTOs;

public class DiscussionDetailDto
{
    public int Id { get; set; }
    public string Comment { get; set; } = String.Empty;
    public DateTime Date { get; set; }
    public int MovieId { get; set; }
    public int EventId { get; set; }
    public int MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public int ResponseCount { get; set; }
    public List<ResponseDto> Responses { get; set; } = new();
}

