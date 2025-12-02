using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.StudentFunctions;

public class GetStudentAssignmentsFunction
{
    public static async Task<APIGatewayProxyResponse> GetResourceAssignments(APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateStudentServiceAsync(identity);
            var studentId = RestIo.GetPathParameter(request, "studentId");
            return service.GetStudentAssignments(studentId, role);
        });
    }
}