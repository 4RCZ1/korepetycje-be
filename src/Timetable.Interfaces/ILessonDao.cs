using Database.Entities;

namespace Timetable.Interfaces;

public interface ILessonDao
{
    IList<DbLesson> GetLessonsInRange(DateOnly startDate, DateOnly endDate);
    void ConfirmLesson(int lessonId);
    void CreateSchedule(DbSchedule schedule);
    IList<DbLesson> GetStudentLessons(int studentId);
}
