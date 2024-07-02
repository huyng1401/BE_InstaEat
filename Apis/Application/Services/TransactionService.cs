using Application.Commons;
using Application.Interfaces;
using Application.ViewModels.TransactionViewModels;
using AutoMapper;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Pagination<TransactionViewModel>> GetTransactionsByDateRangeAsync(DateTime minDate, DateTime maxDate, int pageIndex = 0, int pageSize = 10)
        {
            var transactionsPagination = await _unitOfWork.TransactionRepository.GetTransactionsByDateRangeAsync(minDate, maxDate, pageIndex, pageSize);
            var transactionsViewModel = _mapper.Map<Pagination<TransactionViewModel>>(transactionsPagination);
            return transactionsViewModel;
        }
        public async Task<List<TransactionViewModel>> GetTransactionsAsync()
        {
            var transactions = await _unitOfWork.TransactionRepository.GetAllNotDeletedAsync();
            return _mapper.Map<List<TransactionViewModel>>(transactions);
        }

        public async Task<TransactionViewModel?> GetTransactionByIdAsync(int transactionId)
        {
            var transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(transactionId);
            if (transaction != null && transaction.IsDeleted == false)
            {
                return _mapper.Map<TransactionViewModel>(transaction);
            }
            return null;
        }

        public async Task<TransactionViewModel?> CreateTransactionAsync(CreateTransactionViewModel transaction)
        {
            var transactionObj = _mapper.Map<Transaction>(transaction);
            await _unitOfWork.TransactionRepository.AddAsync(transactionObj);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<TransactionViewModel>(transactionObj);
            }
            return null;
        }

        public async Task<bool> UpdateTransactionAsync(int transactionId, UpdateTransactionViewModel transaction)
        {
            var existingTransaction = await _unitOfWork.TransactionRepository.GetByIdAsync(transactionId);
            if (existingTransaction == null || existingTransaction.IsDeleted == true)
            {
                return false;
            }

            _mapper.Map(transaction, existingTransaction);
            _unitOfWork.TransactionRepository.Update(existingTransaction);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }

        public async Task<bool> DeleteTransactionAsync(int transactionId)
        {
            var transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(transactionId);
            if (transaction == null || transaction.IsDeleted == true)
            {
                return false;
            }

            transaction.IsDeleted = true;
            _unitOfWork.TransactionRepository.Update(transaction);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }
    }
}
