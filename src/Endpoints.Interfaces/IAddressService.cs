using Endpoints.Interfaces.Authorization;

namespace Endpoints.Interfaces;

public interface IAddressService
{
    AddressDto GetAddressByIdAsStudent(string addressId, StudentRole role);
    AddressDto GetAddressByIdAsTutor(string addressId, TutorRole role);
    void AddAddress(AddressDto address, TutorRole role);
    void DeleteAddress(string externalAddressId, TutorRole role);
    void UpdateAddress(string externalAddressId, AddressDto address, TutorRole role);
}
