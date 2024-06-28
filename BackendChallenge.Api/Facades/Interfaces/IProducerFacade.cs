namespace BackendChallenge.Api.Facades.Interfaces
{
    public interface IProducerFacade
    {
        void SendCommand<T>(string method, T message);

        Task<string> SendCommandAndWaitForResponseAsync<T>(string methodName, T message);
    }
}
