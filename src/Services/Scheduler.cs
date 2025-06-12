using Database.Entities;

namespace Services;

public class Scheduler
{
    public static IList<TimeRange> Plan(TimeRange firstEvent, int count, int periodInDays)
    {
        var result = new List<TimeRange>();
        var period = TimeSpan.FromDays(periodInDays);
        for (var i = 0; i < count; ++i)
        {
            result.Add(new TimeRange
            {
                Start = firstEvent.Start + i * period,
                End = firstEvent.End + i * period,
            });
        }
        return result;
    }

    public static IList<TimeRange> RescheduleSeries(
        IList<TimeRange> series, DateTime firstStart, DateTime firstEnd)
    {
        if (series.Count == 0)
            return [];
        var first = series.First();
        var startOffset = firstStart - first.Start;
        var endOffset = firstEnd - first.End;
        return series.Select(r => new TimeRange
        {
            Start = r.Start + startOffset,
            End = r.End + endOffset,
        }).ToList();
    }
}

public struct TimeRange
{
    public required DateTime Start;
    public required DateTime End;

    public override string ToString() => $"({Start}, {End})";
}

public static class DbTimeslotExtensions
{
    public static TimeRange AsRange(this DbTimeslot timeslot)
    {
        return new TimeRange { Start = timeslot.StartTime, End = timeslot.EndTime };
    }
}
