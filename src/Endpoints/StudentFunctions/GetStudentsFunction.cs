using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Endpoints.StudentFunctions;

public class GetStudentsFunction
{
        public async Task<APIGatewayProxyResponse> GetStudents(
            APIGatewayProxyRequest request, ILambdaContext context)
        {
            var service = await ServiceFactory.CreateStudentService();
            string? lessonExternalId = null;
            if(!request.QueryStringParameters?.TryGetValue("lessonId", out lessonExternalId) ?? true)
                return Response.OkJson(service.GetStudents());
            var students = service.GetStudents(lessonExternalId);
            return Response.OkJson(students);
        }
}