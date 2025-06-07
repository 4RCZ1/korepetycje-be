namespace Endpoints.Interfaces;

public interface IStudentService
{
    void AddStudent(StudentDto student);
    StudentDto GetStudent(string studentExternalId);
    void UpdateStudent(string externalStudentId, StudentDto student);
    void DeleteStudent(string studentExternalId);
}