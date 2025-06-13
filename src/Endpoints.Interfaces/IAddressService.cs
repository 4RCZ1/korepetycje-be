namespace Endpoints.Interfaces;

public interface IAddressService
{
    AddressDto GetAddressById(string addressId);
}