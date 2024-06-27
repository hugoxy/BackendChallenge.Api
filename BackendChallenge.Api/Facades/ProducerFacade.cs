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
    }
}
