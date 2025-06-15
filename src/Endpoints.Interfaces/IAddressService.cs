namespace Endpoints.Interfaces;

public interface IAddressService
{
    AddressDto GetAddressById(string addressId);
    void AddAddress(AddressDto address);
    void DeleteAddress(string externalAddressId);
}