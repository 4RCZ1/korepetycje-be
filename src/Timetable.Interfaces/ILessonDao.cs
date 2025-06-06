using Database.Entities;

namespace Timetable.Interfaces;

public interface ILessonDao
{
    IList<DbLesson> GetLessonsInRange(DateTime startTime, DateTime endTime);
    void ConfirmLesson(int lessonId);
    void CreateSchedule(DbSchedule schedule);
    IList<DbLesson> GetStudentLessons(int studentId);
    void AddFreeTerm(DateTime startTime, DateTime endTime);
}
