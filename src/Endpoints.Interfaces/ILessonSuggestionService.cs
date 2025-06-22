using Endpoints.Interfaces.Authorization;

namespace Endpoints.Interfaces;

public interface ILessonSuggestionService
{
    string AddLessonSuggestion(LessonSuggestionDto lessonSuggestionToAdd, TutorRole role);
    void DeleteLessonSuggestion(string externalId, TutorRole role);
    void UpdateLessonSuggestion(
        string externalId, LessonSuggestionDto updatedLessonSuggestion, TutorRole role);
    List<LessonSuggestionDto> GetLessonSuggestion(
        string? suggestedStart, string? suggestedEnd, string? studentExternalId, TutorRole role);
    List<LessonSuggestionDto> GetLessSuggsAsStudent(
        string? suggestedStart, string? suggestedEnd, StudentRole role);
}
