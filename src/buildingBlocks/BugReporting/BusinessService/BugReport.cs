using CredECard.BugReporting.DataService;
using CredECard.Common.BusinessService;
using CredECard.CommonSetting.BusinessService;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;

namespace CredECard.BugReporting.BusinessService
{
    /// <author>Falguni Parikh</author>
    /// <created>20-July-06</created>
    /// <summary>
    /// BusinessService BugReport used for reporting bug to the database
    /// </summary>
    public class BugReport : DataItem, IPersistableV2
    {
        #region Private Member Variables

        internal string _userName = string.Empty;
        internal string _systemType = string.Empty;
        internal DateTime _createdDate = DateTime.MinValue;
        internal string _project = string.Empty;
        internal string _area = string.Empty;
        private string _description = string.Empty;
        private string _extraInformation = string.Empty;
        private string _customerEmail = string.Empty;
        private bool _forceNewBug = false;
        private string _fogBugzUrl = string.Empty;
        private string _defaultMsg = string.Empty;
        internal int _bugID = 0;
        internal Exception _exceptionObj = null;
        internal string _userComment = string.Empty;
        internal byte[] _exceptionBytes = null;
        internal bool _isSuccess = false;
        private const string SUCCESS = "<Success>";
        internal int _cecSystemID = 0;
        internal string _machineName = string.Empty;
        internal string _ipAddress = string.Empty;
        internal int _bugInfoID = 0;
        internal string _errorDescription = string.Empty;
        internal bool _isSaveBugInBugInfo = false;

        private const string FOGBUGZAREA = "1. Undecided";
        private const string DEFAULTPROJECT = "Processor System";
        private const string DEFAULT_DESCRIPTION = "Discription does not available.";
        #endregion

        #region Constructor
        /// <author>Falguni Parikh</author>
        /// <created>20-July-06</created>
        /// <summary>
        /// Constructor to create object of BugReport business service
        /// </summary>
        public BugReport()
        {
            //Set general values
            //this._defaultMsg = "Error reported successfully";
            this._defaultMsg = CommonMessage.GetMessage(EnumErrorConstants.SUCCESSFULL_ERROR_REPORT);
            this._forceNewBug = false;
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets array of byte of Exception
        /// </summary>
        public byte[] ExceptionBytes
        {
            get
            {
                return _exceptionBytes;
            }
        }


        /// <author>Falguni Parikh</author>
        /// <created>20-July-06</created>
        /// <summary>
        /// Gets or sets the url to report the bug to the bugscout application.
        /// It returns value of BugScoutURL settings from the database if exists, otherwise returns 
        /// http://192.168.100.6/fogbugz/scoutSubmit.asp Url
        /// </summary>
        /// <value>
        /// String of the FogBugz Url
        /// </value>
        public string FogBugzUrl
        {
            get
            {
                if (_fogBugzUrl == string.Empty)
                {
                    try
                    {
                        _fogBugzUrl = GeneralSetting.GetSettingValue((int)EnumGeneralSettings.BugScoutURL);
                    }
                    catch
                    {
                        _fogBugzUrl = "http://192.168.100.6/fogbugz/scoutSubmit.asp";
                    }
                    return _fogBugzUrl;
                }
                else
                    return _fogBugzUrl;
            }
            set
            {
                _fogBugzUrl = value;
            }
        }

        /// <author>Falguni Parikh</author>
        /// <created>20-July-06</created>
        /// <summary>
        /// Gets or sets the username to report the bug on to the BugScout application
        /// </summary>
        /// <value>
        /// String
        /// </value>
        public string FogBugzUsername
        {
            get
            {
                if (_userName == string.Empty)
                {
                    try
                    {
                        _userName = GeneralSetting.GetSettingValue((int)EnumGeneralSettings.FogbugzUserName);
                    }
                    catch
                    {
                        _userName = "Jatin Mishra";
                    }
                    return _userName;
                }
                else
                    return _userName;
            }
            set
            {
                _userName = value;
            }
        }

        /// <author>Rishit Rajput</author>
        /// <created>09-Sep-2010</created>
        /// <summary>
        /// Gets or sets System type
        /// </summary>
        /// <value>
        /// String
        /// </value>
        public string SystemType
        {
            get
            {
                return _systemType;
            }
        }

        /// <author>Falguni Parikh</author>
        /// <created>20-July-06</created>
        /// <summary>
        /// Gets or sets the description of bug
        /// </summary>
        /// <value>
        /// String
        /// </value>
        public string Description
        {
            get { return this._description; }
            set
            {
                if (value == null || value.Length == 0)
                {
                    this._description = DEFAULT_DESCRIPTION;
                    //throw new ArgumentNullException("Description");
                }
                this._description = value;
            }
        }

        /// <author>Falguni Parikh</author>
        /// <created>20-July-06</created>
        /// <summary>
        /// Gets or sets the extra information of the bug
        /// </summary>
        /// <value>
        /// String
        /// </value>
        public string ExtraInformation
        {
            get { return this._extraInformation; }
            set
            {
                if (value == null) throw new ArgumentNullException("ExtraInformation");
                this._extraInformation = value;
            }
        }

        /// <author>Falguni Parikh</author>
        /// <created>20-July-06</created>
        /// <summary>
        /// Gets or sets the project name to report the bug.
        /// It returns value of FogbugzProjectName setting from the database if exists, otherwise returns "CEC-Core"
        /// </summary>
        /// <value>
        /// String
        /// </value>
        public string Project
        {
            get
            {

                if (_project == string.Empty) _project = DEFAULTPROJECT;
             
                return _project;
            }
            set
            {
                _project = value;
            }

        }

        /// <author>Falguni Parikh</author>
        /// <created>20-July-06</created>
        /// <summary>
        /// Gets or sets the area of the bug to report.
        /// It returns 1. Undecided.
        /// </summary>
        /// <value>
        /// String
        /// </value>
        public string Area
        {
            get
            {
                if (this._area == string.Empty) this._area = FOGBUGZAREA;
                return this._area;
            }
            set
            {
                if (value == string.Empty) throw new ArgumentNullException("Area");
                this._area = value;
            }
        }

        /// <author>Falguni Parikh</author>
        /// <created>20-July-06</created>
        /// <summary>
        /// Gets or sets email to attach to the bug report.
        /// </summary>
        /// <value>
        /// String
        /// </value>
        public string Email
        {
            get { return this._customerEmail; }
            set
            {
                if (value != string.Empty)
                    this._customerEmail = value;
            }
        }

        /// <author>Falguni Parikh</author>
        /// <created>20-July-06</created>
        /// <summary>
        /// Gets or sets to force a new bug or try to locate an existing one to append to.
        /// </summary>
        /// <value>
        /// String
        /// </value>
        public bool ForceNewBug
        {
            get { return this._forceNewBug; }
            set
            {
                this._forceNewBug = value;
            }
        }

        /// <author>Falguni Parikh</author>
        /// <created>20-July-06</created>
        /// <summary>
        /// Gets or sets default message to be shown to user after inserting on fogbugz.
        /// </summary>
        /// <value>
        /// String
        /// </value>
        public string DefaultMsg
        {
            get { return this._defaultMsg; }
            set
            {
                this._defaultMsg = value;
            }
        }

        /// <author>Sejal Maheshwari</author>
        /// <created>13-Oct-06</created>
        /// <summary>
        /// Gets or sets BugID of the bug
        /// </summary>
        /// <value>
        /// int
        /// </value>
        public int BugID
        {
            get
            {
                return _bugID;
            }
            set
            {
                _bugID = value;
            }
        }
        /// <author>Sejal Maheshwari</author>
        /// <created>13-Oct-06</created>
        /// <summary>
        /// Gets or sets object of exception
        /// </summary>
        /// <value>
        /// Exception object
        /// </value>
        public Exception ExceptionObject
        {
            get
            {
                if (_exceptionObj == null && _exceptionBytes != null && _exceptionBytes.Length > 0)
                {
                    try
                    {
                        MemoryStream ms = new MemoryStream(_exceptionBytes);
                        BinaryFormatter bf = new BinaryFormatter();
                        Exception objEx = null;
                        try
                        {
                            objEx = (Exception)bf.Deserialize(ms);
                        }
                        catch
                        {
                            ms.Position = 0;
                            var sr = new StreamReader(ms);
                            var myStr = sr.ReadToEnd();

                            objEx = new Exception(myStr.ToString());
                        }
                        _exceptionObj = objEx;
                    }
                    catch
                    {
                        _exceptionObj = null;
                    }
                }
                return _exceptionObj;
            }
            set
            {
                _exceptionObj = value;
            }
        }

        /// <author>Sejal Maheshwari</author>
        /// <created>13-Oct-06</created>
        /// <summary>
        /// Gets or sets comment entered by user
        /// </summary>
        /// <value>
        /// string
        /// </value>
        public string UserComment
        {
            get
            {
                return _userComment;
            }
            set
            {
                _userComment = value;
            }
        }

        /// <author>Sejal Maheshwari</author>
        /// <created>23-Nov-06</created>
        /// <summary>
        /// Gets bug is posted successfully or not
        /// </summary>
        /// <value>
        /// bool
        /// </value>
        public bool IsSuccess
        {
            get
            {
                return _isSuccess;
            }
            set
            {
                _isSuccess = value;
            }

        }

        /// <author>Tina Mori</author>
        /// <created>29-Apr-2009</created>
        /// <summary>Gets or sets the CEC System ID
        /// </summary>
        /// <value>int
        /// </value>
        public int CECSystemID
        {
            get
            {
                return _cecSystemID;
            }
            set
            {
                _cecSystemID = value;
            }
        }

        /// <author>Tina Mori</author>
        /// <created>29-Apr-2009</created>
        /// <summary>Gets or sets the Machinename
        /// </summary>
        /// <value>string
        /// </value>
        public string MachineName
        {
            get
            {
                return _machineName;
            }
            set
            {
                _machineName = value;
            }
        }

        /// <author>Tina Mori</author>
        /// <created>29-Apr-2009</created>
        /// <summary>Gets or sets the IP Address
        /// </summary>
        /// <value>string
        /// </value>
        public string IPAddress
        {
            get
            {
                return _ipAddress;
            }
            set
            {
                _ipAddress = value;
            }
        }

        /// <author>Tina Mori</author>
        /// <created>29-Apr-2009</created>
        /// <summary>Gets or sets the date bug reported
        /// </summary>
        /// <value>DateTime
        /// </value>
        public DateTime CreatedDate
        {
            get
            {
                return _createdDate;
            }
        }

        /// <author>Denish Makwana</author>
        /// <created>14-Dec-2020</created>
        /// <summary>
        /// Gets or sets BugInfoID of the bug
        /// </summary>
        /// <value>
        /// int
        /// </value>
        public int BugInfoID
        {
            get
            {
                return _bugInfoID;
            }
            set
            {
                _bugInfoID = value;
            }
        }

        /// <author>Denish Makwna</author>
        /// <created>14-Dec-2020</created>
        /// <summary>Gets or sets the ErrorDescription
        /// </summary>
        /// <value>string</value>
        public string ErrorDescription
        {
            get
            {
                return _errorDescription;
            }
            set
            {
                _errorDescription = value;
            }
        }

        /// <author>Denish Makwana</author>
        /// <created>14-Dec-2020</created>
        /// <summary>
        /// Gets or Sets IsSaveBugInBugInfo
        /// </summary>
        /// <value>bool</value>
        public bool IsSaveBugInBugInfo
        {
            get
            {
                return _isSaveBugInBugInfo;
            }
            set
            {
                _isSaveBugInBugInfo = value;
            }
        }

        #endregion

        #region Public Instance Methods

        /// <author>Falguni Parikh</author>
        /// <created>20-July-06</created>
        /// <summary>
        /// Formats exception details from Exception object
        /// </summary>
        /// <param name="e">Exception object</param>
        /// <param name="customerMsg">String customer message</param>
        /// <returns>Returns formatted message string</returns>
        public string GenerateExceptionData(Exception e, string customerMsg)
        {
            SetException(e, true, "{0}.{1}.{2}.{3}", customerMsg);
            //AppendAssemblyList();
            string errorDetails = this.ExtraInformation;
            errorDetails = errorDetails.Replace("\r\n", "<br />");
            return errorDetails;
        }

        /// <author>Falguni Parikh</author>
        /// <created>20-July-06</created>
        /// <summary>
        /// Submits a new bug report to the FogBugz submission page
        /// </summary>
        /// <returns>Returns response string recieved from FogBugs</returns>
        public string SubmitException()
        {
            if (this.ExceptionBytes != null)
            {
                Exception e = this.ExceptionObject;
                SetException(e, true, "{0}.{1}.{2}.{3}", this._userComment);
            }
            else
            {
                StringBuilder extra = new StringBuilder();

                extra.AppendFormat("Username: " + this.Email + Environment.NewLine);
                extra.AppendFormat("Machine name: " + this.MachineName + Environment.NewLine);
                extra.AppendFormat("IP Address: " + this.IPAddress + Environment.NewLine);
                extra.AppendFormat("System Type: " + this.SystemType + Environment.NewLine);
                extra.AppendFormat("Error Reported On: " + this.CreatedDate + Environment.NewLine);
                extra.Append(Environment.NewLine);

                if (this._userComment != string.Empty)
                {
                    extra.Append(Environment.NewLine);
                    extra.AppendFormat("User Message: " + this._userComment);
                    extra.Append(Environment.NewLine);
                    this.Description = this._userComment;
                }

                this.ExtraInformation = extra.ToString();
            }

            return postException();
        }

        /// <author>Rishit Rajput</author>
        /// <created>09-Sep-06</created>
        /// <summary>
        /// save system error log
        /// </summary>
        /// <returns></returns>
        public void SubmitSystemErrorLog(DataController con)
        {
            SystemErrorLogs objSystemError = updateSystemError();
            objSystemError.Save(con);
        }

        private SystemErrorLogs updateSystemError()
        {
            SystemErrorLogs objSystemError = new SystemErrorLogs();
            Exception e = this.ExceptionObject;
            objSystemError._area = this._area;
            objSystemError._cECSystemID = this._cecSystemID;
            objSystemError._iPAddress = this._ipAddress;
            objSystemError._machineName = this._machineName;
            objSystemError._project = this._project;
            objSystemError._userComments = this._userComment;
            objSystemError._userEmail = this.Email;

            objSystemError._errorMessage = e.Message.ToString();

            if (e.StackTrace != null)
            {
                objSystemError._stackTrace = e.StackTrace.ToString();
            }

            return objSystemError;
        }

        /// <author>Gaurang Majithiya</author>
        /// <created>10-July-09</created>
        /// <summary>
        /// Creates query string and posts exception on FogBugzUrl using POST method
        /// </summary>
        /// <returns>Returns result string</returns>
        private string postException()
        {
            if (this.Description == string.Empty)
            {
                this.Description = DEFAULT_DESCRIPTION;
                //throw new ArgumentNullException("Description");
            }

            // Prepare request
            NameValueCollection list = new NameValueCollection();
            list.Add("Description", this.Description);
            list.Add("Extra", this.ExtraInformation);
            list.Add("Email", this.Email);
            list.Add("ScoutUserName", this.FogBugzUsername);
            list.Add("ScoutProject", this.Project);
            list.Add("ScoutArea", this.Area);
            list.Add("Machine Name", this.MachineName);
            list.Add("IP Address", this.IPAddress);
            list.Add("ForceNewBug", this.ForceNewBug ? "1" : "0");

            //Post exception to FogBugzUrl using "POST" method. Case: 10585
            WebClient client = new WebClient();
            Byte[] response = client.UploadValues(FogBugzUrl, "POST", list);

            string responseText = System.Text.Encoding.UTF8.GetString(response);

            if (responseText.Contains(SUCCESS) == true) this._isSuccess = true;

            responseText = ParseResult(responseText);
            return (responseText == "") ? this.DefaultMsg : responseText;
        }

        /// <author>Falguni Parikh</author>
        /// <created>20-July-06</created>
        /// <summary>
        /// Formats the Exception object and appends extra information.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="addUserAndMachineInformation"></param>
        /// <param name="versionFormat"></param>        
        private void SetException(Exception ex, bool addUserAndMachineInformation, string versionFormat, string customerMsg)
        {
            if (ex == null) throw new ArgumentNullException("ex");
            if (versionFormat == null || versionFormat.Length == 0) throw new ArgumentNullException("versionFormat");

            if (this.Description == string.Empty)
                this.Description = GetExceptionDescription(ex, versionFormat);

            StringBuilder extra = new StringBuilder();
            if (addUserAndMachineInformation)
            {
                extra.AppendFormat("Username: " + this.Email + Environment.NewLine);
                extra.AppendFormat("Machine name: " + this.MachineName + Environment.NewLine);
                extra.AppendFormat("IP Address: " + this.IPAddress + Environment.NewLine);
                extra.AppendFormat("System Type: " + this.SystemType + Environment.NewLine);
                extra.AppendFormat("Error Reported On: " + this.CreatedDate + Environment.NewLine);
                extra.Append(Environment.NewLine);
            }

            extra.Append(ex.ToString());

            #region Old code for formatting exception (Case 10715)
            /*
            if (ex.StackTrace != null)
            {
                extra.Append("Stacktrace:" + Environment.NewLine);

                foreach (string line in ex.StackTrace.Split('\n', '\r'))
                {
                    if (line != null && line.Length > 0)
                        extra.AppendFormat("{0}" + Environment.NewLine, line.Trim());
                }
            }
            extra.Append(Environment.NewLine);

            Regex reUnwantedProperties = new Regex(@"^(StackTrace|Source|TargetSite)$", RegexOptions.IgnoreCase);
            while (ex != null)
            {
                bool any = false;
                foreach (PropertyInfo pi in ex.GetType().GetProperties())
                {
                    if (!reUnwantedProperties.Match(pi.Name).Success)
                    {
                        Object value = pi.GetValue(ex, new Object[] { });
                        if (value != null)
                        {
                            if (IsInteger(value))
                                extra.AppendFormat("{0}={1} (0x{1:X})" + Environment.NewLine, pi.Name, value);
                            else
                                extra.AppendFormat("{0}={1}" + Environment.NewLine, pi.Name, value);
                            any = true;
                        }
                    }
                }
                if (ex.InnerException != null)
                {
                    if (any) extra.Append(Environment.NewLine);
                }
                ex = ex.InnerException;
            }
            */
            #endregion

            //Append User information
            if (customerMsg != string.Empty)
            {
                extra.Append(Environment.NewLine);
                extra.AppendFormat("User Message: " + customerMsg);
                extra.Append(Environment.NewLine);
            }

            if (this.ExtraInformation == string.Empty)
                this.ExtraInformation = extra.ToString();
            else
                this.ExtraInformation += Environment.NewLine + extra.ToString();
        }

        ///// <author>Falguni Parikh</author>
        ///// <created>20-July-06</created>
        ///// <summary>
        ///// Appends a list of assembly names and version numbers to the extra information to send in the report.
        ///// </summary>
        //public void AppendAssemblyList()
        //{
        //    StringBuilder assemblies = new StringBuilder();
        //    if (this.ExtraInformation == string.Empty) this.ExtraInformation = "";
        //    else if (this.ExtraInformation.Length > 0) assemblies.Append(Environment.NewLine);

        //    assemblies.Append("Assemblies:" + Environment.NewLine);
        //    foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
        //    {
        //        assemblies.AppendFormat("   {0}, {1}" + Environment.NewLine, asm.GetName().Name, asm.GetName().Version.ToString());
        //    }
        //    this.ExtraInformation += assemblies.ToString();
        //}

        /// <author>Sejal Maheshwari</author>
        /// <created>13 Oct 2006</created>
        /// <summary>
        /// Gets specific BugReport object for specified unique bug ID from the database
        /// </summary>
        /// <param name="bugID">Bug ID</param>
        /// <returns>Object of BugReport</returns> 
        public static BugReport Specific(int bugID)
        {
            return ReadBugReport.Specific(bugID);
        }

        /// <author>Sejal Maheshwari</author>
        /// <created>13 Oct 2006</created>
        /// <summary>Saves bug to the database
        /// </summary>
        public override void Save()
        {
            try
            {
                DataController conn = new DataController();
                conn.StartDatabase(CredECard.Common.BusinessService.CredECardConfig.GetReadWriteConnectionString());
                ((IPersistableV2)this).Save(conn);
                conn.EndDatabase();
            }
            catch (Exception ex)
            {
                throw new PersistException("Error saving bug detail " + this.BugID.ToString(), ex);
            }
        }

        /// <author>Sejal Maheshwari</author>
        /// <created>13 Oct 2006</created>
        /// <summary>
        /// Saves bug to the database using connection
        /// </summary>
        /// <param name="conn">Object of DataController</param>
        void IPersistableV2.Save(DataController conn)
        {
            MemoryStream memStream = new MemoryStream();
            try
            {
                //serialize exception object
                if (this.BugID == 0 && this._exceptionObj != null)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(memStream, this._exceptionObj);
                    this._exceptionBytes = memStream.GetBuffer();
                }
                WriteBugReport wrBugReport = new WriteBugReport(conn);
                if (this.IsSaveBugInBugInfo)
                {
                    wrBugReport.SaveBugInfo(this);
                }
                else
                {
                    wrBugReport.SaveBug(this);
                }
            }
            catch (Exception ex)
            {
                throw new PersistException("Error saving Bugs " + this.BugID.ToString(), ex);
            }
            finally
            {
                if (memStream != null) memStream.Close();
            }
        }

        #endregion

        #region Private Methods

        /// <author>Falguni Parikh</author>
        /// <created>20-July-06</created>
        /// <summary>
        /// Parses the xml result returned by the scout page and throws an exception in the case of a failure notice.
        /// </summary>
        /// <param name="result">Result string returned by the scout page</param>
        private string ParseResult(string result)
        {
            // Check for a success result first and just return in that case
            Match ma = Regex.Match(result, "<Success>(?<message>.*)</Success>", RegexOptions.IgnoreCase);
            if (ma.Success) return ma.Groups["message"].Value;

            // Check for a failure result second, and throw an exception in that case
            ma = Regex.Match(result, "<Error>(?<message>.*)</Error>", RegexOptions.IgnoreCase);
            if (ma.Success) throw new BugReportSubmitException(ma.Groups["message"].Value);

            // Unknown format, so throw an InvalidOperationException to note the fact
            throw new InvalidOperationException(new System.Resources.ResourceManager(typeof(BugReport)).GetString("UnableToProcessResult"));
        }

        /// <author>Falguni Parikh</author>
        /// <created>20-July-06</created>
        /// <summary>
        /// Formats the description of the exception into a unique identifiable string.
        /// </summary>
        /// <param name="ex">Exception object</param>
        /// <param name="versionFormat">Version format string</param>
        private string GetExceptionDescription(Exception ex, string versionFormat)
        {
            if (ex == null) throw new ArgumentNullException("ex");
            if (versionFormat == null || versionFormat.Length == 0) throw new ArgumentNullException("versionFormat");

            StringBuilder desc = new StringBuilder();

            // We first want the class name of the exception that occured
            desc.Append(ex.GetType().Name);

            // If the exception has a property called ErrorCode, add the value of it to the desc
            Regex rePropertyName = new Regex("^(ErrorCode|HResult)$", RegexOptions.IgnoreCase);
            foreach (PropertyInfo property in ex.GetType().GetProperties())
            {
                if (rePropertyName.Match(property.Name).Success)
                {
                    // Only deal with readable properties
                    if (property.CanRead)
                    {
                        // Only deal with properties that aren't indexed
                        if (property.GetIndexParameters().Length == 0)
                        {
                            // Only add property values that are not null
                            Object propertyValue = property.GetValue(ex, new Object[] { });
                            if (propertyValue != null)
                            {
                                // If the property value converted to a string yields the same name as the class
                                // name of the value, it is uninteresting
                                string propertyValueString = propertyValue.ToString();
                                if (propertyValueString != propertyValue.GetType().FullName)
                                    desc.AppendFormat(" {0}={1}", property.Name, propertyValueString);
                            }
                        }
                    }
                }
            }

            // Work out the first source code reference in the stacktrace and add the unique value for it
            Regex reSourceReference = new Regex("at\\s+.+\\.(?<methodname>[^)]+)\\(.*\\)\\s+in\\s+.+\\\\(?<filename>[^:\\\\]+):line\\s+(?<linenumber>[0-9]+)", RegexOptions.IgnoreCase);
            bool gotReference = false;
            if (ex.StackTrace != null)
            {
                foreach (string line in ex.StackTrace.Split('\n', '\r'))
                {
                    Match ma = reSourceReference.Match(line);
                    if (ma.Success)
                    {
                        desc.AppendFormat(" ({0}:{1}:{2})",
                            ma.Groups["filename"].Value,
                            ma.Groups["methodname"].Value,
                            ma.Groups["linenumber"].Value);
                        gotReference = true;
                        break;
                    }
                }
            }

            // If we didn't get a source reference (release compile ?), try to find a non-System.* reference
            if (!gotReference)
            {
                Regex reMethodReference = new Regex("at\\s+(?<methodname>[^(]+)\\(.*\\)", RegexOptions.IgnoreCase);
                if (ex.StackTrace != null)
                {
                    foreach (string line in ex.StackTrace.Split('\n', '\r'))
                    {
                        Match ma = reMethodReference.Match(line);
                        if (ma.Success)
                        {
                            if (!ma.Groups["methodname"].Value.ToUpper().StartsWith("SYSTEM."))
                            {
                                desc.AppendFormat(" ({0})",
                                    ma.Groups["methodname"].Value);
                                gotReference = true;
                                break;
                            }
                        }
                    }
                }
            }

            // If we can get the entry assembly, add the version number of it
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                AssemblyName entryAssemblyName = entryAssembly.GetName();
                if (entryAssemblyName != null)
                {
                    if (entryAssemblyName.Version != null)
                    {
                        string version = String.Format(versionFormat,
                            entryAssemblyName.Version.Major,
                            entryAssemblyName.Version.Minor,
                            entryAssemblyName.Version.Build,
                            entryAssemblyName.Version.Revision);
                        desc.AppendFormat(" V{0}", version);
                    }
                }
            }
            // Return result
            return desc.ToString();
        }

        /// <author>Falguni Parikh</author>
        /// <created>20-July-06</created>
        /// <summary>
        /// Checks whether passed value is integer or not
        /// </summary>
        /// <param name="x"></param>
        private bool IsInteger(Object x)
        {
            try
            {
                Convert.ToInt32(x);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        /// <summary>
        /// Deletes specified bug from the database
        /// </summary>
        /// <param name="bugID">Unique ID of bug</param>
        public static void DeleteBug(int bugID, DataController conn)
        {
            WriteBugReport objWriteBug = new WriteBugReport(conn);
            objWriteBug.DeleteBug(bugID);
        }

    }
    /// <author>Falguni Parikh</author>
    /// <created>20-July-06</created>
    /// <summary>
    /// Represents errors that occur during submission of <see cref="BugReport"/> reports (from project BugzScout)
    /// </summary>
    [Serializable]
    public class BugReportSubmitException : SystemException, ISerializable
    {
        #region Construction & Destruction

        /// <summary>
        /// Initializes a new instance of the <see cref="BugReportSubmitException">BugReportSubmitException</see>
        /// class with serialized data.
        /// </summary>
        protected BugReportSubmitException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Nothing here
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BugReportSubmitException"/> class with an exception message
        /// loaded from a resource.
        /// </summary>
        public BugReportSubmitException(Type resourceSource, String resourceName, Exception innerException)
            : base(new System.Resources.ResourceManager(resourceSource).GetString(resourceName), innerException)
        {
            // Nothing here
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BugReportSubmitException">BugReportSubmitException</see>
        /// with resource source and resource name
        /// </summary>
        public BugReportSubmitException(Type resourceSource, String resourceName)
            : base(new System.Resources.ResourceManager(resourceSource).GetString(resourceName))
        {
            // Nothing here
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BugReportSubmitException">BugReportSubmitException</see>
        /// with message and inner exception
        /// </summary>
        public BugReportSubmitException(String message, Exception innerException)
            : base(message, innerException)
        {
            // Nothing here
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BugReportSubmitException">BugReportSubmitException</see>
        /// with message
        /// </summary>
        public BugReportSubmitException(String message)
            : base(message)
        {
            // Nothing here
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BugReportSubmitException">BugReportSubmitException</see>
        /// </summary>
        public BugReportSubmitException()
            : base()
        {
            // Nothing here
        }

        #endregion
    }
}
