using Application.Interfaces;
using Application.ViewModels.CustomerViewModels;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<CustomerViewModel>> GetCustomersAsync()
        {
            var customers = await _unitOfWork.CustomerRepository.GetAllNotDeletedAsync();
            var result = _mapper.Map<List<CustomerViewModel>>(customers);
            return result;
        }

        public async Task<CustomerViewModel?> GetCustomerByIdAsync(int customerId)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(customerId);
            if (customer != null && customer.IsDeleted == false)
            {
                return _mapper.Map<CustomerViewModel>(customer);
            }
            return null;
        }

        public async Task<CustomerViewModel?> CreateCustomerAsync(CreateCustomerViewModel customer)
        {
            var customerObj = _mapper.Map<Customer>(customer);
            await _unitOfWork.CustomerRepository.AddAsync(customerObj);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<CustomerViewModel>(customerObj);
            }
            return null;
        }

        public async Task<bool> UpdateCustomerAsync(int customerId, UpdateCustomerViewModel customer)
        {
            var existingCustomer = await _unitOfWork.CustomerRepository.GetByIdAsync(customerId);
            if (existingCustomer == null || existingCustomer.IsDeleted == true)
            {
                return false;
            }

            _mapper.Map(customer, existingCustomer);
            _unitOfWork.CustomerRepository.Update(existingCustomer);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }

        public async Task<bool> DeleteCustomerAsync(int customerId)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(customerId);
            if (customer == null || customer.IsDeleted == true)
            {
                return false;
            }

            customer.IsDeleted = true;
            _unitOfWork.CustomerRepository.Update(customer);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }
    }
}
