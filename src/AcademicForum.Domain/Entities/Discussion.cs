namespace AcademicForum.Domain.Entities;

public class Discussion
{
    public int Id { get; set; }
    public string Comment { get; set; } = String.Empty;
    public DateTime Date { get; set; }
    public int MovieId { get; set; }
    public int EventId { get; set; }
    public int MemberId { get; set; }
    public Movie? Movie { get; set; }
    public Event? Event { get; set; }
    public Member? Member { get; set; }
    public ICollection<Response> Responses {get; set;} = new List<Response>();
}

