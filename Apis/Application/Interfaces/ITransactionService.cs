using Application.Commons;
using Application.ViewModels.TransactionViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITransactionService
    {
        Task<List<TransactionViewModel>> GetTransactionsAsync();
        Task<TransactionViewModel?> GetTransactionByIdAsync(int transactionId);
        Task<TransactionViewModel?> CreateTransactionAsync(CreateTransactionViewModel transaction);
        Task<bool> UpdateTransactionAsync(int transactionId, UpdateTransactionViewModel transaction);
        Task<bool> DeleteTransactionAsync(int transactionId);
        Task<Pagination<TransactionViewModel>> GetTransactionsByDateRangeAsync(DateTime minDate, DateTime maxDate, int pageIndex = 0, int pageSize = 10);

    }
}
