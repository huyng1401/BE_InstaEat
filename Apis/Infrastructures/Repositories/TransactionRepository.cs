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
    internal class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(AppDbContext context) : base(context)
        {

        }
        public async Task<Pagination<Transaction>> GetTransactionsByDateRangeAsync(DateTime minDate, DateTime maxDate, int pageIndex = 0, int pageSize = 10)
        {
            return await PaginateFiltered(o => o.Created >= minDate && o.Created <= maxDate && o.IsDeleted == false, pageIndex, pageSize);
        }
    }
}
