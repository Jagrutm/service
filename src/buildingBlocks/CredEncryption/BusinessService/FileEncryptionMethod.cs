using CredECard.Common.BusinessService;
using CredEcard.CredEncryption.DataService;
using BaseDatabase;

namespace CredEcard.CredEncryption.BusinessService
{
    public class FileEncryptionMethod : DataItem, IPersistableV2
    {
        #region Variables

        internal short _fileEncryptionMethodID = 0;
        internal string _methodName = string.Empty;

        #endregion

        #region Properties

        /// <author>Bhavin Shah</author>
        /// <created>21-Sep-2012</created>
        /// <summary>Get or Set FileEncryptionMethodID
        /// </summary>
        public short FileEncryptionMethodID
        {
            get { return _fileEncryptionMethodID; }
            set { _fileEncryptionMethodID = value; }
        }

        /// <author>Bhavin Shah</author>
        /// <created>21-Sep-2012</created>
        /// <summary>Get or Set MethodName
        /// </summary>
        public string MethodName
        {
            get { return _methodName; }
            set { _methodName = value; }
        }

        #endregion

        #region Method

        #region IPersistableV2 Members

        /// <author>Bhavin Shah</author>
        /// <created>21-Sep-2012</created>
        /// <summary>Save Data 
        /// </summary>
        /// <param name="conn">DataController</param>
        public void Save(DataController conn)
        {
            //WriteFileEncryptionMethod wr = new WriteFileEncryptionMethod(conn);
            //wr.WriteData(this);
        }

        #endregion

        /// <author>Bhavin Shah</author>
        /// <created>21-Sep-2012</created>
        /// <summary>Get specific object
        /// </summary>
        public static FileEncryptionMethod Specific(short id)
        {
            if (id == 0) return null;

            return ReadFileEncryptionMethod.ReadSpecific(id);
        }

        #endregion
    }
}