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
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(AppDbContext context) : base(context)
        {

        }
        public async Task<Pagination<Review>> GetReviewsByStatusAsync(int status, int pageIndex = 0, int pageSize = 10)
        {
            return await PaginateFiltered(r => r.Status == status && r.IsDeleted == false, pageIndex, pageSize);
        }
    }
}
