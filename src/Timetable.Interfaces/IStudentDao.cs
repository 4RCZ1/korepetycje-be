using Database.Entities;
using Endpoints.Interfaces;

namespace Timetable.Interfaces;

public interface IStudentDao
{
    void AddStudent(DbStudent studentToAdd);
    DbStudent GetStudent(int studentId);
    void UpdateStudent(int studentId, DbStudent student);
    void DeleteStudent(int studentId);
}