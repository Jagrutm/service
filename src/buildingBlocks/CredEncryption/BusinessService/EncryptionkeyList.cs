using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CredECard.Common.BusinessService;
using CredEcard.CredEncryption.BusinessService;
using CredEcard.CredEncryption.DataService;
using BaseDatabase;

namespace CredEcard.CredEncryption.BusinessService
{
    public class EncryptionkeyList : DataCollection, IPersistableV2
    {

        #region Methods

        /// <author>vipul</author>
        /// <created>23-Jun-2014</created>
        /// <summary>Add object to list
        /// </summary>
        public void Add(Encryptionkey itemToAdd)
        {
            List.Add(itemToAdd);
        }

        /// <author>vipul</author>
        /// <created>23-Jun-2014</created>
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

        /// <author>vipul</author>
        /// <created>23-Jun-2014</created>
        /// <summary>Remove object from list
        /// </summary>
        public void Remove(Encryptionkey itemToRemove)
        {
            List.Remove(itemToRemove);
        }

        /// <author>vipul</author>
        /// <created>23-Jun-2014</created>
        /// <summary>Return object based on given index
        /// </summary>
        public new Encryptionkey this[int index]
        {
            get
            {
                if (index > this.Count - 1 || index < 0)
                {
                    throw new IndexOutOfRangeException(CommonMessage.GetMessage(EnumErrorConstants.SPECIFIED_INDEX_NOT_FOUND));
                }
                else
                {
                    return (Encryptionkey)List[index];
                }
            }
        }


        
        /// <author>Vipul patel</author>
        /// <created>23-June-2014</created>
        /// <summary>Save</summary>
        public void Save(DataController conn)
        {
            foreach (Encryptionkey  singleItem in this)
            {
                singleItem.Save(conn);

            }

        }

        /// <author>vipul Patel</author>
        /// <created>25-June-2014</created>
        /// <summary>Get list
        /// </summary>
        public static EncryptionkeyList All()
        {
            return ReadEncryptionkey.ReadList();
        }

        /// <author>vipul Patel</author>
        /// <created>25-June-2014</created>
        /// <summary>Get Encryption Decrypt key list 
        /// </summary>
        public static EncryptionkeyList EncryptionKeyList()
        {
            return ReadEncryptionkey.GetEncryptionkeyList(true);
        }
        /// <author>vipul Patel</author>
        /// <created>25-June-2014</created>
        /// <summary>Get Encryption Decrypt key list 
        /// </summary>
        public static EncryptionkeyList DecryptionKeyList()
        {
            return ReadEncryptionkey.GetEncryptionkeyList(false);
        }

        #endregion
    }
}
