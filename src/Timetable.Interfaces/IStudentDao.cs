using Database.Entities;

namespace Timetable.Interfaces;

public interface IStudentDao
{
    DbStudent? GetStudent(int studentId);
    void SaveStudent(DbStudent student);
    void DeleteStudent(int studentId);
}
