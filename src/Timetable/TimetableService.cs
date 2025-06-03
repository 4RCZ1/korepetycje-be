using System.Globalization;
using Database.Entities;
using Endpoints.Interfaces;
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
        return _dao.GetLessonsInRange(ParseDate(startDate), ParseDate(endDate)).Select(lesson =>
            new LessonDto
            {
                StartTime = lesson.StartTime,
                EndTime = lesson.StartTime + (lesson.CustomDuration ?? lesson.Schedule!.LessonDuration),
                Info = String.Empty
            }).ToList();
    }

    public void PlanLessons(
        string startTime,
        string endDate,
        int periodInDays,
        string studentExternalId,
        int durationInMinutes)
    {
        var start = DateTime.ParseExact(
            startTime, "O", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
        var end = ParseDate(endDate).ToDateTime(new TimeOnly(0, 0, 0), DateTimeKind.Utc);
        var period = TimeSpan.FromDays(periodInDays);
        var lessons = new List<DbLesson>();
        for (var t = start; t < end; t += period)
        {
            lessons.Add(new DbLesson
            {
                StartTime = t,
            });
        }
        var schedule = new DbSchedule
        {
            StudentId = DecodeStudentExternalId(studentExternalId),
            Period = period,
            LessonDuration = TimeSpan.FromMinutes(durationInMinutes),
            Lessons = lessons,
        };
        _dao.CreateSchedule(schedule);
    }

    public void ConfirmLesson(string lessonExternalId)
    {
        _dao.ConfirmLesson(int.Parse(lessonExternalId));
    }

    private static int DecodeStudentExternalId(string externalId)
    {
        return int.Parse(externalId);
    }

    private static DateOnly ParseDate(string s)
    {
        try
        {
            return DateOnly.ParseExact(s, "O", CultureInfo.InvariantCulture);
        }
        catch (FormatException)
        {
            throw new InvalidRequestException();
        }
    }

    private readonly ILessonDao _dao;
}
