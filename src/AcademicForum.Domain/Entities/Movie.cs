namespace AcademicForum.Domain.Entities;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Sinopsis { get; set; } = String.Empty;
    public string Director { get; set; } = String.Empty;
    public string Genre { get; set; } = String.Empty;
    public DateOnly ReleaseDate { get; set; }
    public ICollection<Event> Events {get; set;} = new List<Event>();
    public ICollection<Discussion> Discussions {get; set;} = new List<Discussion>();
}
