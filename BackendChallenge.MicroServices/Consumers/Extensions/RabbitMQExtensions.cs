namespace BackendChallenge.MicroServices.Consumers.Extensions
{
    public static class RabbitMQExtensions
    {
        public static IServiceCollection AddRabbitMQConsumer(this IServiceCollection services)
        {
            services.AddScoped<RabbitMQConsumer>(); 

            services.AddTransient(provider =>
            {
                var consumer = provider.GetRequiredService<RabbitMQConsumer>();
                consumer.StartConsuming();
                return consumer;
            });

            return services;
        }
    }

}
