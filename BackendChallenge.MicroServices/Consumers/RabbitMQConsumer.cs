﻿using System.Text;
using BackendChallenge.Api.Models;
using BackendChallenge.Api.Models.Entity;
using BackendChallenge.Api.Services.Database;
using BackendChallenge.MicroServices.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
            _logger.LogInformation("Processing DeleteProductMessage: {message}", message);

            // Adicionar ao contexto do EF Core e salvar no banco de dados dentro de uma transação
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {

                var productId = int.Parse(message);
                var product = await dbContext.Product.FindAsync(productId);
                if (product != null)
                {
                    dbContext.Product.Remove(product);
                    await dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                    _logger.LogInformation("DeleteProductMessage processed and deleted from database.");
                }
                else
                {
                    _logger.LogWarning("Product with ID {ProductId} not found for deletion.", productId);
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error processing DeleteProductMessage: {message}", message);
            }
        }

        private async Task ProcessUpdateProductMessageAsync(string message, OrderDbContext dbContext)
        {
            _logger.LogInformation("Processing UpdateProductMessage: {message}", message);

            // Adicionar ao contexto do EF Core e salvar no banco de dados dentro de uma transação
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                var updatedProduct = JsonConvert.DeserializeObject<ProductsEntity>(message);
                var existingProduct = await dbContext.Product.FindAsync(updatedProduct.Id);
                if (existingProduct != null)
                {
                    existingProduct.ProductName = updatedProduct.ProductName;
                    existingProduct.Quantity = updatedProduct.Quantity;
                    existingProduct.Price = updatedProduct.Price;

                    await dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                    _logger.LogInformation("UpdateProductMessage processed and updated in database.");
                }
                else
                {
                    _logger.LogWarning("Product with ID {ProductId} not found for update.", updatedProduct.Id);
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error processing UpdateProductMessage: {message}", message);
            }
        }

        private async Task ProcessReadProductMessageAsync(string message, OrderDbContext dbContext)
        {
            _logger.LogInformation("Processing ReadProductMessage: {message}", message);
            try
            {
                var productId = int.Parse(message);
                var product = await dbContext.Product.FindAsync(productId);
                if (product != null)
                {
                    _logger.LogInformation("ReadProductMessage found product: {Product}", JsonConvert.SerializeObject(product));
                }
                else
                {
                    _logger.LogWarning("Product with ID {ProductId} not found for reading.", productId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing ReadProductMessage: {message}", message);
            }
        }


        private async Task ProcessCreateProductMessageAsync(string message, OrderDbContext dbContext)
        {
            _logger.LogInformation("Processing CreateProductMessage: {message}", message);

            var product = JsonConvert.DeserializeObject<ProductsEntity>(message);

            // Adicionar ao contexto do EF Core e salvar no banco de dados dentro de uma transação
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                await dbContext.Product.AddAsync(product);
                await dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                _logger.LogInformation("CreateProductMessage processed and saved to database.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error processing CreateProductMessage: {message}", message);
            }
        }

        private async Task ProcessDeleteOrderMessageAsync(string message, OrderDbContext dbContext)
        {
            _logger.LogInformation("Processing DeleteOrderMessage: {message}", message);
 
            // Adicionar ao contexto do EF Core e salvar no banco de dados dentro de uma transação
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {

                var orderId = int.Parse(message);
                var order = await dbContext.Order.FindAsync(orderId);
                if (order != null)
                {
                    dbContext.Order.Remove(order);
                    await dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                    _logger.LogInformation("DeleteOrderMessage processed and deleted from database.");
                }
                else
                {
                    _logger.LogWarning("Order with ID {OrderId} not found for deletion.", orderId);
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error processing DeleteOrderMessage: {message}", message);
            }
        }

        private async Task ProcessUpdateOrderMessageAsync(string message, OrderDbContext dbContext)
        {
            _logger.LogInformation("Processing UpdateOrderMessage: {message}", message);

            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                var updatedOrder = JsonConvert.DeserializeObject<OrderEntity>(message);
                var existingOrder = await dbContext.Order.FindAsync(updatedOrder.OrderId);
                if (existingOrder != null)
                {
                    existingOrder.Itens = updatedOrder.Itens;

                    await dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                    _logger.LogInformation("UpdateOrderMessage processed and updated in database.");
                }
                else
                {
                    _logger.LogWarning("Order with ID {OrderId} not found for update.", updatedOrder.OrderId);
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error processing UpdateOrderMessage: {message}", message);
            }
        }

        private async Task ProcessReadOrderMessageAsync(string message, OrderDbContext dbContext)
        {
            _logger.LogInformation("Processing ReadOrderMessage: {message}", message);

            var orderId = int.Parse(message); 

            try
            {
                var order = await dbContext.Order.FindAsync(orderId);
                if (order != null)
                {
                    _logger.LogInformation("ReadOrderMessage found order: {Order}", JsonConvert.SerializeObject(order));
                }
                else
                {
                    _logger.LogWarning("Order with ID {OrderId} not found for reading.", orderId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing ReadOrderMessage: {message}", message);
            }
        }

        private async Task ProcessCreateOrderMessageAsync(string message, OrderDbContext dbContext)
        {
            _logger.LogInformation("Processing CreateOrderMessage: {message}", message);

            var order = JsonConvert.DeserializeObject<OrderEntity>(message);

            // Adicionar ao contexto do EF Core e salvar no banco de dados dentro de uma transação
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                await dbContext.Order.AddAsync(order);
                await dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                _logger.LogInformation("CreateOrderMessage processed and saved to database.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error processing CreateOrderMessage: {message}", message);
            }
        }

        private async Task ProcessDeleteClientMessageAsync(string message, OrderDbContext dbContext)
        {
            _logger.LogInformation("Processing DeleteClientMessage: {message}", message);

            var clientId = int.Parse(message);

            // Adicionar ao contexto do EF Core e salvar no banco de dados dentro de uma transação
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                var client = await dbContext.Client.FindAsync(clientId);
                if (client != null)
                {
                    dbContext.Client.Remove(client);
                    await dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                    _logger.LogInformation("DeleteClientMessage processed and deleted from database.");
                }
                else
                {
                    _logger.LogWarning("Client with ID {ClientId} not found for deletion.", clientId);
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error processing DeleteClientMessage: {message}", message);
            }
        }

        private async Task ProcessUpdateClientMessageAsync(string message, OrderDbContext dbContext)
        {
            _logger.LogInformation("Processing UpdateClientMessage: {message}", message);

            var updatedClient = JsonConvert.DeserializeObject<ClientEntity>(message);

            // Adicionar ao contexto do EF Core e salvar no banco de dados dentro de uma transação
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                var existingClient = await dbContext.Client.FindAsync(updatedClient.ClientId);
                if (existingClient != null)
                {
                    existingClient.User = updatedClient.User;

                    await dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                    _logger.LogInformation("UpdateClientMessage processed and updated in database.");
                }
                else
                {
                    _logger.LogWarning("Client with ID {ClientId} not found for update.", updatedClient.ClientId);
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error processing UpdateClientMessage: {message}", message);
            }
        }

        private async Task ProcessReadClientMessageAsync(string message, OrderDbContext dbContext)
        {
            _logger.LogInformation("Processing ReadClientMessage: {message}", message);

            var clientId = int.Parse(message); 

            try
            {
                var client = await dbContext.Client.FindAsync(clientId);
                if (client != null)
                {
                    _logger.LogInformation("ReadClientMessage found client: {Client}", JsonConvert.SerializeObject(client));
                }
                else
                {
                    _logger.LogWarning("Client with ID {ClientId} not found for reading.", clientId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing ReadClientMessage: {message}", message);
            }
        }


        private async Task ProcessCreateClientMessageAsync(string message, OrderDbContext dbContext)
        {
            _logger.LogInformation("Processing CreateClientMessage: {message}", message);

            var client = JsonConvert.DeserializeObject<ClientEntity>(message);

            // Adicionar ao contexto do EF Core e salvar no banco de dados dentro de uma transação
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                await dbContext.Client.AddAsync(client);
                await dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                _logger.LogInformation("CreateClientMessage processed and saved to database.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error processing CreateClientMessage: {message}", message);
            }
        }
    }
}
