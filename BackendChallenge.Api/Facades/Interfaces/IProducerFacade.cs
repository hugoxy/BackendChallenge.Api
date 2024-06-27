namespace BackendChallenge.Api.Facades.Interfaces
{
    public interface IProducerFacade
    {
        void SendCommand<T>(string method, T message);
    }
}
