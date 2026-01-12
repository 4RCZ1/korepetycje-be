using Endpoints.Dto;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.Interfaces;

public interface IStudentService
{
    string AddStudent(StudentDto student, TutorRole role);
    StudentDto GetStudent(string studentExternalId, TutorRole role, bool includeDeleted = false);
    List<StudentDto> GetStudents(TutorRole role, string? lessonExternalId = null, bool includeDeleted = false);
    List<StudentGroupDto> GetStudentGroups(TutorRole role);
    void UpdateStudent(string externalStudentId, StudentDto student, TutorRole role);
    void DeleteStudent(string studentExternalId, TutorRole role);
    void CreateStudentGroup(StudentGroupDto group, TutorRole role);
    ReportForInvoiceDto GetReportForInvoice(TutorRole role, string studentExternalId, string startTime, string endTime);
}
