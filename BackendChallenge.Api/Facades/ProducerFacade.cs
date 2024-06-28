using BackendChallenge.Api.Facades.Interfaces;
using BackendChallenge.Api.Services.Producer.Interfaces;

namespace BackendChallenge.Api.Facades
{
    public class ProducerFacade : IProducerFacade
    {
        private readonly IRabbitMQProducer _rabbitMQProducer;

        public ProducerFacade(IRabbitMQProducer rabbitMQProducer)
        {
            _rabbitMQProducer = rabbitMQProducer;
        }

        public void SendCommand<T>(string methodName,T message)
        {
            _rabbitMQProducer.SendCommand(methodName, message);
        }

        public async Task<string> SendCommandAndWaitForResponseAsync<T>(string methodName, T message)
        {
            var response = await _rabbitMQProducer.SendCommandAndWaitForResponseAsync(methodName, message);
            return response;
        }
    }
}
