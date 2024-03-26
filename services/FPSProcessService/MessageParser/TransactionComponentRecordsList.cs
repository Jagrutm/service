using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CredECard.Common.BusinessService;

namespace ContisGroup.MessageParser.ISO8586Parser
{
    /// <author>Sapan Patibandha</author>
    /// <created>30-Aug-2011</created>
    /// <summary>
    /// </summary>
    /// <exception cref="IndexOutOfRangeException">
    /// </exception>
    public class TransactionComponentRecordsList : DataCollection
    {
        /// <author>Sapan Patibandha</author>
        /// <created>30-Aug-2011</created>
        /// <summary>
        /// </summary>
        /// <param name="itemToAdd">
        /// </param>
        public void Add(TransactionComponentRecords itemToAdd)
        {
            List.Add(itemToAdd);
        }

        /// <author>Sapan Patibandha</author>
        /// <created>30-Aug-2011</created>
        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <exception cref="IndexOutOfRangeException">
        /// </exception>
        public void Remove(int index)
        {
            if (index > this.Count - 1 || index < 0)
            {
                throw new IndexOutOfRangeException(CommonMessage.GetMessage(EnumErrorConstants.SPECIFIED_INDEX_NOT_FOUND));
            }
            else
            {
                List.RemoveAt(index);
            }
        }

        /// <author>Sapan Patibandha</author>
        /// <created>30-Aug-2011</created>
        /// <summary>
        /// </summary>
        /// <param name="itemToRemove">
        /// </param>
        public void Remove(TransactionComponentRecords itemToRemove)
        {
            List.Remove(itemToRemove);
        }

        /// <author>Sapan Patibandha</author>
        /// <created>30-Aug-2011</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public TransactionComponentRecords this[int tcrID]
        {
            get
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (((TransactionComponentRecords)List[i])._tcrID == tcrID)
                        return (TransactionComponentRecords)List[i];
                }
                return null;
            }
        }

        /// <author>Sapan Patibandha</author>
        /// <created>30-Aug-2011</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public TransactionComponentRecords this[string strCode]
        {
            get
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (((TransactionComponentRecords)List[i])._tcrCode == strCode)
                        return (TransactionComponentRecords)List[i];
                }
                return null;
            }
        }

        /// <author>Sapan Patibandha</author>
        /// <created>30-Aug-2011</created>
        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static TransactionComponentRecordsList GetAllTCR()
        {
            return ReadTransactionComponentRecords.GetTCRList();
        }

    }
}
