using BackendChallenge.Api.Services.Database;
using BackendChallenge.Api.Services.Repositories.Interfaces;
using BackendChallenge.Api.Services.Repositories;
using BackendChallenge.MicroServices.Consumers;
using Microsoft.EntityFrameworkCore;
using BackendChallenge.MicroServices.Consumers.Extensions;
using BackendChallenge.MicroServices.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var cs = builder.Configuration.GetConnectionString("PostgreSQL");
builder.Services.AddEntityFrameworkNpgsql()
               .AddDbContext<OrderDbContext>(options => options
                   .UseNpgsql(
                       cs,
                       npgsqlOptions =>
                       {
                           npgsqlOptions.MigrationsAssembly(typeof(OrderDbContext).Assembly.FullName);
                       })
               );

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();

// Configuração do RabbitMQ
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddSingleton<RabbitMQConsumer>();
builder.Services.AddSingleton<IHostedService, RabbitMQConsumerHostedService>();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

