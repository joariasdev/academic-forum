namespace AcademicForum.Domain.Entities;

public class Discussion
{
    public int Id { get; set; }
    public string Comment { get; set; } = String.Empty;
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public int MovieId { get; set; }
    public int EventId { get; set; }
    public int MemberId { get; set; }
    public required Movie Movie { get; set; }
    public required Event Event { get; set; }
    public required Member Member { get; set; }
    public ICollection<Response> Responses {get; set;} = new List<Response>();
}

