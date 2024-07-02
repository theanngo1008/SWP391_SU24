using BE.Entities;
using BE.Models;
using Microsoft.EntityFrameworkCore;


namespace BE.Services
{
    public class JewelryService
    {  
        private readonly JewelrySystemDbContext _context;

        public JewelryService(JewelrySystemDbContext context)
        {
            _context = context;
        }

        public async Task<Jewelry> GetJewelryById(int id)
        {
            return await _context.Jewelries.FindAsync(id);
        }

        public async Task<string> UpdateJewelry(int id, UpdateJewelryRequest request)
        {
            var jewelry = await _context.Jewelries.FindAsync(id);
            if (jewelry == null)
            {
                return "Jewelry not found!!!";
            }

            jewelry.JewelryName = request.JewelryName;
            jewelry.Cost = request.Cost;
            jewelry.Quantity = request.Quantity;
            jewelry.Status = request.Status;
            jewelry.SubCateId = request.SubCateId;

            if (request.Image != null)
            {
                using (var ms = new MemoryStream())
                {
                    await request.Image.CopyToAsync(ms);
                    jewelry.Image = ms.ToString();
                }
            }

            _context.Jewelries.Update(jewelry);
            await _context.SaveChangesAsync();

            return "Jewelry updated successfully";
        }

        public async Task<List<Jewelry>> SearchJewelryByName(string jewelryName)
        {
            if (string.IsNullOrWhiteSpace(jewelryName))
            {
                throw new ArgumentException("Jewelry name cannot be empty or whitespace", nameof(jewelryName));
            }

            return await _context.Jewelries
                .Where(j => j.JewelryName != null && j.JewelryName.Contains(jewelryName, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        } 
    }
}
