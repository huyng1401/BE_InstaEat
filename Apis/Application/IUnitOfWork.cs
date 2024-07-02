using Application.Interfaces;
using Application.Repositories;

namespace Application
{
    public interface IUnitOfWork
    {
        public IPackageRepository PackageRepository { get; }
        public IRestaurantRepository RestaurantRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public IReviewRepository ReviewRepository { get; }
        public IAccountRepository AccountRepository { get; }
        public IWalletRepository WalletRepository { get; }
        public ITransactionRepository TransactionRepository { get; }
        public Task<int> SaveChangeAsync();
    }
}
