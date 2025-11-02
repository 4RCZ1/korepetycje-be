using Database.Entities;
using Timetable.Interfaces;

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

        LessonDao = new LessonDao(this);
        StudentDao = new StudentDao(this);
        AddressDao = new AddressDao(this);
        LessonSuggestionDao = new LessonSuggestionDao(this);
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

    public ILessonDao LessonDao { get; }
    public IStudentDao StudentDao { get; }
    public IAddressDao AddressDao { get; }
    public ILessonSuggestionDao LessonSuggestionDao { get; }

    private readonly OurDbContext _impl;
}
