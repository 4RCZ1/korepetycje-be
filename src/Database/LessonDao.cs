using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Timetable.Interfaces;

namespace Database;

public class LessonDao : ILessonDao
{
    public LessonDao(string connection)
    {
        _connection = connection;
    }

    public IList<DbLesson> GetLessonsInRange(DateTime startTime, DateTime endTime)
    {
        using var context = new OurDbContext(_connection);
        return GetLessons(context)
            .Where(TimeslotDaoConditions.LessonOverlap(startTime, endTime))
            .ToList();
    }

    public IList<DbLesson> GetStudentLessonsInRange(int studentId, DateTime startTime, DateTime endTime)
    {
        using var context = new OurDbContext(_connection);
        return GetLessons(context)
            .Where(lesson => lesson.Schedule!.StudentId == studentId)
            .Where(TimeslotDaoConditions.LessonOverlap(startTime, endTime))
            .ToList();
    }

    private static IQueryable<DbLesson> GetLessons(OurDbContext context)
    {
        return context.Lessons
            .AsNoTracking()
            .Include(l => l.Schedule)
            .Include(l => l.Timeslot);
    }

    public IList<DbLesson> GetStudentLessons(int studentId)
    {
        using var context = new OurDbContext(_connection);
        var lessons = context.Lessons
            .AsNoTracking()
            .Include(l => l.Schedule)
            .Include(l => l.Timeslot)
            .Where(l => l.Schedule!.StudentId == studentId)
            .ToList();
        return lessons;
    }

    public void ConfirmLesson(int lessonId)
    {
        using var context = new OurDbContext(_connection);
        context.Lessons.Where(l => l.Id == lessonId).SingleOrDefault().IsConfirmed = true;
        context.SaveChanges();
    }

    public void CreateSchedule(DbSchedule schedule)
    {
        using var context = new OurDbContext(_connection);
        var timeslotsToTake = schedule.Lessons.Select(l => l.Timeslot).ToList();
        var timeslotsTaken = context.Timeslots
            .AsNoTracking()
            .Where(ts => ts.IsFree == false)
            .ToList();

        if (IsTermTaken(timeslotsToTake, timeslotsTaken))
            throw new ApplicationException("There are colliding lessons!");

        context.Add(schedule);
        context.SaveChanges();
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
        using var context = new OurDbContext(_connection);
        var timeslotToAdd = new DbTimeslot()
        {
            StartTime = startTime,
            EndTime = endTime,
            IsFree = true
        };
        var timeslotsTaken = context.Timeslots
            .AsNoTracking()
            .Where(ts => ts.IsFree == false)
            .ToList();
        if (IsTermTaken(new List<DbTimeslot>(){timeslotToAdd}, timeslotsTaken))
            throw new ApplicationException("There are colliding timeslots!");

        var timeslotsFree = context.Timeslots
            .AsNoTracking()
            .Where(ts => ts.IsFree == true)
            .ToList();

        var colliding = GetCollidingTimeslots(timeslotsFree, new List<DbTimeslot>(){timeslotToAdd});

        if (colliding.Any())
        {
            var timeslotsToRemove = context.Timeslots
                .Where(ts => colliding.Select(t => t.Id).Contains(ts.Id))
                .ToList();
            context.RemoveRange(timeslotsToRemove);
        }

        context.Add(timeslotToAdd);
        context.SaveChanges();
    }

    private readonly string _connection;
}
