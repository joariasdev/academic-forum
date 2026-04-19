namespace AcademicForum.Application.DTOs;

public class MovieDto
{
    public int Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Sinopsis { get; set; } = String.Empty;
    public string Director { get; set; } = String.Empty;
    public string Genre { get; set; } = String.Empty;
    public DateTime ReleaseDate { get; set; }
}
