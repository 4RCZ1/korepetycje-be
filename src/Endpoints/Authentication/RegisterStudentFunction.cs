using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Dto;
using Endpoints.Interfaces;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.Authentication;

public class RegisterStudentFunction
{
    public Task<APIGatewayProxyResponse> RegisterStudent(APIGatewayProxyRequest request)
    {
        return RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var body = RestIo.ReadBody<StudentDto>(request);
            if (body.Email is null)
                throw new BadRequestException("Email is required.");
            var authService = await ServiceFactory.CreateAuthenticationService();
            var studentService = await ServiceFactory.CreateStudentServiceAsync();
            var externalStudentId = studentService.AddStudent(body, role);
            try
            {
                await authService.RegisterStudentAsync(externalStudentId, body.Email, role);
            }
            catch (Exception)
            {
                studentService.DeleteStudent(externalStudentId, role);
                throw;
            }
            return string.Empty;
        });
    }
}
