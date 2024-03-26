using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Infrastructure.Contexts
{
    public class EntityFrameworkDbContext : DbContext
    {
        protected string ConnectionString { get; }

        public EntityFrameworkDbContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public EntityFrameworkDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrWhiteSpace(ConnectionString))
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }
    }
}
