namespace Endpoints.Interfaces;

public interface ITimetableService
{
    IList<LessonDto> GetLessons(string startTime, string endTime);
    IList<LessonDto> GetStudentLessons(string studentExternalId, string startTime, string endTime);
    void PlanLessons(
        DateTime firstStart,
        DateTime firstEnd,
        int lessonCount,
        int periodInDays,
        IList<string> externalStudentIds);
    void EditLesson(
        string lessonExternalId,
        DateTime newStartTime,
        DateTime newEndTime,
        bool editFutureLessons);
    void ConfirmLesson(string lessonExternalId, string studentExternalId);
    void AddFreeTerm(string startTime, string endTime);
}
