namespace BackendChallenge.Api.Services.Producer.Interfaces
{
    public interface IRabbitMQProducer
    {
        public void SendCommand<T>(string methodName, T command);

        public Task<string> SendCommandAndWaitForResponseAsync<T>(string methodName, T message);
    }
}
