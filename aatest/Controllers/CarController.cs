using aatest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;


namespace aatest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CarController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        private string GetSource() => _configuration.GetConnectionString("aadb");

        /// <summary>
        /// Get all cars.
        /// </summary>
        [HttpGet]
        [Route("GetCars")]
        public IActionResult GetCars()
        {
            var query = "SELECT * FROM dbo.Cars";
            var table = new DataTable();
            var source = GetSource();

            using (SqlConnection connection = new SqlConnection(source))
            {
                try
                {
                    connection.Open();
                    using SqlCommand command = new SqlCommand(query, connection);
                    using SqlDataReader reader = command.ExecuteReader();
                    table.Load(reader);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex);
                }
            }

            return new JsonResult(table);
        }

        /// <summary>
        /// Get cars by brand id.
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        [Route("GetCarsByBrand/{id}")]
        public IActionResult GetCarsByBrand(int id)
        {
            var query = "SELECT * FROM dbo.Cars WHERE brand_id = @id";
            var table = new DataTable();
            var source = GetSource();

            using (SqlConnection connection = new SqlConnection(source))
            {
                try
                {
                    connection.Open();
                    using var command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id);
                    using SqlDataReader reader = command.ExecuteReader();
                    table.Load(reader);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex);
                }
            }

            return new JsonResult(table);
        }

        /// <summary>
        /// Check if brand id is used for any of the cars.
        /// </summary>
        [HttpPost]
        [Route("IsCarBrandUsed/{id}")]
        public IActionResult IsCarBrandUsed(int id)
        {
            var query = @"
            SELECT CASE 
                WHEN COUNT(*) > 0 THEN 1 
                ELSE COUNT(*) 
            END AS isUsed
            FROM dbo.Cars 
            WHERE brand_id = @id";

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
        /// Add a car.
        /// </summary>
        [HttpPost]
        [Route("AddCar")]
        public IActionResult AddCar([FromForm] Car car)
        {
            var query = "insert into dbo.Cars (brand_id, model, year, in_stock) values (@brandId, @model, @year, @inStock)";
            var source = _configuration.GetConnectionString("aadb");

            using SqlConnection connection = new SqlConnection(source);
            try
            {
                connection.Open();
                using SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@brandId", car.BrandId);
                command.Parameters.AddWithValue("@model", car.Model);
                command.Parameters.AddWithValue("@year", car.Year);
                command.Parameters.AddWithValue("@inStock", car.InStock);

                var rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok("Car added successfully");
                }
                else
                {
                    return BadRequest("Failed to add car");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Delete a car.
        /// </summary>
        [HttpDelete]
        [Route("DeleteCar/{id}")]
        public IActionResult DeleteCar(int id)
        {
            var query = "delete FROM dbo.Cars where id = @id";

            try
            {
                var source = GetSource();

                using SqlConnection connection = new SqlConnection(source);
                connection.Open();
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                var rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok("Car deleted successfully");
                }
                else
                {
                    return NotFound("Car not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Edit a car.
        /// </summary>
        [HttpPut]
        [Route("EditCar")]
        public IActionResult EditCar([FromBody] Car car)
        {
            if (car == null)
            {
                return BadRequest("Invalid data.");
            }

            string query = "update dbo.Cars set brand_id = @brandId, model = @model, year = @year, in_stock = @inStock WHERE id = @id";

            try
            {
                string source = _configuration.GetConnectionString("aadb");

                using (SqlConnection connection = new SqlConnection(source))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@brandId", car.BrandId);
                        command.Parameters.AddWithValue("@model", car.Model);
                        command.Parameters.AddWithValue("@year", car.Year);
                        command.Parameters.AddWithValue("@inStock", car.InStock);
                        command.Parameters.AddWithValue("@id", car.Id);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return Ok("Car updated successfully.");
                        }
                        else
                        {
                            return NotFound("Car not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
