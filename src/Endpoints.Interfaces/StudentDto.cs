namespace Endpoints.Interfaces;

public class StudentDto
{
    public required string ExternalId { get; set; }
    
    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? Address { get; set; }
}