using BuildingBlocks.Infrastructure.Contexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace BuildingBlocks.Tests.TestFixtures
{
    public class InMemorySqliteEFDbContextTestFixture<TDbContext>
        : IDisposable where TDbContext : EntityFrameworkDbContext
    {
        private readonly DbConnection _connection;

        public InMemorySqliteEFDbContextTestFixture()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            InitializeDatabase();
        }

        public async Task ExecuteBaseAsync(Func<TDbContext, Task> action)
        {
            using var dbContext = CreateDbContext();
            using var dbContextTransaction = dbContext.Database.BeginTransaction();
            try
            {
                await action(dbContext);
                await dbContextTransaction.CommitAsync();
            }
            catch (Exception)
            {
                await dbContextTransaction.RollbackAsync();
                throw new Exception();
            }
        }

        public void Dispose()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }

            _connection.Dispose();
        }

        private void InitializeDatabase()
        {
            var dbContext = CreateDbContext();

            dbContext?.Database.EnsureDeleted();
            dbContext?.Database.EnsureCreated();
        }

        private TDbContext CreateDbContext()
        {
            var dbContextOptions = new DbContextOptionsBuilder<TDbContext>()
                .UseSqlite(_connection)
                .Options;
            return (TDbContext)Activator.CreateInstance(typeof(TDbContext), dbContextOptions);
        }        
    }
}
