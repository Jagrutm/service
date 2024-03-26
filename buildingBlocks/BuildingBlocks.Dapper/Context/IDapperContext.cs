using SqlKata.Compilers;
using System.Data;

namespace BuildingBlocks.Dapper.Contexts
{
    public interface IDapperContext
    {
        event EventHandler TransactionCommitted;

        event EventHandler TransactionRollbacked;

        IDbConnection Connection { get; }

        IDbTransaction Transaction { get; }

        Compiler Compiler { get; }

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();

        Task<IDbTransaction> BeginTransactionAsync();
    }
}
