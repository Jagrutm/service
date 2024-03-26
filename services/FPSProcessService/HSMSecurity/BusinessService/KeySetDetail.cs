namespace CredECard.CardProduction.BusinessService
{
    using CredECard.Common.BusinessService;
    using CredECard.CardProduction.DataService;

    public enum EnumKeys
    {
        NONE = 0,

        VIS_PEK = 1,  //Visa Pin Encryption Key 
        VIS_PVKA = 2, //VISA Pin Verification Key A
        VIS_PVKB = 3, //VISA Pin Verification Key B 
        VIS_CVKA = 4, //VISA Card Verification Key A
        VIS_CVKB = 5, //VISA Card Verification Key B
        VIS_C2KA = 6, //VISA Card Verification Key 2 A 
        VIS_C2KB = 7, //VISA Card Verification Key 2 B
        VIS_CAK = 8, //VISA Card Authentication Key  - Double lengh
        VIS_MDK = 10, //VISA Master Derivation Key,

        PER_ZMK = 11,       //Personalizer (IDData) Zone Master Key 
        PER_ZPK = 12,       //Personalizer (IDData) Zone Pin Key
        PER_MDK_MAC = 13,   //Personalizer (IDData) Master Message Authentication Code Key  
        PER_MDK_ENC = 14,   //Personalizer (IDData) Encryption MDK Master Key

        VIS_ZCMK = 15,     //Visa Zone Control Master Key
        CON_PEK = 16,      //Contis Pin Encryption Key
        VIS_CAKA = 17,
        VIS_CAKB = 18,

        VIS_VTS_WSD = 21, //Visa VTS Webservice related WSD key, used for Enc-Dec
        VIS_VTS_WSD_IV = 22, //Visa VTS Webservice related WSD IV, used for Enc-Dec

        VOC_ZMK = 23, // Vocalink Zone master key.
        VOC_NEXT_ZMK = 24, // Vocalink Next Zone master key.
        VOC_MACVKEY_000 = 25, // Vocalink MAC verification key index 000.
        VOC_MACVKEY_001 = 26, // Vocalink MAC verification key index 001.
        VOC_MACVKEY_002 = 27, // Vocalink MAC verification key index 002.
        VOC_MACVKEY_003 = 28, // Vocalink MAC verification key index 003.
        VOC_MACVKEY_004 = 29, // Vocalink MAC verification key index 004.
        VOC_MACVKEY_005 = 30, // Vocalink MAC verification key index 005.
        VOC_MACVKEY_006 = 31, // Vocalink MAC verification key index 006.
        VOC_MACVKEY_007 = 32, // Vocalink MAC verification key index 007.
        VOC_MACVKEY_008 = 33, // Vocalink MAC verification key index 008.
        VOC_MACVKEY_009 = 34, // Vocalink MAC verification key index 009.
        VOC_MACVKEY_010 = 35, // Vocalink MAC verification key index 010.
        VOC_MACVKEY_011 = 36, // Vocalink MAC verification key index 011.
        //MC_AAV = 37 // MC AAV 


        FPS_ZMK_IN = 37,            //ZMK for incoming
        FPS_ZMK_IN_2 = 38, 
        FPS_ZMK_OUT = 39,           //ZMK for outgoing
        FPS_ZMK_OUT_2 = 40,
        FPS_NEXT_ZMK_SLOT_NO = 41, 
        //FPS_MACVKEY_003 = 42, 
        //FPS_MACVKEY_004 = 43, 
        //FPS_MACVKEY_005 = 44, 
        //FPS_MACVKEY_006 = 45, 
        //FPS_MACVKEY_007 = 46, 
        //FPS_MACVKEY_008 = 47, 
        //FPS_MACVKEY_009 = 48, 
        //FPS_MACVKEY_010 = 49, 
        //FPS_MACVKEY_011 = 50, 
    }

    public class KeySetDetail : DataItem, IPersistableV2
    {
        #region Variables

        //internal int _bin = 0;
        internal short _keySetDetailID = 0;
        //internal short _keySetID = 0;
        internal short _keyID = 0;
        internal string _keyValue = string.Empty;
        internal short _keyIndex = 0;
        internal int _slotNo = 0;
        internal string _kcv = string.Empty;
        internal string _KeySetName = string.Empty;
        internal string _KeyName = string.Empty;
        #endregion

        #region Properties

        /// <summary>
        /// Get Key 
        /// </summary>
        public EnumKeys Key
        {
            get
            {
                return (EnumKeys)_keyID;
            }
        }

        /// <author>Keyur Parekh</author>
        /// <created>13-Aug-2010</created>
        /// <summary>Get or Set KeySetDetailID
        /// </summary>
        public short KeySetDetailID
        {
            get { return _keySetDetailID; }
            set { _keySetDetailID = value; }
        }

        /// <author>Keyur Parekh</author>
        /// <created>13-Aug-2010</created>
        /// <summary>Get or Set KeySetID
        /// </summary>
        //public short KeySetID
        //{
        //    get { return _keySetID; }
        //    set { _keySetID = value; }
        //}

        /// <author>Keyur Parekh</author>
        /// <created>13-Aug-2010</created>
        /// <summary>Get or Set KeyID
        /// </summary>
        public short KeyID
        {
            get { return _keyID; }
            set { _keyID = value; }
        }

        /// <author>Keyur Parekh</author>
        /// <created>13-Aug-2010</created>
        /// <summary>Get or Set KeyValue
        /// </summary>
        public string KeyValue
        {
            get { return _keyValue; }
            set { _keyValue = value; }
        }

        /// <author>Keyur Parekh</author>
        /// <created>13-Aug-2010</created>
        /// <summary>Get or Set KeyIndex
        /// </summary>
        public short KeyIndex
        {
            get { return _keyIndex; }
            set { _keyIndex = value; }
        }

        /// <summary>
        /// Get or Set Slot no. of HSM
        /// </summary>
        public int SlotNo
        {
            get { return _slotNo; }
            set { _slotNo = value; }
        }

        /// <author>Bhavin Shah</author>
        /// <created>11-Feb-2013</created>
        /// <summary>Get or Set Kcv
        /// </summary>
        public string Kcv
        {
            get { return _kcv; }
            set { _kcv = value; }
        }

        /// <author>Bhavin Shah</author>
        /// <created>11-Feb-2013</created>
        /// <summary>Get or Set KeySetName
        /// </summary>
        public string KeySetName
        {
            get { return _KeySetName; }
            set { _KeySetName = value; }
        }

        /// <author>Bhavin Shah</author>
        /// <created>11-Feb-2013</created>
        /// <summary>Get or Set KeyName
        /// </summary>
        public string KeyName
        {
            get { return _KeyName; }
            set { _KeyName = value; }
        }


        #endregion

        #region Method

        #region IPersistableV2 Members
        /// <author>Bhavin Shah</author>
        /// <created>11-Feb-2013</created>

        public override ValidateResult Validate()
        {
            FieldErrorList newErrors = new FieldErrorList();

            if (this._KeySetName == "") newErrors.Add(new FieldError("KeySetName", CommonMessage.GetMessage(EnumErrorConstants.MUST_BE_SPECIFIED)));

            if (this._keyID == 0) newErrors.Add(new FieldError("KeyName", CommonMessage.GetMessage(EnumErrorConstants.MUST_BE_SPECIFIED)));

            if (this._slotNo == 0) newErrors.Add(new FieldError("SlotNo", CommonMessage.GetMessage(EnumErrorConstants.MUST_BE_SPECIFIED)));

            if (this._kcv == "") newErrors.Add(new FieldError("Kcv", CommonMessage.GetMessage(EnumErrorConstants.MUST_BE_SPECIFIED)));

            if (this._keyValue == "") newErrors.Add(new FieldError("KeyValue", CommonMessage.GetMessage(EnumErrorConstants.MUST_BE_SPECIFIED)));

            if (this._keyIndex == 0) newErrors.Add(new FieldError("KeyIndex", CommonMessage.GetMessage(EnumErrorConstants.MUST_BE_SPECIFIED)));

            ValidateResult result = new ValidateResult(newErrors);
            return result;
        }

        public void Save(DataController conn)
        {
            WriteKeySetDetail wr = new WriteKeySetDetail(conn);
            wr.WriteData(this);
        }


        /// <author>Bhavin Shah</author>
        /// <created>21-Mar-2013</created>
        /// <summary>Get specific object
        /// </summary>
        public static KeySetDetail Specific(short keySetDetailID)
        {
            if (keySetDetailID == 0) return null;

            return ReadKeySetDetail.ReadSpecific(keySetDetailID);
        }

        #endregion

        #endregion
    }
}
