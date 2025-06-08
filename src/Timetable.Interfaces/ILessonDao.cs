using Database.Entities;

namespace Timetable.Interfaces;

public interface ILessonDao
{
    IList<DbLesson> GetLessonsInRange(DateTime startTime, DateTime endTime);
    IList<DbLesson> GetStudentLessonsInRange(int studentId, DateTime startTime, DateTime endTime);
    void ConfirmLesson(int lessonId);
    void CreateSchedule(DbSchedule schedule);
    void AddFreeTerm(DateTime startTime, DateTime endTime);
}
