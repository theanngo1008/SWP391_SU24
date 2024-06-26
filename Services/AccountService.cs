using BE.Entities;
using BE.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BE.Services
{
    public class AccountService 
    {
        private readonly JewelrySystemDbContext _context;
        private readonly IConfiguration _configuration;

        public AccountService (JewelrySystemDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<(string Token, string RedirectUrl, Account Account)> Login(LoginRequest request)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.Email == request.email);
            if (account == null || account.Password != request.password || account.Status == 3)
            {
                return (null, null, null);
            }
            
            account.LastLoginDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]); // Lấy khóa bí mật từ cấu hình
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, account.Email),
                    new Claim(ClaimTypes.Role, account.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            string redirectUrl = account.Role switch
            {
                "MN" => "/users/jewelry/createJewelry",
                "AD" => "/admin",
                _ => "/home"
            };

            return (tokenString, redirectUrl, account);
        }

        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            return await _context.Accounts
                .Where(a => a.Status == 1 || a.Status == 2)
                .ToListAsync();
        }

        public async Task<Account> GetAccountByEmail(string email)
        {
            return await _context.Accounts.SingleOrDefaultAsync(a => a.Email == email);
        }

        public async Task<Account> GetAccountById(int id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<IEnumerable<Account>> SearchAccounts(string accName)
        {
            return await _context.Accounts
                .Where(a => a.AccName.Contains(accName))
                .ToListAsync();
        }

        public async Task<Account> CreateAccount(Account acc)
        {
            _context.Accounts.Add(acc);
            await _context.SaveChangesAsync();
            return acc;
        }

        public async Task<bool> UpdateAccount(int id, Account acc)
        {
            if (id != acc.AccId)
            {
                return false;
            }

            _context.Entry(acc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id)) return false; 
                else throw; 
            }
            return true;
        }

        public async Task<Account> Register(RegisterRequest request)
        {
            if (await _context.Accounts.AnyAsync(a => a.Email == request.Email))
            {
                throw new Exception("Email is already in use!!!");
            }

            var account = new Account
            {
                Email = request.Email,
                AccName = request.FullName,
                NumberPhone = request.NumberPhone,
                Password = request.Password,
                Address = request.Address,
                Role = "US",
                Status = 1
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return account;
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(a => a.AccId == id);
        }
    }
}
