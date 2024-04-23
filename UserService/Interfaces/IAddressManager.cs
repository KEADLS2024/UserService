using UserService.Models;

namespace UserService.Interfaces
{
    public interface IAddressManager
    {
        Task<IEnumerable<Address>> GetAllAddressesAsync();
        Task<Address> GetAddressByIdAsync(int addressId);
        Task AddAddressAsync(Address address);
        Task UpdateAddressAsync(Address address);
        Task DeleteAddressAsync(Address address);
        Task<bool> AddressExistsAsync(int addressId);
    }
}
