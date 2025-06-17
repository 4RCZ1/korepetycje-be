using Database.Entities;

namespace Services;

// todo: handle offsets
public class Scheduler
{
    public Scheduler(TimeZoneInfo timeZone)
    {
        _timeZone = timeZone;
    }

    public IList<TimeRange> Plan(
        TimeRange firstEvent,
        DateTimeOffset seriesEnd,
        int periodInDays)
    {
        var result = new List<TimeRange>();
        var newEvent = firstEvent;
        var period = TimeSpan.FromDays(periodInDays);
        while (newEvent.Start < seriesEnd)
        {
            result.Add(newEvent);
            newEvent = new TimeRange
            {
                Start = newEvent.Start + period,
                End = newEvent.End + period,
            };
        }
        return result;
    }

    // Assumes the input is sorted.
    public IList<TimeRange> RescheduleSeries(
        IList<TimeRange> series, DateTimeOffset firstStart, DateTimeOffset firstEnd)
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

    private readonly TimeZoneInfo _timeZone;
}

public struct TimeRange
{
    public required DateTimeOffset Start;
    public required DateTimeOffset End;

    public override string ToString() => $"({Start}, {End})";
}

public static class DbTimeslotExtensions
{
    public static TimeRange AsRange(this DbTimeslot timeslot)
    {
        return new TimeRange { Start = timeslot.StartTime, End = timeslot.EndTime };
    }
}
