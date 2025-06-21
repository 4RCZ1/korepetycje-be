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

    public static Expression<Func<DbLessonSuggestion, bool>> SuggestionOverlap(
        DateTimeOffset start, DateTimeOffset end)
    {
        return s => s.Timeslot!.StartTime < end && start < s.Timeslot.EndTime;
    }

    public static Expression<Func<DbTimeslot, bool>> TimeslotOverlap(DbTimeslot ts1)
    {
        return ts2 => ts2.StartTime < ts1.EndTime && ts1.StartTime < ts2.EndTime;
    }
}
