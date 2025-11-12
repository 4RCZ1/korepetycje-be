using Endpoints.Interfaces;
using Endpoints.Interfaces.Authorization;
using FakeItEasy;
using Services.Interfaces;

namespace Services.Tests;

public class TimetableServiceTests
{
    [Fact]
    public void ThrowForWrongDateFormat()
    {
        Assert.Throws<BadRequestException>(() =>
            _service.GetLessons("aaa", "2026-01-01T00:00:00.0000000Z", new TutorRole()));
        Assert.Throws<BadRequestException>(() =>
            _service.GetLessons("2025-01-01T00:00:00.0000000Z", "aaa", new TutorRole()));
    }

    private readonly TimetableService _service = new(
        A.Dummy<ITransactor>(), TimeZoneInfo.Utc, A.Dummy<IClock>());
}
