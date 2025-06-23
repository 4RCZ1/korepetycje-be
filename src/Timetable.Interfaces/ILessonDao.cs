using Database.Entities;

namespace Timetable.Interfaces;

public interface ILessonDao
{
    DbAttendance? GetAttendance(int lessonId, int studentId);
    void SaveAttendance(DbAttendance attendance);
    DbLesson? GetLessonById(int lessonId);
    DbLesson? GetLessonByIdWithAttendances(int lessonId);
    void SaveLesson(DbLesson lesson);
    IList<DbLesson> GetLessonsInRange(
        DateTimeOffset startTime, DateTimeOffset endTime);
    IList<DbLesson> GetStudentLessonsInRange(
        int studentId, DateTimeOffset startTime, DateTimeOffset endTime);
    void RemoveLessonsCascading(IList<int> lessonIds);
    DbSchedule? GetScheduleById(int scheduleId);
    void CreateSchedule(DbSchedule schedule);
    void RemoveEmptySchedules();
    bool IsTermTaken(List<DbTimeslot> tsToTake, List<DbTimeslot> tsTaken);
    List<DbTimeslot> GetTimeslots();
}
