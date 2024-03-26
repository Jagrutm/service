using CredECard.Common.BusinessService;
using CredEcard.CredEncryption.DataService;
using BaseDatabase;

namespace CredEcard.CredEncryption.BusinessService
{
    public class EncryptFileformat : DataItem, IPersistableV2
    {
        #region Variables

        internal short _encryptFileFormatID = 0;
        internal string _formatName = string.Empty;

        #endregion

        #region Properties

        /// <author>Bhavin Shah</author>
        /// <created>21-Sep-2012</created>
        /// <summary>Get or Set EncryptFileFormatID
        /// </summary>
        public short EncryptFileFormatID
        {
            get { return _encryptFileFormatID; }
            set { _encryptFileFormatID = value; }
        }

        /// <author>Bhavin Shah</author>
        /// <created>21-Sep-2012</created>
        /// <summary>Get or Set FormatName
        /// </summary>
        public string FormatName
        {
            get { return _formatName; }
            set { _formatName = value; }
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
            // WriteEncryptFileformat wr = new WriteEncryptFileformat(conn);
            // wr.WriteData(this);
        }

        #endregion

        /// <author>Bhavin Shah</author>
        /// <created>21-Sep-2012</created>
        /// <summary>Get specific object
        /// </summary>
        public static EncryptFileformat Specific(short id)
        {
            if (id == 0) return null;

            return ReadEncryptFileformat.ReadSpecific(id);
        }

        #endregion
    }
}