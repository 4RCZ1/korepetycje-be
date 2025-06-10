using Database.Entities;

namespace Timetable.Interfaces;

public interface ILessonDao
{
    DbAttendance? GetAttendance(int lessonId, int studentId);
    void SaveAttendance(DbAttendance attendance);
    IList<DbLesson> GetLessonsInRange(DateTime startTime, DateTime endTime);
    IList<DbLesson> GetStudentLessonsInRange(int studentId, DateTime startTime, DateTime endTime);
    void CreateSchedule(DbSchedule schedule);
    void AddFreeTerm(DateTime startTime, DateTime endTime);
}
