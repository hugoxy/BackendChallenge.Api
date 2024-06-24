using BackendChallenge.Api.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace BackendChallenge.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        

        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        public List<OrderEntity> Get()
        {
            _logger.LogInformation("Get orders");
            return null;
        }

    }
}
