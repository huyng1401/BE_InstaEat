using Application.Interfaces;
using Application.ViewModels.AccountViewModels;
using Application.ViewModels.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;

        public AccountController (IAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<List<AccountViewModel>>> GetAccounts()
        {
            var accounts = await _accountService.GetAccountsAsync();
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountViewModel>> GetAccountById(int id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound("Account not found.");
            }
            return Ok(account);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<AccountViewModel>> Register([FromBody] RegisterRequestModel register)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var account = await _accountService.Register(register);
            if (account == null)
            {
                return BadRequest("Unable to register account.");
            }
            return CreatedAtAction(nameof(GetAccountById), new { id = account.AccountId }, account);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel loginRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = await _accountService.LoginAsync(loginRequestModel, _configuration["JWTSecretKey"]);
            if (token == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(new { Token = token });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount (int id, [FromBody] UpdateAccountViewModel updateAccountViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isUpdated = await _accountService.UpdateAccountAsync(id, updateAccountViewModel);
            if (!isUpdated)
            {
                return NotFound("Account not found.");
            }
            return Ok("Successfully Updated!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount (int id)
        {
            var isDeleted = await _accountService.DeleteAccountAsync(id);
            if (!isDeleted)
            {
                return NotFound("Account not found");
            }
            return Ok("Successfully Deleted!!!");
        }
    }
}
