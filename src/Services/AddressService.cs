using Database.Entities;
using Endpoints.Dto;
using Endpoints.Interfaces;
using Endpoints.Interfaces.Authorization;
using Services.Interfaces;

namespace Services;

public class AddressService : IAddressService
{
    public AddressService(ITransactor transactor)
    {
        _transactor = transactor;
    }

    public AddressDto GetAddressById(string addressExternalId, TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        var address = GetExistingAddress(int.Parse(addressExternalId), t);
        return new AddressDto
        {
            ExternalId = address.Id.ToString(),
            AddressName = address.AddressName,
            AddressData = address.AddressData,
        };
    }

    public List<AddressDto> GetAddresses(TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        var addresses = t.AddressDao.GetAddresses();
        var addressesDto = new List<AddressDto>();
        foreach (var address in addresses)
            addressesDto.Add(new AddressDto()
            {
                ExternalId = address?.Id.ToString(),
                AddressName = address?.AddressName,
                AddressData = address?.AddressData
            });
        return addressesDto;
    }

    public void AddAddress(AddressDto address, TutorRole role)
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

    public void DeleteAddress(string externalAddressId, TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        if (GetStudentsByAddressId(int.Parse(externalAddressId), t).Any(s => s.IsDeleted==false))
            throw new BadRequestException("Address is connected to student and cannot be removed!");
        t.AddressDao.DeleteAddress(int.Parse(externalAddressId));
        t.Commit();
    }

    public void UpdateAddress(string externalAddressId, AddressDto address, TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        var addressToUpdate = GetExistingAddress(int.Parse(externalAddressId), t);
        addressToUpdate.AddressName = address.AddressName ?? addressToUpdate.AddressName;
        addressToUpdate.AddressData = address.AddressData ?? addressToUpdate.AddressData;
        t.AddressDao.SaveAddress(addressToUpdate);
        t.Commit();
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
