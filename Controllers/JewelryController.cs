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

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JewelryController : ControllerBase
    {

        private readonly JewelrySystemDbContext _context;

        private readonly IConfiguration _config;



        public JewelryController(JewelrySystemDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
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

        [HttpGet]
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
    }
}

        
        /*x
        [HttpGet]
        [Route("GetJewelry")]
        public JsonResult GetJewelries() 
        {
            string query = "select * from Jewelry where status = 1";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("JewelrySystemDBConn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDatasource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }
            return new JsonResult(table);
        }

        /*[HttpGet]
        [Route("DeleteJewelry")]
        public JsonResult DeleteJewelry()
        {
            string query = "update Jewelry set status = 0 where AccId = @AccId";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("JewelrySystemDBConn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDatasource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@AccId", );
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }
            return new JsonResult(table);
        } */
    

