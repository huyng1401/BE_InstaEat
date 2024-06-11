using AutoMapper;
using Application.Commons;
using Domain.Entities;
using Application.ViewModels.PackageViewModels;
using Application.ViewModels.CustomerViewModels;
using Application.ViewModels.RestaurantViewModels;
using Application.ViewModels.OrderViewModels;
using Application.ViewModels.RoleViewModels;
using Application.ViewModels.ReviewViewModels;
using Application.ViewModels.AccountViewModels;
using Application.ViewModels.RequestModels;

namespace Infrastructures.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            // Package mappings
            CreateMap<CreatePackageViewModel, Package>();
            CreateMap<Package, PackageViewModel>().ForMember(dest => dest.PackageId, src => src.MapFrom(x => x.PackageId));
            CreateMap<UpdatePackageViewModel, Package>();

            // Customer mappings
            CreateMap<CreateCustomerViewModel, Customer>();
            CreateMap<Customer, CustomerViewModel>().ForMember(dest => dest.CustomerId, src => src.MapFrom(x => x.CustomerId));
            CreateMap<UpdateCustomerViewModel, Customer>();

            // Restaurant mappings
            CreateMap<CreateRestaurantViewModel, Restaurant>();
            CreateMap<Restaurant, RestaurantViewModel>().ForMember(dest => dest.RestaurantId, src => src.MapFrom(x => x.RestaurantId));
            CreateMap<UpdateRestaurantViewModel, Restaurant>();

            // Order mappings
            CreateMap<CreateOrderViewModel, Order>();
            CreateMap<Order, OrderViewModel>();
            CreateMap<UpdateOrderViewModel, Order>();

            // Review mappings
            CreateMap<CreateReviewViewModel, Review>();
            CreateMap<Review, ReviewViewModel>().ForMember(dest => dest.ReviewId, src => src.MapFrom(x => x.ReviewId));
            CreateMap<UpdateReviewViewModel, Review>();

            // Role mappings
            CreateMap<CreateRoleViewModel, Role>();
            CreateMap<Role, RoleViewModel>().ForMember(dest => dest.RoleId, src => src.MapFrom(x => x.RoleId));
            CreateMap<UpdateRoleViewModel, Role>();

            //Account mappings
            CreateMap<Account, AccountViewModel>();
            CreateMap<UpdateAccountViewModel, Account>();
            CreateMap<RegisterRequestModel, Account>();

            // Pagination mapping (Chỉ định nghĩa một lần duy nhất)
            CreateMap(typeof(Pagination<>), typeof(Pagination<>));
        }
    }
}
