using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CredECard.Common.BusinessService
{
    /// <author>Nidhi Thakrar</author>
    /// <created>07-Mar-2016</created>
    /// <summary></summary>
    public class TabSerializeException : Exception
    {
        private string _fileName = string.Empty;

        /// <author>Nidhi Thakrar</author>
        /// <created>07-Mar-2016</created>
        /// <summary>Initializes a new instance of the <see cref="TabSerializeException"/> class.</summary>
        public TabSerializeException(string fileName, string errorMessage)
            : base(errorMessage)
        {
            _fileName = fileName;
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>18-Mar-2016</created>
        /// <summary>
        /// Report exception with filename , error message and Inner Exception
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="errorMessage"></param>
        /// <param name="inner"></param>
        public TabSerializeException(string fileName, string errorMessage, Exception inner)
            : base(errorMessage, inner)
		{
            _fileName = fileName;
		}

        /// <author>Nidhi Thakrar</author>
        /// <created>07-Mar-2016</created>
        /// <summary>Gets or sets the name of the file.</summary>
        /// <value>The name of the file.</value>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }
    }
}
