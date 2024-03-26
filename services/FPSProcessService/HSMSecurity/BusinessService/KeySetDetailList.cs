namespace CredECard.CardProduction.BusinessService
{
    using System;
    using CredECard.Common.BusinessService;
    using CredECard.CardProduction.DataService;
    using System.Linq;

    public class KeySetDetailList : DataCollection, IPersistableV2
    {

        public short _keyID = 0;
        public short _keySetID = 0;

        #region Methods

        /// <author>Keyur Parekh</author>
        /// <created>13-Aug-2010</created>
        /// <summary>Add object to list
        /// </summary>
        public void Add(KeySetDetail itemToAdd)
        {
            List.Add(itemToAdd);
        }

        /// <author>Keyur Parekh</author>
        /// <created>13-Aug-2010</created>
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

        /// <author>Keyur Parekh</author>
        /// <created>13-Aug-2010</created>
        /// <summary>Remove object from list
        /// </summary>
        public void Remove(KeySetDetail itemToRemove)
        {
            List.Remove(itemToRemove);
        }

        

        //public KeySetDetail this[int bin, EnumKeys key]
        //{
        //    get
        //    {
        //        KeySetDetail objKSD = null;
        //        foreach (KeySetDetail obj in this)
        //        {
        //            if (obj._bin == bin && obj.Key == key)
        //                objKSD = obj;
        //        }

        //        return objKSD;
        //    }
        //}

        public KeySetDetail GetKeySetFromList(int bin, EnumKeys key,short index)
        {
            var newList = from KeySetDetail element in this
                          where element.Key == key & element._keyIndex == index
                          select element;

            if (newList == null) return null;

            KeySetDetail pdsElement = null;
            foreach (KeySetDetail element in newList)
            {
                pdsElement = element;
                break;
            }
            return pdsElement;
        }

        /// <author>Keyur Parekh</author>
        /// <created>13-Aug-2010</created>
        /// <summary>Get list
        /// </summary>
        public static KeySetDetailList All()
        {
            return ReadKeySetDetail.ReadList();
        }

        /// <author>Keyur Parekh</author>
        /// <created>13-Aug-2010</created>
        /// <summary>Get list of Particular Bin
        /// </summary>
        //public static KeySetDetailList AllofBin(int bin)
        //{
        //    return ReadKeySetDetail.ReadList(bin);
        //}

        /// <author>Bhavin Shah</author>
        /// <created>11-Feb-2013</created>
        /// <summary>Get list of All KeyDetails
        /// </summary>
        public static KeySetDetailList GetKeySetDetailInfo()
        {
            return ReadKeySetDetail.GetKeySetDetailInfo();
        }

        /// <author>Bhavin Shah</author>
        /// <created>22-03-2013</created>
        /// <summary>Save list in DB
        /// </summary>
        public void Save(DataController conn)
        {
            foreach (KeySetDetail singleItem in this)
            {
                if (singleItem._keyID != 0)
                {
                    //singleItem._keySetID = this._keySetID;
                 
                    ((IPersistableV2)singleItem).Save(conn);
                }

            }

        }
        #endregion
    }
}
