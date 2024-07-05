using Application.Commons;
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
        Task<Pagination<AccountViewModel>> GetAccountsAsync(int pageIndex = 0, int pageSize = 10);
        Task<AccountViewModel?> GetAccountByIdAsync(int accountId);
        Task<AccountViewModel?> Register (RegisterRequestModel register);
        Task<AccountViewModel?> RegisterAsRestaurant(RegisterAsRestaurantRequestModel register);
        Task<(string Token, AccountViewModel Profile)?> LoginAsync(LoginRequestModel loginRequestModel, string secretKey);
        Task<bool> UpdateAccountAsync(int accountId, UpdateAccountViewModel account);
        Task<bool> DeleteAccountAsync(int accountId);
    }
}
