namespace Endpoints.Interfaces;

public interface IStudentService
{
    void AddStudent(StudentDto student);
    StudentDto GetStudent(string studentExternalId);
    List<StudentDto> GetStudents(string? lessonExternalId = null);
    void UpdateStudent(string externalStudentId, StudentDto student);
    void DeleteStudent(string studentExternalId);
}
