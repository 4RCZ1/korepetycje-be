namespace Endpoints.Interfaces;

public interface ITimetableService
{
    IList<LessonDto> GetLessons(string startDate, string endDate);
    void PlanLessons(
        string startTime,
        string endDate,
        int periodInDays,
        string studentExternalId,
        int durationInMinutes);
    void ConfirmLesson(string lessonExternalId);
}
