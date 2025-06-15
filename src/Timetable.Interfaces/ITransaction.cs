namespace Timetable.Interfaces;

public interface ITransaction : IDisposable
{
    public ILessonDao LessonDao { get; }
    public IStudentDao StudentDao { get; }
    public IAddressDao AddressDao { get; }
    public void Commit();
}
