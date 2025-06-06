using Xunit;
using Assert = Xunit.Assert;

namespace Database.Tests;

public class TimeslotDaoConditionTests
{
    [Fact]
    public void CollisionForPartialOverlap()
    {
        Assert.True(IntervalsOverlap(1, 3, 2, 4));
        Assert.True(IntervalsOverlap(2, 4, 1, 3));
    }

    [Fact]
    public void CollisionForContainment()
    {
        Assert.True(IntervalsOverlap(1, 4, 2, 3));
        Assert.True(IntervalsOverlap(2, 3, 1, 4));
    }

    [Fact]
    public void NoCollisionForNoOverlap()
    {
        Assert.False(IntervalsOverlap(1, 2, 4, 5));
        Assert.False(IntervalsOverlap(4, 5, 1, 2));
    }

    [Fact]
    public void EdgeCase()
    {
        Assert.False(IntervalsOverlap(1, 2, 2, 3));
        Assert.False(IntervalsOverlap(2, 3, 1, 2));
    }

    private bool IntervalsOverlap(int start1, int end1, int start2, int end2)
    {
        return _overlap(Instant(start1), Instant(end1), Instant(start2), Instant(end2));
    }

    private static DateTime Instant(int i)
    {
        return new DateTime(2025, 5, 5, 12, 0, 0) + i * TimeSpan.FromMinutes(30 * i);
    }

    private readonly Func<DateTime, DateTime, DateTime, DateTime, bool> _overlap =
        TimeslotDaoConditions.Overlap.Compile();
}
