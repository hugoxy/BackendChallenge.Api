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

        public void SendMessage<T>(T message)
        {
            _rabbitMQProducer.SendMessage(message);
        }

        public void SendCommand<T>(string method,T message)
        {
            _rabbitMQProducer.SendCommand(method,message);
        }
    }
}
