//using BE.Entities;
using BE.Entities;
using BE.Models;
using BE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore. Http;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BE.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    { 
        private readonly AccountService _service;

        public AccountController(AccountService service)
        {
            _service = service;
        }

        [HttpGet("GetAccounts")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            var accounts = await _service.GetAccountsAsync();
            return Ok(accounts);
        }
        
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginRequest request)
        {

            if (string.IsNullOrEmpty(request.email) || string.IsNullOrEmpty(request.password))
            {
                return BadRequest("Email and password are required!!!");
            }

                var (token, redirectUrl, account) = await _service.Login(request);

                if (token == null)
                {
                    if (account == null)
                    {
                        return Unauthorized(new {Message = "Incorrect Email or Password!!!"});
                    }
                    if (account.Status == 3)
                    {
                        return StatusCode(403, "Your account has been banned and you are not allowed to log in");
                    }
                }

            return Ok(new { 
                Message = "Login successful",
                Token = token,
                RedirectUrl = redirectUrl,
                Account = new
                {
                    account.AccName,
                    account.Email,
                    account.Password,
                    account.NumberPhone,
                    account.Deposit,
                    account.Address,
                    account.Role,
                }
            });
        }

        [HttpPost("Register")]        
        public async Task<IActionResult> Register([FromForm] RegisterRequest request, IFormFile file)
        {
            try
            {
                var account = await _service.Register(request, file);
                return Ok(new { Message = "Registration successful", Account = account });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("Profile")]    
        public async Task<IActionResult> GetProfile()
        {
            var claims = User.Claims;
            foreach (var claim in claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }

            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userEmail == null)
            {
                return Unauthorized("User is not authenticated!!!");
            }

            var account = await _service.GetAccountByEmail(userEmail);
            if (account == null)
            {
                return NotFound("User not found!!!");
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
