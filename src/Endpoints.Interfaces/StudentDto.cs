namespace Endpoints.Interfaces;

public class StudentDto
{
    public string? ExternalId { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; } // todo: persist
    public AddressDto? Address { get; set; }
}
