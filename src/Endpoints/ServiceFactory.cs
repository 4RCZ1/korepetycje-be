using System.Text.Json;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Database;
using Endpoints.Interfaces;
using Services;

namespace Endpoints;

internal static class ServiceFactory
{
    public static async Task<ITimetableService> CreateTimetableServiceAsync()
    {
        var connection = await Connection;
        return new TimetableService(new Transactor(connection), TimeZoneInfo.Utc);
    }

    public static async Task<IAddressService> CreateAddressServiceAsync()
    {
        var connection = await Connection;
        return new AddressService(new Transactor(connection));
    }

    public static async Task<IStudentService> CreateStudentServiceAsync()
    {
        var connection = await Connection;
        return new StudentService(new Transactor(connection));
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

    private static readonly Task<string> Connection = GetConnectionStringAsync();
}
