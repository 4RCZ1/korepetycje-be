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

    public IList<DbLesson> GetLessonsInRange(DateTime startTime, DateTime endTime)
    {
        return GetLessons()
            .Where(TimeslotDaoConditions.LessonOverlap(startTime, endTime))
            .ToList();
    }

    public IList<DbLesson> GetStudentLessonsInRange(int studentId, DateTime startTime, DateTime endTime)
    {
        return GetLessons()
            .Where(lesson => lesson.Schedule!.StudentId == studentId)
            .Where(TimeslotDaoConditions.LessonOverlap(startTime, endTime))
            .ToList();
    }

    private IQueryable<DbLesson> GetLessons()
    {
        return _context.Lessons
            .AsNoTracking()
            .Include(l => l.Schedule)
            .Include(l => l.Timeslot);
    }

    public void CreateSchedule(DbSchedule schedule)
    {
        var timeslotsToTake = schedule.Lessons.Select(l => l.Timeslot).ToList();
        var timeslotsTaken = _context.Timeslots
            .AsNoTracking()
            .ToList();

        if (IsTermTaken(timeslotsToTake, timeslotsTaken))
            throw new ApplicationException("There are colliding lessons!");

        _context.Add(schedule);
    }

    private bool IsTermTaken(List<DbTimeslot> tsToTake, List<DbTimeslot> tsTaken)
    {
        var colliding = GetCollidingTimeslots(tsToTake, tsTaken);
        return colliding.Any();
    }

    private List<DbTimeslot> GetCollidingTimeslots(List<DbTimeslot> tsToTake, List<DbTimeslot> tsTaken)
    {
        return tsToTake
            .Where(ts => tsTaken
                .Any(taken => (taken.StartTime >= ts.StartTime && taken.StartTime <= ts.EndTime)
                              || (taken.StartTime <= ts.StartTime && taken.EndTime >= ts.StartTime)))
            .ToList();
    }

    public void AddFreeTerm(DateTime startTime, DateTime endTime)
    {
        var timeslotToAdd = new DbTimeslot()
        {
            StartTime = startTime,
            EndTime = endTime,
        };
        var timeslotsTaken = _context.Timeslots
            .AsNoTracking()
            .ToList();
        if (IsTermTaken(new List<DbTimeslot>() { timeslotToAdd }, timeslotsTaken))
            throw new ApplicationException("There are colliding timeslots!");

        var timeslotsFree = _context.Timeslots
            .AsNoTracking()
            .ToList();

        var colliding = GetCollidingTimeslots(timeslotsFree, new List<DbTimeslot>(){timeslotToAdd});

        if (colliding.Any())
        {
            var timeslotsToRemove = _context.Timeslots
                .Where(ts => colliding.Select(t => t.Id).Contains(ts.Id))
                .ToList();
            _context.RemoveRange(timeslotsToRemove);
        }

        _context.Add(timeslotToAdd);
    }

    private readonly OurDbContext _context;
}
