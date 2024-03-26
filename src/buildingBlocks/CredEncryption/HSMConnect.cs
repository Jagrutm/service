using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CredEncryption.BusinessService
{
    /// <author>Keyur Parekh</author>
    /// <created>05-Aug-2016</created>
    /// <summary>
    /// This class establish the connection to specified IP of HSM and port
    /// If Primary HSM is not connected then it try to connect Secondary HSM
    /// </summary>
    [Serializable()]
    internal class HSMConnect
    {
        #region Variables

        IPAddress _primaryHSMIP = null;
        IPAddress _secondaryHSMIP = null;

        int _tcpPort = 9000;
        static TcpClient _client = null;
        static NetworkStream _clientStream = null;

        #endregion

        #region Properties

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Get IP of Primary HSM
        /// </summary>
        internal string PrimaryHSMIP
        {
            get { return _primaryHSMIP.ToString(); }
        }

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Get IP of Secondary HSM
        /// </summary>
        internal string SecondaryHSMIP
        {
            get { return _secondaryHSMIP.ToString(); }
        }

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Get connection status
        /// </summary>
        internal bool IsConnected
        {
            get
            {
                if (_client != null && _client.Connected)
                    return true;
                else
                    return false;
            }
        }

        #endregion

        #region Constructor

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Constructor, default prot 9000 will be used
        /// </summary>
        /// <param name="primaryHSMLongIP">Long IP of Primary HSM</param>
        /// <param name="secondaryHSMLongIP">Long IP of Secondary HSM</param>
        internal HSMConnect(long primaryHSMLongIP, long secondaryHSMLongIP)
        {
            IPAddress.TryParse(primaryHSMLongIP.ToString(), out _primaryHSMIP);
            IPAddress.TryParse(secondaryHSMLongIP.ToString(), out _secondaryHSMIP);
        }

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="primaryHSMLongIP">Long IP of Primary HSM</param>
        /// <param name="secondaryHSMLongIP">Long IP of Secondary HSM</param>
        /// <param name="port">Excrypt Port</param>
        internal HSMConnect(long primaryHSMLongIP, long secondaryHSMLongIP, int port)
        {
            IPAddress.TryParse(primaryHSMLongIP.ToString(), out _primaryHSMIP);
            IPAddress.TryParse(secondaryHSMLongIP.ToString(), out _secondaryHSMIP);

            _tcpPort = port;
        }


        #endregion

        #region Methods

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Connect Primary HSM, if unable to connect then connect
        /// Secondary HSM
        /// </summary>
        internal void Connect()
        {
            if (_primaryHSMIP == null)
                throw new Exception("Primary HSM IP must be specified");

            //Connect Primary HSM
            _client = new TcpClient();

            try
            {
                connectHSM(_primaryHSMIP, _tcpPort);
            }
            catch { }

            //If unable to connect primary than secondary
            if (!_client.Connected)
            {
                if (_secondaryHSMIP != null)
                {
                    try
                    {
                        connectHSM(_secondaryHSMIP, _tcpPort);
                    }
                    catch
                    {
                        throw new Exception("Unable to connect Primary and secondary HSM");
                    }
                }
                else
                {
                    throw new Exception("Unable to connect Primary HSM");
                }
            }
        }

        /// <author>Keyur Parekh</author>
        /// <created>05-Aug-2016</created>
        /// <summary>
        /// Connect IP and Port
        /// </summary>
        /// <param name="ip">IP Address</param>
        /// <param name="port">Port</param>
        private void connectHSM(IPAddress ip, int port)
        {
            _client.Connect(ip, port);
            _clientStream = _client.GetStream();
        }

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Disconeect client and flush stream
        /// </summary>
        internal void Disconnect()
        {
            if (_client != null && _client.Connected)
            {
                _client.Close();
                _client = null;
            }

            if (_clientStream != null)
                _clientStream.Flush();

            _client = null;
            _clientStream = null;

        }

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Post HSM Excrypt Request and receive response
        /// </summary>
        /// <param name="request">HSM Excrypt Command</param>
        /// <param name="requestEndChar">Last char of request/response</param>
        /// <returns>Excrypt Response</returns>
        internal string PostRequest(string request, string requestEndChar)
        {
            StringBuilder str = new StringBuilder();

            if (_client != null && _client.Connected)
            {
                byte[] sendData = Encoding.ASCII.GetBytes(request);

                _clientStream.Write(sendData, 0, sendData.Length);

                int bytes = 0;
                string received = string.Empty;
                int buffersize = 1;
                byte[] data = new byte[buffersize];

                while (true)
                {
                    bytes = _clientStream.Read(data, 0, buffersize);
                    received = Encoding.Default.GetString(data);
                    str.Append(received);

                    if (received == requestEndChar)
                        break;
                }
            }
            else
            {
                throw new Exception("HSM is not connected");
            }

            return str.ToString();
        }

        #endregion
    }
}

