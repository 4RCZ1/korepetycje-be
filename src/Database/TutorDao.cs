using Database.Entities;
using Services.Interfaces;

namespace Database;

internal class TutorDao : ITutorDao
{
    public TutorDao(TenantContext context)
    {
        _context = context;
    }

    public DbTutor GetTutor()
    {
        return _context.Tutors.Query().Single();
    }

    private readonly TenantContext _context;
}
