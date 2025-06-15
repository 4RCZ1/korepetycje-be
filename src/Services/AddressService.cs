using Database.Entities;
using Endpoints.Interfaces;
using Timetable.Interfaces;
using Endpoints;


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
        var address = t.AddressDao.GetAddress(int.Parse(addressExternalId));
        if (address is null)
            throw new BadRequestException("Address does not exist");
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
        var addressToAdd =  new DbAddress()
        {
            AddressName = address?.AddressName ?? string.Empty,
            AddressData = address?.AddressData ?? string.Empty
        };
        t.AddressDao.SaveAddress(addressToAdd);
        t.Commit();
    }

    public void DeleteAddress(string externalAddressId)
    {
        using var t = _transactor.BeginTransaction();
        t.AddressDao.DeleteAddress(int.Parse(externalAddressId));
    }
    
    private readonly ITransactor _transactor;
}