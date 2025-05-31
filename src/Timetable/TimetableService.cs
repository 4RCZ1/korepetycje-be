using Timetable.Interfaces;

namespace Timetable;

public class TimetableService : ITimetableService
{
    public TimetableService(ILessonDao dao)
    {
        _dao = dao;
    }

    public IList<LessonDto> GetLessons(string startDate, string endDate)
    {
        return _dao.GetLessonsInRange(ParseDate(startDate), ParseDate(endDate)).Select(lesson => new LessonDto
        {
            StartTime = lesson.StartTime,
            EndTime = lesson.EndTime,
            Info = "",
        }).ToList();
    }

    private static DateOnly ParseDate(string s)
    {
        try
        {
            return DateOnly.ParseExact(s, "O");
        }
        catch (FormatException)
        {
            throw new InvalidRequestException();
        }
    }

    private readonly ILessonDao _dao;
}
