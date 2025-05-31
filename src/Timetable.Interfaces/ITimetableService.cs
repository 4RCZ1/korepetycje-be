namespace Timetable.Interfaces;

public interface ITimetableService
{
    IList<LessonDto> GetLessons(string startDate, string endDate);
}
