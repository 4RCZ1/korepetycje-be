namespace Endpoints.Interfaces;

public interface ILessonSuggestionService
{
    string AddLessonSuggestion(LessonSuggestionDto lessonSuggestionToAdd);
    void DeleteLessonSuggestion(string externalId);
    void UpdateLessonSuggestion(string externalId, LessonSuggestionDto updatedLessonSuggestion);
    List<LessonSuggestionDto> GetLessonSuggestion(string? suggestedStart, string? suggestedEnd, string? studentExternalId);
}