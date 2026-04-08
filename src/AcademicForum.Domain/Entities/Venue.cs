namespace AcademicForum.Domain.Entities;

public class Venue
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string Address { get; set; } = String.Empty;
    public int Capacity { get; set; }
    public ICollection<Event> Events { get; set; } = new List<Event>();
}
