using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CredECard.Common.BusinessService
{
    /// <author>Nidhi Thakrar</author>
    /// <created>26-Apr-2016</created>
    /// <summary>add all extension methods in this class</summary>
    public static class ExtensionMethods
    {
        /// <author>Nidhi Thakrar</author>
        /// <created>26-Apr-2016</created>
        /// <summary>this method will replace pipe "|" with "&pipe;"</summary>
        public static string ReplacePipe(this string data)
        {
            string value = string.Empty;
            if (data != null)
                value = data.Replace("|", "&pipe;");
            return value;
        }
    }
}
