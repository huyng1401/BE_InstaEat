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

        public async Task<List<AccountViewModel>> GetAccountsAsync()
        {
            var accounts = await _unitOfWork.AccountRepository.GetAllNotDeletedAsync();
            return _mapper.Map<List<AccountViewModel>>(accounts);
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
            var accountObj = _mapper.Map<Account>(register);
            accountObj.TotalPoint = 0;

            await _unitOfWork.AccountRepository.AddAsync(accountObj);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<AccountViewModel>(accountObj);
            }
            return null;
        }
        public async Task<string?> LoginAsync(LoginRequestModel loginRequestModel, string secretKey)
        {
            var accounts = await _unitOfWork.AccountRepository.GetAllNotDeletedAsync();
            var account = accounts.FirstOrDefault(a => a.Username == loginRequestModel.UserName && a.Password == loginRequestModel.Password);

            if (account == null)
            {
                return null;
            }

            var accessToken = account.GenerateJsonWebToken(secretKey, DateTime.Now);
            return accessToken;
        }

        public async Task<bool> UpdateAccountAsync(int accountId, UpdateAccountViewModel account)
        {
            var existingAccount = await _unitOfWork.AccountRepository.GetByIdAsync(accountId);
            if (existingAccount == null || existingAccount.IsDeleted == true)
            {
                return false;
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

            account.IsDeleted = true;
            _unitOfWork.AccountRepository.Update(account);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }
    }
}
