using Application.Commons;
using Application.Interfaces;
using Application.Services;
using Application.ViewModels.TransactionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : BaseController
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<TransactionViewModel>>> GetTransactionsByDateRange([FromQuery] DateTime minDate, [FromQuery] DateTime maxDate, int pageIndex = 0, int pageSize = 10)
        {
            var transactions = await _transactionService.GetTransactionsByDateRangeAsync(minDate, maxDate, pageIndex, pageSize);
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionViewModel>> GetTransactionById(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return NotFound("Transaction not found.");
            }
            return Ok(transaction);
        }

        [HttpPost]
        public async Task<ActionResult<TransactionViewModel>> CreateTransaction([FromBody] CreateTransactionViewModel createTransactionViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var transaction = await _transactionService.CreateTransactionAsync(createTransactionViewModel);
            if (transaction == null)
            {
                return BadRequest("Unable to create transaction.");
            }
            return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.TransactionId }, transaction);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] UpdateTransactionViewModel updateTransactionViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isUpdated = await _transactionService.UpdateTransactionAsync(id, updateTransactionViewModel);
            if (!isUpdated)
            {
                return NotFound("Transaction not found.");
            }
            return Ok("Successfully Updated!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var isDeleted = await _transactionService.DeleteTransactionAsync(id);
            if (!isDeleted)
            {
                return NotFound("Transaction not found");
            }
            return Ok("Successfully Deleted!!!");
        }
    }
}
