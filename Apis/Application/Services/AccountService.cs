using Application.Commons;
using Application.Interfaces;
using Application.Utils;
using Application.ViewModels.AccountViewModels;
using Application.ViewModels.RequestModels;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AccountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Pagination<AccountViewModel>> GetAccountsAsync(int pageIndex = 0, int pageSize = 10)
        {
            var accounts = await _unitOfWork.AccountRepository.GetAllNotDeletedAsync();
            var paginatedAccounts = await ListPagination<AccountViewModel>.PaginateList(_mapper.Map<List<AccountViewModel>>(accounts), pageIndex, pageSize);
            return paginatedAccounts;
        }

        public async Task<AccountViewModel?> GetAccountByIdAsync(int accountId)
        {
            var account = await _unitOfWork.AccountRepository.GetByIdAsync(accountId);
            if (account != null && account.IsDeleted == false)
            {
                return _mapper.Map<AccountViewModel>(account);
            }
            return null;
        }

        public async Task<AccountViewModel?> Register(RegisterRequestModel register)
        {
            if (await _unitOfWork.AccountRepository.ExistsAsync(u => u.Username == register.Username))
            {
                throw new ArgumentException("Username already exists.");
            }

            if (!Regex.IsMatch(register.Phone, @"^\d{10}$") && !Regex.IsMatch(register.Phone, @"^\d{11}$"))
            {
                throw new ArgumentException("Phone number must be 10 or 11 digits.");
            }

            var role = await _unitOfWork.RoleRepository.GetByIdAsync(2);
            if (role == null)
            {
                throw new ArgumentException("Role not found.");
            }

            var userObj = _mapper.Map<User>(register);
            userObj.RoleId = role.RoleId;

            await _unitOfWork.AccountRepository.AddAsync(userObj);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                var wallet = new Wallet
                {
                    UserId = userObj.UserId,
                    TotalPoint = 0
                };
                await _unitOfWork.WalletRepository.AddAsync(wallet);
                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<AccountViewModel>(userObj);
            }
            return null;
        }
        public async Task<AccountViewModel?> RegisterAsRestaurant(RegisterAsRestaurantRequestModel register)
        {
            var existingUser = await _unitOfWork.AccountRepository.GetByIdAsync(register.UserId);
            if (existingUser == null || existingUser.IsDeleted == true)
            {
                throw new ArgumentException("User not found or has been deleted.");
            }

            if (existingUser.RoleId != 2)
            {
                throw new ArgumentException("User is not a customer.");
            }

            var role = await _unitOfWork.RoleRepository.GetByIdAsync(3);
            if (role == null)
            {
                throw new ArgumentException("Role not found.");
            }

            var restaurantObj = _mapper.Map<Restaurant>(register.Restaurant);
            restaurantObj.UserId = existingUser.UserId;

            await _unitOfWork.RestaurantRepository.AddAsync(restaurantObj);
            existingUser.RoleId = role.RoleId;
            _unitOfWork.AccountRepository.Update(existingUser);

            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<AccountViewModel>(existingUser);
            }
            return null;
        }

        public async Task<(string Token, AccountViewModel Profile)?> LoginAsync(LoginRequestModel loginRequestModel, string secretKey)
        {
            var accounts = await _unitOfWork.AccountRepository.GetAllNotDeletedAsync();
            var account = accounts.FirstOrDefault(a => a.Username == loginRequestModel.UserName && a.Password == loginRequestModel.Password);

            if (account == null)
            {
                return null;
            }

            var accessToken = account.GenerateJsonWebToken(secretKey, DateTime.Now);
            var profile = _mapper.Map<AccountViewModel>(account);
            return (accessToken, profile);
        }

        public async Task<bool> UpdateAccountAsync(int accountId, UpdateAccountViewModel account)
        {
            var existingAccount = await _unitOfWork.AccountRepository.GetByIdAsync(accountId);
            if (existingAccount == null || existingAccount.IsDeleted == true)
            {
                return false;
            }
            if (!Regex.IsMatch(account.Phone, @"^\d{10}$") && !Regex.IsMatch(account.Phone, @"^\d{11}$"))
            {
                throw new ArgumentException("Phone number must be 10 or 11 digits.");
            }

            _mapper.Map(account, existingAccount);
            _unitOfWork.AccountRepository.Update(existingAccount);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }

        public async Task<bool> DeleteAccountAsync(int accountId)
        {
            var account = await _unitOfWork.AccountRepository.GetByIdAsync(accountId);
            if (account == null || account.IsDeleted == true)
            {
                return false;
            }

            // Mark account as deleted
            account.IsDeleted = true;
            _unitOfWork.AccountRepository.Update(account);

            // Find and mark associated restaurant as deleted if it exists
            var restaurant = await _unitOfWork.RestaurantRepository.GetByUserIdAsync(account.UserId);
            if (restaurant != null)
            {
                restaurant.IsDeleted = true;
                _unitOfWork.RestaurantRepository.Update(restaurant);
            }

            // Find and mark associated wallet as deleted if it exists
            var wallet = await _unitOfWork.WalletRepository.GetByUserIdAsync(account.UserId);
            if (wallet != null)
            {
                wallet.IsDeleted = true;
                _unitOfWork.WalletRepository.Update(wallet);
            }

            return await _unitOfWork.SaveChangeAsync() > 0;
        }
    }
}
