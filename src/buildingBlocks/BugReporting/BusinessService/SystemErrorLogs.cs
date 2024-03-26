using CredECard.BugReporting.DataService;
using CredECard.Common.BusinessService;
using System;

namespace CredECard.BugReporting.BusinessService
{
    public class SystemErrorLogs : DataItem, IPersistableV2
    {
        #region Variables

        internal int _systemErrorLogID = 0;
        internal string _userComments = string.Empty;
        internal string _userEmail = string.Empty;
        internal string _errorMessage = string.Empty;
        internal string _stackTrace = string.Empty;
        internal DateTime _createdDate = DateTime.MinValue;
        internal int _cECSystemID = 0;
        internal string _project = string.Empty;
        internal string _area = string.Empty;
        internal string _machineName = string.Empty;
        internal string _iPAddress = string.Empty;

        #endregion

        #region Properties

        /// <author>Rishit Rajput</author>
        /// <created>09-Sep-2010</created>
        /// <summary>Get or Set SystemErrorLogID
        /// </summary>
        public int SystemErrorLogID
        {
            get { return _systemErrorLogID; }
            set { _systemErrorLogID = value; }
        }

        /// <author>Rishit Rajput</author>
        /// <created>09-Sep-2010</created>
        /// <summary>Get or Set UserComments
        /// </summary>
        public string UserComments
        {
            get { return _userComments; }
            set { _userComments = value; }
        }

        /// <author>Rishit Rajput</author>
        /// <created>09-Sep-2010</created>
        /// <summary>Get or Set UserEmail
        /// </summary>
        public string UserEmail
        {
            get { return _userEmail; }
            set { _userEmail = value; }
        }

        /// <author>Rishit Rajput</author>
        /// <created>09-Sep-2010</created>
        /// <summary>Get or Set ErrorMessage
        /// </summary>
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        /// <author>Rishit Rajput</author>
        /// <created>09-Sep-2010</created>
        /// <summary>Get or Set StackTrace
        /// </summary>
        public string StackTrace
        {
            get { return _stackTrace; }
            set { _stackTrace = value; }
        }

        /// <author>Rishit Rajput</author>
        /// <created>09-Sep-2010</created>
        /// <summary>Get or Set CreatedDate
        /// </summary>
        public DateTime CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        /// <author>Rishit Rajput</author>
        /// <created>09-Sep-2010</created>
        /// <summary>Get or Set CECSystemID
        /// </summary>
        public int CECSystemID
        {
            get { return _cECSystemID; }
            set { _cECSystemID = value; }
        }

        /// <author>Rishit Rajput</author>
        /// <created>09-Sep-2010</created>
        /// <summary>Get or Set Project
        /// </summary>
        public string Project
        {
            get { return _project; }
            set { _project = value; }
        }

        /// <author>Rishit Rajput</author>
        /// <created>09-Sep-2010</created>
        /// <summary>Get or Set Area
        /// </summary>
        public string Area
        {
            get { return _area; }
            set { _area = value; }
        }

        /// <author>Rishit Rajput</author>
        /// <created>09-Sep-2010</created>
        /// <summary>Get or Set MachineName
        /// </summary>
        public string MachineName
        {
            get { return _machineName; }
            set { _machineName = value; }
        }

        /// <author>Rishit Rajput</author>
        /// <created>09-Sep-2010</created>
        /// <summary>Get or Set IPAddress
        /// </summary>
        public string IPAddress
        {
            get { return _iPAddress; }
            set { _iPAddress = value; }
        }

        #endregion

        #region Method

        #region IPersistableV2 Members

        /// <author>Rishit Rajput</author>
        /// <created>09-Sep-2010</created>
        /// <summary>Save Data 
        /// </summary>
        /// <param name="conn">DataController</param>
        public void Save(DataController conn)
        {
            WriteSystemErrorLogs wr = new WriteSystemErrorLogs(conn);
            wr.WriteData(this);
        }

        #endregion

        /// <author>Rishit Rajput</author>
        /// <created>09-Sep-2010</created>
        /// <summary>Get specific object
        /// </summary>
        public static SystemErrorLogs Specific(int id)
        {
            if (id == 0) return null;

            return ReadSystemErrorLogs.ReadSpecific(id);
        }

        #endregion
    }
}
