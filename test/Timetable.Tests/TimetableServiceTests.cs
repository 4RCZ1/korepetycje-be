using Timetable.Interfaces;
using FakeItEasy;

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
        List<Lesson> lessons =
        [
            new() { StartTime = LessonStart, EndTime = LessonEnd, TutorInfo = string.Empty },
        ];
        A.CallTo(() => _dao.GetLessonsInRange(RequestBeginDate, RequestEndDate))
            .Returns(lessons);
        List<LessonDto> expected =
        [
            new()
            {
                StartTime = LessonStart,
                EndTime = LessonEnd,
                Info = string.Empty,
            }
        ];
        Assert.Equivalent(expected, _service.GetLessons(RequestBeginDateString, RequestEndDateString), strict: true);
    }

    [Fact]
    public void ThrowForWrongDateFormat()
    {
        Assert.Throws<InvalidRequestException>(() =>
            _service.GetLessons("aaa", RequestEndDateString));
        Assert.Throws<InvalidRequestException>(() =>
            _service.GetLessons(RequestBeginDateString, "aaa"));
    }

    private readonly ILessonDao _dao = A.Fake<ILessonDao>();
    private readonly ITimetableService _service;

    private const string RequestBeginDateString = "2025-01-01";
    private const string RequestEndDateString = "2026-01-01";
    private static readonly DateOnly RequestBeginDate = new(2025, 1, 1);
    private static readonly DateOnly RequestEndDate = new(2026, 1, 1);
    private static readonly DateTime LessonStart = new(2025, 5, 31, 14, 0, 0);
    private static readonly DateTime LessonEnd = new(2025, 5, 31, 14, 30, 0);
}
