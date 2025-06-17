using Database.Entities;

namespace Timetable.Interfaces;

public interface IAddressDao
{
    DbAddress? GetAddress(int addressId);
    void SaveAddress(DbAddress address);
    void DeleteAddress(int addressId);
    List<DbStudent?> GetStudents(int addressId);
}