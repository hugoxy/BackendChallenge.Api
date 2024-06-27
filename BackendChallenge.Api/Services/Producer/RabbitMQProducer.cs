using System.Collections.Concurrent;
using System.Text;
using System.Threading.Channels;
using BackendChallenge.Api.Services.Producer.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BackendChallenge.Api.Services.Producer
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        private readonly string _hostname;
        private readonly string _userName;
        private readonly string _password;
        private readonly ILogger<RabbitMQProducer> _logger;
        private IConnection _connection;
        private IModel _channel;
        private string _replyQueueName;
        private EventingBasicConsumer _consumer;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _callbackMapper = new ConcurrentDictionary<string, TaskCompletionSource<string>>();

        public RabbitMQProducer(IConfiguration configuration, ILogger<RabbitMQProducer> logger)
        {
            _hostname = configuration["RabbitMQ:HostName"];
            _userName = configuration["RabbitMQ:UserName"];
            _password = configuration["RabbitMQ:Password"];
            _logger = logger;
        }

        public void Initialize()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostname,
                UserName = _userName,
                Password = _password
            }; 
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _replyQueueName = _channel.QueueDeclare().QueueName;
            _consumer = new EventingBasicConsumer(_channel);

            _consumer.Received += (model, ea) =>
            {
                if (_callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
                {
                    var body = ea.Body.ToArray();
                    var response = Encoding.UTF8.GetString(body);
                    tcs.SetResult(response);
                }
            };

            _channel.BasicConsume(consumer: _consumer, queue: _replyQueueName, autoAck: true);
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
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

        public Task<string> SendCommandAndWaitForResponseAsync<T>(string methodName, T message)
        {
            Initialize();

            var queueName = $"{methodName}";
            var correlationId = Guid.NewGuid().ToString();
            var props = _channel.CreateBasicProperties();
            props.CorrelationId = correlationId;
            props.ReplyTo = _replyQueueName;

            var messageJson = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(messageJson);
            var tcs = new TaskCompletionSource<string>();

            _callbackMapper.TryAdd(correlationId, tcs);

            _logger.LogInformation("Sending command '{MethodName}' to RabbitMQ: {@Message}", methodName, message);

            try
            {
                _channel.QueueDeclare(queue: queueName,
                                      durable: false,
                                      exclusive: false,
                                      autoDelete: false,
                                      arguments: null);
                _logger.LogInformation("Queue declared: {QueueName}", queueName);

                _channel.BasicPublish(exchange: "",
                                      routingKey: queueName,
                                      basicProperties: props,
                                      body: body);
                _logger.LogInformation("Message published to queue: {QueueName}", queueName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending message to RabbitMQ.");
                throw;
            }
            return tcs.Task;
        }
    }
}


