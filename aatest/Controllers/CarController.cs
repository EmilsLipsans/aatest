using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;


namespace aatest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private IConfiguration _configuration;

        public CarController(IConfiguration configuration) => _configuration = configuration;

        [HttpGet]
        [Route("GetCars")]
        public JsonResult GetCars()
        {
            string query = "select * from dbo.CarBrands";
            DataTable table = new DataTable();
            string source = _configuration.GetConnectionString("aadb");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(source))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                { 
                    reader = command.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        [Route("PostCars")]
        public JsonResult AddCars([FromForm] string newCars)
        {
            string query = "insert into * dbo.Cars values(@newCars)";
            DataTable table = new DataTable();
            string source = _configuration.GetConnectionString("aadb");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(source))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@newCars", newCars);
                    reader = command.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                }
            }
            return new JsonResult("Car added successfully");
        }

        [HttpDelete]
        [Route("DeleteCars")]
        public JsonResult DeleteCars([FromForm] int id)
        {
            string query = "delete from dbo.Cars where id=@id";
            DataTable table = new DataTable();
            string source = _configuration.GetConnectionString("aadb");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(source))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    reader = command.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                }
            }
            return new JsonResult("Car deleted successfully");
        }
    }
}
