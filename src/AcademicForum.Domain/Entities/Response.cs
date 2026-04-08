namespace AcademicForum.Domain.Entities;

public class Response
{
    public int Id { get; set; }
    public string Comment { get; set; } = String.Empty;
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public int DiscussionId { get; set; }
    public int MemberId { get; set; }
    public required Discussion Discussion { get; set; }
    public required Member Member { get; set; }
}

