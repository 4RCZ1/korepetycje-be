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
        Assert.Throws<BadRequestException>(() =>
            _service.GetLessons("aaa", "2026-01-01T00:00:00.0000000Z"));
        Assert.Throws<BadRequestException>(() =>
            _service.GetLessons("2025-01-01T00:00:00.0000000Z", "aaa"));
    }

    [Fact]
    public void ConfirmAttendance()
    {
        var attendance = new DbAttendance { IsConfirmed = false };
        A.CallTo(() => _dao.GetAttendance(101, 201)).Returns(attendance);
        _service.ConfirmLesson("101", "201");
        A.CallTo(() => _dao.SaveAttendance(A<DbAttendance>.That.Matches(a => a.IsConfirmed)))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => _transaction.Commit()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void IgnoreRemovedAttendances()
    {
        A.CallTo(() => _dao.GetAttendance(101, 201)).Returns(null);
        _service.ConfirmLesson("101", "201");
        A.CallTo(() => _dao.SaveAttendance(A<DbAttendance>._))
            .MustNotHaveHappened();
    }

    private readonly ITransactor _transactor = A.Fake<ITransactor>();
    private readonly ITransaction _transaction = A.Fake<ITransaction>();
    private readonly ILessonDao _dao = A.Fake<ILessonDao>();
    private readonly TimetableService _service;
}
