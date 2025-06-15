using Database.Entities;

namespace Timetable.Interfaces;

public interface IAddressDao
{
    DbAddress? GetAddress(int addressId);
}