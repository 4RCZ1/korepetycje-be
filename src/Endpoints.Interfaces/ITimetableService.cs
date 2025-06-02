namespace Endpoints.Interfaces;

public interface ITimetableService
{
    IList<LessonDto> GetLessons(string startDate, string endDate);
    void PlanLessons(
        string startTime,
        string endDate,
        int periodInDays,
        string studentUuid,
        int durationInMinutes);
    void ConfirmLesson(string lessonUuid);
}
