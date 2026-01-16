using System.Security.Cryptography;
using System.Text;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Endpoints.Interfaces;

namespace Authentication;

internal class CognitoClient
{
    public async Task<GetUserResponse> GetUserAsync(string accessToken)
    {
        try
        {
            return await _impl.GetUserAsync(new GetUserRequest { AccessToken = accessToken });
        }
        catch (NotAuthorizedException)
        {
            throw CreateUserErrorException();
        }
    }

    public async Task<AdminListGroupsForUserResponse> AdminListGroupsForUserAsync(
        string username, string userPoolId)
    {
        return await _impl.AdminListGroupsForUserAsync(
            new AdminListGroupsForUserRequest
            {
                Username = username,
                UserPoolId = userPoolId,
            });
    }

    public async Task<AdminCreateUserResponse> AdminCreateUserAsync(
        string username,
        string userPoolId,
        List<AttributeType> userAttributes)
    {
        return await _impl.AdminCreateUserAsync(
            new AdminCreateUserRequest
            {
                DesiredDeliveryMediums = ["EMAIL"],
                UserPoolId = userPoolId,
                Username = username,
                UserAttributes = userAttributes,
            });
    }

    public async Task AdminAddUserToGroupAsync(string username, string groupName, string userPoolId)
    {
        await _impl.AdminAddUserToGroupAsync(
            new AdminAddUserToGroupRequest
            {
                UserPoolId = userPoolId,
                Username = username,
                GroupName = groupName,
            });
    }

    public async Task<InitiateAuthResponse> InitiateAuthAsync(
        string username, string password, string appClientId, string appClientSecret)
    {
        try
        {
            return await _impl.InitiateAuthAsync(new InitiateAuthRequest
            {
                AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
                AuthParameters = new Dictionary<string, string>
                {
                    { "USERNAME", username },
                    { "PASSWORD", password },
                    { "SECRET_HASH", ComputeSecretHash(username, appClientId, appClientSecret) },
                },
                ClientId = appClientId,
            });
        }
        catch (NotAuthorizedException)
        {
            throw CreateUserErrorException();
        }
    }

    public async Task<RespondToAuthChallengeResponse> RespondToAuthChallengeAsync(
        string authSession,
        string username,
        string newPassword,
        string appClientId,
        string appClientSecret)
    {
        return await _impl.RespondToAuthChallengeAsync(
            new RespondToAuthChallengeRequest
            {
                ChallengeName = ChallengeNameType.NEW_PASSWORD_REQUIRED,
                ClientId = appClientId,
                Session = authSession,
                ChallengeResponses = new Dictionary<string, string>
                {
                    { "USERNAME", username },
                    { "NEW_PASSWORD", newPassword },
                    { "SECRET_HASH", ComputeSecretHash(username, appClientId, appClientSecret) },
                },
            });
    }

    private static string ComputeSecretHash(
        string username, string appClientId, string appClientSecret)
    {
        var ascii = new ASCIIEncoding();
        var hmac = new HMACSHA256(ascii.GetBytes(appClientSecret));
        var hash = hmac.ComputeHash(ascii.GetBytes(username + appClientId));
        return Convert.ToBase64String(hash);
    }

    private static BadRequestException CreateUserErrorException()
    {
        return new BadRequestException(
            "Wystąpił błąd uwierzytelniania." +
            " Spróbuj odświeżyć stronę lub zalogować się ponownie.");
    }

    private readonly AmazonCognitoIdentityProviderClient _impl = new();
}
