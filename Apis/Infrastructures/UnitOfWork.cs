using Application;
using Application.Repositories;

namespace Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IPackageRepository _packageRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;

        public UnitOfWork(AppDbContext dbContext,
            IPackageRepository packageRepository,
            IRestaurantRepository restaurantRepository,
            IOrderRepository orderRepository,
            IRoleRepository roleRepository,
            IReviewRepository reviewRepository,
            IAccountRepository accountRepository,
            IWalletRepository walletRepository,
            ITransactionRepository transactionRepository)
        {
            _dbContext = dbContext;
            _packageRepository = packageRepository;
            _restaurantRepository = restaurantRepository;
            _orderRepository = orderRepository;
            _roleRepository = roleRepository;
            _reviewRepository = reviewRepository;
            _accountRepository = accountRepository;
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
        }


        public IPackageRepository PackageRepository => _packageRepository;
        public IRestaurantRepository RestaurantRepository => _restaurantRepository;
        public IOrderRepository OrderRepository => _orderRepository;
        public IRoleRepository RoleRepository => _roleRepository;
        public IReviewRepository ReviewRepository => _reviewRepository;
        public IAccountRepository AccountRepository => _accountRepository;
        public IWalletRepository WalletRepository => _walletRepository;
        public ITransactionRepository TransactionRepository => _transactionRepository;
        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
