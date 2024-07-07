using aatest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;


namespace aatest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarBrandController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CarBrandController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        private string GetSource() => _configuration.GetConnectionString("aadb");

        /// <summary>
        /// Get all car brands.
        /// </summary>
        [HttpGet]
        [Route("GetCarBrands")]
        public IActionResult GetCarBrands()
        {
            var query = "SELECT * FROM dbo.CarBrands";
            var table = new DataTable();

            try
            {
                var source = GetSource();

                using (var connection = new SqlConnection(source))
                {
                    connection.Open();
                    using var command = new SqlCommand(query, connection);
                    using SqlDataReader reader = command.ExecuteReader();
                    table.Load(reader);
                }

                return new JsonResult(table);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Check if car brand exists by id.
        /// </summary>
        [HttpPost]
        [Route("CarBrandExists/{id}")]
        public IActionResult CarBrandExists(int id)
        {
            string query = @"
            SELECT CASE 
                WHEN COUNT(*) > 0 THEN 1 
                ELSE 0 
            END AS brandExists
            FROM dbo.CarBrands 
            WHERE id = @id";

            var table = new DataTable();

            try
            {
                var source = GetSource();

                using (var connection = new SqlConnection(source))
                {
                    connection.Open();
                    using var command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id);
                    using SqlDataReader reader = command.ExecuteReader();
                    table.Load(reader);
                }

                return new JsonResult(table);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Add car brand.
        /// </summary>
        [HttpPost]
        [Route("AddCarBrand")]
        public IActionResult AddCarBrand([FromForm] string name)
        {
            var query = "INSERT INTO dbo.CarBrands (brand_name) VALUES (@name)";

            try
            {
                var source = GetSource();

                using var connection = new SqlConnection(source);
                connection.Open();
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", name);
                var rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return Ok("Car brand added successfully");
                }
                else
                {
                    return BadRequest("Failed to add car brand");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Delete car brand.
        /// </summary>
        [HttpDelete]
        [Route("DeleteCarBrand/{id}")]
        public IActionResult DeleteCarBrand(int id)
        {
            var query = "DELETE FROM dbo.CarBrands WHERE id = @id";

            try
            {
                var source = GetSource();

                using var connection = new SqlConnection(source);
                connection.Open();
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                var rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return Ok("Car brand deleted successfully");
                }
                else
                {
                    return NotFound("Car brand not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Edit car brand.
        /// </summary>
        [HttpPut]
        [Route("EditCarBrand")]
        public IActionResult EditCarBrand([FromBody] CarBrand carBrand)
        {
            if (carBrand == null || carBrand.Id == 0)
            {
                return BadRequest("Invalid car brand data.");
            }

            string query = "UPDATE dbo.CarBrands SET brand_name = @brandName WHERE id = @id";
            try
            {
                var source = GetSource();

                using var connection = new SqlConnection(source);
                connection.Open();
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@brandName", carBrand.Name);
                command.Parameters.AddWithValue("@id", carBrand.Id);

                var rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok("Car brand updated successfully.");
                }
                else
                {
                    return NotFound("Car brand not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
