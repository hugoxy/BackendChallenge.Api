namespace BackendChallenge.Api.Facades.Interfaces
{
    public interface IProducerFacade
    {
        void SendMessage<T>(T message);

        void SendCommand<T>(string method, T message);
    }
}
