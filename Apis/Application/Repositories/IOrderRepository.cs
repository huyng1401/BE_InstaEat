using Application.Commons;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Pagination<Order>> GetOrdersByDateRangeAsync(DateTime minDate, DateTime maxDate, int pageIndex = 0, int pageSize = 10);

    }
}
