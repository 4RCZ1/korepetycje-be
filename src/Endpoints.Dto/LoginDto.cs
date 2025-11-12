namespace Endpoints.Dto;

public class LoginDto
{
    public required bool NewPasswordRequired { get; set; }
    public string? AccessToken { get; set; }
    public string? UserGroup { get; set; }
    public string? AuthSession { get; set; }
}
