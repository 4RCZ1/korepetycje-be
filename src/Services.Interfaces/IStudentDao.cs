using Database.Entities;

namespace Services.Interfaces;

public interface IStudentDao
{
    DbStudent? GetStudent(int studentId, bool includeDeleted = false);
    List<DbStudent> GetStudents(int? lessonId = null, bool includeDeleted = false);
    void SaveStudent(DbStudent student);
    void DeleteStudent(int studentId);
    List<DbStudentGroup> GetAllStudentGroups();
}
