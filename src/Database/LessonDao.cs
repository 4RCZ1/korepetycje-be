using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Timetable.Interfaces;

namespace Database;

internal class LessonDao : ILessonDao
{
    public LessonDao(TenantContext context)
    {
        _context = context;
    }

    public DbAttendance? GetAttendance(int lessonId, int studentId)
    {
        return _context.Attendances.Query().AsNoTracking().SingleOrDefault(
            a => a.LessonId == lessonId && a.StudentId == studentId);
    }

    public void SaveAttendance(DbAttendance attendance)
    {
        _context.Attendances.Update(attendance);
    }

    public DbLesson? GetLessonById(int lessonId)
    {
        return _context.Lessons
            .Query()
            .AsNoTracking()
            .Include(l => l.Timeslot)
            .SingleOrDefault(l => l.Id == lessonId);
    }

    public DbLesson? GetLessonByIdWithAttendances(int lessonId)
    {
        return _context.Lessons
            .Query()
            .AsNoTracking()
            .Include(l => l.Timeslot)
            .Include(l => l.Attendances)
            .SingleOrDefault(l => l.Id == lessonId);
    }

    public void SaveLesson(DbLesson lesson)
    {
        _context.Lessons.Update(lesson);
    }

    public IList<DbLesson> GetLessonsInRange(DateTimeOffset startTime, DateTimeOffset endTime)
    {
        return GetLessons()
            .Where(TimeslotDaoConditions.LessonOverlap(startTime, endTime))
            .ToList();
    }

    public IList<DbLesson> GetStudentLessonsInRange(
        int studentId, DateTimeOffset startTime, DateTimeOffset endTime)
    {
        return GetLessons()
            .Where(lesson => lesson.Attendances.Any(a => a.StudentId == studentId))
            .Where(TimeslotDaoConditions.LessonOverlap(startTime, endTime))
            .ToList();
    }

    private IQueryable<DbLesson> GetLessons()
    {
        return _context.Lessons
            .Query()
            .AsNoTracking()
            .Include(l => l.Schedule)
            .ThenInclude(s => s!.Address)
            .Include(l => l.Timeslot)
            .Include(l => l.Attendances.OrderBy(a => a.StudentId))
            .ThenInclude(a => a.Student);
    }

    public void RemoveLessonsCascading(IList<int> lessonIds)
    {
        var lessonsToRemove = _context.Lessons.Query().Where(l => lessonIds.Contains(l.Id));
        var timeslotsToCascade = _context.Timeslots
            .Query()
            .Where(t => lessonsToRemove.Any(l => l.TimeslotId == t.Id));
        _context.Lessons.RemoveRange(lessonsToRemove);
        _context.Timeslots.RemoveRange(timeslotsToCascade);
    }

    public DbSchedule? GetScheduleById(int scheduleId)
    {
        return _context.Schedules
            .Query()
            .AsNoTracking()
            .Include(s => s.Lessons)
            .ThenInclude(l => l.Timeslot)
            .Include(s => s.Lessons.OrderBy(l => l.Timeslot!.StartTime))
            .ThenInclude(l => l.Attendances)
            .SingleOrDefault(s => s.Id == scheduleId);
    }

    public void CreateSchedule(DbSchedule schedule)
    {
        var timeslotsToTake = schedule.Lessons.Select(l => l.Timeslot!).ToList();
        if (DetectCollisions(timeslotsToTake))
            throw new ApplicationException("There are colliding lessons!");
        _context.Schedules.Add(schedule);
    }

    private bool DetectCollisions(List<DbTimeslot> timeslotsToSave)
    {
        var deletedTimeslotIds = _context.Timeslots
            .GetFreshlyDeleted()
            .Select(ts => ts.Id)
            .ToList();
        // Updating timeslots is not supported here.
        return timeslotsToSave.Where(IsTimeslotNew).Any(nts => _context.Timeslots
            .Query()
            .AsNoTracking()
            .Where(ts => !deletedTimeslotIds.Contains(ts.Id))
            .Any(TimeslotDaoConditions.TimeslotOverlap(nts)));
    }

    private static bool IsTimeslotNew(DbTimeslot ts)
    {
        return ts.Id == 0;
    }

    public List<DbTimeslot> GetTimeslots()
    {
        return _context.Timeslots
            .Query()
            .AsNoTracking()
            .ToList();
    }

    public bool IsTermTaken(List<DbTimeslot> tsToTake, List<DbTimeslot> tsTaken)
    {
        var colliding = GetCollidingTimeslots(tsToTake, tsTaken);
        return colliding.Count != 0;
    }

    public void RemoveEmptySchedules()
    {
        var emptySchedules = _context.Schedules.Query().Where(s => s.Lessons.Count == 0);
        _context.Schedules.RemoveRange(emptySchedules);
    }

    private static List<DbTimeslot> GetCollidingTimeslots(
        List<DbTimeslot> tsToTake, List<DbTimeslot> tsTaken)
    {
        return tsToTake
            .Where(ts => tsTaken
                .Any(taken => (taken.StartTime >= ts.StartTime && taken.StartTime <= ts.EndTime)
                              || (taken.StartTime <= ts.StartTime && taken.EndTime >= ts.StartTime)))
            .ToList();
    }

    private readonly TenantContext _context;
}
