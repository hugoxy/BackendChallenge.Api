using BackendChallenge.Api.Facades.Interfaces;
using BackendChallenge.Api.Facades;
using BackendChallenge.Api.Services.Producer;
using BackendChallenge.Api.Services.Producer.Interfaces;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Backend Challenge API",
        Version = "v1",
        Description = "API for managing client operations"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();
builder.Services.AddSingleton<IProducerFacade, ProducerFacade>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend Challenge API V1");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
