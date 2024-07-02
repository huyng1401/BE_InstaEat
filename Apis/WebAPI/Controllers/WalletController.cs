using Application.Commons;
using Application.Interfaces;
using Application.Services;
using Application.ViewModels.WalletViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalletController : BaseController
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<WalletViewModel>>> GetWallets([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            var wallets = await _walletService.GetWalletsAsync(pageIndex, pageSize);
            return Ok(wallets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WalletViewModel>> GetWalletById(int id)
        {
            var wallet = await _walletService.GetWalletByIdAsync(id);
            if (wallet == null)
            {
                return NotFound("Wallet not found.");
            }
            return Ok(wallet);
        }

        [HttpPost]
        public async Task<ActionResult<WalletViewModel>> CreateWallet([FromBody] CreateWalletViewModel createWalletViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var wallet = await _walletService.CreateWalletAsync(createWalletViewModel);
            if (wallet == null)
            {
                return BadRequest("Unable to create wallet.");
            }
            return CreatedAtAction(nameof(GetWalletById), new { id = wallet.WalletId }, wallet);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWallet(int id, [FromBody] UpdateWalletViewModel updateWalletViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isUpdated = await _walletService.UpdateWalletAsync(id, updateWalletViewModel);
            if (!isUpdated)
            {
                return NotFound("Wallet not found.");
            }
            return Ok("Successfully Updated!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWallet(int id)
        {
            var isDeleted = await _walletService.DeleteWalletAsync(id);
            if (!isDeleted)
            {
                return NotFound("Wallet not found");
            }
            return Ok("Successfully Deleted!!!");
        }
    }
}
