using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Database;

internal class StudentDao : IStudentDao
{
    private readonly TenantContext _context;

    public StudentDao(TenantContext context)
    {
        _context = context;
    }

    public DbStudent? GetStudent(int studentId, bool includeDeleted = false)
    {
        return QueryStudents(includeDeleted, null)
            .SingleOrDefault(s => s.Id == studentId);
    }

    public List<DbStudent> GetStudents(int? lessonId = null, bool includeDeleted = false)
    {
        return QueryStudents(includeDeleted, lessonId).ToList();
    }

    private IQueryable<DbStudent> QueryStudents(bool includeDeleted, int? lessonId)
    {
        IQueryable<DbStudent> query = _context.Students
            .Query()
            .AsNoTracking()
            .Include(s => s.Address);
        if (includeDeleted)
            query = query.IgnoreQueryFilters();
        if (lessonId is not null)
        {
            query = query.Where(s => _context.Attendances
                .Query().Any(a => a.LessonId == lessonId && a.StudentId == s.Id));
        }
        return query;
    }

    public void SaveStudent(DbStudent student)
    {
        _context.Students.Update(student);
    }

    public void DeleteStudent(int studentId)
    {
        var studentToDelete = _context.Students.Query().SingleOrDefault(s => s.Id == studentId);
        if (studentToDelete is not null)
        {
            studentToDelete.Address = null;
            studentToDelete.AddressId = null;
            _context.Students.Remove(studentToDelete);
        }
    }
    
    public List<DbStudentGroup> GetAllStudentGroups()
    {
        return _context.StudentGroups.Query()
            .Include(g => g.Memberships)
            .ThenInclude(m => m.Student)
            .ThenInclude(s => s.Address)
            .Where(g => !g.IsSingle).ToList();
    }
}
