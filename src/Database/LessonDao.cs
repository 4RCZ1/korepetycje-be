using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Timetable.Interfaces;

namespace Database;

public class LessonDao : ILessonDao
{
    public LessonDao(OurDbContext context)
    {
        _context = context;
    }

    public DbAttendance? GetAttendance(int lessonId, int studentId)
    {
        return _context.Attendances.AsNoTracking().SingleOrDefault(
            a => a.LessonId == lessonId && a.StudentId == studentId);
    }

    public void SaveAttendance(DbAttendance attendance)
    {
        _context.Attendances.Update(attendance);
    }

    public DbLesson? GetLessonById(int lessonId)
    {
        return _context.Lessons.AsNoTracking().SingleOrDefault(l => l.Id == lessonId);
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
            .AsNoTracking()
            .Include(l => l.Schedule)
            .ThenInclude(s => s!.Address)
            .Include(l => l.Timeslot)
            .Include(l => l.Attendances.OrderBy(a => a.StudentId))
            .ThenInclude(a => a.Student);
    }

    public void RemoveLessonsCascading(IList<int> lessonIds)
    {
        var lessonsToRemove = _context.Lessons.Where(l => lessonIds.Contains(l.Id));
        var timeslotsToCascade = _context.Timeslots
            .Where(t => lessonsToRemove.Any(l => l.TimeslotId == t.Id));
        _context.Lessons.RemoveRange(lessonsToRemove);
        _context.Timeslots.RemoveRange(timeslotsToCascade);
    }

    public DbSchedule? GetScheduleById(int scheduleId)
    {
        return _context.Schedules
            .AsNoTracking()
            .Include(s => s.Lessons)
            .ThenInclude(l => l.Timeslot)
            .Include(s => s.Lessons.OrderBy(l => l.Timeslot!.StartTime))
            .SingleOrDefault(s => s.Id == scheduleId);
    }

    public void CreateSchedule(DbSchedule schedule)
    {
        var timeslotsToTake = schedule.Lessons.Select(l => l.Timeslot!).ToList();
        if (DetectCollisions(timeslotsToTake))
            throw new ApplicationException("There are colliding lessons!");
        _context.Add(schedule);
    }

    private bool DetectCollisions(List<DbTimeslot> timeslotsToSave)
    {
        var deletedTimeslotIds = _context.ChangeTracker.Entries<DbTimeslot>()
            .Where(entry => entry.State == EntityState.Deleted)
            .Select(entry => entry.Entity.Id)
            .ToList();
        // Updating timeslots is not supported here.
        return timeslotsToSave.Where(IsTimeslotNew).Any(nts => _context.Timeslots
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
        var emptySchedules = _context.Schedules.Where(s => s.Lessons.Count == 0);
        _context.RemoveRange(emptySchedules);
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

    private readonly OurDbContext _context;
}
