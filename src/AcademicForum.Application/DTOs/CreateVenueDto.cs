namespace AcademicForum.Application.DTOs;

public class CreateVenueDto
{
    public string Name { get; set; } = String.Empty;
    public string Address { get; set; } = String.Empty;
    public int Capacity { get; set; }
}
