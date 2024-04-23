using UserService.Models;

namespace UserService.Interfaces
{
    public interface ICustomerManager
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(int customerId);
        Task AddCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(int customerId);
        Task<bool> Exists(int customerId);
    }
}
