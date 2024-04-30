using Microsoft.AspNetCore.Mvc;
using UserService.Models;
using UserService.Managers;

namespace UserService.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerManager _customersManager;

        public CustomerController(CustomerManager customersManager)
        {
            _customersManager = customersManager;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers()
        {
            var customers = await _customersManager.GetAll();

            if (customers == null)
            {
                return NotFound();
            }

            return Ok(customers);
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _customersManager.Get(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(Customer customer)
        {

            try
            {
                await _customersManager.UpdateCustomerAsync( customer);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            var createdCustomer = await _customersManager.CreateCustomerAsync(customer);

            return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.CustomerId }, createdCustomer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                await _customersManager.DeleteCustomerAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }


    }
}
