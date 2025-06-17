using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Timetable.Interfaces;

namespace Database;

public class AddressDao : IAddressDao
{
    public AddressDao(OurDbContext context)
    {
        _context = context;
    }

    public DbAddress? GetAddress(int addressId)
    {
        return _context.Addresses.AsNoTracking().SingleOrDefault(x => x.Id == addressId);
    }
    
    public void SaveAddress(DbAddress address)
    {
        _context.Addresses.Update(address);
    }

    public void DeleteAddress(int addressId)
    {
        var addressToDelete = _context.Addresses.SingleOrDefault(x => x.Id == addressId);
        if (addressToDelete != null)
            _context.Addresses.Remove(addressToDelete);
    }

    public List<DbStudent?> GetStudents(int addressId)
    {
        return _context.Students.AsNoTracking().Where(x => x.AddressId == addressId).ToList();
    }
    
    private readonly OurDbContext _context;


}