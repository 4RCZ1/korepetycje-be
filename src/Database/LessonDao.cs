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
        return [];
    }

    public void ConfirmLesson(int lessonId)
    {
        using var context = new OurDbContext(_connection);
        context.Lessons.Where(l => l.Id == lessonId).SingleOrDefault().IsConfirmed = true;
        context.SaveChanges();
    }

    public void SaveSchedule(DbSchedule schedule)
    {
        using var context = new OurDbContext(_connection);
        context.Add(schedule);
        context.SaveChanges();
    }

    private readonly string _connection;
}
