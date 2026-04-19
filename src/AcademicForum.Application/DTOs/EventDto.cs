namespace AcademicForum.Application.DTOs;
public class EventDto
{
    public int Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public DateTime Date { get; set; }
    public int MovieId { get; set; }
    public int VenueId { get; set; }

}
