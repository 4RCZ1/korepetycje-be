using Endpoints.Interfaces.Authorization;

namespace Endpoints.Interfaces;

public interface IAddressService
{
    AddressDto GetAddressById(string addressId, TutorRole role);
    List<AddressDto> GetAddresses(TutorRole role);
    void AddAddress(AddressDto address, TutorRole role);
    void DeleteAddress(string externalAddressId, TutorRole role);
    void UpdateAddress(string externalAddressId, AddressDto address, TutorRole role);
}
