using Database.Entities;
using Endpoints.Interfaces;
using Timetable.Interfaces;

namespace Services;

public class StudentService: IStudentService
{
    public StudentService(IStudentDao dao)
    {
        _dao = dao;
    }
    
    public void AddStudent(StudentDto studentToAdd)
    {
        DbStudent student = new DbStudent()
        {
            Name = studentToAdd.Name,
            Surname = studentToAdd.Surname,
            Address = studentToAdd.Address
        };
        _dao.AddStudent(student);
    }

    public StudentDto GetStudent(string studentExternalId)
    {
        var studentDao = _dao.GetStudent(int.Parse(studentExternalId));
        var student = new StudentDto()
        {
            ExternalId = studentDao.Id.ToString(),
            Name = studentDao.Name,
            Surname = studentDao.Surname,
            Address = studentDao.Address
        };
        return student;
    }

    public void UpdateStudent(string externalStudentId, StudentDto studentToAdd)
    {
        DbStudent student = new DbStudent()
        {
            Name = studentToAdd.Name ?? "",
            Surname = studentToAdd.Surname ?? "",
            Address = studentToAdd.Address ?? ""
        };
        _dao.UpdateStudent(int.Parse(externalStudentId), student);
    }

    public void DeleteStudent(string studentExternalId)
    {
        _dao.DeleteStudent(int.Parse(studentExternalId));
    }
    
    private readonly IStudentDao _dao;
}