using System.Text.Json;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Authentication;
using Database;
using Endpoints.Interfaces;
using Endpoints.Interfaces.Authorization;
using FileStorage;
using Services;
using Services.Interfaces;

namespace Endpoints;

internal static class ServiceFactory
{
    public static async Task<IAuthenticationService> CreateAuthenticationService()
    {
        var cognitoSecret = await GetSecretAsync("dev/cognito");
        return new AuthenticationService(
            cognitoSecret["user_pool_id"],
            cognitoSecret["client_app_id"],
            cognitoSecret["client_app_secret"]);
    }

    public static async Task<ITimetableService> CreateTimetableServiceAsync(UserIdentity identity)
    {
        var transactor = await CreateTransactor(identity);
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Warsaw");
        return new TimetableService(transactor, timeZone, new Clock());
    }

    private class Clock : IClock
    {
        public DateTimeOffset Now => DateTimeOffset.UtcNow;
    }

    public static async Task<IAddressService> CreateAddressServiceAsync(UserIdentity identity)
    {
        var transactor = await CreateTransactor(identity);
        return new AddressService(transactor);
    }

    public static async Task<IStudentService> CreateStudentServiceAsync(UserIdentity identity)
    {
        var transactor = await CreateTransactor(identity);
        return new StudentService(transactor);
    }

    public static async Task<ILessonSuggestionService> CreateLessonSuggestionServiceAsync(
        UserIdentity identity)
    {
        var transactor = await CreateTransactor(identity);
        return new LessonSuggestionService(transactor);
    }
    
    public static async Task<IStudentResourcesService> CreateStudentResourcesServiceAsync(
        UserIdentity identity)
    {
        var transactor = await CreateTransactor(identity);
        return new StudentResourcesService(transactor);
    }

    public static async Task<IResourceService> CreateResourceServiceAsync(UserIdentity identity)
    {
        var transactor = await CreateTransactor(identity);
        var bucketName = Environment.GetEnvironmentVariable("S3_BUCKET_NAME")
                         ?? throw new Exception("S3_BUCKET_NAME environment variable not set");
        return new ResourceService(transactor, new S3Client(bucketName));
    }

    private static async Task<string> GetConnectionStringAsync()
    {
        var environmentConnection = Environment.GetEnvironmentVariable("KOREPETYCJE_POSTGRES");
        if (!string.IsNullOrWhiteSpace(environmentConnection))
            return environmentConnection;
        return await GetCloudConnectionStringAsync();
    }

    private static async Task<string> GetCloudConnectionStringAsync()
    {
        var usernamePassword = await GetSecretAsync("rds!db-038b4d1d-cb2e-4fa9-95f9-50e04af2e276");
        var hostPort = await GetSecretAsync("dev/dbhost");
        return $"Server={hostPort["endpoint"]};Database=postgres;Port={hostPort["port"]};" +
               $"User Id={usernamePassword["username"]};Password={usernamePassword["password"]}";
    }

    private static async Task<IDictionary<string, string>> GetSecretAsync(string secretName)
    {
        const string region = "eu-central-1";

        var client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));
        var request = new GetSecretValueRequest
        {
            SecretId = secretName,
        };

        var response = await client.GetSecretValueAsync(request);
        return JsonSerializer.Deserialize<IDictionary<string, string>>(response.SecretString)
               ?? throw new Exception("AWS secret deserialized to null.");
    }

    private static async Task<ITransactor> CreateTransactor(UserIdentity identity)
    {
        var connection = await Connection;
        return new Transactor(connection, identity.ExternalTenantId);
    }

    private static readonly Task<string> Connection = GetConnectionStringAsync();
}
