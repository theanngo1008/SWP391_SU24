using BE.Models;
using Microsoft.AspNetCore. Http;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAccounts")]
        public JsonResult GetAccounts()
        {
            string query = "select * from Account";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("JewelrySystemDBConn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDatasource))
            {
                myConn.Open();
                using(SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        [Route("Login")]
        public JsonResult Login([FromForm] string email, [FromForm] string password) 
        {
            
            DataTable accTable = ValidateCredentials(email, password);
            
            if (accTable.Rows.Count > 0)
            {
                DataRow accRow = accTable.Rows[0];
                if (CheckStatus(accRow))
                {
                    return new JsonResult(accTable);
                }
                else
                    return new JsonResult("The account has been banned");
            }
            else
            {
                return new JsonResult("Invalid email or password!!!");
            }
        }

        private DataTable ValidateCredentials(string email, string password)
        {
            string query = "select * from Account where Email=@Email and Password=@Password";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("JewelrySystemDBConn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@Email", email);
                    myCommand.Parameters.AddWithValue("@Password", password);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }
            return table;
        }
        
        private bool CheckStatus(DataRow accRow)
        {
            int status = Convert.ToInt32(accRow["Status"]);
            return status != 3;
        }

        [HttpPost]
        [Route("AddAccounts")]
        public JsonResult AddAccounts([FromForm] string email, [FromForm] string accName, [FromForm] string password, [FromForm] string phone, [FromForm] string address)
        {
            string query = "insert into Account(Email, AccName, Password, NumberPhone, Deposit, Address, Role, Status) values(@Email, @AccName, @Password, @Phone, null, @Address, 'US', 1)";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("JewelrySystemDBConn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDatasource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@Email", email);
                    myCommand.Parameters.AddWithValue("@AccName", accName);
                    myCommand.Parameters.AddWithValue("@Password", password);
                    myCommand.Parameters.AddWithValue("@Phone", phone);
                    myCommand.Parameters.AddWithValue("@Address", address);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }

        [HttpPost]
        [Route("UpdateBannedStatus")]
        public JsonResult UpdateBannedStatus(int id)
        {
            string query = "update Account set Status = 3 where AccId = @AccId";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("JewelrySystemDBConn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDatasource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@AccId", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }
            return new JsonResult("Updated Status Successfully");
        }

        [HttpPost]
        [Route("UpdateAccount")]
        public JsonResult UpdateAccount(int id)
        {
            string query = "update Account set AccName = @AccName, Email = @Email, Password = @Pass, NumberPhone = @Phone, Address = @Address where AccId = @AccId";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("JewelrySystemDBConn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDatasource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@AccId", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }
            return new JsonResult("Updated Status Successfully");
        }
    }

    
}
