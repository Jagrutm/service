using CredECard.Common.BusinessService;
using CredECard.CommonSetting.BusinessService;
using System;
using System.Configuration;

namespace CredECard.BugReporting.BusinessService
{
    /// <summary>
    /// Business service to post bug to bug scout
    /// </summary>
    public class PostToBugscout
    {
        #region Variables and Properties
        private const string _defaultFogbugzArea = "1. Undecided";
        private const string DEFAULTPROJECT = "Contis Banking Gateway";

        /// <summary>
        /// Gets the value of "FogbugzProjectName" setting from database
        /// </summary>
        private static string Project
        {
            get
            {
                string _project = string.Empty;
                //try
                //{
                //    _project = GeneralSetting.GetSettingValue((int)EnumGeneralSettings.FogbugzProjectName);
                //}
                //catch
                //{
                //}

                if (_project == string.Empty) _project = DEFAULTPROJECT;

                return _project;
            }
        }
        #endregion

        #region PostDataToBugScout
        /// <author>Rishit Rajput</author>
        /// <created>10-Apr-07</created>
        /// <summary>
        /// Saves bug to the database for occured exception.
        /// </summary>
        /// <param name="ex">Exception that occured</param>
        public static int PostDataToBugScout(Exception ex, bool isInfo = false)
        {   
            return PostToBugscout.PostDataToBugScout(ex, string.Empty, string.Empty, string.Empty, 0, string.Empty, null, isInfo);            
        }

        /// <author>Gaurang Majithiya</author>
        /// <created>26-Nov-09</created>
        /// <summary>
        /// Saves bug to the database for occured exception. This method can be used while calling thing assembly using reflection.
        /// e.g. It calls from MaestroCardSecurity and that calls from DTS using reflection.
        /// </summary>
        /// <param name="ex">Exception that occured</param>
        /// <param name="cecSystemID">CECSystemID</param>
        /// <param name="postToBugScoutValue">Post to bug scout is on or not</param>
        /// <param name="connString">Connection string</param>
        /// <returns>returns bug id if success otherwise 0</returns>
        public static int PostDataToBugScout(Exception ex, int cecSystemID, string postToBugscoutValue, string connString, bool isInfo = false)
        {
            DataController conn = new DataController();

            try
            {
                if (connString == string.Empty) connString = CredECardConfig.GetReadWriteConnectionString();

                conn.StartDatabase(connString);

                return PostToBugscout.PostDataToBugScout(ex, string.Empty, string.Empty, string.Empty, cecSystemID, postToBugscoutValue, conn, isInfo);
            }
            catch
            {
                return 0;
            }
            finally
            {
                conn.EndDatabase();
            }
        }

        /// <author>Tina Mori</author>
        /// <created>29-Apr-2009</created>
        /// <summary>Saves bug to the database
        /// </summary>
        /// <param name="ex">Object of Exception</param>
        /// <param name="extraInformation">Extra information of bug</param>
        /// <param name="description">Description of bug</param>
        /// <param name="userComment">Entered comment by user</param>
        /// <returns>Returns bug id if saved successfully, otherwise returns 0 (zero)</returns>
        public static int PostDataToBugScout(Exception ex, string extraInformation, string description, string userComment, bool isInfo = false, int cecSystemID = 0)
        {
            return PostToBugscout.PostDataToBugScout(ex, extraInformation, description, userComment, cecSystemID, string.Empty, null, isInfo);
        }

        /// <author>Gaurang Majithiya</author>
        /// <created>26-Nov-09</created>
        /// <summary>
        /// Saves bug to the database for occured exception.
        /// </summary>
        /// <param name="ex">Exception that occured</param>
        /// <param name="extraInformation">Extra information about the bug</param>
        /// <param name="description">Description of the bug</param>
        /// <param name="userComment">Comment entered by user</param>
        /// <param name="cecSystemID">CECSystemID</param>
        /// <param name="postToBugScoutValue">Post to bug scout is on or not</param>
        /// <param name="conn">Connection</param>
        /// <returns>returns bug id if success otherwise 0</returns>
        private static int PostDataToBugScout(Exception ex, string extraInformation, string description, string userComment, int cecSystemID, string postToBugScoutValue, DataController conn, bool isInfo)
        {
            try
            {
                //_postToBugscoutValue = postToBugScoutValue;

                //if (ex == null || PostToBugscoutValue != "1")
                //{
                //    return 0;
                //}

                if (ex == null) return 0;

                BugReport objReport = PostToBugscout.SaveBugReport(ex, extraInformation, description, userComment, cecSystemID, isInfo);

                if (conn == null)
                    objReport.Save();
                else
                    ((IPersistableV2)objReport).Save(conn);

                if (objReport.IsSaveBugInBugInfo)
                {
                    return objReport.BugInfoID;
                }
                else
                {
                    return objReport.BugID;
                }
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        /// <author>Gaurang Majithiya</author>
        /// <created>01-May-2009</created>
        /// <summary>Creates and returns object of BugReport for specified exception
        /// </summary>
        /// <param name="ex">Object of exception occured
        /// </param>
        /// <returns>Object of BugReport
        /// </returns>
        public static BugReport SaveBugReport(Exception ex)
        {
            return SaveBugReport(ex, string.Empty, string.Empty, string.Empty, 0, false);
        }

        /// <author>Gaurang Majithiya</author>
        /// <created>01-May-2009</created>
        /// <summary>Creates and returns object of BugReport for specified exception, extra information, description and comment of user
        /// </summary>
        /// <param name="ex">Object of exception occured
        /// </param>
        /// <param name="extraInformation">Extra information of bug
        /// </param>
        /// <param name="description">Description of bug
        /// </param>
        /// <param name="userComment">Entered comment by user
        /// </param>
        /// <returns>Object of BugReport
        /// </returns>
        private static BugReport SaveBugReport(Exception ex, string extraInformation, string description, string userComment, int cecSystemID, bool isInfo)
        {
            string area = string.Empty;
            string applicationName = string.Empty;
            try
            {
                area = ConfigurationManager.AppSettings["FogbugzArea"];
                if (area == null || area == string.Empty) area = _defaultFogbugzArea; // area = "CRM";
            }
            catch
            {
                area = _defaultFogbugzArea;
            }

            try
            {
                if (ConfigurationManager.AppSettings["ApplicationName"] != null)
                {
                    applicationName = ConfigurationManager.AppSettings["ApplicationName"].ToString();
                    if (applicationName != string.Empty) applicationName = applicationName + " : ";
                }
            }
            catch{}
            
            //assign properties to bugreport object
            BugReport objReport = new BugReport();
            objReport.Area = area;
            objReport.ExceptionObject = ex;
            objReport.IPAddress = System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList[0].ToString();
            objReport.MachineName = Environment.MachineName;
            objReport.Email = Environment.UserName;
            objReport.Project = Project;

            objReport.CECSystemID = cecSystemID;
            if (objReport.CECSystemID == 0) objReport.CECSystemID = Convert.ToInt16(ConfigurationManager.AppSettings["CECSystemID"]);
            
            if (extraInformation != string.Empty) objReport.ExtraInformation = extraInformation;

            if (applicationName != string.Empty) objReport.UserComment = applicationName;
            if (userComment != string.Empty) objReport.UserComment += userComment;

            if (description != string.Empty) objReport.Description = description;
            if (isInfo)
            {
                objReport.IsSaveBugInBugInfo = true;
                objReport.ErrorDescription = ex.Message;
            }

            return objReport;
        }

        /// <author>Tina Mori</author>
        /// <created>29-Apr-2009</created>
        /// <summary>Updates an existing bug with the provided details
        /// </summary>
        /// <param name="bugID">Unique ID of the bug
        /// </param>
        /// <param name="area">Area of the bug
        /// </param>
        /// <param name="email">email id of user
        /// </param>
        /// <param name="userComment">Entered comment by user
        /// </param>
        /// <returns>Returns true if updated successfully, otherwise false
        /// </returns>
        public static bool UpdateDataToBugScout(int bugID, string area, string email, string userComment)
        {
            BugReport objReport = BugReport.Specific(bugID);
            if (objReport == null) return false;
            objReport.Area = area;
            objReport.Email = email;
            objReport.UserComment = userComment;
            objReport.Project = Project;
            objReport.Save();
            return true;
        }
    }
}
