using Database.Entities;
using Endpoints.Interfaces;
using Timetable.Interfaces;

namespace Services;

public class AddressService : IAddressService
{
    public AddressService(ITransactor transactor)
    {
        _transactor = transactor;
    }

    public AddressDto GetAddressById(string addressExternalId)
    {
        using var t = _transactor.BeginTransaction();
        var address = GetExistingAddress(int.Parse(addressExternalId), t);
        return new AddressDto()
        {
            ExternalId = address?.Id.ToString(),
            AddressName = address?.AddressName,
            AddressData = address?.AddressData,
        };
    }

    public void AddAddress(AddressDto address)
    {
        if(String.IsNullOrEmpty(address.AddressName)||String.IsNullOrEmpty(address.AddressData))
            throw new BadRequestException("All address details are required");
        using var t = _transactor.BeginTransaction();
        var addressToAdd = new DbAddress()
        {
            AddressName = address.AddressName!,
            AddressData = address.AddressData!
        };
        t.AddressDao.SaveAddress(addressToAdd);
        t.Commit();
    }

    public void DeleteAddress(string externalAddressId)
    {
        using var t = _transactor.BeginTransaction();
        if (GetStudentsByAddressId(int.Parse(externalAddressId), t).Any())
            throw new BadRequestException("Address is connected to student and cannot be removed!");
        t.AddressDao.DeleteAddress(int.Parse(externalAddressId));
        t.Commit();
    }

    public void UpdateAddress(string externalAddressId, AddressDto address)
    {
        var addressToUpdate = GetAddressById(externalAddressId);
        addressToUpdate.AddressName = address.AddressName ?? addressToUpdate.AddressName;
        addressToUpdate.AddressData = address.AddressData ?? addressToUpdate.AddressData;
        var convertedAddress = ConvertToDbAddress(addressToUpdate);
        using var t = _transactor.BeginTransaction();
        t.AddressDao.SaveAddress(convertedAddress);
        t.Commit();
    }

    private DbAddress ConvertToDbAddress(AddressDto address)
    {
        if(String.IsNullOrEmpty(address.ExternalId)||String.IsNullOrEmpty(address.AddressName)||String.IsNullOrEmpty(address.AddressData))
            throw new BadRequestException("All address details are required");
        return new DbAddress()
        {
            Id = int.Parse(address.ExternalId),
            AddressName = address.AddressName,
            AddressData = address.AddressData
        };
    }

    private static DbAddress GetExistingAddress(int addressId, ITransaction t)
    {
        return t.AddressDao.GetAddress(addressId) ??
               throw new BadRequestException("Address does not exist");
    }

    private List<DbStudent> GetStudentsByAddressId(int addressId, ITransaction t)
    {
        return t.AddressDao.GetStudents(addressId);
    }

    private readonly ITransactor _transactor;
}
