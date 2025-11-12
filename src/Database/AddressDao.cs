using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Database;

internal class AddressDao : IAddressDao
{
    public AddressDao(TenantContext context)
    {
        _context = context;
    }

    public DbAddress? GetAddress(int addressId)
    {
        return _context.Addresses.Query().AsNoTracking().SingleOrDefault(x => x.Id == addressId);
    }

    public List<DbAddress> GetAddresses()
    {
        return _context.Addresses.Query().AsNoTracking().ToList();
    }

    public void SaveAddress(DbAddress address)
    {
        _context.Addresses.Update(address);
    }

    public void DeleteAddress(int addressId)
    {
        var addressToDelete = _context.Addresses.Query().SingleOrDefault(x => x.Id == addressId);
        if (addressToDelete != null)
            _context.Addresses.Remove(addressToDelete);
    }

    public List<DbStudent> GetStudents(int addressId)
    {
        return _context.Students
            .Query()
            .AsNoTracking()
            .Where(x => x.AddressId == addressId)
            .ToList();
    }

    private readonly TenantContext _context;
}
