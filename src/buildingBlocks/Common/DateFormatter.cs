using System;
using System.IO;

namespace CredECard.Common.BusinessService
{
    /// <summary>
    /// Summary description for GenerateHTML.
    /// </summary>
    public class DateFormatter
    {
        public DateFormatter()
        {
        }
        public string GetDateTime(string date, string format)
        {
            DateTime dt = DateTime.Parse(date);
            return dt.ToString(format);
        }
        /// <author>Shrujal Shah</author>
        /// <created>21-Feb-2006</created>
        /// <summary>Return monthname
        /// </summary>
        /// <param name="month">int
        /// </param>
        /// <returns>string
        /// </returns>
        public static string GetMonth(int month)
        {
            string _monthName = string.Empty;
            switch (month)
            {
                case 1:
                    _monthName = "January";
                    break;
                case 2:
                    _monthName = "February";
                    break;
                case 3:
                    _monthName = "March";
                    break;
                case 4:
                    _monthName = "April";
                    break;
                case 5:
                    _monthName = "May";
                    break;
                case 6:
                    _monthName = "June";
                    break;
                case 7:
                    _monthName = "July";
                    break;
                case 8:
                    _monthName = "August";
                    break;
                case 9:
                    _monthName = "September";
                    break;
                case 10:
                    _monthName = "October";
                    break;
                case 11:
                    _monthName = "November";
                    break;
                case 12:
                    _monthName = "December";
                    break;
            }
            return _monthName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime GetISODateTime(string date, string time)
        {
            if (date == string.Empty && time == string.Empty) return DateTime.MinValue;

            int intDay, intMonth, intYear, intHour = 0, intMin = 0, intSec = 0;
            intYear = DateTime.Now.Year;
            int.TryParse(date.Substring(0, 2), out intMonth);
            int.TryParse(date.Substring(2, 2), out intDay);

            if (time != string.Empty)
            {
                int.TryParse(time.Substring(0, 2), out intHour);
                int.TryParse(time.Substring(2, 2), out intMin);
                int.TryParse(time.Substring(4, 2), out intSec);
            }

            DateTime dt = new DateTime(intYear, intMonth, intDay, intHour, intMin, intSec);

            if (dt.Month > DateTime.Now.Month)
                dt = new DateTime(intYear - 1, intMonth, intDay, intHour, intMin, intSec);

            return dt;
        }

        public static DateTime GetISODateTime(string datetime)
        {
            if (datetime == string.Empty) return DateTime.MinValue;

            string strDate = datetime.Substring(0, 4);

            string strTime = string.Empty;
            if (datetime.Length > 4) strTime = datetime.Substring(4);

            return DateFormatter.GetISODateTime(strDate, strTime);
        }

        /// <author>Krishnan Prajapati</author>
        /// <created>05-Jan-2021</created>
        /// <summary>Return monthname
        /// </summary>
        /// <param name="UnixTime">int64
        /// </param>
        /// <returns>DateTime
        /// </returns>
        public static DateTime UnixTimeStampToDateTime(Int64 UnixTime)
        {
            if (UnixTime > 0)
            {
                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                return dtDateTime.AddSeconds(Convert.ToDouble(UnixTime)).ToUniversalTime();
                
            }
            else {
                return DateTime.MinValue;
            }
        }
    }
    public class SpecialFolderPath
    {
        private string _path = string.Empty;
        private SpecialFolderPath()
        {

        }
        public SpecialFolderPath(string path)
        {
            _path = path;
        }

        public string UserAppDataDir()
        {
            string folderpath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData); // .SpecialFolder.ApplicationData);
            folderpath = Path.Combine(folderpath, _path);
            return folderpath;
        }
        public string ApplicationPath()
        {
            return Environment.CurrentDirectory;
        }
    }
}
