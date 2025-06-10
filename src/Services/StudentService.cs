using Database.Entities;
using Endpoints.Interfaces;
using Timetable.Interfaces;

namespace Services;

public class StudentService : IStudentService
{
    public StudentService(ITransactor transactor)
    {
        _transactor = transactor;
    }

    public void AddStudent(StudentDto studentToAdd)
    {
        var addressToAdd = new DbAddress
        {
            AddressName = "", // todo: fill out
            AddressData = studentToAdd.Address ?? "" // todo: validate, "" is likely not acceptable
        };
        var student = new DbStudent
        {
            Name = studentToAdd.Name ?? "",
            Surname = studentToAdd.Surname ?? "",
            Address = addressToAdd,
        };
        using var t = _transactor.BeginTransaction();
        t.StudentDao.SaveStudent(student);
        t.Commit();
    }

    public StudentDto GetStudent(string studentExternalId)
    {
        using var t = _transactor.BeginTransaction();
        var student = t.StudentDao.GetStudent(int.Parse(studentExternalId));
        if (student is null)
            throw new InvalidRequestException();
        return new StudentDto
        {
            ExternalId = student.Id.ToString(),
            Name = student.Name,
            Surname = student.Surname,
            Address = student.Address?.AddressData
        };
    }

    public void UpdateStudent(string externalStudentId, StudentDto student)
    {
        using var t = _transactor.BeginTransaction();
        var studentId = int.Parse(externalStudentId);
        var studentToUpdate = t.StudentDao.GetStudent(studentId);
        if (studentToUpdate is null)
            throw new InvalidRequestException();
        if (student.Name is not null)
            studentToUpdate.Name = student.Name;
        if (student.Surname is not null)
            studentToUpdate.Surname = student.Surname;
        if (student.Address is not null)
        {
            studentToUpdate.Address = new DbAddress
            {
                AddressData = student.Address,
                AddressName = student.Address, // todo: is address name really needed then?
            };
        }

        t.StudentDao.SaveStudent(studentToUpdate);
        t.Commit();
    }

    public void DeleteStudent(string studentExternalId)
    {
        using var t = _transactor.BeginTransaction();
        t.StudentDao.DeleteStudent(int.Parse(studentExternalId));
        t.Commit();
    }

    private readonly ITransactor _transactor;
}
