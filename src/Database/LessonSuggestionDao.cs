using Database.Entities;
using Endpoints.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Database;

internal class LessonSuggestionDao : ILessonSuggestionDao
{
    private readonly TenantContext _context;

    public LessonSuggestionDao(TenantContext context)
    {
        _context = context;
    }

    public void SaveLessonSuggestion(DbLessonSuggestion lessonSuggestion)
    {
        _context.LessonSuggestions.Update(lessonSuggestion);
    }

    public void DeleteLessonSuggestion(int id)
    {
        var suggestionToDelete = _context.LessonSuggestions
            .Query()
            .Include(s => s.Timeslot)
            .SingleOrDefault(s => s.Id == id);
        if (suggestionToDelete is not null)
        {
            _context.LessonSuggestions.Remove(suggestionToDelete);
            _context.Timeslots.Remove(suggestionToDelete.Timeslot!);
        }
    }

    public void DeleteSuggestionPreservingTimeslot(int id)
    {
        var suggestionToDelete = _context.LessonSuggestions
            .Query()
            .SingleOrDefault(s => s.Id == id);
        if (suggestionToDelete is not null)
            _context.LessonSuggestions.Remove(suggestionToDelete);
    }

    public List<DbLessonSuggestion> GetLessSugg(DateTimeOffset start, DateTimeOffset end, int? studentId)
    {
        var lessonSuggestions = _context.LessonSuggestions.Query()
            .Include(ls => ls.Student)
                .ThenInclude(s => s.Address)
            .Include(ls => ls.Lesson)
                .ThenInclude(l => l.Timeslot)
            .Include(ls => ls.Lesson)
                .ThenInclude(l => l.Attendances)
                    .ThenInclude(a => a.Student)
            .Include(ls => ls.Lesson)
                .ThenInclude(l => l.Schedule)
                    .ThenInclude(s => s.Address)
            .Include(ls => ls.Address)
            .Include(ls => ls.Timeslot)
            .Where(TimeslotDaoConditions.SuggestionOverlap(start, end));
        if(studentId.HasValue)
            lessonSuggestions = lessonSuggestions.Where(ls => ls.StudentId == studentId);
        return lessonSuggestions.ToList();
    }

    public DbLessonSuggestion GetLessSuggById(int id)
    {
        var lessSugg = _context.LessonSuggestions.Query()
            .Include(ls => ls.Student)
                .ThenInclude(s => s.Address)
            .Include(ls => ls.Address)
            .Include(ls => ls.Timeslot)
            .Include(ls => ls.Lesson)
                .ThenInclude(l => l.Attendances)
                    .ThenInclude(a => a.Student)
            .Include(ls => ls.Lesson)
                .ThenInclude(l => l.Timeslot)
            .Include(ls => ls.Lesson)
                .ThenInclude(l => l.Schedule)
                    .ThenInclude(s => s.Address)
            .SingleOrDefault(ls => ls.Id == id);
        if(lessSugg is null)
            throw new BadRequestException("Lesson suggestion not found");
        return lessSugg;
    }
}
