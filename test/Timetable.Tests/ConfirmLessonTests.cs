using Database.Entities;
using Endpoints.Interfaces;
using Endpoints.Interfaces.Authorization;
using FakeItEasy;
using Services;
using Timetable.Interfaces;

namespace Timetable.Tests;

public class ConfirmLessonTests
{
    public ConfirmLessonTests()
    {
        A.CallTo(() => _transactor.BeginTransaction()).Returns(_transaction).Once();
        A.CallTo(() => _transaction.LessonDao).Returns(_dao);
        _service = new TimetableService(_transactor, TimeZoneInfo.Utc);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConfirmOrRejectAttendance(bool confirm)
    {
        DatabaseAttendanceIs(new DbAttendance { IsConfirmed = null });
        _service.ConfirmLesson(confirm, ExternalLessonId, _role);
        A.CallTo(() => _dao.SaveAttendance(
                A<DbAttendance>.That.Matches(a => a.IsConfirmed == confirm)))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => _transaction.Commit()).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ThrowIfAttendanceAlreadySet(bool previousState)
    {
        DatabaseAttendanceIs(new DbAttendance { IsConfirmed = previousState });
        var action = () =>
            _service.ConfirmLesson(!previousState, ExternalLessonId, _role);
        Assert.Throws<BadRequestException>(action);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IgnoreRepeatedRequest(bool confirmed)
    {
        DatabaseAttendanceIs(new DbAttendance { IsConfirmed = confirmed });
        _service.ConfirmLesson(confirmed, ExternalLessonId, _role);
        A.CallTo(() => _dao.SaveAttendance(A<DbAttendance>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public void IgnoreRemovedAttendances()
    {
        DatabaseAttendanceIs(null);
        _service.ConfirmLesson(true, ExternalLessonId, _role);
        A.CallTo(() => _dao.SaveAttendance(A<DbAttendance>._))
            .MustNotHaveHappened();
    }

    private void DatabaseAttendanceIs(DbAttendance? attendance)
    {
        A.CallTo(() => _dao.GetAttendance(101, 201)).Returns(attendance);
    }

    private const string ExternalLessonId = "101";
    private const string ExternalStudentId = "201";

    private readonly StudentRole _role = new() { ExternalStudentId = ExternalStudentId };
    private readonly ITransactor _transactor = A.Fake<ITransactor>();
    private readonly ITransaction _transaction = A.Fake<ITransaction>();
    private readonly ILessonDao _dao = A.Fake<ILessonDao>();
    private readonly TimetableService _service;
}
