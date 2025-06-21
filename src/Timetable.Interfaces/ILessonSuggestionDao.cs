using Database.Entities;

namespace Timetable.Interfaces;

public interface ILessonSuggestionDao
{
    void SaveLessonSuggestion(DbLessonSuggestion lessonSuggestion);
    void DeleteLessonSuggestion(int id);
    DbLessonSuggestion GetLessSuggById(int id);
    List<DbLessonSuggestion> GetLessSugg(
        DateTimeOffset start, DateTimeOffset end, int? studentId);
}
