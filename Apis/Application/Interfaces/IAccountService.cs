using Application.ViewModels.AccountViewModels;
using Application.ViewModels.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAccountService
    {
        Task<List<AccountViewModel>> GetAccountsAsync();
        Task<AccountViewModel?> GetAccountByIdAsync(int accountId);
        Task<AccountViewModel?> Register (RegisterRequestModel register);
        Task<string?> LoginAsync(LoginRequestModel loginRequestModel, string secretKey);
        Task<bool> UpdateAccountAsync(int accountId, UpdateAccountViewModel account);
        Task<bool> DeleteAccountAsync(int accountId);
    }
}
