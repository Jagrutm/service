using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;
using CredECard.BugReporting.BusinessService;
using CredECard.CardProduction.BusinessService;
using CredECard.Common.BusinessService;
using CredEncryption.BusinessService;
using DataLogging.LogEntries;
using DataLogging.LogWriters;

namespace CredECard.HSMSecurity.BusinessService
{
    //Mulitiple HSM client
    public class HSMClient
    {
        internal TcpClient _client = null;
        internal NetworkStream _clientStream = null;

    }
    public class HSMCommon
    {
        #region Variables

        private static KeySetDetailList _keysetDetailList = null;
        private static string _fullLog = string.Empty;
        private static int _tcpPort = Convert.ToInt32(ConfigurationManager.AppSettings["HSMTcpPortNo"]);
        private static int _cmdPort = Convert.ToInt32(ConfigurationManager.AppSettings["HSMCmdPortNo"]);
        private static int _tcpPortAlternate = 0;//Convert.ToInt32(ConfigurationManager.AppSettings["AlternateHSMTcpPortNo"]);
        private static string _priHSM = ConfigurationManager.AppSettings["PrimaryHSMIPAddress"].ToString();
        private static string _secHSM = ConfigurationManager.AppSettings["SecondaryHSMIPAddress"].ToString();
        private static string _HSMAlternate = string.Empty;//ConfigurationManager.AppSettings["AlternateHSMIPAddress"].ToString();
        private static bool? _isMultipleHSMConnection = null;

        private static string _currentHSM = _priHSM;

        private int _bin = 0;
        private short _index = 1;
        private short _zoneKeyIndex = 1;

        internal const int BF_pinGenerationMethod = 3;  //3 Visa - BF Verification Method (VISA)
        internal const int AW_pinBlockFormat = 1;       //Input PIN block format: 1-Ansi,2-IBM3624,3 PIN Pad 
        internal const int CM_pinLength = 4;
        internal string _cardNumber = string.Empty;

        internal const string KD_controlByte = "8C"; //;
        internal const int KG_authenticationMethod = 1; //Password;
        internal const string KF_acsIdentifier = "01";
        internal const int _wsd_VTS_BIN = 474551;

        //Socket sock = null;
        static TcpClient _client = null;
        static NetworkStream _clientStream = null;
        static TcpClient _clientAlternate = null;
        static NetworkStream _clientStreamAlternate = null;
        //Muiltiple start HSM
        static List<HSMClient> objHSMClientList = null;

        static int _HSMClientListCount = 1;
        static int _HSMClientCurrentIndex = 0;
        private static object _clientlock = new object();
        private static bool _isclientInitialized = false;
        //Muiltiple END HSM
        //internal const string BY_PVKI = "1"; //Pin Verification Key Index 
        //internal const int DERIVATION_METHOD = 1;    //Derivation Method 0 = EMV 2000,1 = EMV CSK
        //internal string _cardNumber = string.Empty;
        #endregion

        #region Constructor

        public HSMCommon() { }

        /// <author>Rikunj Suthar</author>
        /// <created>31-Aug-2020</created>
        /// <summary>
        /// New constructor with ix only.
        /// </summary>
        /// <param name="index"></param>
        public HSMCommon(short index)
        {
            if (index > 0) _index = index;
        }

        public HSMCommon(string cardNumber, short index)
        {
            _cardNumber = cardNumber;
            //_cardNumber = cardNumber;
            if (cardNumber.Length > 6) int.TryParse(cardNumber.Substring(0, 6), out _bin);
            if (index > 0) _index = index;
        }

        public HSMCommon(string cardNumber, short index, short zoneKeyIndex)
        {
            _cardNumber = cardNumber;
            if (cardNumber.Length > 6) int.TryParse(cardNumber.Substring(0, 6), out _bin);
            if (index > 0) _index = index;
            if (zoneKeyIndex > 0) _zoneKeyIndex = zoneKeyIndex;
        }

        #endregion

        #region Properties


        /// <summary>
        /// Get IsMultipleHSMConnection
        /// </summary>
        private static bool? IsMultipleHSMConnection
        {
            get
            {
                if (_isMultipleHSMConnection == null)
                {
                    bool flag = false;
                    bool.TryParse(ConfigurationManager.AppSettings["IsMultipleHSMConnection"].ToString(), out flag);
                    _isMultipleHSMConnection = flag;
                }

                return _isMultipleHSMConnection;
            }
        }

        /// <summary>
        /// Get DebugHSM Flag
        /// </summary>
        private static string DebugHSM
        {
            get
            {

                try
                {
                    if (_fullLog == string.Empty)
                        _fullLog = ConfigurationManager.AppSettings["DEBUGHSM"].ToString();
                }
                catch
                {
                    _fullLog = "0";
                }
                return _fullLog;
            }
        }

        /// <summary>
        /// Get Key Index
        /// </summary>
        internal short Index
        {
            get
            {
                return _index;
            }
        }

        ///// <summary>
        /////AK : ZPK Cryptogram Value
        ///// </summary>
        //internal string AK_ZPK_CRYPTOGRAM 
        //{
        //    get
        //    {
        //        return getKeyCryptogram(_bin, EnumKeys.ZPK, _index);
        //    }
        //}

        /// <summary>
        ///AK : ZPK Cryptogram Value
        /// </summary>
        internal int AK_ZPK_SLOT_NO
        {
            get
            {
                return getKeySlotNo(_bin, EnumKeys.PER_ZPK, 1); //Index fixed as there will be only one ZPK in our database
            }
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>17-Apr-17</created>
        /// <summary>Gets the WSD Key for VISA VTS enc-dec.</summary>
        internal int VIS_VTS_WSD
        {
            get
            {
                return getKeySlotNo(_wsd_VTS_BIN, EnumKeys.VIS_VTS_WSD, 1);
            }
        }

        /// <author>Keyur Parekh</author>
        /// <created>12-Oct-17</created>
        /// <summary>Gets the WSD VI for VISA VTS enc-dec.</summary>
        internal int VIS_VTS_WSD_IV
        {
            get
            {
                return getKeySlotNo(_wsd_VTS_BIN, EnumKeys.VIS_VTS_WSD_IV, 1);
            }
        }

        /// <summary>
        ///BT : PEK encrypted under modifier 1 of the MFK 
        ///Issuer Working Key (IWK)
        /// </summary>
        internal int BT_PEK_SLOT_NO
        {
            get
            {
                return getKeySlotNo(_bin, EnumKeys.VIS_PEK, _zoneKeyIndex);
            }
        }

        ///// <summary>
        /////BT : PEK encrypted under modifier 1 of the MFK 
        /////Issuer Working Key (IWK) for Database storage
        ///// </summary>
        //internal int BT_PEK_SLOT_NO_FOR_DB
        //{
        //    get
        //    {
        //        return getKeySlotNo(_bin, EnumKeys.CON_PEK, 1);
        //    }
        //}

        /// <summary>
        ///CA : Cryptogram of left VISA Key Pair encrypted under modifier 4 of the MFK 
        /// </summary>
        internal int PVKA_SLOT_NO
        {
            get
            {
                return getKeySlotNo(_bin, EnumKeys.VIS_PVKA, _index);
            }
        }


        /// <summary>
        ///CB : Cryptogram of right VISA Key Pair encrypted under modifier 4 of the MFK 
        /// </summary>
        internal int PVKB_SLOT_NO
        {
            get
            {
                return getKeySlotNo(_bin, EnumKeys.VIS_PVKB, _index);
            }
        }

        /// <summary>
        ///CA : Cryptogram of left VISA Key Pair encrypted under modifier 4 of the MFK 
        /// </summary>
        internal int CVKA_SLOT_NO
        {
            get
            {
                return getKeySlotNo(_bin, EnumKeys.VIS_CVKA, _index);
            }
        }


        /// <summary>
        ///CB : Cryptogram of right VISA Key Pair encrypted under modifier 4 of the MFK 
        /// </summary>
        internal int CVKB_SLOT_NO
        {
            get
            {
                return getKeySlotNo(_bin, EnumKeys.VIS_CVKB, _index);
            }
        }

        /// <summary>
        ///CB : Cryptogram of Pin Generation Key (PGK) encrypted under modifier 9 of the MFK 
        /// </summary>
        internal int MDK_SLOT_NO
        {
            get
            {
                return getKeySlotNo(_bin, EnumKeys.VIS_MDK, _index);
            }
        }

        /// <summary>
        /// Mac Key Slot No.
        /// </summary>
        internal int MDK_MAC_SLOT_NO
        {
            get
            {
                return getKeySlotNo(_bin, EnumKeys.PER_MDK_MAC, _index);
            }
        }

        /// <summary>
        /// ENC Key Slot No.
        /// </summary>
        internal int MDK_ENC_SLOT_NO
        {
            get
            {
                return getKeySlotNo(_bin, EnumKeys.PER_MDK_ENC, _index);
            }
        }

        /// <summary>
        /// Contis Pin Encryption Key Slot No.
        /// Contis will have only only one key, so hardcoded index 1
        /// </summary>
        internal int CONTIS_PEK_SLOT_NO
        {
            get
            {
                return getKeySlotNo(_bin, EnumKeys.CON_PEK, 1);
            }
        }

        /// <summary>
        ///CB : Cryptogram of Pin Generation Key (PGK) encrypted under modifier 9 of the MFK 
        /// </summary>
        //internal string MDK_CRYPTOGRAM
        //{
        //    get
        //    {
        //        return getKeyCryptogram(_bin, EnumKeys.VIS_MDK, _index);
        //    }
        //}

        //internal string KMAC_CRYPTOGRAM
        //{
        //    get
        //    {
        //        return getKeyCryptogram(_bin, EnumKeys.PER_MDK_MAC, _index);
        //    }
        //}

        //internal string MDK_ENC_CRYPTOGRAM
        //{
        //    get
        //    {
        //        return getKeyCryptogram(_bin, EnumKeys.PER_MDK_ENC, _index);
        //    }
        //}

        /// <summary>
        /// CAK-A Key Slot No.
        /// </summary>
        internal int CAK_SLOT_NO
        {
            get
            {
                return getKeySlotNo(_bin, EnumKeys.VIS_CAK, _index);
            }
        }

        /// <summary>
        /// CAK-A Key Slot No.
        /// </summary>
        internal int CAKA_SLOT_NO
        {
            get
            {
                return getKeySlotNo(_bin, EnumKeys.VIS_CAKA, _index);
            }
        }

        ///// <summary>
        ///// AAV Key Slot No.
        ///// </summary>
        //internal int AAV_SLOT_NO
        //{
        //    get
        //    {
        //        return getKeySlotNo(_bin, EnumKeys.MC_AAV, _index);
        //    }
        //}

        /// <summary>
        /// CAK-B Key Slot No.
        /// </summary>
        internal int CAKB_SLOT_NO
        {
            get
            {
                return getKeySlotNo(_bin, EnumKeys.VIS_CAKB, _index);
            }
        }

        // RS#128208 - Start
        internal int FPS_ZMK_SLOT_IN => getKeySlotNo(_bin, EnumKeys.FPS_ZMK_IN, _index);
        internal int FPS_ZMK_SLOT_IN_2 => getKeySlotNo(_bin, EnumKeys.FPS_ZMK_IN_2, _index);
        internal int FPS_ZMK_SLOT_OUT => getKeySlotNo(_bin, EnumKeys.FPS_ZMK_OUT, _index);
        internal int FPS_ZMK_SLOT_OUT_2 => getKeySlotNo(_bin, EnumKeys.FPS_ZMK_OUT_2, _index);
        internal int FPS_NEXT_ZMK_SLOT_NO => getKeySlotNo(_bin, EnumKeys.FPS_NEXT_ZMK_SLOT_NO, _index);


        // RS#128208 - End

        ///// <summary>
        ///// Mac Key Slot No.
        ///// </summary>
        //internal string CAK_CRYPTOGRAM
        //{
        //    get
        //    {
        //        return getKeyCryptogram(_bin, EnumKeys.VIS_CAK, _index);
        //    }
        //}

        #endregion

        #region Methods

        /// <summary>
        /// Get Key Slot No.
        /// </summary>
        /// <param name="bin"></param>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private int getKeySlotNo(int bin, EnumKeys key, short index)
        {
            int slotNo = 0;
            KeySetDetail objKSD = getAllKeys().GetKeySetFromList(bin, key, index);

            if (objKSD != null)
                slotNo = objKSD.SlotNo;
            else
            {
                try
                {
                    _keysetDetailList = null;
                    objKSD = getAllKeys().GetKeySetFromList(bin, key, index);

                    if (objKSD != null)
                        slotNo = objKSD.SlotNo;
                    else
                    {
                        SimpleLogWriter write = new SimpleLogWriter(SimpleLogWriter.ErrorDetailLogPath);
                        write.LogEntry(new HSMLogEntry("\ngetKeySlotNo", 0, new StringBuilder().AppendFormat("Slot number does not exist for - Bin :{0}, Key :{1}, index : {2}\n", bin.ToString(), key.ToString(), index.ToString()).ToString()));
                    }
                }
                catch (Exception ex)
                {
                    PostToBugscout.PostDataToBugScout(ex);
                }
            }

            return slotNo;
        }

        /// <summary>
        /// Get Key Cryptogram
        /// </summary>
        /// <param name="bin"></param>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string getKeyCryptogram(int bin, EnumKeys key, short index)
        {
            string cryptogram = string.Empty;
            KeySetDetail objKSD = getAllKeys().GetKeySetFromList(bin, key, index);

            if (objKSD != null)
                cryptogram = objKSD.KeyValue;
            else
            {
                try
                {
                    _keysetDetailList = null;
                    objKSD = getAllKeys().GetKeySetFromList(bin, key, index);

                    cryptogram = objKSD.KeyValue;
                }
                catch (Exception ex)
                {
                    PostToBugscout.PostDataToBugScout(ex);
                }
            }

            return cryptogram;
        }

        /// <summary>
        /// Get All Keys from DB
        /// </summary>
        /// <returns></returns>
        private static KeySetDetailList getAllKeys()
        {
            if (_keysetDetailList == null)
                _keysetDetailList = KeySetDetailList.All();

            return _keysetDetailList;
        }

        /// <summary>
        /// Execute Exrypt Command
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected string executeExcrypt(string request)
        {
            string endChar = "]";

            if (IsMultipleHSMConnection == true)
            {
                return executeExcrypt_ManageClient(request, endChar);
            }
            else
            {
                return executeExcrypt(request, endChar);
            }

        }



        /// <summary>
        /// Execute Exrypt Command
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected string executeExcrypt_Alternate(string request)
        {
            string endChar = "]";
            return executeExcrypt_Alternate(request, endChar);
        }

        /// <summary>
        /// Execute Exrypt Command
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected string executeExcrypt(string request, string endChar)
        {
            DateTime dtReqTime = DateTime.Now;
            //Socket sock = null;
            string response = string.Empty;
            int portNo = _tcpPort;

            if (_client == null || !_client.Connected) connect(portNo);


            byte[] sendData = Encoding.ASCII.GetBytes(request);
            //sock.Send(sendData);

            lock (_clientlock)
            {
                _clientStream.Write(sendData, 0, sendData.Length);

                //_clientStream.Read(result, 0, resultSize - 1);
                //sock.Receive(result, 0, resultSize - 1, SocketFlags.None);

                response = getExcryptData(portNo, endChar);
            }

            logData(dtReqTime, request, response);

            return response;

            //if (connect(portNo))
            //{
            //    byte[] sendData = Encoding.ASCII.GetBytes(request);
            //    //sock.Send(sendData);

            //    response = getExcryptData(ref sock, portNo, endChar);

            //    //if (sock != null) sock.Send(Encoding.ASCII.GetBytes("QUIT" + Convert.ToChar(10)));
            //}

            //logData(dtReqTime, request, response);

            //return response;
        }


        /// <summary>
        /// Execute Exrypt Command
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected string executeExcrypt_Alternate(string request, string endChar)
        {
            DateTime dtReqTime = DateTime.Now;
            //Socket sock = null;
            string response = string.Empty;

            if (_clientAlternate == null || !_clientAlternate.Connected) connectHSM_Alternate(_tcpPortAlternate, _HSMAlternate);


            byte[] sendData = Encoding.ASCII.GetBytes(request);
            //sock.Send(sendData);
            _clientStreamAlternate.Write(sendData, 0, sendData.Length);

            response = getExcryptData_Alternate(_tcpPortAlternate, endChar);

            logData_Alternate(dtReqTime, request, response);

            return response;


        }
        /// <summary>
        /// Execute Command
        /// </summary>
        /// <param name="request"></param>
        /// <param name="resultSize"></param>
        /// <returns></returns>
        protected string executeCommand(string request, int resultSize)
        {
            DateTime dtReqTime = DateTime.Now;

            //Socket sock = null;
            string response = String.Empty;
            byte[] result = new byte[resultSize];
            int portNo = _cmdPort;

            if (_client == null || !_client.Connected) connect(portNo);


            byte[] sendData = Encoding.ASCII.GetBytes(request);
            //sock.Send(sendData);

            lock (_clientlock)
            {
                _clientStream.Write(sendData, 0, sendData.Length);

                _clientStream.Read(result, 0, resultSize - 1);
                //sock.Receive(result, 0, resultSize - 1, SocketFlags.None);
            }
            response = Encoding.Default.GetString(result);

            logData(dtReqTime, request, response);

            return response;
        }

        /// <summary>
        /// Log Data for debuging purpose
        /// </summary>
        /// <param name="dtReqStartTime">RequestStartTime</param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        private void logData(DateTime dtReqStartTime, string request, string response)
        {
            StringBuilder sb = new StringBuilder("\t");
            TimeSpan timTaken = DateTime.Now.Subtract(dtReqStartTime);

            if (_cardNumber.Length > 8)
            {
                sb.Append("Card : " + _cardNumber.Substring(0, 4) + _cardNumber.Substring(_cardNumber.Length - 4));
                sb.Append("\t");
            }
            sb.Append("Time Taken = " + timTaken.Minutes.ToString() + ":" + timTaken.Seconds.ToString() + ":" + timTaken.Milliseconds.ToString());
            if (DebugHSM == "1")
            {
                sb.Append("\t");
                sb.Append("Req : " + request);
                sb.Append("\t");
                sb.Append("Res : " + response);
            }
            sb.Append("\t");
            sb.Append("Res : " + response);

            LogWriter(sb.ToString());
        }

        /// <summary>
        /// Log Data for debuging purpose
        /// </summary>
        /// <param name="dtReqStartTime">RequestStartTime</param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        private void logData_Alternate(DateTime dtReqStartTime, string request, string response)
        {
            StringBuilder sb = new StringBuilder("\t");
            TimeSpan timTaken = DateTime.Now.Subtract(dtReqStartTime);

            if (_cardNumber.Length > 8)
            {
                sb.Append("Card : " + _cardNumber.Substring(0, 4) + _cardNumber.Substring(_cardNumber.Length - 4));
                sb.Append("\t");
            }
            sb.Append("Time Taken = " + timTaken.Minutes.ToString() + ":" + timTaken.Seconds.ToString() + ":" + timTaken.Milliseconds.ToString());
            if (DebugHSM == "1")
            {
                sb.Append("\t");
                sb.Append("Req : " + request);
                sb.Append("\t");
                sb.Append("Res : " + response);
            }
            sb.Append("\t");
            sb.Append("Res : " + response);
            sb.Append("\t");
            sb.Append("Alternate_HSM");

            LogWriter(sb.ToString());
        }
        /// <summary>
        /// Connect Socket Port
        /// </summary>
        /// <param name="sock"></param>
        /// <param name="portNo"></param>
        /// <returns></returns>
        private bool connect(int portNo)
        {
            bool flag = connectHSM(portNo, _currentHSM);

            if (!flag)
            {
                string downHSM = _currentHSM;

                if (_currentHSM == _secHSM)
                    _currentHSM = _priHSM;
                else
                    _currentHSM = _secHSM;

                flag = connectHSM(portNo, _currentHSM);

                if (!flag)
                    PostToBugscout.PostDataToBugScout(new PersistException("Both Primary and Secondary HSM are down."));
                else
                    PostToBugscout.PostDataToBugScout(new PersistException(String.Format("HSM : {0} is down.", downHSM)));

            }

            return flag;

        }



        /// <summary>
        /// Connect HSM
        /// </summary>
        /// <param name="sock"></param>
        /// <param name="portNo"></param>
        /// <param name="currentHSM"></param>
        /// <returns></returns>
        private bool connectHSM(int portNo, string currentHSM)
        {
            bool flag = false;
            //sock = null;

            //try
            //{
            //    IPEndPoint endpoint = null;
            //    IPAddress ip = new IPAddress(0);
            //    IPHostEntry local = Dns.GetHostEntry(currentHSM);
            //    foreach (IPAddress ipaddress in local.AddressList)
            //    {
            //        ip = ipaddress;
            //    }

            //    endpoint = new IPEndPoint(ip, portNo);

            //    sock = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            //    sock.Connect(endpoint);
            //}
            //catch (Exception)
            //{
            //    flag = false;
            //}
            //return flag;


            _client = new TcpClient(); // _settings.ListenIP, _settings.ListenPort);

            string serverIp = currentHSM;
            //int portNo = 0;
            //int.TryParse(ConfigurationManager.AppSettings["ClientHostPortNo"], out portNo);

            IPAddress ip = new IPAddress(0);
            IPHostEntry local = Dns.GetHostEntry(currentHSM);
            foreach (IPAddress ipaddress in local.AddressList)
            {
                ip = ipaddress;
            }


            IPEndPoint listionerEndPoint = new IPEndPoint(ip, portNo);

            try
            {
                //socket.Bind(listionerEndPoint);
                // _client.Client.Bind(listionerEndPoint);
                _client.Connect(currentHSM, portNo);
                _clientStream = _client.GetStream();
                flag = true;
            }
            catch (Exception ex)
            {

            }
            return flag;
        }
        #region HSMClient Management

        /// <summary>
        /// Execute Exrypt Command
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected string executeExcrypt_ManageClient(string request)
        {
            string endChar = "]";
            return executeExcrypt_ManageClient(request, endChar);
        }
        private void initializeHSMClient()
        {
            if (!_isclientInitialized)
            {
                lock (_clientlock)
                {
                    if (!_isclientInitialized)
                    {

                        _HSMClientListCount = Convert.ToInt32(ConfigurationManager.AppSettings["HSMClientCount"]); // 20

                        if (_HSMClientListCount == 0)
                        {
                            _HSMClientListCount = 10;
                        }

                        objHSMClientList = new List<HSMClient>();

                        for (int i = 0; i < _HSMClientListCount; i++)
                        {
                            objHSMClientList.Add(new HSMClient() { _client = new TcpClient(), _clientStream = null });
                        }
                        _isclientInitialized = true;
                    }
                }
            }

        }

        private HSMClient GetNextHSMClient()
        {
            initializeHSMClient();

            lock (_clientlock)
            {
                HSMClient objHSMClient = objHSMClientList[_HSMClientCurrentIndex];

                _HSMClientCurrentIndex++;

                if (_HSMClientCurrentIndex >= _HSMClientListCount) _HSMClientCurrentIndex = 0;

                return objHSMClient;
            }

        }
        /// <summary>
        /// Execute Exrypt Command
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected string executeExcrypt_ManageClient(string request, string endChar)
        {

            HSMClient objHSMClient = GetNextHSMClient();

            DateTime dtReqTime = DateTime.Now;

            string response = string.Empty;
            int portNo = _tcpPort;

            if (objHSMClient._client == null || !objHSMClient._client.Connected) connect(portNo, objHSMClient);


            byte[] sendData = Encoding.ASCII.GetBytes(request);

            lock (objHSMClient)
            {
                objHSMClient._clientStream.Write(sendData, 0, sendData.Length);
                response = getExcryptData(portNo, endChar, objHSMClient);
            }

            logData(dtReqTime, request, response);

            return response;

        }
        /// <summary>
        /// Get exrpyt data
        /// </summary>
        /// <param name="sock"></param>
        /// <param name="portNo"></param>
        /// <returns></returns>
        private string getExcryptData(int portNo, string endChar, HSMClient _hsmClient)
        {
            //if (sock == null || sock.Connected == false) connect(ref sock, portNo);

            if (_hsmClient._client == null || !_hsmClient._client.Connected) connect(portNo, _hsmClient);
            //string endChar = "]";
            int bytes = 0;

            StringBuilder str = new StringBuilder();
            string received = string.Empty;
            int buffersize = 1;
            byte[] data = new byte[buffersize];
            while (true)
            {
                //bytes = sock.Receive(data, 0, 1, SocketFlags.None);
                bytes = _hsmClient._clientStream.Read(data, 0, buffersize);
                received = Encoding.Default.GetString(data);
                str.Append(received);
                if (received == endChar)
                    break;
            }

            return str.ToString();
        }

        private bool connect(int portNo, HSMClient _hsmClient)
        {
            bool flag = connectHSM(portNo, _currentHSM, _hsmClient);

            if (!flag)
            {
                string downHSM = _currentHSM;

                if (_currentHSM == _secHSM)
                    _currentHSM = _priHSM;
                else
                    _currentHSM = _secHSM;

                flag = connectHSM(portNo, _currentHSM, _hsmClient);

                if (!flag)
                    PostToBugscout.PostDataToBugScout(new PersistException("Both Primary and Secondary HSM are down."));
                else
                    PostToBugscout.PostDataToBugScout(new PersistException(String.Format("HSM : {0} is down.", downHSM)));

            }

            return flag;

        }
        private bool connectHSM(int portNo, string currentHSM, HSMClient _HSMclient)
        {
            bool flag = false;


            _HSMclient._client = new TcpClient(); // _settings.ListenIP, _settings.ListenPort);

            IPAddress ip = new IPAddress(0);
            IPHostEntry local = Dns.GetHostEntry(currentHSM);
            foreach (IPAddress ipaddress in local.AddressList)
            {
                ip = ipaddress;
            }

            try
            {
                _HSMclient._client.Connect(currentHSM, portNo);
                _HSMclient._clientStream = _HSMclient._client.GetStream();
                flag = true;
            }
            catch { }

            return flag;
        }
        #endregion
        /// <summary>
        /// Connect HSM
        /// </summary>
        /// <param name="sock"></param>
        /// <param name="portNo"></param>
        /// <param name="currentHSM"></param>
        /// <returns></returns>
        private bool connectHSM_Alternate(int portNo, string currentHSM)
        {
            bool flag = false;

            _clientAlternate = new TcpClient(); // _settings.ListenIP, _settings.ListenPort);

            string serverIp = currentHSM;

            IPAddress ip = new IPAddress(0);
            IPHostEntry local = Dns.GetHostEntry(currentHSM);
            foreach (IPAddress ipaddress in local.AddressList)
            {
                ip = ipaddress;
            }


            IPEndPoint listionerEndPoint = new IPEndPoint(ip, portNo);

            try
            {
                _clientAlternate.Connect(currentHSM, portNo);
                _clientStreamAlternate = _clientAlternate.GetStream();
                flag = true;
            }
            catch (Exception ex)
            {

            }
            return flag;
        }
        /// <summary>
        /// Get exrpyt data
        /// </summary>
        /// <param name="sock"></param>
        /// <param name="portNo"></param>
        /// <returns></returns>
        private string getExcryptData(int portNo, string endChar)
        {
            //if (sock == null || sock.Connected == false) connect(ref sock, portNo);

            if (_client == null || !_client.Connected) connect(portNo);
            //string endChar = "]";
            int bytes = 0;

            StringBuilder str = new StringBuilder();
            string received = string.Empty;
            int buffersize = 1;
            byte[] data = new byte[buffersize];
            while (true)
            {
                //bytes = sock.Receive(data, 0, 1, SocketFlags.None);
                bytes = _clientStream.Read(data, 0, buffersize);
                received = Encoding.Default.GetString(data);
                str.Append(received);
                if (received == endChar)
                    break;
            }

            return str.ToString();
        }

        /// <summary>
        /// Get exrpyt data
        /// </summary>
        /// <param name="sock"></param>
        /// <param name="portNo"></param>
        /// <returns></returns>
        private string getExcryptData_Alternate(int portNo, string endChar)
        {

            if (_clientAlternate == null || !_clientAlternate.Connected) connectHSM_Alternate(portNo, _HSMAlternate);
            //string endChar = "]";
            int bytes = 0;

            StringBuilder str = new StringBuilder();
            string received = string.Empty;
            int buffersize = 1;
            byte[] data = new byte[buffersize];
            while (true)
            {
                bytes = _clientStreamAlternate.Read(data, 0, buffersize);
                received = Encoding.Default.GetString(data);
                str.Append(received);
                if (received == endChar)
                    break;
            }

            return str.ToString();
        }
        private void LogWriter(string log)
        {
            if (ConfigurationManager.AppSettings["logdata"] != "1") return;
            try
            {
                SimpleLogWriter write = new SimpleLogWriter(ConfigurationManager.AppSettings["HSMlogpath"].ToString() + "_" + DateTime.Now.ToString("ddMMyyyyHH") + ".log");
                write.LogEntry(new HSMLogEntry("HSM:", 0, log));
            }
            catch
            {
            }
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Method to test excrypt Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string ExcryptRequest(string request)
        {
            HSMResult objResult = new HSMResult();
            return executeExcrypt(request);
        }

        /// <summary>
        /// Method to test Command
        /// </summary>
        /// <param name="commandParam"></param>
        /// <returns></returns>
        public string ExecuteCommand(string commandParam)
        {
            string response = executeCommand(commandParam, 50);
            return response;
        }

        /// <summary>
        /// Method to test Standard Command
        /// </summary>
        /// <param name="commandParam"></param>
        /// <returns></returns>
        public string ExecuteStdCommand(string request)
        {
            string response = executeExcrypt(request, ">");
            return response;
        }

        public string GetSha1HashData(string merchantName)
        {

            return HashCheck.GetHMACSHA1(merchantName).ToUpper();

            //byte[] bytes = Encoding.UTF8.GetBytes(merchantName);
            //var sha1 = SHA1.Create();
            //byte[] hashBytes = sha1.ComputeHash(bytes);

            //return HexEncoding.ToString(hashBytes).ToUpper();
            //return HexStringFromBytes(hashBytes).ToUpper();
        }

        ///// <author>Rikunj Suthar</author>
        ///// <created>01-Sep-2020</created>
        ///// <summary>
        ///// Get dynamic key slot.
        ///// </summary>
        ///// <param name="macKeyIndex">3 digit mac key index.</param>
        ///// <returns>integer slot no.</returns>
        //protected int GetMACKeySlot(string macKeyIndex)
        //{
        //    int _slotNo = 0;

        //    switch (macKeyIndex)
        //    {
        //        case "000":
        //            _slotNo = VOC_MACVKEY_000_SLOT_NO;
        //            break;
        //        case "001":
        //            _slotNo = VOC_MACVKEY_001_SLOT_NO;
        //            break;
        //        case "002":
        //            _slotNo = VOC_MACVKEY_002_SLOT_NO;
        //            break;
        //        case "003":
        //            _slotNo = VOC_MACVKEY_003_SLOT_NO;
        //            break;
        //        case "004":
        //            _slotNo = VOC_MACVKEY_004_SLOT_NO;
        //            break;
        //        case "005":
        //            _slotNo = VOC_MACVKEY_005_SLOT_NO;
        //            break;
        //        case "006":
        //            _slotNo = VOC_MACVKEY_006_SLOT_NO;
        //            break;
        //        case "007":
        //            _slotNo = VOC_MACVKEY_007_SLOT_NO;
        //            break;
        //        case "008":
        //            _slotNo = VOC_MACVKEY_008_SLOT_NO;
        //            break;
        //        case "009":
        //            _slotNo = VOC_MACVKEY_009_SLOT_NO;
        //            break;
        //        case "010":
        //            _slotNo = VOC_MACVKEY_010_SLOT_NO;
        //            break;
        //        case "011":
        //            _slotNo = VOC_MACVKEY_011_SLOT_NO;
        //            break;
        //    }

        //    return _slotNo;
        //}

        ///// <summary>
        ///// Get Hax string from bytes
        ///// </summary>
        ///// <param name="bytes"></param>
        ///// <returns></returns>
        //private static string HexStringFromBytes(byte[] bytes)
        //{
        //    var sb = new StringBuilder();
        //    foreach (byte b in bytes)
        //    {
        //        var hex = b.ToString("x2");
        //        sb.Append(hex);
        //    }
        //    return sb.ToString();
        //}
        #endregion

    }

    public sealed class HSMLogEntry : BaseLogEntry
    {
        public HSMLogEntry(string refData, int errorCode, string errorDesc)
        {
            this.ReferenceData = refData;
            this.ErrorCode = errorCode;
            this.ErrorDescription = errorDesc;
        }
    }

    /// <author>Rikunj Suthar</author>
    /// <created>31-Aug-2020</created>
    /// <summary>
    /// Generate key enum.
    /// </summary>
    public enum EnumGenerateKeyType : byte
    {
        Single3DESKey = 1,
        DoubleLength3DESKey = 2,
        TrippleLength3DESKey = 3,
    }
}
