using BE.Entities;
using BE.Models;
using BE.Services;
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
        private readonly JewelrySystemDbContext _context;
        private readonly AccountService _service;

        public AccountController(JewelrySystemDbContext context, AccountService service)
        {
            _context = context;
            _service = service;
        }

        [HttpGet]
        [Route("GetAccounts")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }
        
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
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
                        return Unauthorized();
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
                    account.Role
                }
            });
        }

        [HttpPost("Register")]        
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            try
            {
                var account = await _service.Register(request);
                return Ok(new { Message = "Registration successful", Account = account });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userEmail = User.FindFirst(ClaimTypes.Name)?.Value;
            if (userEmail == null)
            {
                return Unauthorized("User is not authenticated!!!");
            }

            var account = await _service.GetAccountByEmail(userEmail);
            if (account == null)
            {
                return NotFound("User not found!!!");
            }

            return Ok(account);
        }


    }
}
