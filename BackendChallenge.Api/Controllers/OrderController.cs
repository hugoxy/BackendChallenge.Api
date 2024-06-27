using BackendChallenge.Api.Facades.Interfaces;
using BackendChallenge.Api.Models;
using BackendChallenge.Api.Models.Requests.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace BackendChallenge.Api.Controllers
{
    /// <summary>
    /// Controller for managing order operations.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IProducerFacade _producerFacade;

        /// <summary>
        /// Constructor for the OrdersController.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="producerFacade">The producer facade instance.</param>
        public OrdersController(ILogger<OrdersController> logger, IProducerFacade producerFacade)
        {
            _logger = logger;
            _producerFacade = producerFacade;
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="request">The request containing order data.</param>
        /// <returns>A confirmation message.</returns>
        [HttpPost("/orders")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(500)]
        public IActionResult CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                _producerFacade.SendCommand(CrudOperation.CreateOrder, request);
                _logger.LogInformation("CreateOrder command sent to RabbitMQ.");
                return Ok("CreateOrder command sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending CreateOrder command to RabbitMQ.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        /// <summary>
        /// Retrieves an order by ID.
        /// </summary>
        /// <param name="id">The ID of the order.</param>
        /// <returns>A confirmation message.</returns>
        [HttpGet("/orders/{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(500)]
        public IActionResult GetOrderById(int id)
        {
            try
            {
                var request = new ReadOrderRequest { OrderId = id };
                _producerFacade.SendCommand(CrudOperation.ReadOrder, request);
                _logger.LogInformation("ReadOrder command sent to RabbitMQ.");
                return Ok("ReadOrder command sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending ReadOrder command to RabbitMQ.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        /// <summary>
        /// Updates an existing order.
        /// </summary>
        /// <param name="id">The ID of the order to update.</param>
        /// <param name="request">The request containing updated order data.</param>
        /// <returns>A confirmation message.</returns>
        [HttpPut("/orders/{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(500)]
        public IActionResult UpdateOrder(int id, [FromBody] UpdateOrderRequest request)
        {
            try
            {
                request.OrderId = id;
                _producerFacade.SendCommand(CrudOperation.UpdateOrder, request);
                _logger.LogInformation("UpdateOrder command sent to RabbitMQ.");
                return Ok("UpdateOrder command sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending UpdateOrder command to RabbitMQ.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        /// <summary>
        /// Deletes an order by ID.
        /// </summary>
        /// <param name="id">The ID of the order to delete.</param>
        /// <returns>A confirmation message.</returns>
        [HttpDelete("/orders/{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(500)]
        public IActionResult DeleteOrder(int id)
        {
            try
            {
                var request = new DeleteOrderRequest { OrderId = id };
                _producerFacade.SendCommand(CrudOperation.DeleteOrder, request);
                _logger.LogInformation("DeleteOrder command sent to RabbitMQ.");
                return Ok("DeleteOrder command sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending DeleteOrder command to RabbitMQ.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}
