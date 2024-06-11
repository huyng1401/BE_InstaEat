using Application;
using Application.Repositories;

namespace Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IPackageRepository _packageRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IAccountRepository _accountRepository;

        public UnitOfWork(AppDbContext dbContext,
            IPackageRepository packageRepository,
            ICustomerRepository customerRepository,
            IRestaurantRepository restaurantRepository,
            IOrderRepository orderRepository,
            IRoleRepository roleRepository,
            IReviewRepository reviewRepository,
            IAccountRepository accountRepository)
        {
            _dbContext = dbContext;
            _packageRepository = packageRepository;
            _customerRepository = customerRepository;
            _restaurantRepository = restaurantRepository;
            _orderRepository = orderRepository;
            _roleRepository = roleRepository;
            _reviewRepository = reviewRepository;
            _accountRepository = accountRepository;
        }


        public IPackageRepository PackageRepository => _packageRepository;
        public ICustomerRepository CustomerRepository => _customerRepository;
        public IRestaurantRepository RestaurantRepository => _restaurantRepository;
        public IOrderRepository OrderRepository => _orderRepository;
        public IRoleRepository RoleRepository => _roleRepository;
        public IReviewRepository ReviewRepository => _reviewRepository;
        public IAccountRepository AccountRepository => _accountRepository;
        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
