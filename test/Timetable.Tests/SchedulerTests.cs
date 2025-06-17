using Services;

namespace Timetable.Tests;

public class SchedulerTests
{
    [Fact]
    public void PlanSingleEvent()
    {
        var plan = _scheduler.Plan(FirstEvent, SecondEvent.Start, Weekly);
        Assert.Equivalent(new[] { FirstEvent }, plan, strict: true);
    }

    [Fact]
    public void PlanSeries()
    {
        var plan = _scheduler.Plan(FirstEvent, ThirdEvent.Start + TimeSpan.FromMinutes(5), Weekly);
        Assert.Equivalent(new[] { FirstEvent, SecondEvent, ThirdEvent }, plan, strict: true);
    }

    [Fact]
    public void PlanNothing()
    {
        var plan = _scheduler.Plan(FirstEvent, FirstEvent.Start, Weekly);
        Assert.Empty(plan);
    }

    [Fact]
    public void RescheduleSeries()
    {
        var week = TimeSpan.FromDays(7);
        var series = _scheduler.RescheduleSeries(
            new List<TimeRange> { FirstEvent, ThirdEvent }, NewFirstStart, NewFirstEnd);
        Assert.Equivalent(
            new List<TimeRange>
            {
                new() { Start = NewFirstStart, End = NewFirstEnd },
                new() { Start = NewFirstStart + 2 * week, End = NewFirstEnd + 2 * week }
            }, series, strict: true);
    }

    [Fact]
    public void RescheduleNothing()
    {
        Assert.Empty(_scheduler.RescheduleSeries([], NewFirstStart, NewFirstEnd));
    }

    private static readonly DateTimeOffset NewFirstStart = new(2026, 5, 3, 11, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset NewFirstEnd = new(2026, 5, 3, 12, 0, 0, TimeSpan.Zero);

    private static readonly TimeRange FirstEvent = new()
    {
        Start = new DateTimeOffset(2025, 5, 3, 11, 0, 0, TimeSpan.Zero),
        End = new DateTimeOffset(2025, 5, 3, 13, 0, 0, TimeSpan.Zero),
    };

    private static readonly TimeRange SecondEvent = new()
    {
        Start = new DateTimeOffset(2025, 5, 10, 11, 0, 0, TimeSpan.Zero),
        End = new DateTimeOffset(2025, 5, 10, 13, 0, 0, TimeSpan.Zero),
    };

    private static readonly TimeRange ThirdEvent = new()
    {
        Start = new DateTimeOffset(2025, 5, 17, 11, 0, 0, TimeSpan.Zero),
        End = new DateTimeOffset(2025, 5, 17, 13, 0, 0, TimeSpan.Zero),
    };

    private const int Weekly = 7;

    private readonly Scheduler _scheduler = new(TimeZoneInfo.Utc);
}
