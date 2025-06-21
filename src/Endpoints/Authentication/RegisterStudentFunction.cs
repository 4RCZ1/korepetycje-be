using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces;

namespace Endpoints.Authentication;

public class RegisterStudentFunction
{
    public Task<APIGatewayProxyResponse> RegisterStudent(APIGatewayProxyRequest request)
    {
        // todo: verify tutor JWT
        return RestIo.HandleRestBoilerplateAsync(async () =>
        {
            var body = RestIo.ReadBody<StudentDto>(request);
            if (body.Email is null)
                throw new BadRequestException("Email is required.");
            var authService = await ServiceFactory.CreateAuthenticationService();
            var studentService = await ServiceFactory.CreateStudentServiceAsync();
            var externalStudentId = studentService.AddStudent(body);
            await authService.RegisterStudentAsync(externalStudentId, body.Email);
            return string.Empty;
        });
    }
}
