using Konger.CoreAngular.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace CoreAngular.Controllers
{


    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        protected readonly ILogger<SampleDataController> _logger;

        private readonly IConfiguration _configuration;
        private string connectionString;

        public SampleDataController(IConfiguration configuration, ILogger<SampleDataController> logger = null)
        {
            _logger = logger;

            _logger.LogInformation("Hello world!");
            _logger.LogWarning("Warn you stay away");
            _configuration = configuration;
            _logger.LogError("Exception");
            SqlConnection connection = null;

            //connectionString = _configuration.GetConnectionString("appDbConnection");

            connectionString = SQLHelper.GetDBConnectionString();
            //try
            //{
            //    connection = new SqlConnection(connectionString);

            //    using (var cmd = new SqlCommand
            //    {
            //        Connection = connection,
            //        CommandText = @"INSERT INTO [dbo].[account] ([username] ,[password]) VALUES (@username, @password)",
            //        CommandType = CommandType.Text
            //    })
            //    {
            //        connection = null;

            //        cmd.Parameters.AddWithValue("@username", "Shun Shun");
            //        cmd.Parameters.AddWithValue("@password", "12313");

            //        cmd.Connection.Open();
            //        cmd.ExecuteNonQuery();
            //    }

            //}
            //finally
            //{
            //    connection?.Dispose();
            //}
        }

        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }
    }
}
