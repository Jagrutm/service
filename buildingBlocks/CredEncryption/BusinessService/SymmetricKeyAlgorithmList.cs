using System;
using CredECard.Common.BusinessService;
using CredEcard.CredEncryption.DataService;
using BaseDatabase;

namespace CredEcard.CredEncryption.BusinessService
{
    public class SymmetricKeyAlgorithmList : DataCollection
    {
        #region Methods

        /// <author>Bhavin Shah</author>
        /// <created>21-Sep-2012</created>
        /// <summary>Add object to list
        /// </summary>
        public void Add(SymmetricKeyAlgorithm itemToAdd)
        {
            List.Add(itemToAdd);
        }

        /// <author>Bhavin Shah</author>
        /// <created>21-Sep-2012</created>
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

        /// <author>Bhavin Shah</author>
        /// <created>21-Sep-2012</created>
        /// <summary>Remove object from list
        /// </summary>
        public void Remove(SymmetricKeyAlgorithm itemToRemove)
        {
            List.Remove(itemToRemove);
        }

        /// <author>Bhavin Shah</author>
        /// <created>21-Sep-2012</created>
        /// <summary>Return object based on given index
        /// </summary>
        public new SymmetricKeyAlgorithm this[int index]
        {
            get
            {
                if (index > this.Count - 1 || index < 0)
                {
                    throw new IndexOutOfRangeException(CommonMessage.GetMessage(EnumErrorConstants.SPECIFIED_INDEX_NOT_FOUND));
                }
                else
                {
                    return (SymmetricKeyAlgorithm)List[index];
                }
            }
        }

        /// <author>Bhavin Shah</author>
        /// <created>21-Sep-2012</created>
        /// <summary>Get list
        /// </summary>
        public static SymmetricKeyAlgorithmList All()
        {
            return ReadSymmetricKeyAlgorithm.ReadList();
        }

        #endregion
    }
}