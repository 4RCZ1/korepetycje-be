using HelloWorld.DTOs;

namespace Database.Contract;

public interface ILessonDao
{
    public int CreateSchedule(ScheduleDto schedule);
    Task<List<LessonDto>?> GetAllLessonsAsync();//nie wiem, czy chcesz nullable listę czy nullable lekcję
    Task<LessonDto?> GetLessonAsync(int scheduleId, int ordinal);
    Task<LessonDto?> UpdateLessonAsync(int scheduleId, int ordinal, LessonDto? lesson);
}
