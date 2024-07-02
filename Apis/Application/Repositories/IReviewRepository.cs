﻿using Application.Commons;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<Pagination<Review>> GetReviewsByStatusAsync(int status, int pageIndex = 0, int pageSize = 10);
    }
}
