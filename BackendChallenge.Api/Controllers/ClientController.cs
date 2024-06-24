using BackendChallenge.Api.Facades;
using BackendChallenge.Api.Facades.Interfaces;
using BackendChallenge.Api.Models.Entity;
using BackendChallenge.Api.Models.Requests.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BackendChallenge.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IProducerFacade _producerFacade;

        public ClientController(ILogger<ClientController> logger, IProducerFacade producerFacade)
        {
            _logger = logger;
            _producerFacade = producerFacade;
        }

        [HttpPost("/client/")]
        public IActionResult CreateClient([FromBody] CreateClientRequest request)
        {
            try
            {
                _producerFacade.SendCommand(System.Reflection.MethodBase.GetCurrentMethod().Name.ToLower(), request);
                _logger.LogInformation("CreateClient command sent to RabbitMQ.");
                return Ok("CreateClient command sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending CreateClient command to RabbitMQ.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpGet("/client/{id}")]
        public IActionResult GetClient(int id)
        {
            try
            {
                var request = new ReadClientRequest { Id = id };
                _producerFacade.SendCommand(System.Reflection.MethodBase.GetCurrentMethod().Name.ToLower(), request);
                _logger.LogInformation("ReadClient command sent to RabbitMQ.");
                return Ok("ReadClient command sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending ReadClient command to RabbitMQ.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPut("/client/{id}")]
        public IActionResult UpdateClient([FromBody] UpdateClientRequest request)
        {
            try
            {
                _producerFacade.SendCommand(System.Reflection.MethodBase.GetCurrentMethod().Name.ToLower(), request);
                _logger.LogInformation("UpdateClient command sent to RabbitMQ.");
                return Ok("UpdateClient command sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending UpdateClient command to RabbitMQ.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpDelete("/client/{id}")]
        public IActionResult DeleteClient(int id)
        {
            try
            {
                var request = new DeleteClientRequest { Id = id };
                _producerFacade.SendCommand(System.Reflection.MethodBase.GetCurrentMethod().Name.ToLower(), request);
                _logger.LogInformation("DeleteClient command sent to RabbitMQ.");
                return Ok("DeleteClient command sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending DeleteClient command to RabbitMQ.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

    }
}
