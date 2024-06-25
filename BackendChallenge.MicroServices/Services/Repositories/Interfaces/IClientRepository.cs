using BackendChallenge.Api.Models.Entity;

namespace BackendChallenge.Api.Services.Repositories.Interfaces
{
    public interface IClientRepository
    {
        Task<int> CreateClientAsync(ClientEntity client);

        Task<ClientEntity> GetClientAsync(int clientId);

        Task<int> UpdateClientAsync(ClientEntity client);

        Task<int> DeleteClientAsync(ClientEntity client);

        Task<IEnumerable<ClientEntity>> GetAllClientsAsync();

    }
}
