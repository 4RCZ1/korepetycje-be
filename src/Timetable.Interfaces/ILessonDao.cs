namespace Timetable.Interfaces;

public interface ILessonDao
{
    IList<Lesson> GetLessonsInRange(DateOnly startDate, DateOnly endDate);
}
