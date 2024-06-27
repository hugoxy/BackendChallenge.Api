using BackendChallenge.Api.Facades.Interfaces;
using BackendChallenge.Api.Models;
using BackendChallenge.Api.Models.Requests.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace BackendChallenge.Api.Controllers
{
    /// <summary>
    /// Controller for managing product operations.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProducerFacade _producerFacade;

        /// <summary>
        /// Constructor for the ProductsController.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="producerFacade">The producer facade instance.</param>
        public ProductsController(ILogger<ProductsController> logger, IProducerFacade producerFacade)
        {
            _logger = logger;
            _producerFacade = producerFacade;
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="request">The request containing product data.</param>
        /// <returns>A confirmation message.</returns>
        [HttpPost("/products")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(500)]
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

        /// <summary>
        /// Retrieves a product by ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>A confirmation message.</returns>
        [HttpGet("/products/{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(500)]
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

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="request">The request containing updated product data.</param>
        /// <returns>A confirmation message.</returns>
        [HttpPut("/products/{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(500)]
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

        /// <summary>
        /// Deletes a product by ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>A confirmation message.</returns>
        [HttpDelete("/products/{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(500)]
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
