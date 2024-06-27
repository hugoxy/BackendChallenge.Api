using System.Text;
using BackendChallenge.Api.Services.Producer.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace BackendChallenge.Api.Services.Producer
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _userName;
        private readonly string _password;
        private readonly ILogger<RabbitMQProducer> _logger;

        public RabbitMQProducer(IConfiguration configuration, ILogger<RabbitMQProducer> logger)
        {
            _hostname = configuration["RabbitMQ:HostName"];
            _queueName = configuration["RabbitMQ:QueueName"];
            _userName = configuration["RabbitMQ:UserName"];
            _password = configuration["RabbitMQ:Password"];
            _logger = logger;
        }
        public void SendCommand<T>(string methodName, T message)
        {
            var queueName = $"{methodName}";

            _logger.LogInformation("Sending command '{MethodName}' to RabbitMQ: {@Message}", methodName, message);

            var factory = new ConnectionFactory()
            {
                HostName = _hostname,
                UserName = _userName,
                Password = _password
            };

            try
            {
                using var connection = factory.CreateConnection();
                _logger.LogInformation("RabbitMQ connection created.");

                using var channel = connection.CreateModel();
                _logger.LogInformation("RabbitMQ channel created.");

                channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                _logger.LogInformation("Queue declared: {QueueName}", queueName);

                var messageJson = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(messageJson);

                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: null,
                                     body: body);
                _logger.LogInformation("Message published to queue: {QueueName}", queueName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending message to RabbitMQ.");
                throw;
            }
        }
    }
}
