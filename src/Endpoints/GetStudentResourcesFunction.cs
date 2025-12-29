using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Endpoints.Interfaces.Authorization;

namespace Endpoints;

public class GetStudentResourcesFunction
{
    public async Task<APIGatewayProxyResponse> GetStudentResources(APIGatewayProxyRequest request,
        ILambdaContext context)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateStudentResourcesServiceAsync(identity);
            var studentId = Int32.Parse(RestIo.GetPathParameter(request, "studentId"));
            var result = service.GetStudentWithResources(studentId, role);
            return result;
        });
    }
}