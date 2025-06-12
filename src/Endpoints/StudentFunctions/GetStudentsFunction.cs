using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Endpoints.StudentFunctions;

public class GetStudentsFunction
{

        public async Task<APIGatewayProxyResponse> GetStudents(
            APIGatewayProxyRequest request, ILambdaContext context)
        {
            var service = await ServiceFactory.CreateStudentService();
            if(!request.QueryStringParameters?.Any() ?? true)
                return Response.OkJson(service.GetStudents());
            var lessonExternalId = request.QueryStringParameters["lessonId"];
            var students = service.GetStudents(lessonExternalId);
            return Response.OkJson(students);
        }
}