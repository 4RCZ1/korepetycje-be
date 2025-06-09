using Database.Entities;
using Endpoints.Interfaces;
using FakeItEasy;
using Services;
using Timetable.Interfaces;

namespace Timetable.Tests;

public class TimetableServiceTests
{
    public TimetableServiceTests()
    {
        A.CallTo(() => _transactor.BeginTransaction()).Returns(_transaction).Once();
        A.CallTo(() => _transaction.LessonDao).Returns(_dao);
        _service = new TimetableService(_transactor);
    }

    [Fact]
    public void ThrowForWrongDateFormat()
    {
        Assert.Throws<InvalidRequestException>(() =>
            _service.GetLessons("aaa", RequestEndTimeString));
        Assert.Throws<InvalidRequestException>(() =>
            _service.GetLessons(RequestStartTimeString, "aaa"));
    }

    [Fact]
    public void ExcludeEndDate()
    {
        var schedule = A.Captured<DbSchedule>();
        A.CallTo(() => _dao.CreateSchedule(schedule._)).DoesNothing();
        _service.PlanLessons(
            "2025-06-02T12:00:00.0000000Z", "2025-06-16T00:00:00.0000000Z", 7, StudentExternalId, 30);
        Assert.Collection(schedule.Values, s =>
        {
            Assert.Equal(TimeSpan.FromDays(7), s.Period);
            Assert.Equal(StudentId, s.StudentId);
            Assert.Collection(s.Lessons,
                l => Assert.Equal(new DateTime(2025, 6, 2, 12, 0, 0), l.Timeslot.StartTime),
                l => Assert.Equal(new DateTime(2025, 6, 9, 12, 0, 0), l.Timeslot.StartTime));
        });
    }

    [Fact]
    public void ConfirmLesson()
    {
        _service.ConfirmLesson("13");
        A.CallTo(() => _dao.ConfirmLesson(13)).MustHaveHappenedOnceExactly();
    }

    private readonly ITransactor _transactor = A.Fake<ITransactor>();
    private readonly ITransaction _transaction = A.Fake<ITransaction>();
    private readonly ILessonDao _dao = A.Fake<ILessonDao>();
    private readonly ITimetableService _service;

    private const string RequestStartTimeString = "2025-01-01T00:00:00.0000000Z";
    private const string RequestEndTimeString = "2026-01-01T00:00:00.0000000Z";
    private const string StudentExternalId = "123";
    private const int StudentId = 123;
}
