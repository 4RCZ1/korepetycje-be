using Amazon.Lambda.APIGatewayEvents;

namespace Endpoints.StudentFunctions;

public class DeleteStudentFunction
{
    public static async Task<APIGatewayProxyResponse> DeleteStudent(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var service = await ServiceFactory.CreateStudentServiceAsync();
            var externalStudentId = RestIo.GetPathParameter(request, "studentExternalId");
            service.DeleteStudent(externalStudentId);
            return string.Empty;
        });
    }
}
