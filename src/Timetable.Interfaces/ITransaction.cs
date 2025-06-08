namespace Timetable.Interfaces;

public interface ITransaction : IDisposable
{
    public ILessonDao LessonDao { get; }
    public void Commit();
}
