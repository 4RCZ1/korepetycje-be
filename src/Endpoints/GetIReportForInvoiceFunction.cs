using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

namespace Endpoints;

public class GetReportForInvoiceFunction
{

    public async Task<APIGatewayProxyResponse> GetReport(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var studentExternalId = RestIo.GetPathParameter(request, "studentExternalId");
            var service = await ServiceFactory.CreateStudentServiceAsync(identity);
            var report = service.GetReportForInvoice(role, studentExternalId);
            return report;
        });
    }
}

