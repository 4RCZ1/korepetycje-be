using Database.Entities;
using Services.Interfaces;

namespace Database;

public class TenantContext : ITransaction
{
    public TenantContext(OurDbContext impl, int tenantId)
    {
        _impl = impl;

        Lessons = new TenantTable<DbLesson>(impl.Lessons, impl, tenantId);
        Students = new TenantTable<DbStudent>(impl.Students, impl, tenantId);
        Schedules = new TenantTable<DbSchedule>(impl.Schedules, impl, tenantId);
        Timeslots = new TenantTable<DbTimeslot>(impl.Timeslots, impl, tenantId);
        LessonSuggestions =
            new TenantTable<DbLessonSuggestion>(impl.LessonSuggestions, impl, tenantId);
        Attendances = new TenantTable<DbAttendance>(impl.Attendances, impl, tenantId);
        Addresses = new TenantTable<DbAddress>(impl.Addresses, impl, tenantId);
        Tutors = new TenantTable<DbTutor>(impl.Tutors, impl, tenantId);
        Resources = new TenantTable<DbResource>(impl.Resources, impl, tenantId);
        ResourceGroups = new TenantTable<DbResourceGroup>(impl.ResourceGroups, impl, tenantId);
        ResourceMemberships = new TenantTable<DbResourceMembership>(
            impl.ResourceMemberships, impl, tenantId);
        StudentGroups = new TenantTable<DbStudentGroup>(impl.StudentGroups, impl, tenantId);
        StudentMemberships = new TenantTable<DbStudentMembership>(
            impl.StudentMemberships, impl, tenantId);
        AccessPolicies = new TenantTable<DbAccessPolicy>(impl.AccessPolicies, impl, tenantId);

        LessonDao = new LessonDao(this);
        StudentDao = new StudentDao(this);
        AddressDao = new AddressDao(this);
        LessonSuggestionDao = new LessonSuggestionDao(this);
        TutorDao = new TutorDao(this);
        ResourceDao = new ResourceDao(this);
        ResourceStudentsDao = new ResourceStudentsDao(this);
    }

    public void Dispose()
    {
        _impl.Dispose();
    }

    public void Commit()
    {
        _impl.SaveChanges();
    }

    public ITable<DbLesson> Lessons { get; }
    public ITable<DbStudent> Students { get; }
    public ITable<DbSchedule> Schedules { get; }
    public ITable<DbTimeslot> Timeslots { get; }
    public ITable<DbLessonSuggestion> LessonSuggestions { get; }
    public ITable<DbAttendance> Attendances { get; }
    public ITable<DbAddress> Addresses { get; }
    public ITable<DbTutor> Tutors { get; }
    public ITable<DbResource> Resources { get; }
    public ITable<DbResourceGroup> ResourceGroups { get; }
    public ITable<DbResourceMembership> ResourceMemberships { get; }
    public ITable<DbStudentGroup> StudentGroups { get; }
    public ITable<DbStudentMembership> StudentMemberships { get; }
    public ITable<DbAccessPolicy> AccessPolicies { get; }

    public ILessonDao LessonDao { get; }
    public IStudentDao StudentDao { get; }
    public IAddressDao AddressDao { get; }
    public ILessonSuggestionDao LessonSuggestionDao { get; }
    public ITutorDao TutorDao { get; }
    public IResourceDao ResourceDao { get; }
    public IResourceStudentsDao ResourceStudentsDao { get; }

    private readonly OurDbContext _impl;
}
