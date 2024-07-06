using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class RestaurantRepository : GenericRepository<Restaurant>, IRestaurantRepository
    {
        public RestaurantRepository(AppDbContext context) : base(context)
        {

        }
        public async Task<Restaurant?> GetByUserIdAsync(int userId)
        {
            return await _dbSet.FirstOrDefaultAsync(r => r.UserId == userId && (r.IsDeleted == false || r.IsDeleted == null));
        }
    }

}
