using System.Linq.Expressions;
using Database.Entities;

namespace Database;

public static class TimeslotDaoConditions
{
    public static readonly Expression<Func<DateTime, DateTime, DateTime, DateTime, bool>> Overlap
        = (start1, end1, start2, end2) => start1 < end2 && start2 < end1;

    public static Expression<Func<DbLesson, bool>> LessonOverlap(DateTime start, DateTime end)
    {
        var lessonParam = Expression.Parameter(typeof(DbLesson));
        var timeslotField = Expression.Property(lessonParam, nameof(DbLesson.Timeslot));
        var startField = Expression.Property(timeslotField, nameof(DbTimeslot.StartTime));
        var endField = Expression.Property(timeslotField, nameof(DbTimeslot.EndTime));
        return Expression.Lambda<Func<DbLesson, bool>>(Expression.Invoke(Overlap,
                startField,
                endField,
                Expression.Constant(start),
                Expression.Constant(end)),
            lessonParam);
    }
}
