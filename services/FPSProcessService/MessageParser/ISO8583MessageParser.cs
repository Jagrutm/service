using System;
using System.Collections;
using System.Text;
using System.IO;

using CredECard.Common.BusinessService;

using CredECard.Common.Enums.Transaction;
using ContisGroup.MessageParser.ISO8586Parser;
using ContisGroup.MessageParser.ISO8586Parser.Interface;
using DataLogging.LogWriters;


namespace ContisGroup.MessageParser.ISO8586Parser
{
    /// <author>Prashant Soni</author>
    /// <created>26-Sep-2006</created>
    /// <summary>
    /// </summary>
    /// <exception cref="ValidateException">
    /// </exception>
    [Serializable()]
    public abstract class ISO8583MessageParser : IMessageFormatter
    {
        #region Private declaration

       
        protected string _receivedMessage = string.Empty;
        protected byte[] _binaryDataMessage = null;
        
        protected string _messageType = string.Empty;
        protected int _messageReasonCode = 0;
        protected bool _isTokenTransaction = false;
        protected string _tranID = string.Empty;

        protected const int BITMAPLENGTHINBYTE = 8;
        //BitArray bit1 = null;

        protected BitMap _bitmap = null;
        protected BitMap _primaryBitMap = null;
        protected BitMap _secondaryBitMap = null;
        protected BitMap _thirdBitMap = null;

        protected DataElementList _dataElementList = null;
        protected SubElementList _subDataElementList = null;
        protected SubFieldList _subFieldList = null;

        protected EnumISOVesion _iso8583Version = EnumISOVesion.None;
        protected bool _isMerchandiseReturns = false;

        #endregion

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public DataElementList DataElementList
        {
            get
            {
                return _dataElementList;
            }
            set
            {
                _dataElementList = value;
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public string MessageType
        {
            get
            {
                return _messageType;
            }
        }


        /// <author>Keyur Parekh</author>
        /// <created>07-Apr-2017</created>
        /// <summary>
        /// Get Message Reason Code
        /// </summary>
        public int MessageReasonCode
        {
            get
            {
                return _messageReasonCode;
            }
        }


        /// <author>Keyur Parekh</author>
        /// <created>07-Apr-2017</created>
        /// <summary>
        /// Get IsTokenTransaction
        /// </summary>
        public bool IsTokenTransaction
        {
            get
            {
                return _isTokenTransaction;
            }
        }

        /// <author>Vipul Patel</author>
        /// <created>22-Feb-2021</created>
        /// <summary>
        /// Get IsMerchandiseReturns
        /// </summary>
        public bool IsMerchandiseReturns
        {
            get
            {
                return _isMerchandiseReturns;
            }
        }

        /// <author>Keyur Parekh</author>
        /// <created>16-Jun-2017</created>
        /// <summary>
        /// Get Transaction ID
        /// </summary>
        public string TranID
        {
            get
            {
                return _tranID;
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public SubElementList FullSubElementList
        {
            get
            {
                return _subDataElementList;
            }
            set
            {
                _subDataElementList = value;
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public SubFieldList FullSubFieldList
        {
            get
            {
                return _subFieldList;
            }
            set
            {
                _subFieldList = value;
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public EnumISOVesion ISO8583Version
        {
            get
            {
                return _iso8583Version;
            }
            set
            {
                _iso8583Version = value;
            }
        }

        #region IMessageFormatter Members

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public byte[] BinaryDataMessage
        {
            get
            {
                return _binaryDataMessage;
            }
            set
            {
                _binaryDataMessage = value;
                if (_binaryDataMessage != null) _receivedMessage = Encoding.Default.GetString(_binaryDataMessage);
                //if (_binaryDataMessage != null) _receivedMessage = HexEncoding.ToString(_binaryDataMessage);
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>26-Sep-2006</created>
        /// <summary>This method parse the received message and convert into message objects and create a message list.
        /// </summary>
        /// <exception cref="ValidateException">
        /// </exception>
        public void ParseMessage(string reportSubGroupNo = "")
        {

            this.ParseReceivedMessage();
            this.SetMessageIdentifier();
        }

        protected virtual void ParseReceivedMessage()
        {

        }

        /// <author>Keyur Parekh</author>
        /// <created>07-Apr-2017</created>
        /// <summary>
        /// This method Set the message identifier
        /// to call particular class based on message identification
        /// </summary>
        protected virtual void SetMessageIdentifier()
        {

        }

        protected virtual void SubElementParsing(DataElement element)
        {
            if (element.HasSubElements)
            {
                element.FullSubElementList = this._subDataElementList;
                element.FullSubFieldList = this._subFieldList;
                element.ParseElements();
            }
        }

        #endregion

        protected static byte[] GetByteFromArray(byte[] sourceArray, int dataLength)
        {
            byte[] returnByte = new byte[dataLength];
            Array.Copy(sourceArray, returnByte, dataLength);

            return returnByte;
        }

        protected static byte[] GetByteFromArray(byte[] sourceArray, int startIndex, int dataLength)
        {
            byte[] returnByte = new byte[dataLength];
            Array.Copy(sourceArray, startIndex, returnByte, 0, dataLength);

            return returnByte;
        }

    }
}

