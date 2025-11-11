using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class TenantTable<TEntity> : ITable<TEntity> where TEntity : TenantEntity
{
    public TenantTable(DbSet<TEntity> impl, DbContext context, int tenantId)
    {
        _impl = impl;
        _context = context;
        _tenantId = tenantId;
    }

    public IQueryable<TEntity> Query()
    {
        return _impl.Where(e => e.TenantId == _tenantId);
    }

    public IEnumerable<TEntity> GetFreshlyDeleted()
    {
        return _context.ChangeTracker.Entries<TEntity>()
            .Where(entry => entry.State == EntityState.Deleted)
            .Where(entry => entry.Entity.TenantId == _tenantId)
            .Select(entry => entry.Entity);
    }

    public void Add(TEntity entity)
    {
        entity.SetTenantId(_tenantId);
        _impl.Add(entity);
    }

    public void Update(TEntity entity)
    {
        entity.SetTenantId(_tenantId);
        _impl.Update(entity);
    }

    public void Remove(TEntity entity)
    {
        _impl.Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> range)
    {
        _impl.RemoveRange(range);
    }

    private readonly DbSet<TEntity> _impl;
    private readonly DbContext _context;
    private readonly int _tenantId;
}
