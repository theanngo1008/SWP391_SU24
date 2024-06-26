using BE.Entities;
using BE.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Data;
using System.Data.SqlClient;

using Firebase.Storage;
using Firebase.Auth.Providers;
using Firebase.Auth;
using FirebaseAdmin;
using Google.Cloud.Storage.V1;
using Google.Apis.Auth.OAuth2;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.EntityFrameworkCore;
using BE.Models;
using BE.Services;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JewelryController : ControllerBase
    {

        private readonly JewelrySystemDbContext _context;

        private readonly IConfiguration _config;

        private readonly JewelryService _service;



        public JewelryController(JewelrySystemDbContext context, IConfiguration config, JewelryService service)
        {
            _context = context;
            _config = config;
            _service = service;
        }


        [HttpPost]
        [Route("CreateJewelry")]
        public async Task<IActionResult> CreateJewelry([FromForm] CreateJewelry createJewelry, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var stream = file.OpenReadStream();
            var firebaseStorage = new FirebaseStorage("projectswp-7bb14.appspot.com");
            var fileName = Path.GetFileName(file.FileName);



            var task = firebaseStorage
                .Child("images")
                .Child(fileName)
                .PutAsync(stream);

            var downloadUrl = await task;

            var jewelry = new Jewelry
            {
                JewelryName = createJewelry.JewelryName,
                Image = downloadUrl,
                Cost = createJewelry.Cost,
                Quantity = createJewelry.Quantity,
                Status = true,
                ChargeId = createJewelry.ChargeId,
                QuotationId = createJewelry.QuotationId,
                WarehouseId = createJewelry.WarehouseId,
                SubCateId = createJewelry.SubCateId
            };
            
            _context.Jewelries.Add(jewelry);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetJewelry), new { id = jewelry.JewelryId }, jewelry);
        }

        [HttpGet("GetJewelries")]
        public async Task<ActionResult<IEnumerable<Jewelry>>> GetJewelries()
        {
            return await _context.Jewelries.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Jewelry>> GetJewelry(int id)
        {
            var jewelry = await _context.Jewelries.FindAsync(id);

            if (jewelry == null)
            {
                return NotFound();
            }

            return jewelry;
        }

        [HttpPost]
        [Route("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file, [FromForm] int jewelryId)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var jewelry = await _context.Jewelries.FindAsync(jewelryId);
            if (jewelry == null)
            {
                return NotFound("Jewelry not found!!!");
            }

            var stream = file.OpenReadStream();
            var firebaseStorage = new FirebaseStorage("projectswp-7bb14.appspot.com");
            var fileName = Path.GetFileName(file.FileName);



            var task = firebaseStorage
                .Child("images")
                .Child(fileName)
                .PutAsync(stream);

            var downloadUrl = await task;

            jewelry.Image = downloadUrl;
            _context.Entry(jewelry).State = EntityState.Modified;
            await _context.SaveChangesAsync();


            return Ok(new { downloadUrl });
        }

        private void UploadFile(Stream stream, string bucketName, string objectName)
        {
            var storage = StorageClient.Create();
            stream.Position = 0; // Reset stream position
            storage.UploadObject(bucketName, objectName, null, stream);
        }

        [HttpPut("UpdateJewelry/{id}")]
        public async Task<IActionResult> UpdateJewelry(int id, [FromForm] UpdateJewelryRequest request)
        {
            var result = await _service.UpdateJewelry(id, request);
            if (result == null)
                return NotFound(result);
            return Ok(result);
        }
    }
}

        
        
    

