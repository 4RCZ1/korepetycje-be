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
        DbAddress addressToAdd = new DbAddress()
        {
            AddressName = "",
            AddressData = studentToAdd.Address ?? ""
        };
        DbStudent student = new DbStudent()
        {
            Name = studentToAdd.Name,
            Surname = studentToAdd.Surname,
            Address = addressToAdd,
            AddressId = addressToAdd.Id
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
            Address = studentDao.Address.AddressData
        };
        return student;
    }

    public void UpdateStudent(string externalStudentId, StudentDto studentToAdd)
    {
        DbAddress addressToAdd = new DbAddress()
        {
            AddressName = "",
            AddressData = studentToAdd.Address ?? ""
        };
        DbStudent student = new DbStudent()
        {
            Name = studentToAdd.Name ?? "",
            Surname = studentToAdd.Surname ?? "",
            AddressId = addressToAdd.Id,
            Address = addressToAdd
        };
        _dao.UpdateStudent(int.Parse(externalStudentId), student);
    }

    public void DeleteStudent(string studentExternalId)
    {
        _dao.DeleteStudent(int.Parse(studentExternalId));
    }
    
    private readonly IStudentDao _dao;
}