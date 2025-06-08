using Timetable.Interfaces;

namespace Database;

public class Transactor : ITransactor
{
    public Transactor(string connection)
    {
        _connection = connection;
    }

    public ITransaction BeginTransaction()
    {
        return new OurDbContext(_connection);
    }

    private string _connection;
}
