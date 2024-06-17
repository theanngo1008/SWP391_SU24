using BE.Entities;
using Microsoft.AspNetCore. Http;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IConfiguration _configuration;

        public AccountController(JewelrySystemDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAccounts")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }
        
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var email = request.email;
            var password = request.password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Email and password are required!!!");
            }
            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.Email == email);
            if (account == null)
            {
                return Unauthorized("Invalid email or password");
            }
            if (account.Password != password)
            {
                return Unauthorized("Invalid email or password");
            }
            if (account.Status == 3)
            {
                return StatusCode(403, "Your account has been banned and you are not allowed to log in");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]); // Lấy khóa bí mật từ cấu hình
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, account.Email),
            new Claim(ClaimTypes.Role, account.Role),
            new Claim("AccId", account.AccId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            string redirectUrl = account.Role switch
            {
                "MN" => "/manager/create-product",
                _ => "/home"
            };

            return Ok(new { 
                Message = "Login successful",
                Token = tokenString,
                RedirectUrl = redirectUrl,
                Account = new
                {
                    account.AccName,
                    account.Email,
                    account.Password,
                    account.NumberPhone,
                    account.Deposit,
                    account.Address
                }
            });
        }

        public class LoginRequest
        {
            public string email { get; set; }
            public string password { get; set; }
        }

        
    }
}
