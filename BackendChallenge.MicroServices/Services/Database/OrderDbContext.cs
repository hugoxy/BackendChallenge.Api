using BackendChallenge.Api.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace BackendChallenge.Api.Services.Database
{
    public class OrderDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public OrderDbContext(DbContextOptions<OrderDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<OrderEntity> Order { get; set; }
        public DbSet<ClientEntity> Client { get; set; }
        public DbSet<ProductsEntity> Product { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostgreSQL"));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("BackendChallenge");
            base.OnModelCreating(modelBuilder);
        }
    }
}