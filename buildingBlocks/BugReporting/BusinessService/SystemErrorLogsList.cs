using CredECard.BugReporting.DataService;
using CredECard.Common.BusinessService;
using System;

namespace CredECard.BugReporting.BusinessService
{
    public class SystemErrorLogsList : DataCollection
    {
        #region Methods

        /// <author>Rishit Rajput</author>
        /// <created>09-Sep-2010</created>
        /// <summary>Add object to list
        /// </summary>
        public void Add(SystemErrorLogs itemToAdd)
        {
            List.Add(itemToAdd);
        }

        /// <author>Rishit Rajput</author>
        /// <created>09-Sep-2010</created>
        /// <summary>Remove object from list based on index
        /// </summary>
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

        /// <author>Rishit Rajput</author>
        /// <created>09-Sep-2010</created>
        /// <summary>Remove object from list
        /// </summary>
        public void Remove(SystemErrorLogs itemToRemove)
        {
            List.Remove(itemToRemove);
        }

        /// <author>Rishit Rajput</author>
        /// <created>09-Sep-2010</created>
        /// <summary>Return object based on given index
        /// </summary>
        public new SystemErrorLogs this[int index]
        {
            get
            {
                if (index > this.Count - 1 || index < 0)
                {
                    throw new IndexOutOfRangeException(CommonMessage.GetMessage(EnumErrorConstants.SPECIFIED_INDEX_NOT_FOUND));
                }
                else
                {
                    return (SystemErrorLogs)List[index];
                }
            }
        }

        /// <author>Rishit Rajput</author>
        /// <created>09-Sep-2010</created>
        /// <summary>Get list
        /// </summary>
        public static SystemErrorLogsList All()
        {
            return ReadSystemErrorLogs.ReadList();
        }

        #endregion
    }
}
