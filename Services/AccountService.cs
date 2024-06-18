using BE.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BE.Services
{
    public class AccountService
    {
        private readonly JewelrySystemDbContext _context;

        public AccountService (JewelrySystemDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            return await _context.Accounts
                .Where(a => a.Status == 1 || a.Status == 2)
                .ToListAsync();
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

        

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(a => a.AccId == id);
        }
    }
}
