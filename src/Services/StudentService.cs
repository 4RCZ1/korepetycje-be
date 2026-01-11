using Database.Entities;
using Endpoints.Dto;
using Endpoints.Interfaces;
using Endpoints.Interfaces.Authorization;
using Services.Interfaces;

namespace Services;

public class StudentService : IStudentService
{
    public StudentService(ITransactor transactor)
    {
        _transactor = transactor;
    }
    public string AddStudent(StudentDto studentToAdd, TutorRole role)
    {
        DbAddress? addressToAdd = null;
        using var t = _transactor.BeginTransaction();
        if (int.TryParse(studentToAdd?.Address?.ExternalId, out var addressId))
            addressToAdd = t.AddressDao.GetAddress(addressId);
        var student = new DbStudent
        {
            Name = studentToAdd?.Name ?? "",
            Surname = studentToAdd?.Surname ?? "",
            PhoneNumber = studentToAdd?.PhoneNumber ?? "",
            Address = addressToAdd ?? new DbAddress()
            {
                AddressName = studentToAdd?.Address?.AddressName ?? "NOWY ADRES",
                AddressData = studentToAdd?.Address?.AddressData ?? "Uzupełnij dane"
            }
        };
        t.StudentDao.SaveSingleStudent(student, $"(single) {student.Name}_{student.Surname}");
        t.Commit();
        return student.Id.ToString();
    }

    public StudentDto GetStudent(
        string studentExternalId, TutorRole role, bool includeDeleted = false)
    {
        using var t = _transactor.BeginTransaction();
        var student = t.StudentDao.GetStudent(int.Parse(studentExternalId), includeDeleted);
        if (student is null)
            throw new BadRequestException("No student found.");
        return new StudentDto
        {
            ExternalId = student.Id.ToString(),
            Name = student.Name,
            Surname = student.Surname,
            PhoneNumber = student.PhoneNumber,
            IsDeleted = student.IsDeleted,
            Address = new AddressDto()
            {
                ExternalId = student.Address?.Id.ToString(),
                AddressName = student.Address?.AddressName,
                AddressData = student.Address?.AddressData,
            }
        };
    }

    public List<StudentDto> GetStudents(TutorRole role, string? lessonExternalId = null, bool includeDeleted = false)
    {
        using var t = _transactor.BeginTransaction();
        List<DbStudent> students;
        if (String.IsNullOrEmpty(lessonExternalId))
            students = t.StudentDao.GetStudents(null, includeDeleted);
        else
        {
            int.TryParse(lessonExternalId, out var lessonId);
            students = t.StudentDao.GetStudents(lessonId, includeDeleted);
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
                PhoneNumber = s.PhoneNumber,
                IsDeleted = s.IsDeleted,
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

    public void UpdateStudent(string externalStudentId, StudentDto student, TutorRole role)
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
        studentToUpdate.PhoneNumber = String.IsNullOrEmpty(student.PhoneNumber)
            ? studentToUpdate.PhoneNumber : student.PhoneNumber;
        DbAddress? addressToUpdate = null;
        if(int.TryParse(student?.Address?.ExternalId, out var addressId))
            addressToUpdate = t.AddressDao.GetAddress(addressId);
        if (addressToUpdate is not null)
        {
            studentToUpdate!.Address!.AddressData = String.IsNullOrEmpty(student?.Address?.AddressData)
                ? addressToUpdate.AddressData
                : student.Address.AddressData;
            studentToUpdate!.Address!.AddressName = String.IsNullOrEmpty(student?.Address?.AddressName)
                ? addressToUpdate.AddressName
                : student.Address.AddressName;
        }
        else
        {
            if (student?.Address is not null)
            {
                studentToUpdate.Address = new DbAddress
                {
                    AddressData = String.IsNullOrEmpty(student?.Address?.AddressData)
                        ? studentToUpdate.Address!.AddressData
                        : student.Address.AddressData,
                    AddressName = String.IsNullOrEmpty(student?.Address?.AddressName)
                        ? studentToUpdate.Address!.AddressName
                        : student.Address.AddressName,
                };
            }
        }
        t.StudentDao.SaveStudent(studentToUpdate);
        t.Commit();
    }

    public void DeleteStudent(string studentExternalId, TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        var group = t.StudentDao.GetStudentSingleGroupByStudentId(int.Parse(studentExternalId));
        t.StudentDao.DeleteStudent(int.Parse(studentExternalId));
        t.StudentDao.DeleteStudentGroup(group);
        t.Commit();
    }

    public void CreateStudentGroup(StudentGroupDto group, TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        var memberships = CreateMemberships(t.StudentDao, group);
        if (group.Name == null)
            throw new BadRequestException("Nazwa grupy musi zostać podana.");
        t.StudentDao.SaveStudentGroup(new DbStudentGroup
        {
            IsSingle = false,
            Name = group.Name,
            Memberships = memberships
        });
        t.Commit();
    }

    private static List<DbStudentMembership> CreateMemberships(
        IStudentDao dao, StudentGroupDto group)
    {
        return group.Students.Select(s =>
        {
            if (s.ExternalId == null)
                throw new BadRequestException("Null student ID encountered.");
            var student = dao.GetStudent(int.Parse(s.ExternalId));
            if (student == null)
            {
                throw new BadRequestException(
                    "Nie znaleziono jednego z podanych uczniów."
                    + " Prawdopodobnie został usunięty. Odśwież stronę i spróbuj ponownie.");
            }
            return new DbStudentMembership
            {
                StudentId = student.Id,
            };
        }).ToList();
    }

    public List<StudentGroupDto> GetStudentGroups(TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        return t.StudentDao.GetAllStudentGroups().Select(r => new StudentGroupDto
        {
            Id = r.Guid.ToString(),
            Name = r.Name,
            Students = r.Memberships.Select(m => new StudentDto()
            {
                ExternalId = m.Student?.Id.ToString(),
                Name = m.Student?.Name,
                Surname = m.Student?.Surname,
                PhoneNumber = m.Student?.PhoneNumber,
                IsDeleted = m.Student?.IsDeleted
            }).ToList()
        }).ToList();
    }

    public ReportForInvoiceDto GetReportForInvoice(TutorRole role, string studentExternalId)
    {
        var studentId = int.Parse(studentExternalId);
        using var t = _transactor.BeginTransaction();
        var student = t.StudentDao.GetStudent(studentId, true);
        if(student is null)
            throw new BadRequestException("No student found.");
            
        var minutesList = t.StudentDao.GetStudentMinutes(int.Parse(studentExternalId));

        var report = new ReportForInvoiceDto()
        {
            IndividualHours = (int)Math.Round(minutesList[0] / 60),
            GroupHours = (int)Math.Round(minutesList[1] / 60)
        };

        return report;
    }

    private readonly ITransactor _transactor;
}
