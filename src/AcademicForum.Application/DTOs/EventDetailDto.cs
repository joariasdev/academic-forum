namespace AcademicForum.Application.DTOs;
public class EventDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int MovieId { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public int VenueId { get; set; }
    public string VenueName { get; set; } = string.Empty;
    public List<DiscussionDto> Discussions { get; set; } = new();
    public List<AttendeeRecordDto> AttendeeRecords { get; set; } = new();
}
