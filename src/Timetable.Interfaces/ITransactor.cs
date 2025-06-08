namespace Timetable.Interfaces;

public interface ITransactor
{
    public ITransaction BeginTransaction();
}
