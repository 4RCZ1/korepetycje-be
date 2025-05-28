namespace Database.Contract;

public interface ILessonDao
{
    public int CreateSchedule(
        int studentId,
        DateTime beginTime,
        TimeSpan period,
        int endOrdinal,
        TimeSpan lessonDuration);
}
