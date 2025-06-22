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
        A.CallTo(() => _dao.GetLessonById(LessonId)).Returns(new DbLesson
        {
            Timeslot = new DbTimeslot { StartTime = _lessonStart },
        });
        A.CallTo(() => _clock.Now).Returns(_lessonStart - TimeSpan.FromHours(26));
        _service = new TimetableService(_transactor, TimeZoneInfo.Utc, _clock);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConfirmOrRejectAttendance(bool confirm)
    {
        DatabaseAttendanceIs(null);
        _service.ConfirmLesson(confirm, ExternalLessonId, _role);
        A.CallTo(() => _dao.SaveAttendance(
                A<DbAttendance>.That.Matches(a => a.IsConfirmed == confirm)))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => _transaction.Commit()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void ThrowIfConfirmationTooLate()
    {
        DatabaseAttendanceIs(null);
        A.CallTo(() => _clock.Now).Returns(_lessonStart - TimeSpan.FromHours(20));
        var action = () => _service.ConfirmLesson(true, ExternalLessonId, _role);
        Assert.Throws<BadRequestException>(action);
        A.CallTo(() => _transaction.Commit()).MustNotHaveHappened();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ThrowIfAttendanceAlreadySet(bool previousState)
    {
        DatabaseAttendanceIs(previousState);
        var action = () => _service.ConfirmLesson(!previousState, ExternalLessonId, _role);
        Assert.Throws<BadRequestException>(action);
        A.CallTo(() => _transaction.Commit()).MustNotHaveHappened();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IgnoreRepeatedRequest(bool confirmed)
    {
        DatabaseAttendanceIs(confirmed);
        _service.ConfirmLesson(confirmed, ExternalLessonId, _role);
        A.CallTo(() => _dao.SaveAttendance(A<DbAttendance>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public void IgnoreRemovedAttendances()
    {
        A.CallTo(() => _dao.GetAttendance(LessonId, 201)).Returns(null);
        _service.ConfirmLesson(true, ExternalLessonId, _role);
        A.CallTo(() => _dao.SaveAttendance(A<DbAttendance>._))
            .MustNotHaveHappened();
    }

    private void DatabaseAttendanceIs(bool? confirmed)
    {
        A.CallTo(() => _dao.GetAttendance(101, 201)).Returns(new DbAttendance
        {
            LessonId = LessonId,
            IsConfirmed = confirmed,
        });
    }

    private const string ExternalLessonId = "101";
    private const int LessonId = 101;
    private const string ExternalStudentId = "201";

    private readonly StudentRole _role = new() { ExternalStudentId = ExternalStudentId };
    private readonly ITransactor _transactor = A.Fake<ITransactor>();
    private readonly ITransaction _transaction = A.Fake<ITransaction>();
    private readonly ILessonDao _dao = A.Fake<ILessonDao>();
    private readonly TimetableService _service;
    private readonly IClock _clock = A.Fake<IClock>();
    private readonly DateTimeOffset _lessonStart = new(2025, 3, 3, 14, 0, 0, TimeSpan.Zero);
}
