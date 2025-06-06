using System.Linq.Expressions;
using Database.Entities;

namespace Database;

public static class TimeslotDaoConditions
{
    public static Expression<Func<DbLesson, bool>> LessonOverlap(DateTime start, DateTime end)
    {
        return lesson => lesson.Timeslot.StartTime < end && start < lesson.Timeslot.EndTime;
    }
}
