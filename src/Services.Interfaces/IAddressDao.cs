using Database.Entities;

namespace Services.Interfaces;

public interface IAddressDao
{
    DbAddress? GetAddress(int addressId);
    List<DbAddress> GetAddresses();
    void SaveAddress(DbAddress address);
    void DeleteAddress(int addressId);
    List<DbStudent> GetStudents(int addressId);
}
