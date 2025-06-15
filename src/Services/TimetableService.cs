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
                Address = lesson.Schedule!.Address!.AddressData,
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
                Address = lesson.Schedule!.Address!.AddressData,
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
        string externalAddressId,
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
            AddressId = DecodeExternalId(externalAddressId),
            Period = TimeSpan.FromDays(periodInDays),
            Lessons = lessons,
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
        var lessonId = int.Parse(lessonExternalId);
        var lessonToEdit = t.LessonDao.GetLessonById(lessonId);
        if (lessonToEdit is null)
            throw new BadRequestException("Lesson not found.");
        var schedule = GetScheduleForLesson(t.LessonDao, lessonToEdit);
        var lessonsToEdit = PickLessons(schedule, lessonId, editFutureLessons);
        t.LessonDao.RemoveLessonsCascading(lessonsToEdit.Select(l => l.Id).ToList());
        t.LessonDao.CreateSchedule(new DbSchedule
        {
            AddressId = schedule.AddressId,
            Period = schedule.Period,
            Lessons = UpdateLessonTimes(lessonsToEdit, newStartTime, newEndTime),
        });
        t.LessonDao.RemoveEmptySchedules();
        t.Commit();
    }

    public void DeleteLesson(string externalLessonId, bool deleteFutureLessons)
    {
        using var t = _transactor.BeginTransaction();
        var lessonId = int.Parse(externalLessonId);
        var lessonToDelete = t.LessonDao.GetLessonById(lessonId);
        if (lessonToDelete is not null)
        {
            var schedule = GetScheduleForLesson(t.LessonDao, lessonToDelete);
            var lessonsToDelete = PickLessons(schedule, lessonId, deleteFutureLessons);
            t.LessonDao.RemoveLessonsCascading(lessonsToDelete.Select(l => l.Id).ToList());
            t.LessonDao.RemoveEmptySchedules();
            t.Commit();
        }
    }

    private static DbSchedule GetScheduleForLesson(ILessonDao dao, DbLesson lesson)
    {
        var schedule = dao.GetScheduleById(lesson.ScheduleId);
        return schedule ?? throw new ApplicationException("Schedule not found.");
    }

    private static List<DbLesson> PickLessons(
        DbSchedule schedule, int lessonId, bool pickFutureLessons)
    {
        // Sorting might not be strictly necessary here, however not sorting would be brittle.
        var sortedLessons = schedule.Lessons.OrderBy(l => l.Timeslot.StartTime);
        if (pickFutureLessons)
        {
            return sortedLessons.SkipWhile(l => l.Id != lessonId).ToList();
        }
        else
        {
            return sortedLessons.Where(l => l.Id == lessonId).ToList();
        }
    }

    private static ICollection<DbLesson> UpdateLessonTimes(
        ICollection<DbLesson> lessons, DateTime newStartTime, DateTime newEndTime)
    {
        var newLessonTimes = Scheduler.RescheduleSeries(
            lessons.Select(l => l.Timeslot.AsRange()).ToList(),
            newStartTime,
            newEndTime);

        if (lessons.Count != newLessonTimes.Count)
        {
            throw new ArgumentException(
                $"Collection sizes {lessons.Count} and {newLessonTimes.Count} mismatched.");
        }

        return lessons.Zip(newLessonTimes, (lesson, range) => new DbLesson
        {
            TutorInfo = lesson.TutorInfo,
            Timeslot = new DbTimeslot { StartTime = range.Start, EndTime = range.End },
        }).ToList();
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
