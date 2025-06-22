using Endpoints.Interfaces.Authorization;

namespace Endpoints.Interfaces;

public interface ITimetableService
{
    IList<LessonDto> GetLessons(string startTime, string endTime, TutorRole role);
    IList<LessonDto> GetStudentLessons(
        string studentExternalId, string startTime, string endTime, TutorRole role);
    IList<LessonDto> GetLessonsAsStudent(string startTime, string endTime, StudentRole role);
    void PlanLessons(
        DateTimeOffset firstStart,
        DateTimeOffset firstEnd,
        DateTimeOffset scheduleEnd,
        int periodInDays,
        string externalAddressId,
        IList<string> externalStudentIds,
        TutorRole role);
    void EditLesson(
        string lessonExternalId,
        DateTimeOffset newStartTime,
        DateTimeOffset newEndTime,
        bool editFutureLessons,
        TutorRole role);
    void AcceptSuggestion(string externalSuggestionId, bool accept, StudentRole role);
    void DeleteLesson(string externalLessonId, bool deleteFutureLessons, TutorRole role);
    void ConfirmLesson(bool confirmed, string lessonExternalId, StudentRole role);
    void EditLessonDetails(string externalLessonId, string newDescription, TutorRole role);
}
