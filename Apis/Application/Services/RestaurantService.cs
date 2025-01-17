﻿using Application.Commons;
using Application.Interfaces;
using Application.Utils;
using Application.ViewModels.RestaurantViewModels;
using AutoMapper;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RestaurantService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Pagination<RestaurantViewModel>> GetRestaurantsAsync(int pageIndex = 0, int pageSize = 10)
        {
            var restaurants = await _unitOfWork.RestaurantRepository.GetAllNotDeletedAsync();
            var paginatedRestaurants = await ListPagination<RestaurantViewModel>.PaginateList(_mapper.Map<List<RestaurantViewModel>>(restaurants), pageIndex, pageSize);
            return paginatedRestaurants;
        }

        public async Task<RestaurantViewModel?> GetRestaurantByIdAsync(int restaurantId)
        {
            var restaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(restaurantId);
            if (restaurant != null && restaurant.IsDeleted == false)
            {
                return _mapper.Map<RestaurantViewModel>(restaurant);
            }
            return null;
        }

        public async Task<RestaurantViewModel?> CreateRestaurantAsync(CreateRestaurantViewModel restaurant)
        {
            var restaurantObj = _mapper.Map<Restaurant>(restaurant);
            await _unitOfWork.RestaurantRepository.AddAsync(restaurantObj);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<RestaurantViewModel>(restaurantObj);
            }
            return null;
        }

        public async Task<bool> UpdateRestaurantAsync(int restaurantId, UpdateRestaurantViewModel restaurant)
        {
            var existingRestaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(restaurantId);
            if (existingRestaurant == null || existingRestaurant.IsDeleted == true)
            {
                return false;
            }

            _mapper.Map(restaurant, existingRestaurant);
            _unitOfWork.RestaurantRepository.Update(existingRestaurant);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }

        public async Task<bool> DeleteRestaurantAsync(int restaurantId)
        {
            var restaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(restaurantId);
            if (restaurant == null || restaurant.IsDeleted == true)
            {
                return false;
            }

            restaurant.IsDeleted = true;
            _unitOfWork.RestaurantRepository.Update(restaurant);

            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                var userId = restaurant.UserId;
                if (userId.HasValue)
                {
                    var userRestaurants = await _unitOfWork.RestaurantRepository.GetAllNotDeletedAsync();
                    if (userRestaurants.All(r => r.UserId != userId))
                    {
                        var user = await _unitOfWork.AccountRepository.GetByIdAsync(userId.Value);
                        if (user != null)
                        {
                            user.RoleId = 2;
                            _unitOfWork.AccountRepository.Update(user);
                            await _unitOfWork.SaveChangeAsync();
                        }
                    }
                }
            }

            return isSuccess;
        }
    }
}
