using System.Globalization;
using Database.Entities;
using Endpoints.Interfaces;
using Timetable.Interfaces;

namespace Services;

public class TimetableService : ITimetableService
{
    public TimetableService(ITransactor transactor)
    {
        _transactor = transactor;
    }

    public IList<LessonDto> GetLessons(string startTime, string endTime)
    {
        using var t = _transactor.BeginTransaction();
        return t.LessonDao.GetLessonsInRange(ParseDateTime(startTime), ParseDateTime(endTime))
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
        using var t = _transactor.BeginTransaction();
        return t.LessonDao.GetStudentLessonsInRange(
                DecodeExternalId(studentExternalId),
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
        string endTime,
        int periodInDays,
        string studentExternalId,
        int durationInMinutes)
    {
        var start = DateTime.ParseExact(
            startTime, "O", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
        var end = ParseDateTime(endTime);
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
            StudentId = DecodeExternalId(studentExternalId),
            Period = period,
            Lessons = lessons,
        };
        using var transaction = _transactor.BeginTransaction();
        transaction.LessonDao.CreateSchedule(schedule);
        transaction.Commit();
    }

    public void AddFreeTerm(string startTime, string endTime)
    {
        using var t = _transactor.BeginTransaction();
        t.LessonDao.AddFreeTerm(ParseDateTime(startTime), ParseDateTime(endTime));
        t.Commit();
    }

    public void ConfirmLesson(string lessonExternalId, string studentExternalId)
    {
        using var t = _transactor.BeginTransaction();
        var attendance = t.LessonDao.GetAttendance(DecodeExternalId(lessonExternalId),
            DecodeExternalId(studentExternalId));
        if (attendance is not null)
        {
            attendance.IsConfirmed = true;
            t.LessonDao.SaveAttendance(attendance);
        }
        t.Commit();
    }

    private static int DecodeExternalId(string externalId)
    {
        return int.Parse(externalId);
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

    private readonly ITransactor _transactor;
}
