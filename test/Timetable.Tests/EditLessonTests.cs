using Database.Entities;
using Endpoints.Interfaces;
using FakeItEasy;
using Services;
using Timetable.Interfaces;

namespace Timetable.Tests;

public class EditLessonTests
{
    public EditLessonTests()
    {
        A.CallTo(() => _transactor.BeginTransaction()).Returns(_transaction).Once();
        A.CallTo(() => _transaction.LessonDao).Returns(_dao);
        _service = new TimetableService(_transactor);
        A.CallTo(() => _dao.GetLessonById(EditedLessonId)).Returns(new DbLesson
        {
            Id = EditedLessonId,
            ScheduleId = ScheduleId,
            Timeslot = new DbTimeslot { StartTime = _oldStartTime, EndTime = _oldEndTime }
        });
    }

    [Fact]
    public void EditSingleLesson()
    {
        A.CallTo(() => _dao.GetScheduleById(ScheduleId))
            .Returns(ScheduleWith([LessonToday, LessonInAWeek]));
        _service.EditLesson(
            "12",
            _newStartTime,
            _newEndTime,
            false);
        A.CallTo(() => _dao.RemoveLessonsCascading(
                A<IList<int>>.That.IsSameSequenceAs(new[] { 12 })))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => _dao.CreateSchedule(A<DbSchedule>.That.Matches(
                s => s.Lessons
                    .Select(l => l.Timeslot.StartTime)
                    .SequenceEqual(new[] { _newStartTime }))))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => _transaction.Commit()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void RemoveEmptyLeftoverSchedule()
    {
        A.CallTo(() => _dao.GetScheduleById(ScheduleId))
            .Returns(ScheduleWith([LessonToday]));
        _service.EditLesson("12", _newStartTime, _newEndTime, false);
        A.CallTo(() => _dao.RemoveSchedule(ScheduleId)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void EditWholeSchedule()
    {
        A.CallTo(() => _dao.GetScheduleById(ScheduleId))
            .Returns(ScheduleWith([LessonToday, LessonInAWeek]));
        _service.EditLesson("12", _newStartTime, _newEndTime, true);
        A.CallTo(() => _dao.RemoveLessonsCascading(
                A<IList<int>>.That.IsSameSequenceAs(new[] { 12, 13 })))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => _dao.CreateSchedule(A<DbSchedule>.That.Matches(s =>
                s.Period == _week
                && s.Lessons.Select(l => l.Timeslot.StartTime)
                    .SequenceEqual(new[] { _newStartTime, _newStartTime + _week })
                && s.Lessons.Select(l => l.TutorInfo).SequenceEqual(new[] { "info 1", "info 2" })
            )))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void EditOneAndSubsequentLessons()
    {
        A.CallTo(() => _dao.GetScheduleById(ScheduleId))
            .Returns(ScheduleWith([LessonWeekAgo, LessonToday, LessonInAWeek]));
        _service.EditLesson("12", _newStartTime, _newEndTime, true);
        A.CallTo(() => _dao.RemoveLessonsCascading(
                A<IList<int>>.That.IsSameSequenceAs(new[] { 12, 13 })))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => _dao.CreateSchedule(A<DbSchedule>.That.Matches(s =>
                s.Lessons.Select(l => l.Timeslot.StartTime)
                    .SequenceEqual(new[] { _newStartTime, _newStartTime + _week })
                && s.Lessons.Select(l => l.TutorInfo).SequenceEqual(new[] { "info 1", "info 2" })
            )))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void ThrowForNonexistentLesson()
    {
        A.CallTo(() => _dao.GetLessonById(10101)).Returns(null);
        var action = () => _service.EditLesson("10101", _newStartTime, _newEndTime, false);
        Assert.Throws<BadRequestException>(action);
    }

    private DbLesson LessonWeekAgo => new()
    {
        Id = 11,
        TutorInfo = "info 0",
        Timeslot = new DbTimeslot
        {
            StartTime = _oldStartTime - _week,
            EndTime = _oldEndTime - _week,
        },
    };

    private DbLesson LessonToday => new()
    {
        Id = 12,
        TutorInfo = "info 1",
        Timeslot = new DbTimeslot
        {
            StartTime = _oldStartTime,
            EndTime = _oldEndTime,
        },
    };

    private DbLesson LessonInAWeek => new()
    {
        Id = 13,
        TutorInfo = "info 2",
        Timeslot = new DbTimeslot
        {
            StartTime = _oldStartTime + _week,
            EndTime = _oldEndTime + _week,
        },
    };

    private DbSchedule ScheduleWith(List<DbLesson> lessons)
    {
        return new DbSchedule
        {
            Id = ScheduleId,
            Period = _week,
            Lessons = lessons,
        };
    }

    private readonly DateTime _oldStartTime = new(2024, 1, 1, 6, 0, 0);
    private readonly DateTime _oldEndTime = new(2024, 1, 1, 6, 30, 0);
    private readonly DateTime _newStartTime = new(2025, 5, 3, 13, 0, 0);
    private readonly DateTime _newEndTime = new(2025, 5, 3, 14, 0, 0);
    private readonly TimeSpan _week = TimeSpan.FromDays(7);
    private const int EditedLessonId = 12;
    private const int ScheduleId = 101;

    private readonly ITransactor _transactor = A.Fake<ITransactor>();
    private readonly ITransaction _transaction = A.Fake<ITransaction>();
    private readonly ILessonDao _dao = A.Fake<ILessonDao>();
    private readonly TimetableService _service;
}
