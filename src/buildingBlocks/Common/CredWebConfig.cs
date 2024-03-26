using System.Configuration;
using System.Text;
using System.Web;

namespace CredECard.Common
{
    public class CredWebConfig
    {
        /// <summary>
        /// Gets the Help path 
        /// </summary>
        /// <value>string</value>
        public static string HelpPath
        {
            get
            {
                return GetDomain() + ConfigurationManager.AppSettings["HelpPath"];
            }
        }

       
        /// <summary>
        /// Gets the account portal path
        /// </summary>
        /// <value>string</value>
        public static string AccountPath
        {
            get
            {
                return GetDomain() + ConfigurationManager.AppSettings["AccountPath"];
            }
        }

        /// <summary>
        /// Gets the main website path
        /// </summary>
        /// <value>string</value>
        public static string WebsitePath
        {
            get
            {
                return GetDomain() + ConfigurationManager.AppSettings["WebsitePath"];
            }
        }

        /// <summary>
        /// Gets the secure login path
        /// </summary>
        /// <value>string</value>
        public static string ACSPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ACSPath"];
            }
        }

        /// <summary>
        /// Gets the barcode site path
        /// </summary>
        /// <value>string</value>
        public static string ADSPath
        {
            get
            {
                return  ConfigurationManager.AppSettings["ADSPath"];
            }
        }

        /// <summary>
        /// Gets the Continue on error page path
        /// </summary>
        /// <value>string</value>
        public static string ErrorContinuePage
        {
            get
            {
                return GetDomain() + ConfigurationManager.AppSettings["ErrorContinuePage"];
            }
        }


        public static string SignupPath
        {
            get
            {
                return GetDomain() + ConfigurationManager.AppSettings["SignupPath"];
            }
        }


        /// <summary>
        /// Gets the account portal path
        /// </summary>
        /// <value>string</value>
        public static string TDSSitePath
        {
            get
            {
                return GetDomain() + ConfigurationManager.AppSettings["TDSSitePath"];
            }
        }

        /// <summary>
        /// Gets the domain name of the current request
        /// </summary>
        /// <value>string</value>
        public static string DomainName
        {
            get
            {
                return HttpContext.Current.Request.Host.Value.ToLower();
            }
        }

        public static string CRMPath
        {
            get
            {
                return GetDomain() + ConfigurationManager.AppSettings["CRMPath"];
            }
        }

        /// <summary>
        /// GEts the domain with either http or https based on whether its secure connection or not.
        /// </summary>
        /// <returns></returns>
        private static string GetDomain()
        {
            StringBuilder domain = new StringBuilder();
            domain.Append(HttpContext.Current.Request.IsHttps ? @"https://" : @"http://");
            domain.Append(HttpContext.Current.Request.Host.Value.ToLower());
            domain.Append("/");
            return domain.ToString();
        }

        /// <summary>
        /// Gets the Report Path
        /// </summary>
        /// <value>string</value>
        public static string ReportPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ReportPath"];
            }
        }
    }
}
