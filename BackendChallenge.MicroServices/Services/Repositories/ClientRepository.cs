using BackendChallenge.Api.Models.Entity;
using BackendChallenge.Api.Services.Database;
using BackendChallenge.Api.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendChallenge.Api.Services.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly OrderDbContext _context;

        public ClientRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateClientAsync(ClientEntity client)
        {
            await _context.Client.AddAsync(client);
            return await _context.SaveChangesAsync();
        }

        public async Task<ClientEntity> GetClientAsync(int clientId)
        {
            return await _context.Client.FirstOrDefaultAsync(c => c.ClientId == clientId);
        }

        public async Task<int> UpdateClientAsync(ClientEntity client)
        {
            _context.Client.Update(client);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteClientAsync(ClientEntity client)
        {
            _context.Client.Remove(client);
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ClientEntity>> GetAllClientsAsync()
        {
            return await _context.Client.ToListAsync();
        }
    }
}