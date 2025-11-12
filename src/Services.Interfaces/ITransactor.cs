namespace Services.Interfaces;

public interface ITransactor
{
    public ITransaction BeginTransaction();
}
