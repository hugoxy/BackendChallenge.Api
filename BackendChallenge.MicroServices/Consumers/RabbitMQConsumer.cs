using System.Text;
using BackendChallenge.Api.Models;
using BackendChallenge.Api.Services.Database;
using BackendChallenge.Api.Services.Repositories.Interfaces;
using BackendChallenge.MicroServices.Models;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BackendChallenge.MicroServices.Consumers
{
    public class RabbitMQConsumer
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RabbitMQConsumer> _logger;
        private readonly RabbitMQSettings _rabbitMQSettings;

        public RabbitMQConsumer(IServiceScopeFactory scopeFactory, ILogger<RabbitMQConsumer> logger, IOptions<RabbitMQSettings> rabbitMQSettings)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _rabbitMQSettings = rabbitMQSettings.Value;
        }

        public Task StartConsumingAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting RabbitMQConsumer Service");
            return ConsumeAsync(cancellationToken);
        }

        private async Task ConsumeAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting ConsumeAsync Service");

            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMQSettings.HostName,
                UserName = _rabbitMQSettings.UserName,
                Password = _rabbitMQSettings.Password
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var queueNames = CrudOperation.GetValues();

            foreach (var queueName in queueNames)
            {
                channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (sender, ea) =>
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);

                        try
                        {
                            switch (queueName)
                            {
                                case CrudOperation.CreateClient:
                                    await ProcessCreateClientMessageAsync(message, dbContext);
                                    break;
                                case CrudOperation.ReadClient:
                                    await ProcessReadClientMessageAsync(message, dbContext);
                                    break;
                                case CrudOperation.UpdateClient:
                                    await ProcessUpdateClientMessageAsync(message, dbContext);
                                    break;
                                case CrudOperation.DeleteClient:
                                    await ProcessDeleteClientMessageAsync(message, dbContext);
                                    break;
                                case CrudOperation.CreateOrder:
                                    await ProcessCreateOrderMessageAsync(message, dbContext);
                                    break;
                                case CrudOperation.ReadOrder:
                                    await ProcessReadOrderMessageAsync(message, dbContext);
                                    break;
                                case CrudOperation.UpdateOrder:
                                    await ProcessUpdateOrderMessageAsync(message, dbContext);
                                    break;
                                case CrudOperation.DeleteOrder:
                                    await ProcessDeleteOrderMessageAsync(message, dbContext);
                                    break;
                                case CrudOperation.CreateProduct:
                                    await ProcessCreateProductMessageAsync(message, dbContext);
                                    break;
                                case CrudOperation.ReadProduct:
                                    await ProcessReadProductMessageAsync(message, dbContext);
                                    break;
                                case CrudOperation.UpdateProduct:
                                    await ProcessUpdateProductMessageAsync(message, dbContext);
                                    break;
                                case CrudOperation.DeleteProduct:
                                    await ProcessDeleteProductMessageAsync(message, dbContext);
                                    break;
                                default:
                                    _logger.LogInformation("Received message from unknown queue: {QueueName}. Message: {Message}", queueName, message);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error processing message: {Message}", message);
                        }
                    }
                };

                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                _logger.LogInformation("Consumer started. Listening on queue: {QueueName}", queueName);
            }

            await Task.Delay(Timeout.Infinite, cancellationToken);
        }

        private async Task ProcessDeleteProductMessageAsync(string message, OrderDbContext dbContext)
        {
            throw new NotImplementedException();
        }

        private async Task ProcessUpdateProductMessageAsync(string message, OrderDbContext dbContext)
        {
            throw new NotImplementedException();
        }

        private async Task ProcessReadProductMessageAsync(string message, OrderDbContext dbContext)
        {
            throw new NotImplementedException();
        }

        private async Task ProcessCreateProductMessageAsync(string message, OrderDbContext dbContext)
        {
            throw new NotImplementedException();
        }

        private async Task ProcessDeleteOrderMessageAsync(string message, OrderDbContext dbContext)
        {
            throw new NotImplementedException();
        }

        private async Task ProcessUpdateOrderMessageAsync(string message, OrderDbContext dbContext)
        {
            throw new NotImplementedException();
        }

        private async Task ProcessReadOrderMessageAsync(string message, OrderDbContext dbContext)
        {
            throw new NotImplementedException();
        }

        private async Task ProcessCreateOrderMessageAsync(string message, OrderDbContext dbContext)
        {
            throw new NotImplementedException();
        }

        private async Task ProcessDeleteClientMessageAsync(string message, OrderDbContext dbContext)
        {
            throw new NotImplementedException();
        }

        private async Task ProcessUpdateClientMessageAsync(string message, OrderDbContext dbContext)
        {
            throw new NotImplementedException();
        }

        private async Task ProcessReadClientMessageAsync(string message, OrderDbContext dbContext)
        {
            throw new NotImplementedException();
        }

        private async Task ProcessCreateClientMessageAsync(string message, OrderDbContext dbContext)
        {
            throw new NotImplementedException();
        }
    }
}
