using Application.Commons;
using Application.ViewModels.WalletViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IWalletService
    {
        Task<Pagination<WalletViewModel>> GetWalletsAsync(int pageIndex = 0, int pageSize = 10);
        Task<WalletViewModel?> GetWalletByIdAsync(int walletId);
        Task<WalletViewModel?> CreateWalletAsync(CreateWalletViewModel wallet);
        Task<bool> UpdateWalletAsync(int walletId, UpdateWalletViewModel wallet);
        Task<bool> DeleteWalletAsync(int walletId);
    }
}
