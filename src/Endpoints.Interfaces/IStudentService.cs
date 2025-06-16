using Endpoints.Interfaces.Authorization;

namespace Endpoints.Interfaces;

public interface IStudentService
{
    void AddStudent(StudentDto student, TutorRole role);
    StudentDto GetStudent(TutorRole role, string studentExternalId, bool? includeDeleted = false);
    List<StudentDto> GetStudents(TutorRole role, string? lessonExternalId = null, bool? includeDeleted = false);
    void UpdateStudent(string externalStudentId, StudentDto student, TutorRole role);
    void DeleteStudent(string studentExternalId, TutorRole role);
}
