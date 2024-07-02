using BE.Entities;
using BE.Models;
using Firebase.Storage;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtSettings _jwtSettings;

        public AccountService(JewelrySystemDbContext context, IOptionsMonitor<JwtSettings> optionsMonitor, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _jwtSettings = optionsMonitor.CurrentValue;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Account>> GetAccountsAsync()
        {
            return await _context.Accounts.ToListAsync();
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
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Key); // Lấy khóa bí mật từ cấu hình
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Id", account.AccId.ToString()),
                    new Claim(ClaimTypes.Email, account.Email),
                    new Claim(ClaimTypes.Name, account.AccName),
                    new Claim(ClaimTypes.Role, account.Role),
                    new Claim("TokenId", Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
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

            _httpContextAccessor.HttpContext.Session.SetInt32("AccId", account.AccId);

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

        public async Task<Account> Register(RegisterRequest request, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No file uploaded.");
            }

            var stream = file.OpenReadStream();
            var firebaseStorage = new FirebaseStorage("projectswp-7bb14.appspot.com");
            var fileName = Path.GetFileName(file.FileName);

            var task = firebaseStorage
                .Child("accountImages")
                .Child(fileName)
                .PutAsync(stream);

            var downloadUrl = await task;

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
                Role = request.Role,
                Status = 1,
                Image = downloadUrl,
                CreateAt = DateTime.Now,
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
