using CredECard.BugReporting.BusinessService;
using System;
using System.Data;
using System.Data.SqlClient;

namespace CredECard.BugReporting.DataService
{
    internal class ReadSystemErrorLogs
    {
        /// <author>Rishit Rajput</author>
        /// <created>09-Sep-2010</created>
        /// <summary>
        /// Read data from DB
        /// </summary>
        /// <param name="data">SystemErrorLogs</param>
        internal static SystemErrorLogs ReadSpecific(int id)
        {
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetSystemErrorLogs", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramID = new SqlParameter("@intSystemErrorLogID", SqlDbType.Int, 4);
            paramID.Value = id;
            myCommand.Parameters.Add(paramID);

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            SystemErrorLogs data = null;

            try
            {
                if (myReader.Read())
                {
                    data = new SystemErrorLogs();

                    setValues(myReader, data);
                }
            }
            finally
            {
                if (myReader != null) myReader.Close();
            }

            return data;
        }

        /// <author>Rishit Rajput</author>
        /// <created>09-Sep-2010</created>
        /// <summary>
        /// Read data list from DB
        /// </summary>
        /// <param name="data">SystemErrorLogs</param>
        internal static SystemErrorLogsList ReadList()
        {
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetSystemErrorLogs", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            SystemErrorLogs data = null;
            SystemErrorLogsList list = null;
            try
            {
                while (myReader.Read())
                {
                    if (list == null) list = new SystemErrorLogsList();
                    data = new SystemErrorLogs();

                    setValues(myReader, data);

                    list.Add(data);
                }
            }
            finally
            {
                if (myReader != null) myReader.Close();
            }

            return list;
        }


        /// <author>Rishit Rajput</author>
        /// <created>09-Sep-2010</created>
        /// <summary>
        /// Read data from DB
        /// </summary>
        /// <param name="data">set object value</param>
        private static void setValues(SqlDataReader myReader, SystemErrorLogs data)
        {
            data._systemErrorLogID = (int)myReader["intSystemErrorLogID"];
            if (!(myReader["strUserComments"] is DBNull))
                data._userComments = (string)myReader["strUserComments"];
            if (!(myReader["strUserEmail"] is DBNull))
                data._userEmail = (string)myReader["strUserEmail"];
            if (!(myReader["strErrorMessage"] is DBNull))
                data._errorMessage = (string)myReader["strErrorMessage"];
            if (!(myReader["strStackTrace"] is DBNull))
                data._stackTrace = (string)myReader["strStackTrace"];
            data._createdDate = (DateTime)myReader["dteCreatedDate"];
            if (!(myReader["intCECSystemID"] is DBNull))
                data._cECSystemID = (int)myReader["intCECSystemID"];
            if (!(myReader["strProject"] is DBNull))
                data._project = (string)myReader["strProject"];
            if (!(myReader["strArea"] is DBNull))
                data._area = (string)myReader["strArea"];
            if (!(myReader["strMachineName"] is DBNull))
                data._machineName = (string)myReader["strMachineName"];
            if (!(myReader["strIPAddress"] is DBNull))
                data._iPAddress = (string)myReader["strIPAddress"];
        }
    }
}
