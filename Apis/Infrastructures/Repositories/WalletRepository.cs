using Application.Repositories;
using Domain.Entities;
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
    }
}
