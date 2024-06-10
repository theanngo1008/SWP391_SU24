using BE.Entities;
using BE.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Data;
using System.Data.SqlClient;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JewelryController : ControllerBase
    {

        private readonly JewelrySystemDbContext _context;

        private readonly IHostEnvironment _env;

        public JewelryController(IHostEnvironment env)
        {
            _env = env;
        }
        // Configure Firebase
        private static string apiKey = "AIzaSyCXOJSqeJWUWGzN2bN31XdiZURckizm4fI";
        private static string Bucket = "projectswp-7bb14.appspot.com";
        private static string AuthEmail = "Huydqse151428@fpt.edu.vn";
        private static string AuthPassword = "Kid30032001";

        public JewelryController(JewelrySystemDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        private async Task<IActionResult> Index(Jewelry jewelry)  
        {
            var jewelryUpload = jewelry.Image;
            if (jewelryUpload.Length > 0)
            {
                //Upload the file to firebase
                string folderName = "firebaseFiles";
                //string path = Path.Combine(_env.WebRootPath, $" images/")
            }
            return BadRequest();
        }

        /*public async Task<IActionResult> UploadImage(IFormFile formFile, string productCode) 
        {
            APIResponse response = new APIResponse();
            try
            {
               string FilePath = GetFilepath(productCode);
                if (!System.IO.Directory.Exists(FilePath))
                {
                    System.IO.Directory.CreateDirectory(FilePath);
                }

                string imagePath = FilePath + "\\" + productCode + ".png";
                if (!System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                using (FileStream stream = System.IO.File.Create(imagePath))
                {
                    await formFile.CopyToAsync(stream);
                    response.ResponseCode = 200;
                    response.Result = "pass";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return Ok(response);
        }

        [HttpPut("MultiUploadImage")]
        public async Task<IActionResult> MultiUploadImage(IFormFileCollection fileCollection, int jewelryId)
        {
            APIResponse response = new APIResponse();
            int passCount = 0;
            int errorCount = 0;
            try
            {
                var jewelry = await _context.Jewelries.FindAsync(jewelryId);
                if (!System.IO.Directory.Exists(FilePath))
                {
                    System.IO.Directory.CreateDirectory(FilePath);
                }
                foreach (var file in fileCollection)
                {
                    string imagePath = FilePath + "\\" + file.FileName;
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    using (FileStream stream = System.IO.File.Create(imagePath))
                    {
                        await file.CopyToAsync(stream);
                        passCount++;
                    }
                }  
            }
            catch (Exception ex)
            {
                errorCount++;
                response.Message = ex.Message;
            }
            response.ResponseCode = 200;
            response.Result = passCount + " files uploaded &" + errorCount + " files failed";
            return Ok(response);
        }
        [NonAction]
        private string GetFilepath(string productCode)
        {
            return this.environment.WebRootPath + "\\Upload\\jewelry\\" + productCode;
        }

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
    }
}
