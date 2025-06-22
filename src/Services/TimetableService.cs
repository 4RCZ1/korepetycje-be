using System.Globalization;
using Database.Entities;
using Endpoints.Interfaces;
using Endpoints.Interfaces.Authorization;
using Timetable.Interfaces;

namespace Services;

public class TimetableService : ITimetableService
{
    public TimetableService(ITransactor transactor, TimeZoneInfo timeZone, IClock clock)
    {
        _transactor = transactor;
        _scheduler = new Scheduler(timeZone);
        _clock = clock;
    }

    public IList<LessonDto> GetLessons(string startTime, string endTime, TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        return t.LessonDao.GetLessonsInRange(ParseDateTime(startTime), ParseDateTime(endTime))
            .Select(ConvertLessonToDtoForTutor)
            .ToList();
    }

    public IList<LessonDto> GetStudentLessons(
        string studentExternalId,
        string startTime,
        string endTime,
        TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        return t.LessonDao.GetStudentLessonsInRange(
                DecodeExternalId(studentExternalId),
                ParseDateTime(startTime),
                ParseDateTime(endTime))
            .Select(ConvertLessonToDtoForTutor)
            .ToList();
    }

    private static LessonDto ConvertLessonToDtoForTutor(DbLesson lesson)
    {
        return new LessonDto
        {
            LessonId = EncodeExternalId(lesson.Id),
            StartTime = lesson.Timeslot!.StartTime,
            EndTime = lesson.Timeslot.EndTime,
            Address = lesson.Schedule!.Address!.AddressData,
            Description = lesson.TutorInfo ?? string.Empty,
            Attendances = ConvertAttendancesToDtos(lesson.Attendances),
        };
    }

    public IList<LessonDto> GetLessonsAsStudent(string startTime, string endTime, StudentRole role)
    {
        using var t = _transactor.BeginTransaction();
        var studentId = DecodeExternalId(role.ExternalStudentId);
        return t.LessonDao.GetStudentLessonsInRange(
                studentId,
                ParseDateTime(startTime),
                ParseDateTime(endTime))
            .Select(l => ConvertLessonToDtoForStudent(studentId, l))
            .ToList();
    }

    private static LessonDto ConvertLessonToDtoForStudent(int studentId, DbLesson lesson)
    {
        return new LessonDto
        {
            LessonId = EncodeExternalId(lesson.Id),
            StartTime = lesson.Timeslot!.StartTime,
            EndTime = lesson.Timeslot.EndTime,
            Address = lesson.Schedule!.Address!.AddressData,
            Description = string.Empty,
            Attendances = ConvertAttendancesToDtos(
                lesson.Attendances.Where(a => a.StudentId == studentId)),
        };
    }

    private static IList<AttendanceDto> ConvertAttendancesToDtos(
        IEnumerable<DbAttendance> attendances)
    {
        return attendances.Select(a => new AttendanceDto
        {
            StudentName = a.Student!.Name,
            StudentSurname = a.Student.Surname,
            Confirmed = a.IsConfirmed,
        }).ToList();
    }

    public void PlanLessons(
        DateTimeOffset firstStart,
        DateTimeOffset firstEnd,
        DateTimeOffset scheduleEnd,
        int periodInDays,
        string externalAddressId,
        IList<string> externalStudentIds,
        TutorRole role)
    {
        var plan = _scheduler.Plan(
            new TimeRange { Start = firstStart, End = firstEnd },
            scheduleEnd,
            periodInDays);
        var lessons = plan.Select(range => new DbLesson
            {
                Timeslot = new DbTimeslot { StartTime = range.Start, EndTime = range.End },
                Attendances = externalStudentIds.Select(id => new DbAttendance
                    {
                        StudentId = DecodeExternalId(id),
                        IsConfirmed = null,
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
        DateTimeOffset newStartTime,
        DateTimeOffset newEndTime,
        bool editFutureLessons,
        TutorRole role)
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

    public void AcceptSuggestion(string externalSuggestionId, bool accept, StudentRole role)
    {
        var suggestionId = DecodeExternalId(externalSuggestionId);
        using var t = _transactor.BeginTransaction();
        var suggestion = t.LessonSuggestionDao.GetLessSuggById(suggestionId);
        if (suggestion.StudentId != DecodeExternalId(role.ExternalStudentId))
            throw new AuthException();
        if (accept)
        {
            t.LessonDao.CreateSchedule(new DbSchedule
            {
                AddressId = suggestion.AddressId,
                Lessons =
                [
                    new DbLesson
                    {
                        TimeslotId = suggestion.TimeslotId,
                        Attendances = [new DbAttendance { StudentId = suggestion.StudentId }],
                    },
                ],
            });
            t.LessonSuggestionDao.DeleteSuggestionPreservingTimeslot(suggestionId);
            if (suggestion.LessonId.HasValue)
            {
                t.LessonDao.RemoveLessonsCascading([suggestion.LessonId.Value]);
                t.LessonDao.RemoveEmptySchedules();
            }
        }
        else
        {
            t.LessonSuggestionDao.DeleteLessonSuggestion(suggestionId);
        }
        t.Commit();
    }

    public void DeleteLesson(string externalLessonId, bool deleteFutureLessons, TutorRole role)
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
        var sortedLessons = schedule.Lessons.OrderBy(l => l.Timeslot!.StartTime);
        if (pickFutureLessons)
        {
            return sortedLessons.SkipWhile(l => l.Id != lessonId).ToList();
        }
        else
        {
            return sortedLessons.Where(l => l.Id == lessonId).ToList();
        }
    }

    private ICollection<DbLesson> UpdateLessonTimes(
        ICollection<DbLesson> lessons, DateTimeOffset newStartTime, DateTimeOffset newEndTime)
    {
        var newLessonTimes = _scheduler.RescheduleSeries(
            lessons.Select(l => l.Timeslot!.AsRange()).ToList(),
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

    public void ConfirmLesson(bool confirmed, string lessonExternalId, StudentRole role)
    {
        using var t = _transactor.BeginTransaction();
        var attendance = t.LessonDao.GetAttendance(
            DecodeExternalId(lessonExternalId),
            DecodeExternalId(role.ExternalStudentId));
        if (attendance is null)
            return;
        ValidateConfirmationTime(t, attendance);
        if (attendance.IsConfirmed is null)
        {
            attendance.IsConfirmed = confirmed;
            t.LessonDao.SaveAttendance(attendance);
            t.Commit();
        }
        else
        {
            if (attendance.IsConfirmed != confirmed)
                throw new BadRequestException("Cannot change attendance status again.");
        }
    }

    private void ValidateConfirmationTime(ITransaction t, DbAttendance attendance)
    {
        var lesson = t.LessonDao.GetLessonById(attendance.LessonId)!;
        var lessonStart = lesson.Timeslot!.StartTime;
        if (lessonStart - AttendanceDeadline < _clock.Now)
            throw new BadRequestException("Zbyt późna próba potwierdzenia obecności.");
    }

    public void EditLessonDetails(string externalLessonId, string newDescription, TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        var lesson = t.LessonDao.GetLessonById(DecodeExternalId(externalLessonId));
        if (lesson is null)
            throw new BadRequestException("Lesson not found.");
        lesson.TutorInfo = newDescription;
        t.LessonDao.SaveLesson(lesson);
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

    private static DateTimeOffset ParseDateTime(string s)
    {
        if (DateTimeOffset.TryParse(
                s,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal,
                out var time))
        {
            return time;
        }
        else
        {
            throw new BadRequestException("Invalid datetime.");
        }
    }

    private static readonly TimeSpan AttendanceDeadline = TimeSpan.FromHours(23);
    private readonly ITransactor _transactor;
    private readonly Scheduler _scheduler;
    private readonly IClock _clock;
}
