using System;
using System.Data;
using System.Data.SqlClient;

namespace CredECard.Common.BusinessService
{
	public class SafeDataController : DataController
	{
		private SqlTransaction _currentTransaction = null;

		//public SafeDataController() : base()
		//{

		//}

		//public SafeDataController()
		//{

		//}

		/// <summary>
		/// Assigns the provided command an existing connection. If no connection exists then throws exception
		/// </summary>
		/// <param name="command">SqlCommand</param>
		public override void AddCommand(SqlCommand command)
		{
			if (!InTransaction) throw new Exception("Command must be in transaction");
			base.AddCommand(command);
			command.Transaction = _currentTransaction;
		}

		/// <summary>
		/// Begins transactions in an opened connection
		/// </summary>
		/// <param name="tranName">string</param>
		public void BeginTransaction(string tranName)
		{
			try
			{
				
				if (_currentConnection == null || _currentConnection.State == ConnectionState.Closed) throw new Exception("There is no open connection");
				_currentTransaction = _currentConnection.BeginTransaction(IsolationLevel.ReadUncommitted, tranName);
				InTransaction = true;
			}
			catch (Exception ex)
			{
				InTransaction = false;

				if (_currentTransaction != null) _currentTransaction.Dispose();

				throw ex;
			}
		}

		/// <summary>
		/// Opens Connection and begins transaction
		/// </summary>
		/// <param name="connection">string</param>
		/// <param name="tranName">string</param>
		public void BeginTransaction(string connection, string tranName)
		{
			if (InTransaction) throw new Exception("Already in a transaction scope");
			this.StartDatabase(connection);
			this.BeginTransaction(tranName);
		}

		/// <summary>
		/// Commits the transaction and ends connection
		/// </summary>
		public void CommitTransaction()
		{
			this.CommitTransaction(true);
		}

		/// <summary>
		/// Commits transaction and ends connection based on value of endconnection
		/// </summary>
		/// <param name="endConnection">bool</param>
		public void CommitTransaction(bool endConnection)
		{
			if (!InTransaction) throw new Exception("Command must be in transaction");

			_currentTransaction.Commit();

			if (_currentTransaction != null) _currentTransaction.Dispose();
			if (endConnection) this.EndDatabase();

			InTransaction = false; //Rajeshree Gajjar - Case : 90058
		}

		/// <summary>
		/// Roll back trans and ends connection
		/// </summary>
		public void RollbackTransaction()
		{
			this.RollbackTransaction(true);
		}

		/// <summary>
		/// Roll back trans and ends connection based on value of endconnection
		/// </summary>
		/// <param name="endConnection">bool</param>
		public void RollbackTransaction(bool endConnection)
		{
			if (!InTransaction) throw new Exception("Command must be in transaction");

			_currentTransaction.Rollback();

			if (_currentTransaction != null) _currentTransaction.Dispose();
			if (endConnection) this.EndDatabase();

			InTransaction = false; //Rajeshree Gajjar - Case : 90058
		}

		/// <summary>
		/// Gets whether a transaction exists
		/// </summary>
		public bool InTransaction { get; private set; } = false;
	}
}
