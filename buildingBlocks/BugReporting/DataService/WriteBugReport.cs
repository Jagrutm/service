using CredECard.BugReporting.BusinessService;
using CredECard.Common.BusinessService;
using System;
using System.Data;
using System.Data.SqlClient;

namespace CredECard.BugReporting.DataService
{
    /// <author>Sejal Maheshwari</author>
    /// <created>06-Oct-2006</created>
    /// <summary>
    /// DataService to write bugs into database
    /// </summary>
    public class WriteBugReport : StandardDataService
    {
        #region Constructor
        /// <author>Sejal Maheshwari</author>
        /// <created>06-Oct-2006</created>
        /// <summary>Constructor to create object with DataController
        /// </summary>
        /// <param name="data">Object of DataController
        /// </param>
        public WriteBugReport(DataController data)
            : base(data)
        {
        }
        #endregion

        #region Public Methods

        /// <author>Sejal Maheshwari</author>
        /// <created>12-Oct-2006</created>
        /// <summary>Saves bug into the database
        /// </summary>
        /// <param name="data">Object of BugReport
        /// </param>
        public void SaveBug(BugReport data)
        {
            // Create Instance of Connection and Command Object
            SqlCommand myCommand = new SqlCommand("spSaveBug");

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC

            SqlParameter parameterBugID = new SqlParameter("@intBugID", SqlDbType.Int, 4);
            parameterBugID.Direction = ParameterDirection.InputOutput;
            parameterBugID.Value = data.BugID;
            myCommand.Parameters.Add(parameterBugID);

            if (data.BugID == 0)
            {
                SqlParameter parameterExceptionObj = new SqlParameter("@binaryException", SqlDbType.VarBinary);
                parameterExceptionObj.Value = data._exceptionBytes;
                myCommand.Parameters.Add(parameterExceptionObj);
            }

            if (data._userComment != string.Empty)
            {
                SqlParameter parameterUserComment = new SqlParameter("@strUserComments", SqlDbType.NVarChar, 2000);
                parameterUserComment.Value = data._userComment;
                myCommand.Parameters.Add(parameterUserComment);
            }

            if (data.Email != null || data.Email != string.Empty)
            {
                SqlParameter parameterUserEmail = new SqlParameter("@strUserEmail", SqlDbType.NVarChar, 100);
                parameterUserEmail.Value = data.Email;
                myCommand.Parameters.Add(parameterUserEmail);
            }

            if (data.Area != string.Empty)
            {
                SqlParameter parameterArea = new SqlParameter("@strArea", SqlDbType.NVarChar, 20);
                parameterArea.Value = data.Area;
                myCommand.Parameters.Add(parameterArea);
            }

            if (data.Project != string.Empty)
            {
                SqlParameter parameterProject = new SqlParameter("@strProject", SqlDbType.NVarChar, 20);
                parameterProject.Value = data.Project;
                myCommand.Parameters.Add(parameterProject);
            }

            if (data.MachineName != string.Empty)
            {
                SqlParameter parameterMachineName = new SqlParameter("@strMachineName", SqlDbType.NVarChar, 20);
                parameterMachineName.Value = data.MachineName;
                myCommand.Parameters.Add(parameterMachineName);
            }

            if (data.IPAddress != string.Empty)
            {
                SqlParameter parameterIPAddress = new SqlParameter("@strIPAddress", SqlDbType.NVarChar, 20);
                parameterIPAddress.Value = data.IPAddress;
                myCommand.Parameters.Add(parameterIPAddress);
            }

            SqlParameter parameterCECSystemID = new SqlParameter("@intCECSystemID", SqlDbType.SmallInt, 2);
            parameterCECSystemID.Value = data.CECSystemID;
            myCommand.Parameters.Add(parameterCECSystemID);


            //Sejal Maheshwari : 20 Dec 2006 : New field @dteCreatedDate added in tblbugs
            SqlParameter parameterCreatedDate = new SqlParameter("@dteCreatedDate", SqlDbType.DateTime);
            parameterCreatedDate.Value = DateTime.Now;
            myCommand.Parameters.Add(parameterCreatedDate);

            try
            {
                this.Controller.AddCommand(myCommand);
                myCommand.ExecuteNonQuery();
                data.BugID = (int)parameterBugID.Value;
            }
            finally
            {
                myCommand.Dispose();
            }
        }

        /// <author>Denish Makwna</author>
        /// <created>14-Dec-2020</created>
        /// <summary>Saves buginfo into the database
        /// </summary>
        /// <param name="data">Object of BugReport</param>
        public void SaveBugInfo(BugReport data)
        {
            // Create Instance of Connection and Command Object
            SqlCommand myCommand = new SqlCommand("spSaveBugInfo");

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC

            SqlParameter parameterBugInfoID = new SqlParameter("@intBugInfoID", SqlDbType.Int, 4);
            parameterBugInfoID.Direction = ParameterDirection.InputOutput;
            parameterBugInfoID.Value = data.BugInfoID;
            myCommand.Parameters.Add(parameterBugInfoID);

            SqlParameter parameterExceptionObj = new SqlParameter("@binaryException", SqlDbType.VarBinary);
            parameterExceptionObj.Value = data._exceptionBytes;
            myCommand.Parameters.Add(parameterExceptionObj);            

            if (!string.IsNullOrEmpty(data._userComment))
            {
                SqlParameter parameterUserComment = new SqlParameter("@strUserComments", SqlDbType.NVarChar, 2000);
                parameterUserComment.Value = data._userComment;
                myCommand.Parameters.Add(parameterUserComment);
            }

            if (!string.IsNullOrEmpty(data.Email))
            {
                SqlParameter parameterUserEmail = new SqlParameter("@strUserEmail", SqlDbType.NVarChar, 100);
                parameterUserEmail.Value = data.Email;
                myCommand.Parameters.Add(parameterUserEmail);
            }

            if (!string.IsNullOrEmpty(data.Area))
            {
                SqlParameter parameterArea = new SqlParameter("@strArea", SqlDbType.NVarChar, 20);
                parameterArea.Value = data.Area;
                myCommand.Parameters.Add(parameterArea);
            }

            if (!string.IsNullOrEmpty(data.Project))
            {
                SqlParameter parameterProject = new SqlParameter("@strProject", SqlDbType.NVarChar, 20);
                parameterProject.Value = data.Project;
                myCommand.Parameters.Add(parameterProject);
            }

            if (!string.IsNullOrEmpty(data.MachineName))
            {
                SqlParameter parameterMachineName = new SqlParameter("@strMachineName", SqlDbType.NVarChar, 20);
                parameterMachineName.Value = data.MachineName;
                myCommand.Parameters.Add(parameterMachineName);
            }

            if (!string.IsNullOrEmpty(data.IPAddress))
            {
                SqlParameter parameterIPAddress = new SqlParameter("@strIPAddress", SqlDbType.NVarChar, 20);
                parameterIPAddress.Value = data.IPAddress;
                myCommand.Parameters.Add(parameterIPAddress);
            }

            SqlParameter parameterCECSystemID = new SqlParameter("@intCECSystemID", SqlDbType.SmallInt, 2);
            parameterCECSystemID.Value = data.CECSystemID;
            myCommand.Parameters.Add(parameterCECSystemID);

            if (!string.IsNullOrEmpty(data.ErrorDescription))
            {
                SqlParameter parameterErrorDescription = new SqlParameter("@strErrorDescription", SqlDbType.NVarChar, 250);
                parameterErrorDescription.Value = data.ErrorDescription;
                myCommand.Parameters.Add(parameterErrorDescription);
            }

            try
            {
                this.Controller.AddCommand(myCommand);
                myCommand.ExecuteNonQuery();
                data.BugInfoID = (int)parameterBugInfoID.Value;
            }
            finally
            {
                myCommand.Dispose();
            }
        }
        #endregion

        /// <author>Tina Mori</author>
        /// <created>29-Apr-2009</created>
        /// <summary>Deletes specified bug
        /// </summary>
        /// <param name="bugID">Unique ID of bug to delete
        /// </param>
        internal void DeleteBug(int bugID)
        {
            // Create Instance of Connection and Command Object
            SqlCommand myCommand = new SqlCommand("spDeleteBug");

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterBugID = new SqlParameter("@intBugID", SqlDbType.Int, 4);
            parameterBugID.Value = bugID;
            myCommand.Parameters.Add(parameterBugID);

            try
            {
                this.Controller.AddCommand(myCommand);
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                myCommand.Dispose();
            }
        }
    }
}
