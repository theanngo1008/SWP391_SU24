using JewelryProductionOrder.API.Middlewares;
using JewelryProductionOrder.BusinessLogic.Common;
using JewelryProductionOrder.BusinessLogic.RequestModels.Account;
using JewelryProductionOrder.BusinessLogic.Services.Implementation;
using JewelryProductionOrder.BusinessLogic.Services.Interface;
using JewelryProductionOrder.BusinessLogic.ViewModels;
using JewelryProductionOrder.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace JewelryProductionOrder.API.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {  
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }
        
        /*
        [HttpGet("GetAccounts")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            var accounts = await _service.GetAccountsAsync();
            return Ok(accounts);
        } */
        
        [HttpPost("Login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(EntityResponse<LoginViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login(LoginRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _service.Login(request);

            if (result == null)
            {
               if (result.Data.Status == 3)
               {
                   return StatusCode(403, "Your account has been banned and you are not allowed to log in");
               }
            }
            return Ok(result);
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(EntityResponse<AccountProfileViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register([FromForm] RegisterRequestModel request, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var account = await _service.Register(request, file);
            return Ok(account);
        }

        [HttpPut("UpdateAccount")]
        [Authorize]
        [ProducesResponseType(typeof(EntityResponse<AccountProfileViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateAccountRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _service.UpdateAccount(request);
            return Ok(result);
        }

        
        [HttpPost("Profile")]   
        public async Task<IActionResult> GetProfile()
        {
            var claims = User.Claims;
            foreach (var claim in claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (email == null)
            {
                return Unauthorized("Account is not authenticated!!!");
            }

            var account = await _service.GetAccountByEmail(email);
            if (account == null)
            {
                return NotFound("Account not found!!!");
            }

            return Ok(new
            {
                account.AccName,
                account.Email,
                account.NumberPhone,
                account.Deposit,
                account.Address,
                account.Role,
                account.CreateAt,
                account.LastLoginDate
            });
        }
        

    }
}
