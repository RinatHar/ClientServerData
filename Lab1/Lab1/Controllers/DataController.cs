using Lab1.Dao;
using Lab1.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace Lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DataController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("read")]
        [ProducesResponseType(typeof(Data), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            try
            {
                string query = @"select id, value from data";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
                using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
                {
                    await mycon.OpenAsync();
                    using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                    {
                        using (var myReader = await myCommand.ExecuteReaderAsync())
                        {
                            table.Load(myReader);
                        }
                    }
                }

                return new JsonResult(table);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("write")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(DataDao data)
        {
            try
            {
                string query = @"insert into data(value) values (@value)";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
                using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
                {
                    await mycon.OpenAsync();
                    using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                    {
                        myCommand.Parameters.AddWithValue("@value", data.value);

                        using (var myReader = await myCommand.ExecuteReaderAsync())
                        {
                            table.Load(myReader);
                        }
                    }
                }

                return new JsonResult("Added successfully!");
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
