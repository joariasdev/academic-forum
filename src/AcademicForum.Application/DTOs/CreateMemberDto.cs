namespace AcademicForum.Application.DTOs;

public class CreateMemberDto
{
    public string Name { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string Role { get; set; } = String.Empty;
}
