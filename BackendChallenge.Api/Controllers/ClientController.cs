using BackendChallenge.Api.Facades.Interfaces;
using BackendChallenge.Api.Models;
using BackendChallenge.Api.Models.Requests.Client;
using Microsoft.AspNetCore.Mvc;

namespace BackendChallenge.Api.Controllers
{
    /// <summary>
    /// Controller for managing client operations.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IProducerFacade _producerFacade;

        /// <summary>
        /// Constructor for the ClientController.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="producerFacade">The producer facade instance.</param>
        public ClientController(ILogger<ClientController> logger, IProducerFacade producerFacade)
        {
            _logger = logger;
            _producerFacade = producerFacade;
        }

        /// <summary>
        /// Creates a new client.
        /// </summary>
        /// <param name="request">The request containing client data.</param>
        /// <returns>A confirmation message.</returns>
        [HttpPost("/client/")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(500)]
        public IActionResult CreateClient([FromBody] CreateClientRequest request)
        {
            try
            {
                _producerFacade.SendCommand(CrudOperation.CreateClient, request);
                _logger.LogInformation("CreateClient command sent to RabbitMQ.");
                return Ok("CreateClient command sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending CreateClient command to RabbitMQ.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        /// <summary>
        /// Retrieves a client by ID.
        /// </summary>
        /// <param name="id">The ID of the client.</param>
        /// <returns>A confirmation message.</returns>
        [HttpGet("/client/{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetClientById(int id)
        {
            try
            {
                var request = new ReadClientRequest { ClientId = id };
                _logger.LogInformation("ReadClient command sent to RabbitMQ.");
                var response = await _producerFacade.SendCommandAndWaitForResponseAsync(CrudOperation.ReadClient, request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending ReadClient command to RabbitMQ.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        /// <summary>
        /// Updates an existing client.
        /// </summary>
        /// <param name="request">The request containing updated client data.</param>
        /// <returns>A confirmation message.</returns>
        [HttpPut("/client/{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(500)]
        public IActionResult UpdateClient([FromBody] UpdateClientRequest request)
        {
            try
            {
                _producerFacade.SendCommand(CrudOperation.UpdateClient, request);
                _logger.LogInformation("UpdateClient command sent to RabbitMQ.");
                return Ok("UpdateClient command sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending UpdateClient command to RabbitMQ.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        /// <summary>
        /// Deletes a client by ID.
        /// </summary>
        /// <param name="id">The ID of the client to delete.</param>
        /// <returns>A confirmation message.</returns>
        [HttpDelete("/client/{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(500)]
        public IActionResult DeleteClient(int id)
        {
            try
            {
                var request = new DeleteClientRequest { ClientId = id };
                _producerFacade.SendCommand(CrudOperation.DeleteClient, request);
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
