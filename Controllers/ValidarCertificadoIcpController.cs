using CertificadoNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace TesteValidarCertificadoIcp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValidarCertificadoIcpController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };



        private readonly ILogger<ValidarCertificadoIcpController> _logger;

        public ValidarCertificadoIcpController(ILogger<ValidarCertificadoIcpController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var cert = new CertificadoDigital("cert.cer");

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public IActionResult Post(X509Certificate2 cert)
        {
            try
            {
                var certificadoIcp = new CertificadoDigital(cert);
                if (certificadoIcp.IcpBrasil)
                {
                    return Ok();
                }
                else
                    return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Unauthorized(ex);
            }
        }
    }
}
