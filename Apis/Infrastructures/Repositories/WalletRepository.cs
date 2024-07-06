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
    internal class WalletRepository : GenericRepository<Wallet>, IWalletRepository
    {
        public WalletRepository(AppDbContext context) : base(context)
        {

        }
        public async Task<Wallet?> GetByUserIdAsync(int userId)
        {
            return await _dbSet.FirstOrDefaultAsync(w => w.UserId == userId && (w.IsDeleted == false || w.IsDeleted == null));
        }
    }
}
