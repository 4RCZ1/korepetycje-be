namespace Endpoints.Interfaces;

public interface IStudentService
{
    string AddStudent(StudentDto student);
    StudentDto GetStudent(string studentExternalId);
    List<StudentDto> GetStudents(string? lessonExternalId = null);
    void UpdateStudent(string externalStudentId, StudentDto student);
    void DeleteStudent(string studentExternalId);
}
