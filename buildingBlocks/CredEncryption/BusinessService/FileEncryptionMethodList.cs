using System;
using CredECard.Common.BusinessService;
using CredEcard.CredEncryption.DataService;
using BaseDatabase;

namespace CredEcard.CredEncryption.BusinessService
{
    public class FileEncryptionMethodList : DataCollection
    {
        #region Methods

        /// <author>Bhavin Shah</author>
        /// <created>21-Sep-2012</created>
        /// <summary>Add object to list
        /// </summary>
        public void Add(FileEncryptionMethod itemToAdd)
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
        public void Remove(FileEncryptionMethod itemToRemove)
        {
            List.Remove(itemToRemove);
        }

        /// <author>Bhavin Shah</author>
        /// <created>21-Sep-2012</created>
        /// <summary>Return object based on given index
        /// </summary>
        public new FileEncryptionMethod this[int index]
        {
            get
            {
                if (index > this.Count - 1 || index < 0)
                {
                    throw new IndexOutOfRangeException(CommonMessage.GetMessage(EnumErrorConstants.SPECIFIED_INDEX_NOT_FOUND));
                }
                else
                {
                    return (FileEncryptionMethod)List[index];
                }
            }
        }

        /// <author>Bhavin Shah</author>
        /// <created>21-Sep-2012</created>
        /// <summary>Get list
        /// </summary>
        public static FileEncryptionMethodList All()
        {
            return ReadFileEncryptionMethod.ReadList();
        }

        #endregion
    }
}