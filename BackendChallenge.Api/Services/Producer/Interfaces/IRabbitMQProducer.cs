namespace BackendChallenge.Api.Services.Producer.Interfaces
{
    public interface IRabbitMQProducer
    {
        public void SendMessage<T>(T message);

        public void SendCommand<T>(string method, T command);
    }
}
