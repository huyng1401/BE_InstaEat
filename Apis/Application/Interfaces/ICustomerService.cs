using Application.ViewModels.CustomerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerViewModel>> GetCustomersAsync();
        Task<CustomerViewModel?> GetCustomerByIdAsync(int customerId);
        Task<CustomerViewModel?> CreateCustomerAsync(CreateCustomerViewModel customer);
        Task<bool> UpdateCustomerAsync(int customerId, UpdateCustomerViewModel customer);
        Task<bool> DeleteCustomerAsync(int customerId);
    }
}
