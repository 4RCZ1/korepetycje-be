namespace Timetable.Interfaces;

public interface ITransaction : IDisposable
{
    public ILessonDao LessonDao { get; }
    public IStudentDao StudentDao { get; }
    public void Commit();
}
