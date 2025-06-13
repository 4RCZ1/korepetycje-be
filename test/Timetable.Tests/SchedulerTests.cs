using Services;

namespace Timetable.Tests;

public class SchedulerTests
{
    [Fact]
    public void PlanSingleEvent()
    {
        var plan = Scheduler.Plan(FirstEvent, SecondEvent.Start, Weekly);
        Assert.Equivalent(new[] { FirstEvent }, plan, strict: true);
    }

    [Fact]
    public void PlanSeries()
    {
        var plan = Scheduler.Plan(FirstEvent, ThirdEvent.Start + TimeSpan.FromMinutes(5), Weekly);
        Assert.Equivalent(new[] { FirstEvent, SecondEvent, ThirdEvent }, plan, strict: true);
    }

    [Fact]
    public void PlanNothing()
    {
        var plan = Scheduler.Plan(FirstEvent, FirstEvent.Start, Weekly);
        Assert.Empty(plan);
    }

    [Fact]
    public void RescheduleSeries()
    {
        var week = TimeSpan.FromDays(7);
        var firstStart = new DateTime(2026, 5, 3, 11, 0, 0);
        var firstEnd = new DateTime(2026, 5, 3, 12, 0, 0);
        var series = Scheduler.RescheduleSeries(
            new List<TimeRange> { FirstEvent, ThirdEvent }, firstStart, firstEnd);
        Assert.Equivalent(
            new List<TimeRange>
            {
                new() { Start = firstStart, End = firstEnd },
                new() { Start = firstStart + 2 * week, End = firstEnd + 2 * week }
            }, series, strict: true);
    }

    [Fact]
    public void RescheduleNothing()
    {
        var firstStart = new DateTime(2026, 5, 3, 11, 0, 0);
        var firstEnd = new DateTime(2026, 5, 3, 12, 0, 0);
        Assert.Empty(Scheduler.RescheduleSeries(new List<TimeRange>(), firstStart, firstEnd));
    }

    private static readonly TimeRange FirstEvent = new()
    {
        Start = new DateTime(2025, 5, 3, 11, 0, 0),
        End = new DateTime(2025, 5, 3, 13, 0, 0),
    };

    private static readonly TimeRange SecondEvent = new()
    {
        Start = new DateTime(2025, 5, 10, 11, 0, 0),
        End = new DateTime(2025, 5, 10, 13, 0, 0),
    };

    private static readonly TimeRange ThirdEvent = new()
    {
        Start = new DateTime(2025, 5, 17, 11, 0, 0),
        End = new DateTime(2025, 5, 17, 13, 0, 0),
    };

    private const int Weekly = 7;
}
