namespace Timetable.Interfaces;

public interface IClock
{
    DateTimeOffset Now { get; }
}
