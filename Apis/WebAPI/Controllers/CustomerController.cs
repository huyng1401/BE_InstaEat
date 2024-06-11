using Application.Interfaces;
using Application.ViewModels.CustomerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebAPI.Controllers;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CustomerViewModel>>> GetCustomers()
        {
            var customers = await _customerService.GetCustomersAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerViewModel>> GetCustomerById(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerViewModel>> CreateCustomer([FromBody] CreateCustomerViewModel createCustomerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!IsPhoneValid(createCustomerViewModel.Phone))
            {
                return BadRequest("Phone number must be 10 or 11 digits.");
            }
            var customer = await _customerService.CreateCustomerAsync(createCustomerViewModel);
            if (customer == null)
            {
                return BadRequest("Unable to create package.");
            }
            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.CustomerId }, customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerViewModel updateCustomerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!IsPhoneValid(updateCustomerViewModel.Phone))
            {
                return BadRequest("Phone number must be 10 or 11 digits.");
            }
            var isUpdated = await _customerService.UpdateCustomerAsync(id, updateCustomerViewModel);
            if (!isUpdated)
            {
                return NotFound();
            }
            return Ok("Successfully Updated!!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var isDeleted = await _customerService.DeleteCustomerAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return Ok("Successfully Deleted!!");
        }
        private bool IsPhoneValid(string phone)
        {
            return Regex.IsMatch(phone, @"^\d{10}$") || Regex.IsMatch(phone, @"^\d{11}$");
        }
    }
}
