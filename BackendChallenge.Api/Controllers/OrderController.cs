using BackendChallenge.Api.Facades.Interfaces;
using BackendChallenge.Api.Models.Entity;
using BackendChallenge.Api.Models.Requests.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace BackendChallenge.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IProducerFacade _producerFacade;

        public OrdersController(ILogger<OrdersController> logger, IProducerFacade producerFacade)
        {
            _logger = logger;
            _producerFacade = producerFacade;
        }

        [HttpPost("/orders")]
        public IActionResult CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                _producerFacade.SendCommand(System.Reflection.MethodBase.GetCurrentMethod().Name.ToLower(), request);
                _logger.LogInformation("CreateOrder command sent to RabbitMQ.");
                return Ok("CreateOrder command sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending CreateOrder command to RabbitMQ.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpGet("/orders/{id}")]
        public IActionResult GetOrder(int id)
        {
            try
            {
                var request = new ReadOrderRequest { OrderId = id };
                _producerFacade.SendCommand(System.Reflection.MethodBase.GetCurrentMethod().Name.ToLower(), request);
                _logger.LogInformation("ReadOrder command sent to RabbitMQ.");
                return Ok("ReadOrder command sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending ReadOrder command to RabbitMQ.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPut("/orders/{id}")]
        public IActionResult UpdateOrder(int id, [FromBody] UpdateOrderRequest request)
        {
            try
            {
                request.OrderId = id;
                _producerFacade.SendCommand(System.Reflection.MethodBase.GetCurrentMethod().Name.ToLower(), request);
                _logger.LogInformation("UpdateOrder command sent to RabbitMQ.");
                return Ok("UpdateOrder command sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending UpdateOrder command to RabbitMQ.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpDelete("/orders/{id}")]
        public IActionResult DeleteOrder(int id)
        {
            try
            {
                var request = new DeleteOrderRequest { OrderId = id };
                _producerFacade.SendCommand(System.Reflection.MethodBase.GetCurrentMethod().Name.ToLower(), request);
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
