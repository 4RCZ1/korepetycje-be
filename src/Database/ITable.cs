using Microsoft.EntityFrameworkCore.Query;

namespace Database;

public interface ITable<TEntity>
{
    IQueryable<TEntity> Query();
    IEnumerable<TEntity> GetFreshlyDeleted();
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> range);
}
