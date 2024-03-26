using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CredECard.Common.BusinessService;
using ContisGroup.MessageParser.ISO8586Parser;

namespace ContisGroup.MessageParser.ISO8586Parser
{
    /// <author>Sapan Patibandha</author>
    /// <created>30-Aug-2011</created>
    /// <summary>
    /// </summary>
    /// <exception cref="IndexOutOfRangeException">
    /// </exception>
    public class TransactionCodeList : DataCollection
    {
        /// <author>Sapan Patibandha</author>
        /// <created>30-Aug-2011</created>
        /// <summary>
        /// </summary>
        /// <param name="itemToAdd">
        /// </param>
        public void Add(TransactionCode itemToAdd)
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
        public void Remove(TransactionCode itemToRemove)
        {
            List.Remove(itemToRemove);
        }

        /// <author>Sapan Patibandha</author>
        /// <created>30-Aug-2011</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public TransactionCode this[int TransactionCodeID]
        {
            get
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (((TransactionCode)List[i])._transactionCodeID == TransactionCodeID)
                        return (TransactionCode)List[i];
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
        public TransactionCode this[string strCode]
        {
            get
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (((TransactionCode)List[i])._code == strCode)
                        return (TransactionCode)List[i];
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
        public static TransactionCodeList GetAllTransactionCode()
        {
            return ReadTransactionCode.GetTransactionCodeList();
        }
    }
}
