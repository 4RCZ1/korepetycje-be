using Database.Entities;
using Endpoints.Interfaces;
using FakeItEasy;
using Timetable.Interfaces;

namespace Timetable.Tests;

public class TimetableServiceTests
{
    public TimetableServiceTests()
    {
        _service = new TimetableService(_dao);
    }

    [Fact]
    public void GetLessonsFromDao()
    {
        List<DbLesson> lessons =
        [
            new()
            {
                StartTime = LessonStart,
                Schedule = new DbSchedule { LessonDuration = LessonDuration },
            },
        ];
        A.CallTo(() => _dao.GetLessonsInRange(RequestBeginDate, RequestEndDate))
            .Returns(lessons);
        var actual = _service.GetLessons(RequestBeginDateString, RequestEndDateString);
        List<LessonDto> expected =
        [
            new()
            {
                StartTime = LessonStart,
                EndTime = LessonEnd,
                Info = string.Empty,
            },
        ];
        Assert.Equivalent(expected, actual, strict: true);
    }

    [Fact]
    public void ThrowForWrongDateFormat()
    {
        Assert.Throws<InvalidRequestException>(() =>
            _service.GetLessons("aaa", RequestEndDateString));
        Assert.Throws<InvalidRequestException>(() =>
            _service.GetLessons(RequestBeginDateString, "aaa"));
    }

    [Fact]
    public void ExcludeEndDate()
    {
        var schedule = A.Captured<DbSchedule>();
        A.CallTo(() => _dao.SaveSchedule(schedule._)).DoesNothing();
        _service.PlanLessons(
            "2025-06-02T12:00:00.0000000Z", "2025-06-16", 7, StudentExternalId, 30);
        Assert.Collection(schedule.Values, s =>
        {
            Assert.Equal(TimeSpan.FromDays(7), s.Period);
            Assert.Equal(LessonDuration, s.LessonDuration);
            Assert.Equal(StudentId, s.StudentId);
            Assert.Collection(s.Lessons,
                l => Assert.Equal(new DateTime(2025, 6, 2, 12, 0, 0), l.StartTime),
                l => Assert.Equal(new DateTime(2025, 6, 9, 12, 0, 0), l.StartTime));
        });
    }

    [Fact]
    public void ConfirmLesson()
    {
        _service.ConfirmLesson("13");
        A.CallTo(() => _dao.ConfirmLesson(13)).MustHaveHappenedOnceExactly();
    }

    private readonly ILessonDao _dao = A.Fake<ILessonDao>();
    private readonly ITimetableService _service;

    private const string RequestBeginDateString = "2025-01-01";
    private const string RequestEndDateString = "2026-01-01";
    private static readonly DateOnly RequestBeginDate = new(2025, 1, 1);
    private static readonly DateOnly RequestEndDate = new(2026, 1, 1);
    private static readonly DateTime LessonStart = new(2025, 5, 31, 14, 0, 0);
    private static readonly DateTime LessonEnd = new(2025, 5, 31, 14, 30, 0);
    private static readonly TimeSpan LessonDuration = TimeSpan.FromMinutes(30);
    private const string StudentExternalId = "123";
    private const int StudentId = 123;
}
