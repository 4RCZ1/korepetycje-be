using System.Security.Cryptography;
using System.Text;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Endpoints.Interfaces;
using Endpoints.Interfaces.Authorization;

namespace Authentication;

public class AuthenticationService : IAuthenticationService
{
    public AuthenticationService(
        string userPoolId,
        string appClientId,
        string appClientSecret)
    {
        _userPoolId = userPoolId;
        _appClientId = appClientId;
        _appClientSecret = appClientSecret;
    }

    public async Task RegisterStudentAsync(string externalStudentId, string email)
    {
        var createUserResponse = await _identityProvider.AdminCreateUserAsync(
            new AdminCreateUserRequest
            {
                DesiredDeliveryMediums = ["EMAIL"],
                UserPoolId = _userPoolId,
                Username = email,
                UserAttributes =
                [
                    new AttributeType
                    {
                        Name = "email",
                        Value = email,
                    },
                    new AttributeType
                    {
                        Name = StudentIdAttributeName,
                        Value = externalStudentId,
                    },
                ],
            });
        await _identityProvider.AdminAddUserToGroupAsync(
            new AdminAddUserToGroupRequest
            {
                UserPoolId = _userPoolId,
                Username = createUserResponse.User.Username,
                GroupName = StudentGroupName,
            });
    }

    public async Task<LoginDto> LogInAsync(string username, string password)
    {
        var initAuthResponse = await _identityProvider.InitiateAuthAsync(new InitiateAuthRequest
        {
            AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
            AuthParameters = new Dictionary<string, string>
            {
                { "USERNAME", username },
                { "PASSWORD", password },
                { "SECRET_HASH", ComputeSecretHash(username) },
            },
            ClientId = _appClientId,
        });
        if (initAuthResponse.AuthenticationResult is not null)
            return await SuccessfulLogin(initAuthResponse.AuthenticationResult.AccessToken);

        if (initAuthResponse.ChallengeName.Value == ChallengeNameType.NEW_PASSWORD_REQUIRED)
        {
            return new LoginDto
            {
                NewPasswordRequired = true,
                AuthSession = initAuthResponse.Session,
            };
        }

        throw new ApplicationException(
            $"Unexpected auth challenge: {initAuthResponse.ChallengeName.Value}");
    }

    public async Task<LoginDto> ChangePasswordAsync(
        string authSession, string username, string newPassword)
    {
        var response = await _identityProvider.RespondToAuthChallengeAsync(
            new RespondToAuthChallengeRequest
            {
                ChallengeName = ChallengeNameType.NEW_PASSWORD_REQUIRED,
                ClientId = _appClientId,
                Session = authSession,
                ChallengeResponses = new Dictionary<string, string>
                {
                    { "USERNAME", username },
                    { "NEW_PASSWORD", newPassword },
                    { "SECRET_HASH", ComputeSecretHash(username) },
                },
            });
        if (response.AuthenticationResult is not null)
            return await SuccessfulLogin(response.AuthenticationResult.AccessToken);

        throw new ApplicationException($"Further challenge: {response.ChallengeName.Value}");
    }

    public async Task<UserIdentity> AuthenticateAsync(string accessToken)
    {
        var response = await _identityProvider.GetUserAsync(
            new GetUserRequest { AccessToken = accessToken });
        var groupResponse = await _identityProvider.AdminListGroupsForUserAsync(
            new AdminListGroupsForUserRequest
            {
                Username = response.Username,
                UserPoolId = _userPoolId,
            });
        if (groupResponse.Groups.Any(g => g.GroupName == StudentGroupName))
        {
            var studentIdAttribute =
                response.UserAttributes.Find(a => a.Name == StudentIdAttributeName);
            if (studentIdAttribute is null)
                throw new ApplicationException("Student without ID encountered.");
            return new UserIdentity
            {
                AsStudent = new StudentRole { ExternalStudentId = studentIdAttribute.Value },
            };
        }

        if (groupResponse.Groups.Any(g => g.GroupName == TutorGroupName))
            return new UserIdentity { AsTutor = new TutorRole() };
        throw new ApplicationException(UserWithoutGroupErrorMessage);
    }

    private string ComputeSecretHash(string username)
    {
        var ascii = new ASCIIEncoding();
        var hmac = new HMACSHA256(ascii.GetBytes(_appClientSecret));
        var hash = hmac.ComputeHash(ascii.GetBytes(username + _appClientId));
        return Convert.ToBase64String(hash);
    }

    private async Task<LoginDto> SuccessfulLogin(string accessToken)
    {
        var identity = await AuthenticateAsync(accessToken);
        return new LoginDto
        {
            NewPasswordRequired = false,
            AccessToken = accessToken,
            UserGroup = GetUserGroupName(identity),
        };
    }

    private static string GetUserGroupName(UserIdentity identity)
    {
        if (identity.AsStudent.HasValue)
            return StudentGroupName;
        if (identity.AsTutor.HasValue)
            return TutorGroupName;
        throw new ApplicationException(UserWithoutGroupErrorMessage);
    }

    private const string StudentGroupName = "students";
    private const string TutorGroupName = "tutors";
    private const string StudentIdAttributeName = "custom:student_id";
    private const string UserWithoutGroupErrorMessage = "User not in any group encountered.";

    private readonly string _userPoolId;
    private readonly string _appClientId;
    private readonly string _appClientSecret;
    private readonly AmazonCognitoIdentityProviderClient _identityProvider = new();
}
