using UserService.Interfaces;
using UserService.Models;

namespace UserService.Managers
{
    public class CustomerManager
    {
        private readonly ICustomerManager _customersRepository;

        public CustomerManager(ICustomerManager customersRepository)
        {
            _customersRepository = customersRepository;
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await _customersRepository.GetAllCustomersAsync();
        }

        public async Task<Customer> Get(int id)
        {
            return await _customersRepository.GetCustomerByIdAsync(id);
        }

        public async Task Update(int id, Customer customer)
        {
            await _customersRepository.UpdateCustomerAsync(customer);
        }

        public async Task<Customer> Create(Customer customer)
        {
            await _customersRepository.AddCustomerAsync(customer);
            return customer; // Assuming the repository handles SaveChangesAsync and returns the added entity
        }

        public async Task Delete(int id)
        {
            var customer = await _customersRepository.GetCustomerByIdAsync(id);
            if (customer != null)
            {
                await _customersRepository.DeleteCustomerAsync(customer.CustomerId); // Use the correct property name
            }
        }
    }
}
