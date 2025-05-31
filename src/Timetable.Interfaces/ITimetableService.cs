namespace Timetable.Interfaces;

public interface ITimetableService
{
    IList<LessonDto> GetLessons(DateOnly startDate, DateOnly endDate);
}
