using System.Linq.Expressions;
using Database.Entities;

namespace Database;

public static class TimeslotDaoConditions
{
    public static Expression<Func<DbLesson, bool>> LessonOverlap(
        DateTimeOffset start, DateTimeOffset end)
    {
        return lesson => lesson.Timeslot.StartTime < end && start < lesson.Timeslot.EndTime;
    }
}
