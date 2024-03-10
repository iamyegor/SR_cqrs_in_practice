namespace DTOs;

public class StudentForRegistrationDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    public string? Course1 { get; set; }
    public string? Course1Grade { get; set; }
    
    public string? Course2 { get; set; }
    public string? Course2Grade { get; set; }
}