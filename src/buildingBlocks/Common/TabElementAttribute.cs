using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CredECard.Common.BusinessService
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class TabElementAttribute : Attribute
    {
        private string _elementName = string.Empty;
        private int _elementLength = 0;
        private bool _isMandatory = false;
        private bool  _isRequreToSerialize = true;
        /// <author>Nidhi Thakrar</author>
        /// <created>24-Feb-2016</created>
        /// <summary>Gets or sets the name of the element.</summary>
        /// <value>The name of the element.</value>
        public string ElementName
        {
            get { return _elementName; }
            set { _elementName = value; }
        }

        /// <author>vipul patel</author>
        /// <created>27-Dec-2016</created>
        /// <summary>Gets or sets ElementLength.</summary>
        /// <value>ElementLength.</value>
        public int ElementLength
        {
            get { return _elementLength; }
            set { _elementLength = value; }
        }

        /// <author>vipul patel</author>
        /// <created>27-Dec-2016</created>
        /// <summary>Gets or sets IsMandatory.</summary>
        /// <value>IsMandatory.</value>
        public bool  IsMandatory
        {
            get { return _isMandatory; }
            set { _isMandatory = value; }
        }

        /// <author>vipul patel</author>
        /// <created>27-Dec-2016</created>
        /// <summary>Gets or sets IsMandatory.</summary>
        /// <value>IsMandatory.</value>
        public bool IsRequreToSerialize
        {
            get { return _isRequreToSerialize; }
            set { _isRequreToSerialize = value; }
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>24-Feb-2016</created>
        /// <summary>Initializes a new instance of the <see cref="TabElementAttribute"/> class.</summary>
        /// <param name="elementName">Name of the element.</param>
        public TabElementAttribute(string elementName)
        {
            _elementName = elementName;
            _isRequreToSerialize = true;
        }

        /// <author>vipul patel</author>
        /// <created>27-Dec-2016</created>
        /// <summary>Initializes a new instance of the <see cref="TabElementAttribute"/> class.</summary>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="elementLength">Element Length.</param>
        public TabElementAttribute(string elementName, int elementLength)
        {
            _elementName = elementName;
            _elementLength = elementLength;
        }

        /// <author>vipul patel</author>
        /// <created>27-Dec-2016</created>
        /// <summary>Initializes a new instance of the <see cref="TabElementAttribute"/> class.</summary>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="elementLength">Element Length.</param>
        public TabElementAttribute(string elementName, int elementLength, bool isMandatory)
        {
            _elementName = elementName;
            _elementLength = elementLength;
            _isMandatory = isMandatory;            
        }

        /// <author>vipul patel</author>
        /// <created>27-Dec-2016</created>
        /// <summary>Initializes a new instance of the <see cref="TabElementAttribute"/> class.</summary>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="elementLength">Element Length.</param>
        public TabElementAttribute(string elementName, bool isRequreToSerialize)
        {
            _elementName = elementName;
            _isRequreToSerialize = isRequreToSerialize;
        }
    }
}