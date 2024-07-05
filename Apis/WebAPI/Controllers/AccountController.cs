using Application.Commons;
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
        public async Task<ActionResult<Pagination<AccountViewModel>>> GetAccounts([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            var accounts = await _accountService.GetAccountsAsync(pageIndex, pageSize);
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountViewModel>> GetAccountById(int id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound("User not found.");
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
            try
            {
                var account = await _accountService.Register(register);
                if (account == null)
                {
                    return BadRequest("Unable to register user.");
                }
                return CreatedAtAction(nameof(GetAccountById), new { id = account.UserId }, account);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("register/restaurant")]
        public async Task<ActionResult<AccountViewModel>> RegisterAsRestaurant([FromBody] RegisterAsRestaurantRequestModel register)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = await _accountService.RegisterAsRestaurant(register);
                if (user == null)
                {
                    return BadRequest("Unable to register user as restaurant.");
                }
                return CreatedAtAction(nameof(GetAccountById), new { id = user.UserId }, user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
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

            return Ok(new { Token = token.Value.Token, UserId = token.Value.UserId });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount (int id, [FromBody] UpdateAccountViewModel updateAccountViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var isUpdated = await _accountService.UpdateAccountAsync(id, updateAccountViewModel);
                if (!isUpdated)
                {
                    return NotFound("Account not found.");
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
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
