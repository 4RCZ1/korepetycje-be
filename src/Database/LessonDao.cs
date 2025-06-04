using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Timetable.Interfaces;

namespace Database;

public class LessonDao : ILessonDao
{
    private readonly string _connection;
    public LessonDao(string connection)
    {
        _connection = connection;
    }

    public IList<DbLesson> GetLessonsInRange(DateOnly startDate, DateOnly endDate)
    {
        using var context = new OurDbContext(_connection);
        var lessons = context.Lessons
            .AsNoTracking()    
            .Include(l => l.Schedule)
            .Include(l => l.Timeslot)
            .Where(l=>DateOnly.FromDateTime(l.Timeslot.StartTime.Date)>=startDate
                      &&DateOnly.FromDateTime(l.Timeslot.EndTime.Date)<=endDate)
            .ToList();
        return lessons;
    }

    public IList<DbLesson> GetStudentLessons(int studentId)
    {
        using var context = new OurDbContext(_connection);
        var lessons = context.Lessons
            .AsNoTracking()
            .Include(l => l.Schedule)
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

    public bool IsTermTaken(List<DbTimeslot> tsToTake, List<DbTimeslot> tsTaken)
    {
        var colliding = tsToTake
            .Where(ts => tsTaken
                .Any(taken => (taken.StartTime >= ts.StartTime && taken.StartTime <= ts.EndTime)
                              || (taken.StartTime <= ts.StartTime && taken.EndTime >= ts.StartTime)))
            .ToList();
        return colliding.Any();
    }

    public void AddFreeTerm(DateTime startTime, DateTime endTime)
    {
        using var context = new OurDbContext(_connection);
        context.Add(new DbTimeslot()
        {
            StartTime = startTime,
            EndTime = endTime,
            IsFree = true
        });
    }


}
