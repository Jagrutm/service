using CredECard.BugReporting.BusinessService;
using CredECard.Common.BusinessService;
using System.Data;
using System.Data.SqlClient;

namespace CredECard.BugReporting.DataService
{
    internal class WriteSystemErrorLogs : StandardDataService
    {

        internal WriteSystemErrorLogs(DataController conn)
            : base(conn)
        {
        }

        /// <author>Rishit Rajput</author>
        /// <created>09-Sep-2010</created>
        /// <summary>
        /// Write data to DB
        /// </summary>
        /// <param name="data">SystemErrorLogs</param>
        internal void WriteData(SystemErrorLogs data)
        {

            SqlCommand myCommand = new SqlCommand("spSaveSystemErrorLogs");
            myCommand.CommandType = CommandType.StoredProcedure;


            SqlParameter paramSystemErrorLogID = new SqlParameter("@intSystemErrorLogID", SqlDbType.Int, 4);
            paramSystemErrorLogID.Value = data._systemErrorLogID;
            paramSystemErrorLogID.Direction = ParameterDirection.InputOutput;
            myCommand.Parameters.Add(paramSystemErrorLogID);

            if (data._userComments != string.Empty)
            {
                SqlParameter paramUserComments = new SqlParameter("@strUserComments", SqlDbType.NVarChar, 2000);
                paramUserComments.Value = data._userComments;
                myCommand.Parameters.Add(paramUserComments);
            }

            if (data._userEmail != string.Empty)
            {
                SqlParameter paramUserEmail = new SqlParameter("@strUserEmail", SqlDbType.NVarChar, 100);
                paramUserEmail.Value = data._userEmail;
                myCommand.Parameters.Add(paramUserEmail);
            }

            if (data._errorMessage != string.Empty)
            {
                SqlParameter paramErrorMessage = new SqlParameter("@strErrorMessage", SqlDbType.NVarChar);
                paramErrorMessage.Value = data._errorMessage;
                myCommand.Parameters.Add(paramErrorMessage);
            }

            if (data._stackTrace != string.Empty)
            {
                SqlParameter paramStackTrace = new SqlParameter("@strStackTrace", SqlDbType.NVarChar);
                paramStackTrace.Value = data._stackTrace;
                myCommand.Parameters.Add(paramStackTrace);
            }

            if (data._cECSystemID > 0)
            {
                SqlParameter paramCECSystemID = new SqlParameter("@intCECSystemID", SqlDbType.Int, 4);
                paramCECSystemID.Value = data._cECSystemID;
                myCommand.Parameters.Add(paramCECSystemID);
            }

            if (data._project != string.Empty)
            {
                SqlParameter paramProject = new SqlParameter("@strProject", SqlDbType.NVarChar, 20);
                paramProject.Value = data._project;
                myCommand.Parameters.Add(paramProject);
            }

            if (data._area != string.Empty)
            {
                SqlParameter paramArea = new SqlParameter("@strArea", SqlDbType.NVarChar, 20);
                paramArea.Value = data._area;
                myCommand.Parameters.Add(paramArea);
            }

            if (data._machineName != string.Empty)
            {
                SqlParameter paramMachineName = new SqlParameter("@strMachineName", SqlDbType.NVarChar, 20);
                paramMachineName.Value = data._machineName;
                myCommand.Parameters.Add(paramMachineName);
            }

            if (data._iPAddress != string.Empty)
            {
                SqlParameter paramIPAddress = new SqlParameter("@strIPAddress", SqlDbType.NVarChar, 20);
                paramIPAddress.Value = data._iPAddress;
                myCommand.Parameters.Add(paramIPAddress);
            }

            try
            {
                this.Controller.AddCommand(myCommand);
                myCommand.ExecuteNonQuery();

                data._systemErrorLogID = (int)paramSystemErrorLogID.Value;
            }
            finally
            {
                myCommand.Dispose();
            }
        }
    }
}
