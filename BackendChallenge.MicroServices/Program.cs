using BackendChallenge.Api.Services.Database;
using BackendChallenge.MicroServices.Consumers;
using Microsoft.EntityFrameworkCore;

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

builder.Services.AddSingleton<RabbitMQConsumer>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

var consumer = app.Services.GetRequiredService<RabbitMQConsumer>();
consumer.StartConsuming("nome_da_fila"); // Substitua "nome_da_fila" pelo nome da fila que deseja ouvir

app.Run();
