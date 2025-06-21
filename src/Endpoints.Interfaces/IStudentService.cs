using Endpoints.Interfaces.Authorization;

namespace Endpoints.Interfaces;

public interface IStudentService
{
    string AddStudent(StudentDto student, TutorRole role);
    StudentDto GetStudent(string studentExternalId, TutorRole role, bool includeDeleted = false);
    List<StudentDto> GetStudents(TutorRole role, string? lessonExternalId = null, bool includeDeleted = false);
    void UpdateStudent(string externalStudentId, StudentDto student, TutorRole role);
    void DeleteStudent(string studentExternalId, TutorRole role);
}
