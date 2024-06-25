using System.Text;
using BackendChallenge.Api.Models;
using BackendChallenge.Api.Services.Repositories.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BackendChallenge.MicroServices.Consumers
{
    public class RabbitMQConsumer
    {
        private readonly string _hostname;
        private readonly string _userName;
        private readonly string _password;
        private readonly ILogger<RabbitMQConsumer> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly IClientRepository _clientRepository;

        public RabbitMQConsumer(ILogger<RabbitMQConsumer> logger, IOrderRepository orderRepository, IClientRepository clientRepository, IConfiguration configuration)
        {
            _logger = logger;
            _orderRepository = orderRepository;
            _clientRepository = clientRepository;
            _hostname = configuration["RabbitMQ:HostName"];
            _userName = configuration["RabbitMQ:UserName"];
            _password = configuration["RabbitMQ:Password"];
        }

        public void StartConsuming()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostname,
                UserName = _userName,
                Password = _password
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
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    switch (queueName)
                    {
                        case CrudOperation.CreateClient:
                            await ProcessCreateClientMessageAsync(message);
                            break;
                        case CrudOperation.ReadClient:
                            await ProcessReadClientMessageAsync(message);
                            break;
                        case CrudOperation.UpdateClient:
                            await ProcessUpdateClientMessageAsync(message);
                            break;
                        case CrudOperation.DeleteClient:
                            await ProcessDeleteClientMessageAsync(message);
                            break;
                        case CrudOperation.CreateOrder:
                            await ProcessCreateOrderMessageAsync(message);
                            break;
                        case CrudOperation.ReadOrder:
                            await ProcessReadOrderMessageAsync(message);
                            break;
                        case CrudOperation.UpdateOrder:
                            await ProcessUpdateOrderMessageAsync(message);
                            break;
                        case CrudOperation.DeleteOrder:
                            await ProcessDeleteOrderMessageAsync(message);
                            break;
                        case CrudOperation.CreateProduct:
                            await ProcessCreateProductMessageAsync(message);
                            break;
                        case CrudOperation.ReadProduct:
                            await ProcessReadProductMessageAsync(message);
                            break;
                        case CrudOperation.UpdateProduct:
                            await ProcessUpdateProductMessageAsync(message);
                            break;
                        case CrudOperation.DeleteProduct:
                            await ProcessDeleteProductMessageAsync(message);
                            break;
                        default:
                            _logger.LogInformation("Received message from unknown queue: {QueueName}. Message: {Message}", queueName, message);
                            break;
                    }
                
                };

                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                _logger.LogInformation("Consumer started. Listening on queue: {QueuesName}", queueName);
            }

            while (true)
            {
                Thread.Sleep(1000);
            }
        }

        private async Task ProcessDeleteProductMessageAsync(string message)
        {
            throw new NotImplementedException();
        }

        private async Task ProcessUpdateProductMessageAsync(string message)
        {
            throw new NotImplementedException();
        }

        private async Task ProcessReadProductMessageAsync(string message)
        {
            throw new NotImplementedException();
        }

        private async Task ProcessCreateProductMessageAsync(string message)
        {
            throw new NotImplementedException();
        }

        private async Task ProcessDeleteOrderMessageAsync(string message)
        {
            throw new NotImplementedException();
        }

        private async Task ProcessUpdateOrderMessageAsync(string message)
        {
            throw new NotImplementedException();
        }

        private async Task ProcessReadOrderMessageAsync(string message)
        {
            throw new NotImplementedException();
        }

        private async Task ProcessCreateOrderMessageAsync(string message)
        {
            throw new NotImplementedException();
        }

        private async Task ProcessDeleteClientMessageAsync(string message)
        {
            throw new NotImplementedException();
        }

        private async Task ProcessUpdateClientMessageAsync(string message)
        {
            throw new NotImplementedException();
        }

        private async Task ProcessReadClientMessageAsync(string message)
        {
            throw new NotImplementedException();
        }

        private async Task ProcessCreateClientMessageAsync(string message)
        {
            throw new NotImplementedException();
        }
    }
