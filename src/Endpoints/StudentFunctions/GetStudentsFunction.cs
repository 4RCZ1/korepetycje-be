using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Endpoints.StudentFunctions;

public class GetStudentsFunction
{
        public async Task<APIGatewayProxyResponse> GetStudents(
            APIGatewayProxyRequest request, ILambdaContext context)
        {
            var service = await ServiceFactory.CreateStudentServiceAsync();
            string? lessonExternalId = null;
            if(!request.QueryStringParameters?.TryGetValue("lessonId", out lessonExternalId) ?? true)
                return RestIo.OkJson(service.GetStudents());
            var students = service.GetStudents(lessonExternalId);
            return RestIo.OkJson(students);
        }
}