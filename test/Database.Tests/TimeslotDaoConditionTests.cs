using Database.Entities;
using Xunit;
using Assert = Xunit.Assert;

namespace Database.Tests;

public class TimeslotDaoConditionTests
{
    [Fact]
    public void CollisionForPartialOverlap()
    {
        RunTestBothWays(1, 3, 2, 4, true);
    }

    [Fact]
    public void CollisionForContainment()
    {
        RunTestBothWays(1, 4, 2, 3, true);
    }

    [Fact]
    public void NoCollisionForNoOverlap()
    {
        RunTestBothWays(1, 2, 4, 5, false);
    }

    [Fact]
    public void EdgeCase()
    {
        RunTestBothWays(1, 2, 2, 3, false);
    }

    private static void RunTestBothWays(int start1, int end1, int start2, int end2, bool overlapExpected)
    {
        RunTest(start1, end1, start2, end2, overlapExpected);
        RunTest(start2, end2, start1, end1, overlapExpected);
    }

    private static void RunTest(int start1, int end1, int start2, int end2, bool overlapExpected)
    {
        var s1 = Instant(start1);
        var e1 = Instant(end1);
        var s2 = Instant(start2);
        var e2 = Instant(end2);
        Assert.Equal(overlapExpected,
            TimeslotDaoConditions.LessonOverlap(s1, e1).Compile().Invoke(
                new DbLesson
                {
                    Timeslot = new DbTimeslot { StartTime = s2, EndTime = e2 }
                }));
        Assert.Equal(overlapExpected,
            TimeslotDaoConditions.TimeslotOverlap(new DbTimeslot { StartTime = s1, EndTime = e1 })
                .Compile()
                .Invoke(new DbTimeslot { StartTime = s2, EndTime = e2 }));
        Assert.Equal(overlapExpected, TimeslotDaoConditions.SuggestionOverlap(s1, e1)
            .Compile()
            .Invoke(
                new DbLessonSuggestion
                {
                    Timeslot = new DbTimeslot
                    {
                        StartTime = s2,
                        EndTime = e2
                    },
                }));
    }

    private static DateTimeOffset Instant(int i)
    {
        return new DateTimeOffset(2025, 5, 5, 12, 0, 0, TimeSpan.Zero) + TimeSpan.FromMinutes(i);
    }
}
