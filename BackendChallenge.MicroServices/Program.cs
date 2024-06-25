using BackendChallenge.Api.Services.Database;
using BackendChallenge.Api.Services.Repositories.Interfaces;
using BackendChallenge.Api.Services.Repositories;
using BackendChallenge.MicroServices.Consumers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BackendChallenge.MicroServices.Consumers.Extensions;

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
//builder.Services.AddScoped<RabbitMQConsumer>();
builder.Services.AddRabbitMQConsumer();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

var consumer = app.Services.GetRequiredService<RabbitMQConsumer>();
consumer.StartConsuming();

app.Run();
