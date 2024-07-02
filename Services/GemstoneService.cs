//using BE.Entities;
using BE.Entities;
using BE.Models;
using Firebase.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BE.Services
{
    public class GemstoneService
    { 
        private readonly JewelrySystemDbContext _context;

        public GemstoneService(JewelrySystemDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Gemstone>> GetGemstonesAsync()
        {
            return await _context.Gemstones.ToListAsync();
        }

        public async Task<Gemstone> GetGemstoneByIdAsync(int id)
        {
            return await _context.Gemstones.FindAsync(id);
        }

        public async Task<Gemstone> CreateGemstoneAsync(CreateGemstone createGemstone, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No file uploaded.");
            }

            var stream = file.OpenReadStream();
            var firebaseStorage = new FirebaseStorage("projectswp-7bb14.appspot.com");
            var fileName = Path.GetFileName(file.FileName);

            var task = firebaseStorage
                .Child("gemstoneImages")
                .Child(fileName)
                .PutAsync(stream);

            var downloadUrl = await task;

            var gemstone = new Gemstone
            {
                GemstoneName = createGemstone.GemstoneName,
                Image = downloadUrl,
                GemstoneCost = createGemstone.Cost,
                Status = createGemstone.Status
            };

            _context.Gemstones.Add(gemstone);
            await _context.SaveChangesAsync();

            return gemstone;

        } 
    }
}
