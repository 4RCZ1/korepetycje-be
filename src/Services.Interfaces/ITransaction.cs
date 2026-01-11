namespace Services.Interfaces;

public interface ITransaction : IDisposable
{
    public ILessonDao LessonDao { get; }
    public IStudentDao StudentDao { get; }
    public IAddressDao AddressDao { get; }
    public ILessonSuggestionDao LessonSuggestionDao { get; }
    public ITutorDao TutorDao { get; }
    public IResourceDao ResourceDao { get; }
    public IResourceStudentsDao ResourceStudentsDao { get; }
    public void Commit();
}
