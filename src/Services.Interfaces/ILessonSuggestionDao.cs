using Database.Entities;

namespace Services.Interfaces;

public interface ILessonSuggestionDao
{
    void SaveLessonSuggestion(DbLessonSuggestion lessonSuggestion);
    void DeleteLessonSuggestion(int id);
    void DeleteSuggestionPreservingTimeslot(int id);
    DbLessonSuggestion GetLessSuggById(int id);
    List<DbLessonSuggestion> GetLessSugg(
        DateTimeOffset start, DateTimeOffset end, int? studentId);
}
