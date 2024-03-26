//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Configuration;
//using CredECard.CardProduction.BusinessService;
//using CredECard.BugReporting.BusinessService;
//using System.Net.Sockets;
//using System.Net;
//using CredECard.Common.BusinessService;
//using DataLogging.LogWriters;
//using DataLogging.LogEntries;

//namespace CredECard.HSMSecurity.BusinessService
//{
//    public class HSMCommon
//    {
//        #region Variables

//        private static KeySetDetailList _keysetDetailList = null;
                
//        private static int _debugHSM = Convert.ToInt32(ConfigurationManager.AppSettings["DEBUGHSM"]);
//        private static int _tcpPort = Convert.ToInt32(ConfigurationManager.AppSettings["HSMTcpPortNo"]);
//        private static int _cmdPort = Convert.ToInt32(ConfigurationManager.AppSettings["HSMCmdPortNo"]);

//        private static string _priHSM = ConfigurationManager.AppSettings["PrimaryHSMIPAddress"].ToString();
//        private static string _secHSM = ConfigurationManager.AppSettings["SecondaryHSMIPAddress"].ToString();
//        private static string _currentHSM = _priHSM;

//        private int _bin = 0;
//        private short _index = 1;
//        private short _zoneKeyIndex = 1;

//        internal const int BF_pinGenerationMethod = 3;  //3 Visa - BF Verification Method (VISA)
//        internal const int AW_pinBlockFormat = 1;       //Input PIN block format: 1-Ansi,2-IBM3624,3 PIN Pad 
//        internal const int CM_pinLength = 4;
//        internal string _cardNumber = string.Empty;

//        //internal const string BY_PVKI = "1"; //Pin Verification Key Index 
//        //internal const int DERIVATION_METHOD = 1;    //Derivation Method 0 = EMV 2000,1 = EMV CSK
//        //internal string _cardNumber = string.Empty;

//        #endregion

//        #region Constructor

//        public HSMCommon() { }
//        public HSMCommon(string cardNumber, short index)
//        {
//            _cardNumber = cardNumber;
//            //_cardNumber = cardNumber;
//            if (cardNumber.Length > 6) int.TryParse(cardNumber.Substring(0, 6), out _bin);
//            if(index > 0) _index = index;
//        }

//        public HSMCommon(string cardNumber, short index,short zoneKeyIndex)
//        {
//            _cardNumber = cardNumber;
//            if (cardNumber.Length > 6) int.TryParse(cardNumber.Substring(0, 6), out _bin);
//            if (index > 0) _index = index;
//            if (zoneKeyIndex > 0) _zoneKeyIndex = zoneKeyIndex;
//        }

//        #endregion

//        #region Properties

//        /// <summary>
//        /// Get Key Index
//        /// </summary>
//        internal short Index
//        {
//            get
//            {
//                return _index;
//            }
//        }

//        ///// <summary>
//        /////AK : ZPK Cryptogram Value
//        ///// </summary>
//        //internal string AK_ZPK_CRYPTOGRAM 
//        //{
//        //    get
//        //    {
//        //        return getKeyCryptogram(_bin, EnumKeys.ZPK, _index);
//        //    }
//        //}

//        /// <summary>
//        ///AK : ZPK Cryptogram Value
//        /// </summary>
//        internal int AK_ZPK_SLOT_NO
//        {
//            get
//            {
//                return getKeySlotNo(_bin, EnumKeys.PER_ZPK, 1); //Index fixed as there will be only one ZPK in our database
//            }
//        }

//        /// <summary>
//        ///BT : PEK encrypted under modifier 1 of the MFK 
//        ///Issuer Working Key (IWK)
//        /// </summary>
//        internal int BT_PEK_SLOT_NO
//        {
//            get
//            {
//                return getKeySlotNo(_bin, EnumKeys.VIS_PEK,_zoneKeyIndex);
//            }
//        }

//        ///// <summary>
//        /////BT : PEK encrypted under modifier 1 of the MFK 
//        /////Issuer Working Key (IWK) for Database storage
//        ///// </summary>
//        //internal int BT_PEK_SLOT_NO_FOR_DB
//        //{
//        //    get
//        //    {
//        //        return getKeySlotNo(_bin, EnumKeys.CON_PEK, 1);
//        //    }
//        //}

//        /// <summary>
//        ///CA : Cryptogram of left VISA Key Pair encrypted under modifier 4 of the MFK 
//        /// </summary>
//        internal int PVKA_SLOT_NO
//        {
//            get
//            {
//                return getKeySlotNo(_bin, EnumKeys.VIS_PVKA,_index);
//            }
//        }

        
//        /// <summary>
//        ///CB : Cryptogram of right VISA Key Pair encrypted under modifier 4 of the MFK 
//        /// </summary>
//        internal int PVKB_SLOT_NO
//        {
//            get
//            {
//                return getKeySlotNo(_bin, EnumKeys.VIS_PVKB, _index);
//            }
//        }

//        /// <summary>
//        ///CA : Cryptogram of left VISA Key Pair encrypted under modifier 4 of the MFK 
//        /// </summary>
//        internal int CVKA_SLOT_NO
//        {
//            get
//            {
//                return getKeySlotNo(_bin, EnumKeys.VIS_CVKA, _index);
//            }
//        }


//        /// <summary>
//        ///CB : Cryptogram of right VISA Key Pair encrypted under modifier 4 of the MFK 
//        /// </summary>
//        internal int CVKB_SLOT_NO
//        {
//            get
//            {
//                return getKeySlotNo(_bin, EnumKeys.VIS_CVKB, _index);
//            }
//        }

//        /// <summary>
//        ///CB : Cryptogram of Pin Generation Key (PGK) encrypted under modifier 9 of the MFK 
//        /// </summary>
//        internal int MDK_SLOT_NO
//        {
//            get
//            {
//                return getKeySlotNo(_bin, EnumKeys.VIS_MDK, _index);
//            }
//        }

//        /// <summary>
//        /// Mac Key Slot No.
//        /// </summary>
//        internal int MDK_MAC_SLOT_NO
//        {
//            get
//            {
//                return getKeySlotNo(_bin, EnumKeys.PER_MDK_MAC, _index);
//            }
//        }

//        /// <summary>
//        /// ENC Key Slot No.
//        /// </summary>
//        internal int MDK_ENC_SLOT_NO
//        {
//            get
//            {
//                return getKeySlotNo(_bin, EnumKeys.PER_MDK_ENC, _index);
//            }
//        }

//        /// <summary>
//        /// Contis Pin Encryption Key Slot No.
//        /// Contis will have only only one key, so hardcoded index 1
//        /// </summary>
//        internal int CONTIS_PEK_SLOT_NO
//        {
//            get
//            {
//                return getKeySlotNo(_bin, EnumKeys.CON_PEK, 1);
//            }
//        }

//        /// <summary>
//        ///CB : Cryptogram of Pin Generation Key (PGK) encrypted under modifier 9 of the MFK 
//        /// </summary>
//        //internal string MDK_CRYPTOGRAM
//        //{
//        //    get
//        //    {
//        //        return getKeyCryptogram(_bin, EnumKeys.VIS_MDK, _index);
//        //    }
//        //}

//        //internal string KMAC_CRYPTOGRAM
//        //{
//        //    get
//        //    {
//        //        return getKeyCryptogram(_bin, EnumKeys.PER_MDK_MAC, _index);
//        //    }
//        //}

//        //internal string MDK_ENC_CRYPTOGRAM
//        //{
//        //    get
//        //    {
//        //        return getKeyCryptogram(_bin, EnumKeys.PER_MDK_ENC, _index);
//        //    }
//        //}

//        #endregion

//        #region Methods

//        /// <summary>
//        /// Get Key Slot No.
//        /// </summary>
//        /// <param name="bin"></param>
//        /// <param name="key"></param>
//        /// <param name="index"></param>
//        /// <returns></returns>
//        private int getKeySlotNo(int bin, EnumKeys key, short index)
//        {
//            int slotNo = 0;
//            KeySetDetail objKSD = getAllKeys().GetKeySetFromList(bin, key, index);

//            if (objKSD != null)
//                slotNo = objKSD.SlotNo;
//            else
//            {
//                try
//                {
//                    _keysetDetailList = null;
//                    objKSD = getAllKeys().GetKeySetFromList(bin, key, index);

//                    slotNo = objKSD.KeyIndex;
//                }
//                catch (Exception ex)
//                {
//                    PostToBugscout.PostDataToBugScout(ex);
//                }
//            }

//            return slotNo;
//        }

//        /// <summary>
//        /// Get Key Cryptogram
//        /// </summary>
//        /// <param name="bin"></param>
//        /// <param name="key"></param>
//        /// <param name="index"></param>
//        /// <returns></returns>
//        private string getKeyCryptogram(int bin, EnumKeys key, short index)
//        {
//            string cryptogram = string.Empty;
//            KeySetDetail objKSD = getAllKeys().GetKeySetFromList(bin, key, index);

//            if (objKSD != null)
//                cryptogram = objKSD.KeyValue;
//            else
//            {
//                try
//                {
//                    _keysetDetailList = null;
//                    objKSD = getAllKeys().GetKeySetFromList(bin, key, index);

//                    cryptogram = objKSD.KeyValue;
//                }
//                catch (Exception ex)
//                {
//                    PostToBugscout.PostDataToBugScout(ex);
//                }
//            }

//            return cryptogram;
//        }

//        /// <summary>
//        /// Get All Keys from DB
//        /// </summary>
//        /// <returns></returns>
//        private static KeySetDetailList getAllKeys()
//        {
//            if (_keysetDetailList == null)
//                _keysetDetailList = KeySetDetailList.All();

//            return _keysetDetailList;
//        }

//        /// <summary>
//        /// Execute Exrypt Command
//        /// </summary>
//        /// <param name="request"></param>
//        /// <returns></returns>
//        protected string executeExcrypt(string request)
//        {
//            string endChar = "]";
//            return executeExcrypt(request, endChar);
//        }

//        /// <summary>
//        /// Execute Exrypt Command
//        /// </summary>
//        /// <param name="request"></param>
//        /// <returns></returns>
//        protected string executeExcrypt(string request,string endChar)
//        {
//            DateTime dtReqTime = DateTime.Now;
//            Socket sock = null;
//            string response = string.Empty;
//            int portNo = _tcpPort;

//            if (connect(ref sock, portNo))
//            {
//                byte[] sendData = Encoding.ASCII.GetBytes(request);
//                sock.Send(sendData);

//                response = getExcryptData(ref sock, portNo, endChar);

//                if (sock != null) sock.Send(Encoding.ASCII.GetBytes("QUIT" + Convert.ToChar(10)));
//            }

//            logData(dtReqTime, request, response);

//            return response;
//        }

//        /// <summary>
//        /// Execute Command
//        /// </summary>
//        /// <param name="request"></param>
//        /// <param name="resultSize"></param>
//        /// <returns></returns>
//        protected string executeCommand(string request, int resultSize)
//        {
//            DateTime dtReqTime = DateTime.Now;
            
//            Socket sock = null;
//            string response = String.Empty;
//            byte[] result = new byte[resultSize];
//            int portNo = _cmdPort;

//            if (connect(ref sock, portNo))
//            {
//                byte[] sendData = Encoding.ASCII.GetBytes(request);
//                sock.Send(sendData);

//                sock.Receive(result, 0, resultSize - 1, SocketFlags.None);
//                response = Encoding.Default.GetString(result);

//                if (sock != null) sock.Send(Encoding.ASCII.GetBytes("QUIT" + Convert.ToChar(10)));
//            }

//            logData(dtReqTime, request, response);
            
//            return response;
//        }

//        /// <summary>
//        /// Log Data for debuging purpose
//        /// </summary>
//        /// <param name="dtReqStartTime">RequestStartTime</param>
//        /// <param name="request"></param>
//        /// <param name="response"></param>
//        private void logData(DateTime dtReqStartTime, string request,string response)
//        {
//            StringBuilder sb = new StringBuilder("\t");
//            TimeSpan timTaken = DateTime.Now.Subtract(dtReqStartTime);

//            sb.Append("Card : " + _cardNumber);
//            sb.Append("\t");
//            sb.Append("Req : " + ((request.Length > 7) ? request.Substring(0, 7) : ""));
//            sb.Append("\t");
//            sb.Append("Time Taken = " + timTaken.Minutes.ToString() + ":" + timTaken.Seconds.ToString() + ":" + timTaken.Milliseconds.ToString());

//            if (_debugHSM == 1)
//            {
//                sb.Append("\t");
//                sb.Append("Req : " + request);
//                sb.Append("\t");
//                sb.Append("Res : " + response);
//            }

//            LogWriter(sb.ToString());
//        }
//        /// <summary>
//        /// Connect Socket Port
//        /// </summary>
//        /// <param name="sock"></param>
//        /// <param name="portNo"></param>
//        /// <returns></returns>
//        private bool connect(ref Socket sock, int portNo)
//        {
//            bool flag = connectHSM(ref sock, portNo, _currentHSM);

//            if (!flag)
//            {
//                string downHSM = _currentHSM;

//                if (_currentHSM == _secHSM)
//                    _currentHSM = _priHSM;
//                else
//                    _currentHSM = _secHSM;

//                flag = connectHSM(ref sock, portNo, _currentHSM);

//                if (!flag)
//                    PostToBugscout.PostDataToBugScout(new PersistException("Both Primary and Secondary HSM are down."));
//                else
//                    PostToBugscout.PostDataToBugScout(new PersistException(String.Format("HSM : {0} is down.", downHSM)));

//            }

//            return flag;

//        }

//        /// <summary>
//        /// Connect HSM
//        /// </summary>
//        /// <param name="sock"></param>
//        /// <param name="portNo"></param>
//        /// <param name="currentHSM"></param>
//        /// <returns></returns>
//        private bool connectHSM(ref Socket sock, int portNo, string currentHSM)
//        {
//            bool flag = true;
//            sock = null;

//            try
//            {
//                IPEndPoint endpoint = null;
//                IPAddress ip = new IPAddress(0);
//                IPHostEntry local = Dns.GetHostEntry(currentHSM);
//                foreach (IPAddress ipaddress in local.AddressList)
//                {
//                    ip = ipaddress;
//                }

//                endpoint = new IPEndPoint(ip, portNo);

//                sock = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
//                sock.Connect(endpoint);
//            }
//            catch (Exception)
//            {
//                flag = false;
//            }
//            return flag;
//        }

//        /// <summary>
//        /// Get exrpyt data
//        /// </summary>
//        /// <param name="sock"></param>
//        /// <param name="portNo"></param>
//        /// <returns></returns>
//        private string getExcryptData(ref Socket sock, int portNo,string endChar)
//        {
//            if (sock == null || sock.Connected == false) connect(ref sock, portNo);

//            //string endChar = "]";
//            int bytes = 0;
//            byte[] data = new byte[1];
//            StringBuilder str = new StringBuilder();
//            string received = string.Empty;
//            while (true)
//            {
//                bytes = sock.Receive(data, 0, 1, SocketFlags.None);

//                received = Encoding.Default.GetString(data);
//                str.Append(received);
//                if (received == endChar)
//                    break;
//            }

//            return str.ToString();
//        }

//        private void LogWriter(string log)
//        {
//            if (ConfigurationManager.AppSettings["logdata"] != "1") return;
//            try
//            {
//                SimpleLogWriter write = new SimpleLogWriter(ConfigurationManager.AppSettings["HSMlogpath"].ToString() + "_" + DateTime.Now.ToString("ddMMyyyyHH") + ".log");
//                write.LogEntry(new HSMLogEntry("HSM:", 0, log));
//            }
//            catch
//            {
//            }
//        }

//        #endregion

//        #region Public Method

//        /// <summary>
//        /// Method to test excrypt Request
//        /// </summary>
//        /// <param name="request"></param>
//        /// <returns></returns>
//        public string ExcryptRequest(string request)
//        {
//            HSMResult objResult = new HSMResult();
//            return executeExcrypt(request);
//        }

//        /// <summary>
//        /// Method to test Command
//        /// </summary>
//        /// <param name="commandParam"></param>
//        /// <returns></returns>
//        public string ExecuteCommand(string commandParam)
//        {
//            string response = executeCommand(commandParam, 50);
//            return response;
//        }

//        /// <summary>
//        /// Method to test Standard Command
//        /// </summary>
//        /// <param name="commandParam"></param>
//        /// <returns></returns>
//        public string ExecuteStdCommand(string request)
//        {
//            string response = executeExcrypt(request,">" );
//            return response;
//        }

//        #endregion
//    }

//    public sealed class HSMLogEntry : BaseLogEntry
//    {
//        public HSMLogEntry(string refData, int errorCode, string errorDesc)
//        {
//            this.ReferenceData = refData;
//            this.ErrorCode = errorCode;
//            this.ErrorDescription = errorDesc;
//        }
//    }
//}
