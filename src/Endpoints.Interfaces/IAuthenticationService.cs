using Endpoints.Interfaces.Authorization;

namespace Endpoints.Interfaces;

public interface IAuthenticationService
{
    Task RegisterStudentAsync(string externalStudentId, string email);
    Task<LoginDto> LogInAsync(string username, string password);
    Task<LoginDto> ChangePasswordAsync(string authSession, string username, string newPassword);
    Task<UserIdentity> AuthenticateAsync(string accessToken);
}
