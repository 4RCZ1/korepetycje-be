using Database.Entities;

namespace Timetable.Interfaces;

public interface ILessonDao
{
    DbAttendance? GetAttendance(int lessonId, int studentId);
    void SaveAttendance(DbAttendance attendance);
    DbLesson? GetLessonById(int lessonId);
    IList<DbLesson> GetLessonsInRange(DateTime startTime, DateTime endTime);
    IList<DbLesson> GetStudentLessonsInRange(int studentId, DateTime startTime, DateTime endTime);
    void RemoveLessonsCascading(IList<int> lessonIds);
    DbSchedule? GetScheduleById(int scheduleId);
    void CreateSchedule(DbSchedule schedule);
    void RemoveEmptySchedules();
    void AddFreeTerm(DateTime startTime, DateTime endTime);
}
