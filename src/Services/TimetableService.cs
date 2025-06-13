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
            .Select(lesson => new LessonDto
            {
                LessonId = EncodeExternalId(lesson.Id),
                StartTime = lesson.Timeslot.StartTime,
                EndTime = lesson.Timeslot.EndTime,
                Address = MockLessonAddress,
                Description = lesson.TutorInfo ?? string.Empty,
                Attendances = ConvertAttendancesToDtos(lesson.Attendances),
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
            .Select(lesson => new LessonDto
            {
                LessonId = EncodeExternalId(lesson.Id),
                StartTime = lesson.Timeslot.StartTime,
                EndTime = lesson.Timeslot.EndTime,
                Address = MockLessonAddress,
                Description = string.Empty,
                Attendances = ConvertAttendancesToDtos(lesson.Attendances),
            }).ToList();
    }

    private static IList<AttendanceDto> ConvertAttendancesToDtos(
        ICollection<DbAttendance> attendances)
    {
        return attendances.Select(a => new AttendanceDto
        {
            StudentName = a.Student!.Name,
            StudentSurname = a.Student.Surname,
            Confirmed = a.IsConfirmed,
        }).ToList();
    }

    public void PlanLessons(
        DateTime firstStart,
        DateTime firstEnd,
        DateTime scheduleEnd,
        int periodInDays,
        IList<string> externalStudentIds)
    {
        var plan = Scheduler.Plan(
            new TimeRange { Start = firstStart, End = firstEnd },
            scheduleEnd,
            periodInDays);
        var lessons = plan.Select(range => new DbLesson
            {
                Timeslot = new DbTimeslot { StartTime = range.Start, EndTime = range.End },
                Attendances = externalStudentIds.Select(id => new DbAttendance
                    {
                        StudentId = DecodeExternalId(id),
                        IsConfirmed = false,
                        HasOccurred = false,
                    })
                    .ToList(),
            })
            .ToList();
        var schedule = new DbSchedule
        {
            Lessons = lessons,
            Period = TimeSpan.FromDays(periodInDays),
        };
        using var t = _transactor.BeginTransaction();
        t.LessonDao.CreateSchedule(schedule);
        t.Commit();
    }

    public void EditLesson(
        string lessonExternalId,
        DateTime newStartTime,
        DateTime newEndTime,
        bool editFutureLessons)
    {
        using var t = _transactor.BeginTransaction();
        var editedLessonId = int.Parse(lessonExternalId);
        var editedLesson = t.LessonDao.GetLessonById(editedLessonId);
        if (editedLesson is null)
            throw new BadRequestException("Lesson not found.");
        var schedule = t.LessonDao.GetScheduleById(editedLesson.ScheduleId);
        if (schedule is null)
            throw new ApplicationException("Schedule not found.");
        var lessonsToEdit = PickLessonsToEdit(schedule, editedLessonId, editFutureLessons);
        var newLessonTimes = Scheduler.RescheduleSeries(
            lessonsToEdit.Select(l => l.Timeslot.AsRange()).ToList(),
            newStartTime,
            newEndTime);
        t.LessonDao.RemoveLessonsCascading(lessonsToEdit.Select(l => l.Id).ToList());
        if (lessonsToEdit.Count == schedule.Lessons.Count)
            t.LessonDao.RemoveSchedule(schedule.Id);
        t.LessonDao.CreateSchedule(new DbSchedule
        {
            Period = schedule.Period,
            Lessons = UpdateLessonTimes(lessonsToEdit, newLessonTimes),
        });
        t.Commit();
    }

    private static List<DbLesson> PickLessonsToEdit(
        DbSchedule schedule, int editedLessonId, bool editFutureLessons)
    {
        // Sorting might not be strictly necessary here, however not sorting would be brittle.
        var sortedLessons = schedule.Lessons.OrderBy(l => l.Timeslot.StartTime);
        if (editFutureLessons)
        {
            return sortedLessons.SkipWhile(l => l.Id != editedLessonId).ToList();
        }
        else
        {
            return sortedLessons.Where(l => l.Id == editedLessonId).ToList();
        }
    }

private static ICollection<DbLesson> UpdateLessonTimes(
        ICollection<DbLesson> lessons, IList<TimeRange> newTimes)
    {
        if (lessons.Count != newTimes.Count)
        {
            throw new ArgumentException(
                $"Collection sizes {lessons.Count} and {newTimes.Count} mismatched.");
        }

        return lessons.Zip(newTimes, (lesson, range) => new DbLesson
        {
            TutorInfo = lesson.TutorInfo,
            Timeslot = new DbTimeslot { StartTime = range.Start, EndTime = range.End },
        }).ToList();
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

    private static string EncodeExternalId(int databaseId)
    {
        return databaseId.ToString();
    }

    private static DateTime ParseDateTime(string s)
    {
        try
        {
            return DateTime.ParseExact(
                s, "O", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
        }
        catch (FormatException)
        {
            throw new BadRequestException("Invalid datetime.");
        }
    }

    private readonly ITransactor _transactor;
}
