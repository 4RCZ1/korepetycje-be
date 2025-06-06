namespace Endpoints.Interfaces;

public interface ITimetableService
{
    IList<LessonDto> GetLessons(string startTime, string endTime);
    IList<LessonDto> GetStudentLessons(string studentExternalId, string startTime, string endTime);
    void PlanLessons(
        string startTime,
        string endDate,
        int periodInDays,
        string studentExternalId,
        int durationInMinutes);
    void ConfirmLesson(string lessonExternalId);
    void AddFreeTerm(string startTime, string endTime);
}
