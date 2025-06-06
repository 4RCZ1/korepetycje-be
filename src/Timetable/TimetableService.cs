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

    public IList<LessonDto> GetLessons(string startTime, string endTime)
    {
        return _dao.GetLessonsInRange(ParseDateTime(startTime), ParseDateTime(endTime))
            .Select(lesson =>
                new LessonDto
                {
                    StartTime = lesson.Timeslot.StartTime,
                    EndTime = lesson.Timeslot.EndTime,
                    Info = lesson.TutorInfo ?? string.Empty
                }).ToList();
    }

    public IList<LessonDto> GetStudentLessons(
        string studentExternalId,
        string startTime,
        string endTime)
    {
        return _dao.GetStudentLessonsInRange(
                DecodeStudentExternalId(studentExternalId),
                ParseDateTime(startTime),
                ParseDateTime(endTime))
            .Select(l => new LessonDto
            {
                StartTime = l.Timeslot.StartTime,
                EndTime = l.Timeslot.EndTime,
                Info = string.Empty
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
                Timeslot = new DbTimeslot
                {
                    StartTime = t,
                    EndTime = t + TimeSpan.FromMinutes(durationInMinutes),
                }
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

    public void AddFreeTerm(string startTime, string endTime)
    {
        _dao.AddFreeTerm(ParseDateTime(startTime), ParseDateTime(endTime));
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
    private static DateTime ParseDateTime(string s)
    {
        try
        {
            return DateTime.ParseExact(s, "O", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
        }
        catch (FormatException)
        {
            throw new InvalidRequestException();
        }
    }
    private readonly ILessonDao _dao;
}
