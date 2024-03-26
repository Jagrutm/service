using System;
using System.Data;
using System.Data.SqlClient;

namespace CredECard.Common.BusinessService
{
	public class DataController
    {
		protected SqlConnection _currentConnection = null;
		//

  //      public DataController()
  //      {
  //          
  //      }
		//public DataController()
		//{
		//}
		public bool IsDBStarted { get; private set; } = false;

		public virtual void AddCommand(SqlCommand command)
		{
			if (command == null) throw new Exception("Null command passed");
			command.Connection = _currentConnection;
		}

		public void StartDatabase()
		{
			StartDatabase(CredECardConfig.WriteConnectionString);
		}

		////public static string ConnectionString
		////{
		////	get { return this.databaseConfiguration.ConnectionString; }
		////}

		public void StartDatabase(string connection)
		{
			if (IsDBStarted) throw new Exception("DB already started");
			try
			{
				_currentConnection = new SqlConnection(connection);
				_currentConnection.Open();
				this.IsDBStarted = true;
			}
			catch (Exception ex)
			{
				if (_currentConnection != null && _currentConnection.State != ConnectionState.Closed) _currentConnection.Close();
				if (_currentConnection != null) _currentConnection.Dispose();

				throw ex;
			}
		}

		public void EndDatabase()
		{
			if (!IsDBStarted) throw new Exception("DB is not started yet");
			if (_currentConnection != null && _currentConnection.State != ConnectionState.Closed) _currentConnection.Close();
			if (_currentConnection != null) _currentConnection.Dispose();
			this.IsDBStarted = false;
		}


		public SqlDataReader ExecuteCommandReader(SqlCommand myCommand)
		{
			string connectionstring = CredECard.Common.BusinessService.CredECardConfig.WriteConnectionString;
			return ExecuteCommandReader(myCommand, connectionstring);
		}

		//public SqlDataReader ExecuteCommandReader(SqlCommand myCommand, string connectionString)
		//{
		//	if (string.IsNullOrEmpty(connectionString)) throw new Exception("connectiono string must be specified");

		//	if (myCommand == null) throw new Exception("command cannot be null");
		//	// Mark the Command as a SPROC
		//	SqlConnection myConnection = new SqlConnection(connectionString);
		//	myCommand.Connection = myConnection;
		//	myCommand.CommandType = CommandType.StoredProcedure;

		//	SqlDataReader myReader = null;
		//	try
		//	{
		//		myConnection.Open();
		//		myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
		//	}
		//	catch (Exception ex)
		//	{ }
		//	//finally
		//	//{
		//	//if (myConnection.State == ConnectionState.Open) myConnection.Close();
		//	//}
		//	return myReader;
		//}

		////public static SqlDataReader ExecuteCommandReader(SqlCommand myCommand)
		////{
		////	string connectionstring = Configuration.GetConnectionString("userdb");
		////	return ExecuteCommandReader(myCommand, connectionstring);
		////}

		//public static SqlDataReader ExecuteCommandReader(string procedureName, SqlCommand myCommand)
		//      {
		//          myCommand.CommandText = procedureName;
		//          string connectionstring = Configuration.GetConnectionString("userdb");

		//	return ExecuteCommandReader(myCommand,connectionstring);
		//}

		public static SqlDataReader ExecuteCommandReader(SqlCommand myCommand, string connectionString)
		{
			if (string.IsNullOrEmpty(connectionString)) throw new Exception("connectiono string must be specified");

			if (myCommand == null) throw new Exception("command cannot be null");
			// Mark the Command as a SPROC
			SqlConnection myConnection = new SqlConnection(connectionString);
			myCommand.Connection = myConnection;
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlDataReader myReader = null;
			try
			{
				myConnection.Open();
				myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch (Exception){}
			//finally
			//{
			//if (myConnection.State == ConnectionState.Open) myConnection.Close();
			//}
			return myReader;
		}

		//public static SqlDataReader ExecuteNonQueryReader(SqlCommand myCommand, string connectionString)
		//{
		//	if (string.IsNullOrEmpty(connectionString)) throw new Exception("connectiono string must be specified");

		//	if (myCommand == null) throw new Exception("command cannot be null");
		//	// Mark the Command as a SPROC
		//	SqlConnection myConnection = new SqlConnection(connectionString);

		//	myCommand.CommandType = CommandType.StoredProcedure;

		//	SqlDataReader myReader = null;
		//	try
		//	{
		//		myConnection.Open();
		//		myReader = myCommand.ExecuteNonQuery(); // (CommandBehavior.CloseConnection);
		//	}
		//	//catch (Exception ex)
		//	//{ }
		//	finally
		//	{
		//		if (myConnection.State == ConnectionState.Open) myConnection.Close();
		//	}
		//	return myReader;
		//}

	}
}
