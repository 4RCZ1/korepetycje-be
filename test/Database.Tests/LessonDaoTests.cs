using Database.Entities;
using Timetable.Interfaces;
using Xunit;
using Assert = Xunit.Assert;

namespace Database.Tests;

// todo: extract out of LessonDao
public class LessonDaoTests
{
    public LessonDaoTests()
    {
        _dao = new LessonDao(_connection);
    }

    [Fact]
    public void ThrowForCollidingTimeslotsFound()
    {
        //case: koniec pierwszego nachodzi na początek drugiego
        Assert.True(_dao.IsTermTaken(_timeslots1, _timeslots2));
        //case: drugi zawiera się w pierwszym
        Assert.True(_dao.IsTermTaken(_timeslots2, _timeslots4));
        //case: koniec drugiego nachodzi na początek pierwszego
        Assert.True(_dao.IsTermTaken(_timeslots3, _timeslots4));
        //case: pierwszy zawiera się w drugim
        Assert.True(_dao.IsTermTaken(_timeslots4, _timeslots2));
    }

    [Fact]
    public void ReturnsTrue()
    {
        Assert.False(_dao.IsTermTaken(_timeslots1, _timeslots3));
    }

    private readonly ILessonDao _dao;
    private readonly string _connection;

    private List<DbTimeslot> _timeslots1 = new List<DbTimeslot>()
    {
        new DbTimeslot
        {
            Id = 1,
            IsFree = false,
            StartTime = new DateTime(2025, 6, 10, 10, 0, 0),
            EndTime = new DateTime(2025, 6, 10, 11, 0, 0)
        },
        new DbTimeslot
        {
            Id = 2,
            IsFree = false,
            StartTime = new DateTime(2025, 6, 10, 12, 0, 0),
            EndTime = new DateTime(2025, 6, 10, 13, 0, 0)
        }
    };

    private List<DbTimeslot> _timeslots2 = new List<DbTimeslot>()
    {
        new DbTimeslot
        {
            Id = 3,
            IsFree = false,
            StartTime = new DateTime(2025, 6, 10, 9, 0, 0),
            EndTime = new DateTime(2025, 6, 10, 9, 45, 0)
        },
        new DbTimeslot
        {
            Id = 4,
            IsFree = false,
            StartTime = new DateTime(2025, 6, 10, 10, 30, 0),
            EndTime = new DateTime(2025, 6, 10, 11, 30, 0)
        }
    };

    private List<DbTimeslot> _timeslots3 = new List<DbTimeslot>()
    {
        new DbTimeslot
        {
            Id = 3,
            IsFree = false,
            StartTime = new DateTime(2025, 6, 10, 9, 0, 0),
            EndTime = new DateTime(2025, 6, 10, 9, 45, 0)
        },
        new DbTimeslot
        {
            Id = 4,
            IsFree = false,
            StartTime = new DateTime(2025, 6, 10, 14, 30, 0),
            EndTime = new DateTime(2025, 6, 10, 15, 30, 0)
        }
    };

    private List<DbTimeslot> _timeslots4 = new List<DbTimeslot>()
    {
        new DbTimeslot
        {
            Id = 3,
            IsFree = false,
            StartTime = new DateTime(2025, 6, 10, 14, 0, 0),
            EndTime = new DateTime(2025, 6, 10, 15, 0, 0)
        },
        new DbTimeslot
        {
            Id = 4,
            IsFree = false,
            StartTime = new DateTime(2025, 6, 10, 9, 10, 0),
            EndTime = new DateTime(2025, 6, 10, 9, 40, 0)
        }
    };
}
