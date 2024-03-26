
namespace ContisGroup.MessageParser.ISO8586Parser
{
    using System;
    using CredECard.Common.BusinessService;
using CredECard.Common.Enums.Authorization;

    public class IssuerScriptCommandList : DataCollection
    {
        #region Methods

        /// <author>Nidhi Thakrar</author>
        /// <created>23-Dec-2014</created>
        /// <summary>Add object to list
        /// </summary>
        public void Add(IssuerScriptCommand itemToAdd)
        {
            List.Add(itemToAdd);
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>23-Dec-2014</created>
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

        /// <author>Nidhi Thakrar</author>
        /// <created>23-Dec-2014</created>
        /// <summary>Remove object from list
        /// </summary>
        public void Remove(IssuerScriptCommand itemToRemove)
        {
            List.Remove(itemToRemove);
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>23-Dec-2014</created>
        /// <summary>Return object based on given index
        /// </summary>
        public IssuerScriptCommand this[int index]
        {
            get
            {
                if (index > this.Count - 1 || index < 0)
                    throw new IndexOutOfRangeException(CommonMessage.GetMessage(EnumErrorConstants.SPECIFIED_INDEX_NOT_FOUND));
                else
                    return (IssuerScriptCommand)List[index];
            }
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>23-Dec-2014</created>
        /// <summary>Get list
        /// </summary>
        public static IssuerScriptCommandList All(EnumIssuerScriptDataElement FieldNo)
        {
            return ReadIssuerScriptCommand.ReadList((int)FieldNo);
        }

        public static IssuerScriptCommandList All()
        {
            return ReadIssuerScriptCommand.ReadList(0);
        }

        #endregion
    }
}
