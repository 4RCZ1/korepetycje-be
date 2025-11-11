using Database.Entities;
using Xunit;
using Assert = Xunit.Assert;

namespace Database.Tests;

public class TenantEntityTests
{
    [Fact]
    public void SetTenantIdOnSelf()
    {
        var schedule = new DbSchedule();
        schedule.SetTenantId(2);
        Assert.Equal(2, schedule.TenantId);
    }

    [Fact]
    public void SetTenantIdOnProperties()
    {
        var schedule = new DbSchedule
        {
            Address = new DbAddress
            {
                AddressName = "name",
                AddressData = "address",
            },
        };
        schedule.SetTenantId(2);
        Assert.Equal(2, schedule.Address.TenantId);
    }

    [Fact]
    public void SetTenantIdOnCollections()
    {
        var schedule = new DbSchedule
        {
            Lessons = [new DbLesson(), new DbLesson()]
        };
        schedule.SetTenantId(2);
        Assert.Equal([2, 2], schedule.Lessons.Select(l => l.TenantId));
    }

    [Fact]
    public void SetTenantIdRecursively()
    {
        var schedule = new DbSchedule
        {
            Lessons = [
                new DbLesson
                {
                    TenantId = 2,
                    Attendances = [new DbAttendance()]
                }
            ]
        };
        schedule.SetTenantId(2);
        Assert.Equal([2], schedule.Lessons.Single().Attendances.Select(a => a.TenantId));
    }

    [Fact]
    public void AvoidInfiniteRecursion()
    {
        var lesson = new DbLesson();
        var schedule = new DbSchedule
        {
            Lessons = [lesson]
        };
        lesson.Schedule = schedule;
        schedule.SetTenantId(2);
        Assert.Equal([2], schedule.Lessons.Select(l => l.TenantId));
    }
}
