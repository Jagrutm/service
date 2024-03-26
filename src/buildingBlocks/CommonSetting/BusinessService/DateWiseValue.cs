using CommonSetting.DataService;
using CredECard.Common.BusinessService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonSetting.BusinessService
{
    public class DateWiseValue : DataItem
    {
        #region Variables

        internal int _dateWiseValueID = 0;
        internal DateTime _date = DateTime.MinValue;
        internal int _valueTypeID = 0;
        internal Int64 _value = 0;
        internal Int64 _minValue = 0;
        internal Int64 _maxValue = 0;

        #endregion

        #region Properties

        /// <author>Mahesh Vala</author>
        /// <created>07-Feb-2017</created>
        /// <summary>Get or Set DateWiseValueID
        /// </summary>
        public int DateWiseValueID
        {
            get { return _dateWiseValueID; }
            set { _dateWiseValueID = value; }
        }

        /// <author>Mahesh Vala</author>
        /// <created>07-Feb-2017</created>
        /// <summary>Get or Set Date
        /// </summary>
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        /// <author>Mahesh Vala</author>
        /// <created>07-Feb-2017</created>
        /// <summary>Get or Set ValueTypeID
        /// </summary>
        public int ValueTypeID
        {
            get { return _valueTypeID; }
            set { _valueTypeID = value; }
        }

        /// <author>Mahesh Vala</author>
        /// <created>07-Feb-2017</created>
        /// <summary>Get or Set Value
        /// </summary>
        public Int64 Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <author>Mahesh Vala</author>
        /// <created>07-Feb-2017</created>
        /// <summary>Get or Set MinValue
        /// </summary>
        public Int64 MinValue
        {
            get { return _minValue; }
            set { _minValue = value; }
        }

        /// <author>Mahesh Vala</author>
        /// <created>07-Feb-2017</created>
        /// <summary>Get or Set MaxValue
        /// </summary>
        public Int64 MaxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; }
        }

        #endregion

        #region Methods

        public static DateWiseValue GetDateWiseValueByValueTypeID(int valueTypeID)
        {
            if (valueTypeID == 0) return null;

            return ReadDateWiseValue.ReadDateWiseValueByValueTypeID(valueTypeID);
        }

        /// <author>Mahesh Vala</author>
        /// <created>07-Feb-2017</created>
        /// <summary>UpdateDataWiseValue
        /// </summary>
        public void UpdateDataWiseValue()
        {
            SafeDataController con = new SafeDataController();
            con.BeginTransaction(CredECard.Common.BusinessService.CredECardConfig.GetReadWriteConnectionString(), "TxnUpdateDataWiseValue");
            try
            {
                WriteDataWiseValue objWriteDataWiseValue = new WriteDataWiseValue(con);
                objWriteDataWiseValue.UpdateDataWiseValue(this);
                con.CommitTransaction();
            }
            catch
            {
                if (con.InTransaction) con.RollbackTransaction(); //Rajeshree Gajjar - Case : 90058
                throw;
            }
        }

        #endregion
    }
}
