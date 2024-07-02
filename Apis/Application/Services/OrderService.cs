using Application.Commons;
using Application.Interfaces;
using Application.ViewModels.OrderViewModels;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Pagination<OrderViewModel>> GetOrdersByDateRangeAsync(DateTime minDate, DateTime maxDate, int pageIndex = 0, int pageSize = 10)
        {
            var ordersPagination = await _unitOfWork.OrderRepository.GetOrdersByDateRangeAsync(minDate, maxDate, pageIndex, pageSize);
            var ordersViewModel = _mapper.Map<Pagination<OrderViewModel>>(ordersPagination);
            return ordersViewModel;
        }
        public async Task<List<OrderViewModel>> GetOrdersAsync()
        {
            var orders = await _unitOfWork.OrderRepository.GetAllNotDeletedAsync();
            return _mapper.Map<List<OrderViewModel>>(orders);
        }

        public async Task<OrderViewModel?> GetOrderByIdAsync(int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
            if (order != null && order.IsDeleted == false)
            {
                return _mapper.Map<OrderViewModel>(order);
            }
            return null;
        }

        public async Task<OrderViewModel?> CreateOrderAsync(CreateOrderViewModel order)
        {
            if (order.RestaurantId.HasValue)
            {
                var restaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(order.RestaurantId.Value);
                if (restaurant == null || restaurant.IsDeleted == true)
                {
                    throw new ArgumentException("Restaurant ID does not exist or has been deleted.");
                }
            }

            if (order.PackageId.HasValue)
            {
                var package = await _unitOfWork.PackageRepository.GetByIdAsync(order.PackageId.Value);
                if (package == null || package.IsDeleted == true)
                {
                    throw new ArgumentException("Package ID does not exist or has been deleted.");
                }
            }

            var orderEntity = _mapper.Map<Order>(order);
            orderEntity.OrderDate = DateTime.Now;
            await _unitOfWork.OrderRepository.AddAsync(orderEntity);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<OrderViewModel>(orderEntity);
            }
            return null;
        }
        public async Task<bool> AddPointsToRestaurantWalletAsync(int restaurantId, int packageId)
        {
            var restaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(restaurantId);
            if (restaurant == null || restaurant.IsDeleted == true)
            {
                throw new ArgumentException("Restaurant not found or has been deleted.");

            }
            var user = await _unitOfWork.AccountRepository.GetByIdAsync(restaurant.UserId.Value);
            if (user == null || user.IsDeleted == true || user.RoleId != 3)
            {
                throw new ArgumentException("User not found, has been deleted, or is not a restaurant.");
            }
            var restaurantWallet = await _unitOfWork.WalletRepository.GetByIdAsync(restaurant.UserId.Value);
            if (restaurantWallet == null || restaurantWallet.IsDeleted == true)
            {
                throw new ArgumentException("Restaurant wallet not found.");
            }

            var package = await _unitOfWork.PackageRepository.GetByIdAsync(packageId);
            if (package == null || package.IsDeleted == true)
            {
                throw new ArgumentException("Package not found or has been deleted.");
            }

            var order = new Order
            {
                RestaurantId = restaurantId,
                PackageId = packageId,
                OrderDate = DateTime.Now,
                IsDeleted = false
            };
            await _unitOfWork.OrderRepository.AddAsync(order);
            await _unitOfWork.SaveChangeAsync();

            restaurantWallet.TotalPoint += package.Point.Value;
            _unitOfWork.WalletRepository.Update(restaurantWallet);

            return await _unitOfWork.SaveChangeAsync() > 0;
        }

        public async Task<bool> UpdateOrderAsync(int orderId, UpdateOrderViewModel order)
        {
            var existingOrder = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
            if (existingOrder == null || existingOrder.IsDeleted == true)
            {
                return false;
            }

            if (order.RestaurantId.HasValue)
            {
                var restaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(order.RestaurantId.Value);
                if (restaurant == null || restaurant.IsDeleted == true)
                {
                    throw new ArgumentException("Restaurant ID does not exist or has been deleted.");
                }
            }

            if (order.PackageId.HasValue)
            {
                var package = await _unitOfWork.PackageRepository.GetByIdAsync(order.PackageId.Value);
                if (package == null || package.IsDeleted == true)
                {
                    throw new ArgumentException("Package ID does not exist or has been deleted.");
                }
            }

            _mapper.Map(order, existingOrder);
            _unitOfWork.OrderRepository.Update(existingOrder);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
            if (order == null || order.IsDeleted == true)
            {
                return false;
            }

            order.IsDeleted = true;
            _unitOfWork.OrderRepository.Update(order);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }
    }
}
