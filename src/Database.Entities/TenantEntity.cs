using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities;

public class TenantEntity
{
    [Required]
    [Column("tenant_id")]
    public int TenantId { get; set; }

    public void SetTenantId(int id)
    {
        SetTenantIdImpl(id, []);
    }

    private void SetTenantIdImpl(int id, HashSet<TenantEntity> visitedEntities)
    {
        if (visitedEntities.Contains(this))
            return;
        TenantId = id;
        visitedEntities.Add(this);
        foreach (var value in GetType().GetProperties().Select(p => p.GetValue(this)))
        {
            if (value is TenantEntity propertyEntity)
            {
                propertyEntity.SetTenantIdImpl(id, visitedEntities);
            }
            else if (value is ICollection collection)
            {
                foreach (var element in collection)
                {
                    if (element is TenantEntity elementEntity)
                        elementEntity.SetTenantIdImpl(id, visitedEntities);
                }
            }
        }
    }
}
