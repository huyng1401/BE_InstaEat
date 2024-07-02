using Application.Commons;
using Application.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context) { }
        public async Task<Pagination<Order>> GetOrdersByDateRangeAsync(DateTime minDate, DateTime maxDate, int pageIndex = 0, int pageSize = 10)
        {
            return await PaginateFiltered(o => o.OrderDate >= minDate && o.OrderDate <= maxDate && o.IsDeleted == false, pageIndex, pageSize);
        }
    }
}
