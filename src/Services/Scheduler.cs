using Database.Entities;

namespace Services;

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
        var localStart = ConvertToLocalDateTime(firstEvent.Start);
        var localEnd = ConvertToLocalDateTime(firstEvent.End);
        var period = TimeSpan.FromDays(periodInDays);
        while (ConvertToUtcInstant(localStart) < seriesEnd)
        {
            result.Add(new TimeRange
            {
                Start = ConvertToUtcInstant(localStart),
                End = ConvertToUtcInstant(localEnd),
            });
            localStart += period;
            localEnd += period;
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
        var startOffset = ConvertToLocalDateTime(firstStart) - ConvertToLocalDateTime(first.Start);
        var endOffset = ConvertToLocalDateTime(firstEnd) - ConvertToLocalDateTime(first.End);
        return series.Select(r => new TimeRange
        {
            Start = ConvertToUtcInstant(ConvertToLocalDateTime(r.Start) + startOffset),
            End = ConvertToUtcInstant(ConvertToLocalDateTime(r.End) + endOffset),
        }).ToList();
    }

    // Here "local" means "belonging to _timeZone".
    private DateTimeOffset ConvertToUtcInstant(DateTime localTime)
    {
        var localTimeWithOffset = new DateTimeOffset(localTime, _timeZone.GetUtcOffset(localTime));
        return TimeZoneInfo.ConvertTime(localTimeWithOffset, TimeZoneInfo.Utc);
    }

    private DateTime ConvertToLocalDateTime(DateTimeOffset instant)
    {
        return TimeZoneInfo.ConvertTime(instant, _timeZone).DateTime;
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
