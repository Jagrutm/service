using CredECard.BugReporting.BusinessService;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CredECard.BugReporting.DataService
{
    /// <summary>
    /// DataService to read bugs from the database
    /// </summary>
    public class ReadBugReport
    {
        #region Constructor
        /// <author>Sejal Maheshwari</author>
        /// <created>13-Oct-2006</created>
        /// <summary>Constructor to create an object
        /// </summary>
        public ReadBugReport()
        { }
        #endregion

        /// <author>Sejal Maheshwari</author>
        /// <created>13-Oct-2006</created>
        /// <summary>Gets BugReport object for specified unique bug ID.
        /// </summary>
        /// <param name="bugID">Unique bug ID to get object of BugReport</param>
        /// <returns>Object of BugReport</returns>
        public static BugReport Specific(int bugID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetBugs", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterBugID = new SqlParameter("@intBugID", SqlDbType.Int, 4);
            parameterBugID.Value = bugID;
            myCommand.Parameters.Add(parameterBugID);


            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            BugReport data = null;
            try
            {
                if (myReader.Read())
                {
                    data = new BugReport();
                    SetValues(data, myReader);
                }
            }
            finally
            {
                if (myReader != null) myReader.Close();
            }
            return data;
        }

        /// <summary>
        /// Sets the value of properties of BugReport object from database.
        /// </summary>
        /// <param name="data">Object of BugReport</param>
        /// <param name="myReader">SqlDataReader object</param>
        private static void SetValues(BugReport data, SqlDataReader myReader)
        {
            data._bugID = (int)myReader["intBugID"];
            if (myReader["binaryException"] != DBNull.Value)
            {
                data._exceptionBytes = (Byte[])myReader["binaryException"];
                if (data._exceptionBytes.Length > 0)
                {
                    MemoryStream ms = new MemoryStream(data._exceptionBytes);
                    BinaryFormatter bf = new BinaryFormatter();
                    Exception objEx = (Exception)bf.Deserialize(ms);
                    data.ExceptionObject = objEx;
                }
            }

            if (myReader["strUserComments"] != DBNull.Value) data.UserComment = (string)myReader["strUserComments"];
            if (myReader["strUserEmail"] != DBNull.Value) data.Email = (string)myReader["strUserEmail"];
            if (myReader["intCECSystemID"] != DBNull.Value) data._cecSystemID = (int)myReader["intCECSystemID"];
            if (myReader["strProject"] != DBNull.Value) data.Project = (string)myReader["strProject"];
            if (myReader["strArea"] != DBNull.Value) data._area = (string)myReader["strArea"];
            if (myReader["strMachineName"] != DBNull.Value) data._machineName= (string)myReader["strMachineName"];
            if (myReader["strIPAddress"] != DBNull.Value) data._ipAddress = (string)myReader["strIPAddress"];
            if (myReader["dteCreatedDate"] != DBNull.Value) data._createdDate = (DateTime)myReader["dteCreatedDate"];
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>04-sep-2007</created>
        /// <summary>Gets list of BugReport object between the specified dates
        /// </summary>
        /// <param name="fromDate">Starting date of the range</param>
        /// <param name="toDate">Ending date of the range</param>
        /// <returns>List of BugReport object</returns>
        public static BugReportList GetBugsByDate(DateTime fromDate, DateTime toDate)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetBugsByDate", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter fromDateParam = new SqlParameter("@dteFromDate", SqlDbType.DateTime);
            fromDateParam.Value = fromDate;
            myCommand.Parameters.Add(fromDateParam);

            SqlParameter toDateParam = new SqlParameter("@dteToDate", SqlDbType.DateTime);
            toDateParam.Value = toDate;
            myCommand.Parameters.Add(toDateParam);

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            BugReport data = null;
            BugReportList listData = null;

            try
            {
                while (myReader.Read())
                {
                    if (listData == null) listData = new BugReportList();
                    data = new BugReport();
                    SetValues(data, myReader);
                 
                    listData.Add(data);
                }
            }
            finally
            {
                if (myReader != null) myReader.Close();
            }

            return listData;
        }


        /// <author>Tina Mori</author>
        /// <created>29-Apr-2009</created>
        /// <summary>Gets all bugs in db to be posted to bugscout
        /// </summary>
        /// <returns>List of BugReport object
        /// </returns>
        internal static BugReportList GetBugs()
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetBugs", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            BugReport data = null;
            BugReportList list = null;
            try
            {
                while (myReader.Read())
                {
                    data = new BugReport();
                    SetValues(data, myReader);

                    if (list == null) list = new BugReportList();
                    list.Add(data);
                }
            }
            finally
            {
                if (myReader != null) myReader.Close();
            }
            return list;
        }
    }
}
