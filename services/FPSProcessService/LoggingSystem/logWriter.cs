using System;
using System.IO;

namespace DataLogging
{
    namespace LogWriters
    {
        using System.Diagnostics;
        using System.Runtime.Remoting.Messaging;
        using CredECard.BugReporting.BusinessService;
        using CredECard.CommonSetting.BusinessService;
        using DataLogging.LogEntries;
        using System.Configuration;
        using System.Threading.Tasks;

        public interface ILogWriter
        {
            string FilePath
            {
                get;
                set;
            }

            void LogEntry(ILogEntry logEntry);
        }

        [Serializable()]
        public class BaseLogWriter : ILogWriter
        {
            private static object _lock = new object();
            public const string DEFAULT_LOGPATH = "log.txt";

            private string _logFilePath = DEFAULT_LOGPATH;

            public virtual string FilePath
            {
                get { return _logFilePath; }
                set { _logFilePath = value; }
            }

            public virtual void LogEntry(ILogEntry logEntry)
            {
                lock (_lock)
                {
                    try
                    {
                        //FileStream _logStream = new FileStream(_logFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
                        using (FileStream _logStream = new FileStream(_logFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
                        {
                            StreamWriter _logWriter = new StreamWriter(_logStream);

                            _logStream.Seek(0, SeekOrigin.End);
                            _logWriter.WriteLine(logEntry.ToString());
                            _logWriter.Flush();
                            _logWriter.Close();
                            _logStream.Close();
                        }
                    }
                    catch (IOException)
                    { }
                }
            }
        }

        /// <author>Rikunj Suthar</author>
        /// <created>03-Sep-2020</created>
        /// <summary>
        /// Basic log with only log detail.
        /// </summary>
        public class BasicLog : ILogEntry
        {
            public int ErrorCode { get; set; } = 0;
            public string ErrorDescription { get; set; } = string.Empty;
            public string ReferenceData { get; set; } = String.Empty;
            public string DateStamp { get; } = string.Empty;
            public string TimeStamp { get; } = string.Empty;

            private readonly DateTime _timeStamp;            

            /// <summary>
            /// Default constructor reference data.
            /// </summary>
            /// <param name="referenceData"></param>
            public BasicLog(string referenceData)
            {
                ReferenceData = referenceData;
                _timeStamp = DateTime.Now;
            }
            
            /// <summary>
            /// Return log string
            /// </summary>
            /// <returns></returns>
            public override string ToString() => $"{_timeStamp:dd-MM-yyyy HH:mm:ss:fff}, {ReferenceData}";
        }

        [Serializable()]
        public class SimpleLogWriter : BaseLogWriter
        {
            private static object _lock = new object();
            public delegate void WriteLogDelegate(ILogEntry logEntry);

            /// <author>Aarti Meswania</author>
            /// <created>06-Feb-2018</created>
            /// <summary>ErrorDetailLogPath</summary>
            public static string ErrorDetailLogPath
            {
                get
                {
                    string errorDetailLogPath = string.Empty;
                    if (ConfigurationManager.AppSettings["ErrorDetailLogPath"] != null)
                        errorDetailLogPath = ConfigurationManager.AppSettings["ErrorDetailLogPath"] + "_" + DateTime.Now.ToString("ddMMyyyy") + ".log";
                    return errorDetailLogPath;
                }
            }

            /// <author>Rikunj Suthar</author>
            /// <created>03-Sep-2020</created>
            /// <summary>
            /// Detailed log path.
            /// </summary>
            public static string DetailLogPath => ConfigurationManager.AppSettings["Detaillogpath"];

            public SimpleLogWriter()
                : this(DEFAULT_LOGPATH)
            {
            }

            public SimpleLogWriter(string filePath)
            {
                this.FilePath = filePath;
            }

            public override void LogEntry(ILogEntry logEntry)
            {
                WriteLogDelegate _writeLogDelegate = new WriteLogDelegate(writeLogEntry);
                AsyncCallback _writeLogCallBack = new AsyncCallback(logWritten);
                //_writeLogDelegate.BeginInvoke(logEntry,_writeLogCallBack, null);
                Task task = Task.Run(() => _writeLogDelegate.Invoke(logEntry));
            }

            private void writeLogEntry(ILogEntry logEntry)
            {
                lock (_lock)
                {
                    try
                    {
                        using (FileStream _logStream = new FileStream(this.FilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                        {
                            StreamWriter _logWriter = new StreamWriter(_logStream);

                            _logStream.Seek(0, SeekOrigin.End);
                            _logWriter.WriteLine(logEntry.ToString());
                            _logWriter.Flush();
                            _logWriter.Close();
                            _logStream.Close();
                        }
                    }
                    catch (IOException)
                    { }
                }
            }

            private void logWritten(IAsyncResult myResult)
            {
                //Todo : Change AsyncResult to IAsyncResult
                IAsyncResult Result = (IAsyncResult)myResult;

                //Todo : Change AsyncDelegate to AsyncState
                if (Result.AsyncState is WriteLogDelegate)
                {
                    //Todo : Change AsyncDelegate to AsyncState
                    WriteLogDelegate _endProcess = (WriteLogDelegate)Result.AsyncState;
                    try
                    {
                        _endProcess.EndInvoke(myResult);
                    }
                    catch{}
                }
            }
        }

        [Serializable()]
        public class BinaryLogWriter : BaseLogWriter
        {
            private static object _lock = new object();
            private byte[] binaryData = null;
            public BinaryLogWriter()
                : this(DEFAULT_LOGPATH)
            {
            }

            public BinaryLogWriter(string filePath)
            {
                this.FilePath = filePath;
            }

            public BinaryLogWriter(string filePath, byte[] data)
            {
                this.FilePath = filePath;
                binaryData = data;
            }

            public override void LogEntry(ILogEntry logEntry)
            {
                lock (_lock)
                {
                    try
                    {
                        using (FileStream _logStream = new FileStream(this.FilePath, FileMode.Append, FileAccess.Write, FileShare.Read))
                        {
                            //FileStream _logStream = new FileStream(this.FilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
                            BinaryWriter _logWriter = new BinaryWriter(_logStream);
                            _logWriter.Write(binaryData);
                            _logWriter.Flush();
                            _logWriter.Close();
                            _logStream.Close();
                        }
                    }
                    catch (IOException)
                    { }
                }
            }
        }

        [Serializable()]
        public class FogBugzLogWriter
        {
            /// <author>Falguni Parikh</author>
            /// <created>20-Feb-2006</created>
            /// <summary>To log the error occured in tblEmailLog
            /// </summary>
            /// <param name="ex">
            /// </param>
            /// <exception cref="Exception">
            /// </exception>
            public static void LogErrors(Exception ex)
            {
                int postToFogbugz = 0;

                try
                {
                    string settingVal = "1"; //GeneralSetting.GetSettingValue(EnumGeneralSettings.PostToFogbugz);
                    System.Int32.TryParse(settingVal, out postToFogbugz);
                }
                catch { }

                PostToBugscout.PostDataToBugScout(ex);

                if (postToFogbugz == 0)
                {
                    AddinEventLog(ex);
                }
            }

            /// <summary>Adds Exception in EventLog
            /// </summary>
            /// <param name="ex">Exception
            /// </param>
            private static void AddinEventLog(Exception ex)
            {
                if (!EventLog.SourceExists("TCPIP Service"))
                {
                    EventLog.CreateEventSource("TCPIP Service", "Application");
                }
            }
        }
    }
    namespace LogEntries
    {
        using System;
        using CredEcard.CredEncryption.BusinessService;

        public interface ILogEntry
        {
            int ErrorCode
            {
                get;
                set;
            }

            string ErrorDescription
            {
                get;
                set;
            }

            string ReferenceData
            {
                get;
                set;
            }

            string DateStamp
            {
                get;
            }

            string TimeStamp
            {
                get;
            }
        }

        public class BaseLogEntry : ILogEntry
        {
            private const string DEFAULT_DELIMITER = ",";

            private DateTime _curDate = System.DateTime.Now;
            private int _errorCode = 0;
            private string _refData = String.Empty;
            private string _errorDesc = String.Empty;
            private string _delimiter = DEFAULT_DELIMITER;
            private Encryptionkey _logEncryptionKey = null;

            public virtual int ErrorCode
            {
                get { return _errorCode; }
                set { _errorCode = value; }
            }

            public virtual string ErrorDescription
            {
                get { return _errorDesc; }
                set { _errorDesc = value; }
            }

            public virtual string ReferenceData
            {
                get { return _refData; }
                set { _refData = value; }
            }

            public virtual string DateStamp
            {
                get { return _curDate.ToString("yyyyMMdd"); }
            }

            public virtual string TimeStamp
            {
                get { return _curDate.ToString("hh:mm:ss.fff"); }
            }

            protected virtual string Delimiter
            {
                get { return _delimiter; }
                set { _delimiter = value; }
            }

            /// <created>10-Aug-2015</created>
            /// <author>Nidhi Thakrar</author>
            /// <summary>Gets or sets the log encryption key.</summary>
            /// <value>The log encryption key.</value>
            public virtual Encryptionkey LogEncryptionKey
            {
                get { return _logEncryptionKey; }
                set { _logEncryptionKey = value; }
            }

            public override string ToString()
            {
                string description = this.ErrorDescription;

                if (_logEncryptionKey != null)
                {
                    MasterCardEncrypt objEncrypt = new MasterCardEncrypt(_logEncryptionKey);
                    description = objEncrypt.EncryptDataUsingKey(description);
                }

                return this.ReferenceData + this.Delimiter
                    + this.DateStamp + this.Delimiter
                    + this.TimeStamp + this.Delimiter
                    + this.ErrorCode.ToString() + this.Delimiter
                    + description;
            }
        }
    }
}
