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
        DbAddress? addressToAdd = null;
        using var t = _transactor.BeginTransaction();
        if(int.TryParse(studentToAdd?.Address?.ExternalId, out var addressId))
            addressToAdd = t.AddressDao.GetAddress(addressId);
        var student = new DbStudent
        {
            Name = studentToAdd?.Name ?? "",
            Surname = studentToAdd?.Surname ?? "",
            Address = addressToAdd ?? new DbAddress()
            {
                AddressName = studentToAdd?.Address?.AddressName ?? "NOWY ADRES",
                AddressData = studentToAdd?.Address?.AddressData ?? "Uzupełnij dane"
            }
        };
        
        t.StudentDao.SaveStudent(student);
        t.Commit();
    }

    public StudentDto GetStudent(string studentExternalId)
    {
        using var t = _transactor.BeginTransaction();
        var student = t.StudentDao.GetStudent(int.Parse(studentExternalId));
        if (student is null)
            throw new BadRequestException("No student found.");
        return new StudentDto
        {
            ExternalId = student.Id.ToString(),
            Name = student.Name,
            Surname = student.Surname,
            Address = new AddressDto()
            {
                ExternalId = student.Address?.Id.ToString(),
                AddressName = student.Address?.AddressName,
                AddressData = student.Address?.AddressData,
            }
        };
    }
    
    public List<StudentDto> GetStudents(string? lessonExternalId = null)
    {
        using var t = _transactor.BeginTransaction();
        List<DbStudent> students;
        if (String.IsNullOrEmpty(lessonExternalId))
            students = t.StudentDao.GetStudents();
        else
        {
            int lessonId;
            int.TryParse(lessonExternalId, out lessonId);
            students = t.StudentDao.GetStudents(lessonId);
        }
        if (students is null)
            throw new ApplicationException("No students found");
        List<StudentDto> studentDtos = new List<StudentDto>();
        foreach (var s in students)
        {
            studentDtos.Add(new StudentDto
            {
                ExternalId = s.Id.ToString(),
                Name = s.Name,
                Surname = s.Surname,
                Address = new AddressDto()
                {
                    ExternalId = s.Address?.Id.ToString(),
                    AddressName = s.Address?.AddressName,
                    AddressData = s.Address?.AddressData,
                }
            });
        }
        return studentDtos;
    }

    public void UpdateStudent(string externalStudentId, StudentDto student)
    {
        using var t = _transactor.BeginTransaction();
        var studentId = int.Parse(externalStudentId);
        var studentToUpdate = t.StudentDao.GetStudent(studentId);
        if (studentToUpdate is null)
            throw new BadRequestException("No student found.");
        studentToUpdate.Name = String.IsNullOrEmpty(student.Name) 
           ? studentToUpdate.Name : student.Name;
        studentToUpdate.Surname = String.IsNullOrEmpty(student.Surname) 
            ? studentToUpdate.Surname : student.Surname;
        DbAddress? addressToAdd = null;
        if(int.TryParse(student?.Address?.ExternalId, out var addressId))
            addressToAdd = t.AddressDao.GetAddress(addressId);
        if (addressToAdd is not null)
        {
            studentToUpdate.Address = addressToAdd;
        }
        else
        {
            studentToUpdate.Address = new DbAddress
            {
                AddressData = String.IsNullOrEmpty(student.Address.AddressData) 
                    ? studentToUpdate.Address!.AddressData : student.Address.AddressData,
                AddressName = String.IsNullOrEmpty(student.Address.AddressName) 
                    ? studentToUpdate.Address!.AddressName : student.Address.AddressName, 
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
