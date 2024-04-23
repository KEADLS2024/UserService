using UserService.Models;
using UserService.Interfaces;

namespace UserService.Managers
{
    public class AddressManager
    {
        private readonly IAddressManager _addressesRepository;

        public AddressManager(IAddressManager addressesRepository)
        {
            _addressesRepository = addressesRepository;
        }

        public async Task<IEnumerable<Address>> GetAddressesAsync()
        {
            return await _addressesRepository.GetAllAddressesAsync();
        }

        public async Task<Address> GetAddressByIdAsync(int id)
        {
            return await _addressesRepository.GetAddressByIdAsync(id);
        }

        public async Task<bool> UpdateAddressAsync(Address address)
        {
            var existingAddress = await _addressesRepository.GetAddressByIdAsync(address.AddressId);
            if (existingAddress == null)
            {
                return false;
            }

            await _addressesRepository.UpdateAddressAsync(address);
            return true;
        }

        public async Task<Address> CreateAddressAsync(Address address)
        {
            await _addressesRepository.AddAddressAsync(address);
            // Assuming the repository method does the SaveChangesAsync and returns the added entity
            return address;
        }

        public async Task<bool> DeleteAddressAsync(int id)
        {
            var address = await _addressesRepository.GetAddressByIdAsync(id);
            if (address == null)
            {
                return false;
            }

            await _addressesRepository.DeleteAddressAsync(address);
            return true;
        }
    }
}
