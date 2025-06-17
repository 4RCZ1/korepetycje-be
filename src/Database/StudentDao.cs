using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Timetable.Interfaces;

namespace Database;

public class StudentDao : IStudentDao
{
    private readonly OurDbContext _context;

    public StudentDao(OurDbContext context)
    {
        _context = context;
    }

    public DbStudent? GetStudent(int studentId)
    {
        return _context.Students
            .AsNoTracking()
            .Include(s => s.Address)
            .SingleOrDefault(s => s.Id == studentId);
    }

    public List<DbStudent> GetStudents(int? lessonId = null)
    {
        if (lessonId is null)
            return _context.Students
                .AsNoTracking()
                .Include(s => s.Address)
                .ToList();
        return _context.Students
            .AsNoTracking()
            .Include(s => s.Address)
            .Where(s => _context.Attendances
                .Where(a => a.LessonId == lessonId)
                .Select(a => a.StudentId)
                .Contains(s.Id))
            .ToList();
    }

    public void SaveStudent(DbStudent student)
    {
        _context.Students.Update(student);
    }

    public void DeleteStudent(int studentId)
    {
        var studentToDelete = _context.Students.SingleOrDefault(s => s.Id == studentId);
        if (studentToDelete is not null)
            _context.Students.Remove(studentToDelete);
    }
}
