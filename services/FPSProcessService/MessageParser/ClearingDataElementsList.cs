using System;
using System.Linq;
using System.Text;
using CredECard.Common.BusinessService;
using CredECard.Common.Enums.Authorization;

namespace ContisGroup.MessageParser.ISO8586Parser
{
    [Serializable()]
    public class ClearingDataElementsList : DataCollection, ICloneable
    {
        public void Add(ClearingDataElements itemToAdd)
        {
            List.Add(itemToAdd);
        }

        public void Remove(int index)
        {
            if (index > this.Count - 1 || index < 0)
            {
                throw new IndexOutOfRangeException("Specified index not found");
            }
            else
            {
                List.RemoveAt(index);
            }
        }

        public void Remove(ClearingDataElements itemToRemove)
        {
            List.Remove(itemToRemove);
        }

        public ClearingDataElements this[int index]
        {
            get
            {
                if (index > this.Count - 1 || index < 0)
                {
                    throw new IndexOutOfRangeException("Specified index not found");
                }
                else
                {
                    return (ClearingDataElements)List[index];
                }
            }
            set
            {
                if (index > this.Count - 1 || index < 0)
                {
                    throw new IndexOutOfRangeException("Specified index not found");
                }
                else
                {
                    List[index] = value;
                }
            }
        }

        public ClearingDataElements this[EnumClearingDataElement enumDataElement]
        {
            get
            {
                int index = (int)enumDataElement;
                return this[index.ToString()];
            }
        }

        public ClearingDataElements this[string fieldNo]
        {
            get
            {
                ClearingDataElements messageFound = null;

                for (int i = 0; i < this.Count; i++)
                {
                    if (((ClearingDataElements)List[i]).FieldNo == Convert.ToInt32(fieldNo))
                    {
                        messageFound = (ClearingDataElements)List[i];
                        break;
                    }
                }
                return messageFound;
            }
        }

        public static ClearingDataElementsList GetAllElementList()
        {
            return ReadClearingDataElement.GetClearingDataElementList(true);
        }


        /// <author>Sapan Patibandha</author>
        /// <created>10-Jul-2020</created>
        /// <summary>
        /// Get Clearing data elements by product type
        /// </summary>
        /// <param name="productType"></param>
        /// <returns></returns>
        public static ClearingDataElementsList GetAllElementList(EnumProductType productType)
        {
            return ReadClearingDataElement.GetClearingDataElementList(productType);
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>27-Feb-2013</created>
        /// <summary>Gets all outgoing element list.</summary>
        public static ClearingDataElementsList GetAllOutgoingElementList()
        {
            return ReadClearingDataElement.GetClearingDataElementList(false);
        }

        public static ClearingDataElementsList GetElementListByTC(ClearingDataElementsList list, int tc)
        {
            var newList = from ClearingDataElements element in list
                          where element._transactionCodeID == tc
                          orderby element.FieldNo
                          select element;

            if (newList == null) return null;

            ClearingDataElementsList newFilterlist = new ClearingDataElementsList();
            foreach (ClearingDataElements element in newList)
            {
                newFilterlist.Add(element);
            }

            return newFilterlist;
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>27-Feb-2013</created>
        /// <summary>Gets the outgoing element list for given TC order by TCR then by sequence</summary>
        public static ClearingDataElementsList GetOutgoingElementListByTC(ClearingDataElementsList list, int tc)
        {
            var newList = from ClearingDataElements element in list
                          where element._transactionCodeID == tc
                          orderby Convert.ToInt32(element.TCR._tcrCode) , element._sequence
                          select element;

            if (newList == null) return null;

            ClearingDataElementsList newFilterlist = new ClearingDataElementsList();
            foreach (ClearingDataElements element in newList)
            {
                newFilterlist.Add(element);
            }

            return newFilterlist;
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>28-Feb-2013</created>
        /// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
        public override string ToString()
        {
            if (this == null) return string.Empty;
            StringBuilder response = new StringBuilder();
            string prvTCR = string.Empty;
            foreach (ClearingDataElements element in this)
            {
                if (prvTCR == string.Empty || prvTCR == element.TCR.TCRCode)
                    response.Append(element.ToString());
                else if (prvTCR != element.TCR.TCRCode)
                {
                    response.AppendLine();
                    response.Append(element.ToString());
                }
                prvTCR = element.TCR.TCRCode;
            }
            return response.ToString();
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>28-Feb-2013</created>
        /// <summary>Gets the number of TC rs.</summary>
        public int GetNumberOfTCRs()
        {
            int tcrs = 0;
            var count = (from ClearingDataElements element in this
                         select element.TCRID).Distinct();

            if (count != null)
            {
                foreach (int item in count)
                {
                    tcrs++;
                }
            }

                return tcrs;
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>28-Feb-2013</created>
        /// <summary>Clears the value.</summary>
        public void ClearValue()
        {
            foreach (ClearingDataElements element in this)
            {
                element.FieldValue = string.Empty;
                element.ActualElementLength = 0;
            }
        }

        public static ClearingDataElementsList GetElementListByTCAndTCR(ClearingDataElementsList list, int tc, int tcr)
        {
            var newList = from ClearingDataElements element in list
                          where element._transactionCodeID == tc && element._tcrID == tcr
                          orderby element.FieldNo
                          select element;

            if (newList == null) return null;

            ClearingDataElementsList newFilterlist = new ClearingDataElementsList();
            foreach (ClearingDataElements element in newList)
            {
                newFilterlist.Add(element);
            }

            return newFilterlist;
        }

        public static ClearingDataElementsList GetElementListByTCAndTCR(ClearingDataElementsList list, int tc, int tcr, short reportSubGroupID)
        {
            var newList = from ClearingDataElements element in list
                          where element._transactionCodeID == tc && element._tcrID == tcr && element._settlementReportSubGroupID == reportSubGroupID
                          orderby element._startPosition
                          select element;

            if (newList == null) return null;

            ClearingDataElementsList newFilterlist = new ClearingDataElementsList();
            foreach (ClearingDataElements element in newList)
            {
                newFilterlist.Add(element);
            }

            return newFilterlist;
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>28-Dec-2015</created>
        /// <summary>Distincts this instance.</summary>
        public ClearingDataElementsList Distinct()
        {
            var distinctList = (from ClearingDataElements element in this
                               select element).GroupBy(x => x._dataElementID).Select(y => y.FirstOrDefault());

            if (distinctList == null)
                return null;

            ClearingDataElementsList newDistinctList = new ClearingDataElementsList();
            foreach (ClearingDataElements element in distinctList)
            {
                newDistinctList.Add(element);
            }

            return newDistinctList;
        }

        #region ICloneable Members

        /// <summary>
        /// clone AgreementLimit object
        /// </summary>
        /// <returns>AgreementLimit</returns>
        public ClearingDataElementsList Clone()
        {
            return (ClearingDataElementsList)((ICloneable)this).Clone();
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// clone AgreementLimit object
        /// </summary>
        /// <returns>object</returns>
        object ICloneable.Clone()
        {
            ClearingDataElementsList list = new ClearingDataElementsList();
            foreach (ClearingDataElements element in this)
            {
                ClearingDataElements newElement = element.Clone();
                list.Add(newElement);
            }
            return list;
        }
        #endregion

    }
}
