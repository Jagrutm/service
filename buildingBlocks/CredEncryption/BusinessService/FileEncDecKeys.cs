using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CredECard.Common.BusinessService;
using CredEncryption.DataService;
using CredEncryption.BusinessService;
using BaseDatabase;

namespace CredEcard.CredEncryption.BusinessService
{
    [Serializable]
    public class FileEncDecKeys : DataItem, IPersistableV2, IPersistable
    {

        #region Variables

        internal int _fileEncDecKeyID = 0;
        internal int _providerFileSetupID = 0;
        internal int _encKeyTypeID = 0;
        internal int _decKeyTypeID = 0;

        #endregion

        #region Properties

        /// <author>vipul</author>
        /// <created>25-Jun-2014</created>
        /// <summary>Get or Set FileEncDecKeyID
        /// </summary>
        public int FileEncDecKeyID
        {
            get { return _fileEncDecKeyID; }
            set { _fileEncDecKeyID = value; }
        }

        /// <author>vipul</author>
        /// <created>25-Jun-2014</created>
        /// <summary>Get or Set ProviderFileSetupID
        /// </summary>
        public int ProviderFileSetupID
        {
            get { return _providerFileSetupID; }
            set { _providerFileSetupID = value; }
        }

        /// <author>vipul</author>
        /// <created>25-Jun-2014</created>
        /// <summary>Get or Set EncKeyTypeID
        /// </summary>
        public int EncKeyTypeID
        {
            get { return _encKeyTypeID; }
            set { _encKeyTypeID = value; }
        }

        /// <author>vipul</author>
        /// <created>25-Jun-2014</created>
        /// <summary>Get or Set DecKeyTypeID
        /// </summary>
        public int DecKeyTypeID
        {
            get { return _decKeyTypeID; }
            set { _decKeyTypeID = value; }
        }

        #endregion

        #region Method

        #region IPersistableV2 Members

        /// <author>vipul</author>
        /// <created>25-Jun-2014</created>
        /// <summary>Save Data 
        /// </summary>
        /// <param name="conn">DataController</param>
        public void Save(DataController conn)
        {
            WriteFileEncDecKeys wr = new WriteFileEncDecKeys(conn);
            wr.WriteData(this);
        }


        void IPersistableV2.Save(DataController conn)
        {
            WriteFileEncDecKeys wr = new WriteFileEncDecKeys(conn);
            wr.WriteData(this);
        }

        #endregion

        /// <author>vipul</author>
        /// <created>25-Jun-2014</created>
        /// <summary>Get specific object
        /// </summary>
        public static FileEncDecKeys Specific(int id)
        {
            if (id == 0) return null;

            return ReadFileEncDecKeys.ReadSpecific(id);
        }

        #endregion



    }
}
