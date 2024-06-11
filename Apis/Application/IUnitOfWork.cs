using Application.Interfaces;
using Application.Repositories;

namespace Application
{
    public interface IUnitOfWork
    {
        public IPackageRepository PackageRepository { get; }
        public ICustomerRepository CustomerRepository { get; }
        public IRestaurantRepository RestaurantRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public IReviewRepository ReviewRepository { get; }
        public IAccountRepository AccountRepository { get; }
        public Task<int> SaveChangeAsync();
    }
}
