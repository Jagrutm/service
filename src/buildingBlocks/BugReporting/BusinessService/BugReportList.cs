using CredECard.BugReporting.DataService;
using CredECard.Common.BusinessService;
using System;

namespace CredECard.BugReporting.BusinessService
{
    /// <summary>
    /// Collection class of BugReport business service
    /// </summary>
    public class BugReportList : DataCollection
    {
        /// <summary>
        /// Adds object of BugReport to the collection
        /// </summary>
        /// <param name="typeToAdd">Object to add into the collection</param>
        public void Add(BugReport typeToAdd)
        {
            List.Add(typeToAdd);
        }

        /// <summary>
        /// Removes object of BugReport from the list at specified index
        /// </summary>
        /// <param name="index">Index of the object to be removed</param>
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

        /// <summary>
        /// Removes specified object of BugReport from the list
        /// </summary>
        /// <param name="itemToRemove">Object of BugReport to be removed</param>
        public void Remove(BugReport itemToRemove)
        {
            List.Remove(itemToRemove);
        }

        /// <summary>
        /// Returns object of BugReport from the list at the specified index 
        /// </summary>
        /// <param name="index">Index of the object in list</param>
        /// <returns>Object of BugReport</returns>
        public new BugReport this[int index]
        {
            get
            {
                if (index > this.Count - 1 || index < 0)
                {
                    throw new IndexOutOfRangeException(CommonMessage.GetMessage(EnumErrorConstants.SPECIFIED_INDEX_NOT_FOUND));
                }
                else
                {
                    return (BugReport)List[index];
                }
            }
        }

        /// <summary>
        /// Gets list of BugReport objects between the specified dates
        /// </summary>
        /// <param name="fromDate">Starting date of the range</param>
        /// <param name="toDate">Ending date of the range</param>
        /// <returns>List of BugReport objects</returns>
        public static BugReportList GetBugsByDate(DateTime fromDate, DateTime toDate)
        {
            return ReadBugReport.GetBugsByDate(fromDate, toDate);
        }

        /// <author>Tina Mori</author>
        /// <created>29-Apr-2009</created>
        /// <summary>Gets list of all BugReport objects
        /// </summary>
        /// <returns>List of BugReport objects
        /// </returns>
        public static BugReportList GetBugs()
        {
            return ReadBugReport.GetBugs();
        }

    }
}
