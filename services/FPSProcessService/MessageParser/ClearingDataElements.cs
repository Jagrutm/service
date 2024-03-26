using System;

namespace ContisGroup.MessageParser.ISO8586Parser
{
    [Serializable()]
    public class ClearingDataElements : DataElement, ICloneable
    {
        internal int _clearingDataFieldID = 0;
        internal short _clearingDataTypeID = 0;
        internal int _startPosition = 0;
        internal int _tcrID = 0;
        internal int _transactionCodeID = 0;
        internal short _settlementReportSubGroupID = 0;
        internal int _sequence = 0;

        internal TransactionComponentRecords _tcr = null;
        internal TransactionCode _transactionCode = null;

        public short SettlementReportSubGroupID
        {
            get { return _settlementReportSubGroupID; }
        }

        public int ClearingDataFieldID
        {
            get{return _clearingDataFieldID;}
            set{_clearingDataFieldID = value;}
        }

        public short ClearingDataTypeID
        {
            get { return _clearingDataTypeID; }
            set { _clearingDataTypeID = value; }
        }

        public int StartPosition
        {
            get{return _startPosition;}
            set{_startPosition = value;}
        }

        public int TCRID
        {
            get { return _tcrID; }
            set { _tcrID = value; }
        }

        public int TransactionCodeID
        {
            get { return _transactionCodeID; }
            set { _transactionCodeID = value; }
        }

        public TransactionComponentRecords TCR
        {
            get { return _tcr; }
            set { _tcr = value; }
        }

        public TransactionCode TransactionCode
        {
            get { return _transactionCode; }
            set { _transactionCode = value; }
        }

        #region ICloneable Members

        new public ClearingDataElements Clone()
        {
            return (ClearingDataElements)((ICloneable)this).Clone();
        }
        
        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

        public override byte[] GetBytesFromMessage(byte[] receivedBytes, int start, int length)
        {
            byte[] returnByte = new byte[length];
            try
            {
                for (int i = start, j = 0; i < start + length; i++, j++)
                {
                    if (i < receivedBytes.Length)
                        returnByte[j] = receivedBytes[i];
                    else
                        returnByte[j] = Convert.ToByte(' ');
                }
            }
            catch (Exception ex) { }

            return returnByte;
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>28-Feb-2013</created>
        /// <summary>This method returns string of this object into message format, this method works only for Fixed Data Representation, and without sub element.</summary>
        public override string ToString()
        {
            string dataelement = string.Empty;

            if (this._dataRepresentation == EnumDataRepresentment.Fixed)
            {
                if ((_elementFormat.ToLower() == "n" || _elementFormat.ToLower() == "un") && this._fieldValue.Length < this.ISODataElementLength)
                    this._fieldValue = this._fieldValue.PadLeft(this.ISODataElementLength, '0');
                else if ((_elementFormat.ToLower() == "ans" || _elementFormat.ToLower() == "an") && this._fieldValue.Length < this.ISODataElementLength)
                    this._fieldValue = this._fieldValue.PadRight(this.ISODataElementLength, ' ');
                dataelement = this._fieldValue;
            }
            return dataelement;
        }
    }
}
