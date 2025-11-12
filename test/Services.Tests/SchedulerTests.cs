namespace Services.Tests;

public class SchedulerTests
{
    [Fact]
    public void PlanSingleEvent()
    {
        var plan = _utcScheduler.Plan(FirstEvent, SecondEvent.Start, Weekly);
        Assert.Equal([FirstEvent], plan);
    }

    [Fact]
    public void PlanSeries()
    {
        var plan = _utcScheduler.Plan(
            FirstEvent, ThirdEvent.Start + TimeSpan.FromMinutes(5), Weekly);
        Assert.Equal([FirstEvent, SecondEvent, ThirdEvent], plan);
    }

    [Fact]
    public void PlanNothing()
    {
        var plan = _utcScheduler.Plan(FirstEvent, FirstEvent.Start, Weekly);
        Assert.Empty(plan);
    }

    [Fact]
    public void RescheduleSeries()
    {
        var week = TimeSpan.FromDays(7);
        var series = _utcScheduler.RescheduleSeries(
            new List<TimeRange> { FirstEvent, ThirdEvent }, NewFirstStart, NewFirstEnd);
        Assert.Equal(
            new List<TimeRange>
            {
                new() { Start = NewFirstStart, End = NewFirstEnd },
                new() { Start = NewFirstStart + 2 * week, End = NewFirstEnd + 2 * week }
            }, series);
    }

    [Fact]
    public void RescheduleNothing()
    {
        Assert.Empty(_utcScheduler.RescheduleSeries([], NewFirstStart, NewFirstEnd));
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

    private readonly Scheduler _utcScheduler = new(TimeZoneInfo.Utc);
}
