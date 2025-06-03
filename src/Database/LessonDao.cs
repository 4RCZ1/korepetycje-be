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

    public IList<DbLesson> GetLessonsInRange(DateOnly startDate, DateOnly endDate)
    {
        using var context = new OurDbContext(_connection);
        var lessons = context.Lessons
            .AsNoTracking()    
            .Include(l => l.Schedule)    
            .Where(l=>DateOnly.FromDateTime(l.StartTime.Date)>=startDate
                      &&DateOnly.FromDateTime((l.StartTime+(l.CustomDuration ?? l.Schedule!.LessonDuration)).Date)<=endDate)
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
        var starts = schedule.Lessons.Select(l => l.StartTime).ToList();
        var ends = schedule.Lessons.Select(l => (l.StartTime+schedule.LessonDuration)).ToList();
        var isTermTaken = context.Lessons
            .Include(l => l.Schedule)
            .Any(l => (starts
                .Any(s => s >= l.StartTime &&
                            s <= l.StartTime + (l.CustomDuration ?? l.Schedule!.LessonDuration)))
            || (ends.Any(e => e<=l.StartTime + (l.CustomDuration ?? l.Schedule!.LessonDuration)
            && e >= l.StartTime)));
        //rozkmiń proszę, czy na pewno ma to  i pokrywa corner casy, ale chyba jest git
        if (isTermTaken)
            throw new ApplicationException("There are colliding lessons!");
        
        context.Add(schedule);
        context.SaveChanges();
    }

    private readonly string _connection;
}
