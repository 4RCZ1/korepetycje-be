namespace Endpoints.Interfaces;

public interface ITimetableService
{
    IList<LessonDto> GetLessons(string startTime, string endTime);
    IList<LessonDto> GetStudentLessons(string studentExternalId, string startTime, string endTime);
    void PlanLessons(
        string startTime,
        string endTime,
        int periodInDays,
        string studentExternalId,
        int durationInMinutes);
    void ConfirmLesson(string lessonExternalId, string studentExternalId);
    void AddFreeTerm(string startTime, string endTime);
}
