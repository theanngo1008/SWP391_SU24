using Firebase.Storage;
using JewelryProductionOrder.BusinessLogic.RequestModels.Gemstone;
using JewelryProductionOrder.BusinessLogic.Services.Implementation;
//using JewelryProductionOrder.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JewelryProductionOrder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GemstoneController : ControllerBase
    {   /*
        private readonly GemstoneService _service;

        public GemstoneController(GemstoneService service)
        {
            _service = service;
        }

        [HttpGet("GetGemstones")]
        public async Task<ActionResult<IEnumerable<Gemstone>>> GetGemstones()
        {
            var gemstones = await _service.GetGemstonesAsync();
            return Ok(gemstones);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Gemstone>> GetGemstone(int id)
        {
            var gemstone = await _service.GetGemstoneByIdAsync(id);

            if (gemstone == null)
            {
                return NotFound();
            }

            return gemstone;
        }

        [HttpPost("CreateGemstone")]
        public async Task<IActionResult> CreateGemstone([FromForm] CreateGemstone createGemstone, IFormFile file)
        {
            try
            {
                var gemstone = await _service.CreateGemstoneAsync(createGemstone, file);
                return CreatedAtAction(nameof(GetGemstone), new { id = gemstone.GemstoneId }, gemstone);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }





        /*
        
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
        }*/
    }
}
