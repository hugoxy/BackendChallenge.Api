using BackendChallenge.Api.Facades.Interfaces;
using BackendChallenge.Api.Models;
using BackendChallenge.Api.Models.Requests.Products;
using Microsoft.AspNetCore.Mvc;

namespace BackendChallenge.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProducerFacade _producerFacade;

        public ProductsController(ILogger<ProductsController> logger, IProducerFacade producerFacade)
        {
            _logger = logger;
            _producerFacade = producerFacade;
        }

        [HttpPost("/products")]
        public IActionResult CreateProduct([FromBody] CreateProductRequest request)
        {
            try
            {
                _producerFacade.SendCommand(CrudOperation.CreateProduct, request);
                _logger.LogInformation("CreateProduct command sent to RabbitMQ.");
                return Ok("CreateProduct command sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending CreateProduct command to RabbitMQ.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpGet("/products/{id}")]
        public IActionResult GetProductById(int id)
        {
            try
            {
                var request = new ReadProductRequest { ProductId = id };
                _producerFacade.SendCommand(CrudOperation.ReadProduct, request);
                _logger.LogInformation("ReadProduct command sent to RabbitMQ.");
                return Ok("ReadProduct command sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending ReadProduct command to RabbitMQ.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPut("/products/{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] UpdateProductRequest request)
        {
            try
            {
                request.ProductId = id;
                _producerFacade.SendCommand(CrudOperation.UpdateProduct, request);
                _logger.LogInformation("UpdateProduct command sent to RabbitMQ.");
                return Ok("UpdateProduct command sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending UpdateProduct command to RabbitMQ.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpDelete("/products/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                var request = new DeleteProductRequest { ProductId = id };
                _producerFacade.SendCommand(CrudOperation.DeleteProduct, request);
                _logger.LogInformation("DeleteProduct command sent to RabbitMQ.");
                return Ok("DeleteProduct command sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending DeleteProduct command to RabbitMQ.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}
