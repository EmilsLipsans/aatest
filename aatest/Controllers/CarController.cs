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
    }
}
