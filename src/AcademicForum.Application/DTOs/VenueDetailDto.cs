namespace AcademicForum.Application.DTOs;

public class VenueDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string Address { get; set; } = String.Empty;
    public int Capacity { get; set; }

    public List<EventDto> Events { get; set; } = new();
}
