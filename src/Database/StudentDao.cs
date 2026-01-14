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
        return _context.StudentGroups.Query().SingleOrDefault(g => g.IsSingle
                   && g.Memberships.Any(m => m.StudentId == studentId))
               ?? throw new ApplicationException("Single student group not found");
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

    public DbStudentGroup? GetStudentGroupByGuid(Guid groupGuid)
    {
        return _context.StudentGroups.Query().SingleOrDefault(g => g.Guid == groupGuid);
    }

    public List<double> GetStudentMinutes(int studentId, DateTimeOffset startTime, DateTimeOffset endTime)
    {
        var studentAttendances = _context.Attendances.Query()
            .Include(a => a.Lesson)
                .ThenInclude(l => l.Timeslot)
            .Where(ia => ia.Lesson != null && ia.Lesson.Timeslot != null)
            .Where(a => a.StudentId == studentId && a.IsConfirmed == true
            && a.Lesson!.Timeslot!.StartTime >= startTime
            && a.Lesson.Timeslot.EndTime <= endTime).ToList();
        var studentLessonsIds = studentAttendances.Select(a => a.LessonId).ToList();
        var otherAttendances = _context.Attendances.Query()
            .Where(a => studentLessonsIds.Contains(a.LessonId)
                        && a.StudentId != studentId
                        && a.IsConfirmed == true).ToList();
        var groupLessons = otherAttendances.Select(a => a.LessonId).ToList();

        var individualAttendances = studentAttendances
            .Where(sa => !groupLessons.Contains(sa.LessonId)).ToList();
        var individualTimeslots = individualAttendances
            .Select(ia => ia.Lesson!.Timeslot!).ToList();
        double individualMinutes = individualTimeslots.Select(it => (it.EndTime - it.StartTime).TotalMinutes).Sum();

        var groupAttendances = studentAttendances
            .Where(sa => groupLessons.Contains(sa.LessonId)).ToList();
        var groupTimeslots = groupAttendances
            .Where(ga => ga.Lesson != null && ga.Lesson.Timeslot != null)
            .Select(ga => ga.Lesson!.Timeslot!).ToList();
        double groupMinutes = groupTimeslots.Select(gt => (gt.EndTime - gt.StartTime).TotalMinutes).Sum();

        return [individualMinutes, groupMinutes];
    }

    public DbStudentGroup GetStudentGroupAssignments(Guid studentGroupId)
    {
        return _context.StudentGroups.Query()
            .Include(g => g.Memberships)
                .ThenInclude(rm => rm.Student)
            .Include(g => g.AccessPolicies)
                .ThenInclude(ap => ap.ResourceGroup)
                    .ThenInclude(sg => sg.Memberships)
                        .ThenInclude(sm => sm.Resource)
            .SingleOrDefault(rg => rg.Guid == studentGroupId);
    }

    public void EmptyStudentGroup(int groupId)
    {
        _context.StudentMemberships.RemoveRange(
            _context.StudentMemberships.Query().Where(m => m.GroupId == groupId));
    }

    public void SaveStudentGroup(DbStudentGroup group)
    {
        _context.StudentGroups.Update(group);
    }
}
