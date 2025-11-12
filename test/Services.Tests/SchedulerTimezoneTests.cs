namespace Services.Tests;

public class SchedulerTimezoneTests
{
    [Fact]
    public void PlanAcrossDstSwitch()
    {
        var plan = _polishScheduler.Plan(
            WinterEvent, SummerTimePlusWeek - TimeSpan.FromMinutes(30), Weekly);
        Assert.Equal([WinterEvent, SummerEvent], plan);
    }

    [Fact]
    public void RescheduleAcrossDstSwitch()
    {
        var series = _polishScheduler.RescheduleSeries(
            [WinterEvent, SummerEvent], SummerEvent.Start, SummerEvent.End);
        Assert.Equal([SummerEvent, NextSummerEvent], series);
    }

    private const int Weekly = 7;

    private static readonly DateTimeOffset WinterTime = new(2025, 3, 27, 7, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset SummerTime = new(2025, 4, 3, 6, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset SummerTimePlusWeek = new(2025, 4, 10, 6, 0, 0, TimeSpan.Zero);
    private static readonly TimeSpan Hour = TimeSpan.FromHours(1);

    private static readonly TimeRange WinterEvent = new() { Start = WinterTime, End = WinterTime + Hour };
    private static readonly TimeRange SummerEvent = new() { Start = SummerTime, End = SummerTime + Hour };
    private static readonly TimeRange NextSummerEvent = new()
        { Start = SummerTimePlusWeek, End = SummerTimePlusWeek + Hour };

    private readonly Scheduler _polishScheduler =
        new(TimeZoneInfo.FindSystemTimeZoneById("Europe/Warsaw"));
}
