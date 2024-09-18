using Microsoft.AspNetCore.Mvc;
using PdfService.Interfaces;

namespace PdfService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IPdfService _pdfService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IPdfService pdfService)
        {
            _logger = logger;
            _pdfService = pdfService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("GetPdf")]
        public async Task<IActionResult> GetPdf(string s3Url, string fileName)
        {
            try
            {
                // Generate PDF
                var pdfBytes = await _pdfService.CreatePdfAsync(s3Url);

                //// Upload PDF to S3
                //var preSignedUrl = await _s3Service.UploadPdf(pdfBytes, fileName);

                //// Return the pre-signed URL
                //return Ok(new { Url = preSignedUrl });
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating or uploading the PDF.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
