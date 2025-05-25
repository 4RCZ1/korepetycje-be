using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace HelloWorld;

public class Function
{

    private static readonly HttpClient client = new HttpClient();
    private static readonly IConfiguration _configuration;

    static Function()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    private static async Task<string> GetCallingIP()
    {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Add("User-Agent", "AWS Lambda .Net Client");

        var msg = await client.GetStringAsync("http://checkip.amazonaws.com/").ConfigureAwait(continueOnCapturedContext:false);

        return msg.Replace("\n","");
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
    {
        var connectionString = _configuration.GetConnectionString("InzyniergaCon");

        var optionsBuilder = new DbContextOptionsBuilder<OurDbContext>();
        optionsBuilder.UseNpgsql(connectionString); 

        using var dbContext = new OurDbContext(optionsBuilder.Options);
        var location = await GetCallingIP();
        var body = new Dictionary<string, string>
        {
            { "message", "hello world" },
            { "location", location }
        };

        return new APIGatewayProxyResponse
        {
            Body = JsonSerializer.Serialize(body),
            StatusCode = 200,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}