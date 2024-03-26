using BuildingBlocks.Dapper.Extensions;
using BuildingBlocks.Dapper.Extensions.Dialects;
using BuildingBlocks.Dapper.Extensions.Mappers;
using Microsoft.Data.SqlClient;
using SqlKata.Compilers;
using System.Data;

namespace BuildingBlocks.Dapper.Contexts
{
    public class MsSqlDapperContext : IDapperContext
    {
        private bool _isDisposed = false;
        private string _connectionString;
        private IDbConnection _connection;

        public MsSqlDapperContext(string connectionString, SqlServerCompiler sqlServerCompiler)
        {
            _connectionString = connectionString;

            // set the DapperExtension dialect.
            DapperExtensions.DefaultMapper = typeof(AutoModelMapper<>);
            DapperExtensions.SqlDialect = new SqlCeDialect();
            DapperAsyncExtensions.DefaultMapper = typeof(AutoModelMapper<>);
            DapperAsyncExtensions.SqlDialect = new SqlCeDialect();

            Compiler = sqlServerCompiler;
        }

        public event EventHandler TransactionCommitted;

        public event EventHandler TransactionRollbacked;

        public IDbConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    var connection = new SqlConnection(_connectionString);
                    _connection = connection;
                }

                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }

                return _connection;
            }
        }

        public IDbTransaction Transaction { get; private set; }

        public bool CanStartNewTransaction
        {
            get
            {
                return Transaction == null;
            }
        }

        public Compiler Compiler { get; }

        public virtual async Task<IDbTransaction> BeginTransactionAsync()
        {
            if (CanStartNewTransaction)
            {
                Transaction = await (Connection as SqlConnection).BeginTransactionAsync();
            }

            return Transaction;
        }

        public virtual async Task CommitTransactionAsync()
        {
            var transaction = Transaction as SqlTransaction;

            try
            {
                // Commit transaction
                await transaction.CommitAsync();
                // _logger.LogDebug($"Transaction {_transaction.GetHashCode()} was committed.");

                Transaction.Dispose();
                Transaction = null;

                // Send event
                TransactionCommitted?.Invoke(this, new EventArgs());
            }
            catch (Exception ex)
            {
                if (Transaction != null && Transaction.Connection != null)
                {
                    await RollbackTransactionAsync();
                }

                throw new NullReferenceException("Tried Commit on closed Transaction", ex);
            }
        }

        public virtual async Task RollbackTransactionAsync()
        {
            if (Transaction == null)
                return;

            var transaction = Transaction as SqlTransaction;

            try
            {
                // Rollback transaction
                await transaction.RollbackAsync();
                // _logger.LogDebug($"Transaction {_transaction.GetHashCode()} was rollbacked.");

                Transaction.Dispose();
                Transaction = null;

                // Send event
                TransactionRollbacked?.Invoke(this, new EventArgs());
            }
            catch (Exception ex)
            {
                throw new NullReferenceException("Tried Rollback on closed Transaction", ex);
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);

            // TODO: uncomment the following line if the finalizer is overridden above.

            // GC.SuppressFinalize(this);   
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (_isDisposed)
                return;
            if (isDisposing)
            {
                if (Transaction != null)
                {
                    var transaction = Transaction as SqlTransaction;
                    // _logger.LogDebug($"Transaction {transaction.GetHashCode()} was disposed without commit or rollback.");

                    Transaction.Dispose();
                    Transaction = null;
                }

                if (_connection != null && _connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                    _connection.Dispose();
                    _connection = null;
                }

                _isDisposed = true;
            }
        }

    }
}
