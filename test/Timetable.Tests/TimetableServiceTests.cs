using Endpoints.Interfaces;
using FakeItEasy;
using Services;
using Timetable.Interfaces;

namespace Timetable.Tests;

public class TimetableServiceTests
{
    [Fact]
    public void ThrowForWrongDateFormat()
    {
        Assert.Throws<BadRequestException>(() =>
            _service.GetLessons("aaa", "2026-01-01T00:00:00.0000000Z"));
        Assert.Throws<BadRequestException>(() =>
            _service.GetLessons("2025-01-01T00:00:00.0000000Z", "aaa"));
    }

    private readonly TimetableService _service = new(A.Dummy<ITransactor>(), TimeZoneInfo.Utc);
}
