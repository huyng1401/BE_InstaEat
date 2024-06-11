using Application.ViewModels.RestaurantViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRestaurantService
    {
        Task<List<RestaurantViewModel>> GetRestaurantsAsync();
        Task<RestaurantViewModel?> GetRestaurantByIdAsync(int restaurantId);
        Task<RestaurantViewModel?> CreateRestaurantAsync(CreateRestaurantViewModel restaurant);
        Task<bool> UpdateRestaurantAsync(int restaurantId, UpdateRestaurantViewModel restaurant);
        Task<bool> DeleteRestaurantAsync(int restaurantId);
    }
}
