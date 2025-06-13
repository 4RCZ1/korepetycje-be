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
        var address = t.AddressDao.GetAddress(int.Parse(addressExternalId));
        if (address is null)
            throw new InvalidRequestException();
        return new AddressDto()
        {
            ExternalId = address?.Id.ToString(),
            AddressName = address?.AddressName,
            AddressData = address?.AddressData,
        };
    }
    private readonly ITransactor _transactor;
}