using Timetable.Interfaces;

namespace Database;

public class LessonDao : ILessonDao
{
    public IList<Lesson> GetLessonsInRange(DateOnly startDate, DateOnly endDate)
    {
        return [];
    }
}
