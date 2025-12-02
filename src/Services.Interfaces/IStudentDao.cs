using Database.Entities;

namespace Services.Interfaces;

public interface IStudentDao
{
    DbStudent? GetStudent(int studentId, bool includeDeleted = false);
    List<DbStudent> GetStudents(int? lessonId = null, bool includeDeleted = false);
    void SaveStudent(DbStudent student);
    void SaveSingleStudent(DbStudent student, string singleGroupName);
    void DeleteStudent(int studentId);
    DbStudentGroup GetStudentSingleGroupByStudentId(int studentId);
    void DeleteStudentGroup(DbStudentGroup studentGroup);
    List<DbStudentGroup> GetAllStudentGroups();
}
