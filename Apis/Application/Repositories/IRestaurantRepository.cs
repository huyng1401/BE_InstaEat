using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IRestaurantRepository : IGenericRepository<Restaurant>
    {
        Task<Restaurant?> GetByUserIdAsync(int userId);

    }
}
