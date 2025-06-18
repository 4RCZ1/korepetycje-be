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
        IList<string> externalStudentIds);
    void EditLesson(
        string lessonExternalId,
        DateTimeOffset newStartTime,
        DateTimeOffset newEndTime,
        bool editFutureLessons);
    void DeleteLesson(string externalLessonId, bool deleteFutureLessons);
    void ConfirmLesson(bool confirmed, string lessonExternalId, string studentExternalId);
}
