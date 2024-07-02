using Application.Commons;
using Application.Interfaces;
using Application.Utils;
using Application.ViewModels.WalletViewModels;
using AutoMapper;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WalletService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Pagination<WalletViewModel>> GetWalletsAsync(int pageIndex = 0, int pageSize = 10)
        {
            var wallets = await _unitOfWork.WalletRepository.GetAllNotDeletedAsync();
            var paginatedWallets = await ListPagination<WalletViewModel>.PaginateList(_mapper.Map<List<WalletViewModel>>(wallets), pageIndex, pageSize);
            return paginatedWallets;
        }

        public async Task<WalletViewModel?> GetWalletByIdAsync(int walletId)
        {
            var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(walletId);
            if (wallet != null && wallet.IsDeleted == false)
            {
                return _mapper.Map<WalletViewModel>(wallet);
            }
            return null;
        }

        public async Task<WalletViewModel?> CreateWalletAsync(CreateWalletViewModel wallet)
        {
            var walletObj = _mapper.Map<Wallet>(wallet);
            await _unitOfWork.WalletRepository.AddAsync(walletObj);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<WalletViewModel>(walletObj);
            }
            return null;
        }

        public async Task<bool> UpdateWalletAsync(int walletId, UpdateWalletViewModel wallet)
        {
            var existingWallet = await _unitOfWork.WalletRepository.GetByIdAsync(walletId);
            if (existingWallet == null || existingWallet.IsDeleted == true)
            {
                return false;
            }

            _mapper.Map(wallet, existingWallet);
            _unitOfWork.WalletRepository.Update(existingWallet);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }

        public async Task<bool> DeleteWalletAsync(int walletId)
        {
            var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(walletId);
            if (wallet == null || wallet.IsDeleted == true)
            {
                return false;
            }

            wallet.IsDeleted = true;
            _unitOfWork.WalletRepository.Update(wallet);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }
    }
}
