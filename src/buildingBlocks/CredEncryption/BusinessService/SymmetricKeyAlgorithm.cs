using CredECard.Common.BusinessService;
using CredEcard.CredEncryption.DataService;
using BaseDatabase;

namespace CredEcard.CredEncryption.BusinessService
{
    public class SymmetricKeyAlgorithm : DataItem, IPersistableV2
    {
        #region Variables

        internal short _symmetricKeyAlgorithmID = 0;
        internal string _algorithmName = string.Empty;

        #endregion

        #region Properties

        /// <author>Bhavin Shah</author>
        /// <created>21-Sep-2012</created>
        /// <summary>Get or Set SymmetricKeyAlgorithmID
        /// </summary>
        public short SymmetricKeyAlgorithmID
        {
            get { return _symmetricKeyAlgorithmID; }
            set { _symmetricKeyAlgorithmID = value; }
        }

        /// <author>Bhavin Shah</author>
        /// <created>21-Sep-2012</created>
        /// <summary>Get or Set AlgorithmName
        /// </summary>
        public string AlgorithmName
        {
            get { return _algorithmName; }
            set { _algorithmName = value; }
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
            //WriteSymmetricKeyAlgorithm wr = new WriteSymmetricKeyAlgorithm(conn);
            // wr.WriteData(this);
        }

        #endregion

        /// <author>Bhavin Shah</author>
        /// <created>21-Sep-2012</created>
        /// <summary>Get specific object
        /// </summary>
        public static SymmetricKeyAlgorithm Specific(short id)
        {
            if (id == 0) return null;

            return ReadSymmetricKeyAlgorithm.ReadSpecific(id);
        }

        #endregion
    }
}