using BackendChallenge.Api.Facades.Interfaces;
using BackendChallenge.Api.Facades;
using BackendChallenge.Api.Services.Database;
using BackendChallenge.Api.Services.Producer;
using BackendChallenge.Api.Services.Producer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

builder.Services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();
builder.Services.AddSingleton<IProducerFacade, ProducerFacade>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
