using Services.Interfaces;

namespace Database;

public class Transactor : ITransactor
{
    public Transactor(string connection, string externalTenantId)
    {
        _connection = connection;
        _tenantId = int.Parse(externalTenantId);
    }

    public ITransaction BeginTransaction()
    {
        return new TenantContext(new OurDbContext(_connection), _tenantId);
    }

    private readonly string _connection;
    private readonly int _tenantId;
}
