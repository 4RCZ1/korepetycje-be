using Database.Entities;
using Endpoints.Interfaces;

namespace Timetable.Interfaces;

public interface ILessonSuggestionDao
{
    void SaveLessonSuggestion(DbLessonSuggestion lessonSuggestion);
    void DeleteLessonSuggestion(int id);
    DbLessonSuggestion GetLessSuggById(int id);
    List<DbLessonSuggestion>GetLessSugg(DateTimeOffset startOffset, DateTimeOffset endOffset, int? studentId);
}