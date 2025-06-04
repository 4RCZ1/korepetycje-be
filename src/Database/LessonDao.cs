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
        var starts = schedule.Lessons.Select(l => l.Timeslot.StartTime).ToList();
        var ends = schedule.Lessons.Select(l => l.Timeslot.EndTime).ToList();
        var isTermTaken = context.Lessons
            .Include(l => l.Schedule)
            .Include(l => l.Timeslot)
            .Any(l => (starts
                          .Any(s => s >= l.Timeslot.StartTime && s <= l.Timeslot.EndTime))
                      || (ends.Any(e => e >= l.Timeslot.StartTime) && starts.Any(s => s <= l.Timeslot.StartTime)));
        if (isTermTaken)
            throw new ApplicationException("There are colliding lessons!");
        
        context.Add(schedule);
        context.SaveChanges();
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
