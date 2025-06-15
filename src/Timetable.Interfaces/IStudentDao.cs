using Database.Entities;

namespace Timetable.Interfaces;

public interface IStudentDao
{
    DbStudent? GetStudent(int studentId);
    List<DbStudent> GetStudents(int? lessonId = null);
    void SaveStudent(DbStudent student);
    void DeleteStudent(int studentId);
}
