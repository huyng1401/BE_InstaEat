using Application.ViewModels.OrderViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderViewModel>> GetOrdersAsync();
        Task<OrderViewModel?> GetOrderByIdAsync(int orderId);
        Task<OrderViewModel?> CreateOrderAsync(CreateOrderViewModel order);
        Task<bool> UpdateOrderAsync(int orderId, UpdateOrderViewModel order);
        Task<bool> DeleteOrderAsync(int orderId);
    }
}
