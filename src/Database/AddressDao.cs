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
    
    private readonly OurDbContext _context;
}