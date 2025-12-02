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
    
    public void SaveSingleStudent(DbStudent student, string singleGroupName)
    {
        var group = new DbStudentGroup
        {
            IsSingle = true,
            Name = singleGroupName
        };
    
        var membership = new DbStudentMembership
        {
            Student = student,
            Group = group
        };
        _context.StudentMemberships.Add(membership);
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
    
    public DbStudentGroup GetStudentSingleGroupByStudentId(int studentId)
    {
        var group = _context.StudentMemberships.Query()
            .Where(m => m.StudentId == studentId)
            .Select(m => m.Group)
            .FirstOrDefault(g => g != null && g.IsSingle);
        if(group == null)
            throw new ApplicationException("Single resource group not found");
        return group;
    }

    public void DeleteStudentGroup(DbStudentGroup studentGroup)
    {
        _context.StudentGroups.Remove(studentGroup);
    }
    
    public List<DbStudentGroup> GetAllStudentGroups()
    {
        return _context.StudentGroups.Query()
            .Include(g => g.Memberships)
            .ThenInclude(m => m.Student)
            .Where(g => !g.IsSingle).ToList();
    }
}
