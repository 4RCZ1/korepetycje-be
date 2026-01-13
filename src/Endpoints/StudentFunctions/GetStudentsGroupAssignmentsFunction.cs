using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.StudentFunctions;

public class GetStudentsGroupAssignmentsFunction
{
    public async Task<APIGatewayProxyResponse> GetStudentGroupAssignments(APIGatewayProxyRequest request,
        ILambdaContext context)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateStudentServiceAsync(identity);
            var studentGroupGuid = RestIo.GetPathGuid(request, "studentGroupGuid");
            var result = service.GetStudentGroupAssignments(studentGroupGuid, role);
            return result;
        });
    }
}