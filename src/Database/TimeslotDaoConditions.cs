using System.Linq.Expressions;

namespace Database;

public static class TimeslotDaoConditions
{
    public static readonly Expression<Func<DateTime, DateTime, DateTime, DateTime, bool>> Overlap
        = (start1, end1, start2, end2) => start1 < end2 && start2 < end1;
}
