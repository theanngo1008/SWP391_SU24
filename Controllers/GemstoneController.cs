using BE.Entities;
using BE.Models;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GemstoneController : ControllerBase
    {
        private readonly JewelrySystemDbContext _context;
        
        public GemstoneController (JewelrySystemDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetGemstones")]
        public async Task<ActionResult<IEnumerable<Gemstone>>> GetGemstones()
        {
            return await _context.Gemstones.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Gemstone>> GetGemstone(int id)
        {
            var gemstone = await _context.Gemstones.FindAsync(id);

            if (gemstone == null)
            {
                return NotFound();
            }

            return gemstone;
        }

        [HttpPost]
        [Route("CreateGemstone")]
        public async Task<IActionResult> CreateJewelry([FromForm] CreateGemstone createGemstone, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
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

            return CreatedAtAction(nameof(GetGemstone), new { id = gemstone.GemstoneId }, gemstone);
        }
    }
}
