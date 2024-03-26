using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CredEncryption.BusinessService
{
    /// <author>Keyur Parekh</author>
    /// <created>05-Aug-2016</created>
    /// <summary>
    /// This class execute HSM Commands with Excrypt PORT
    /// </summary>
    [Serializable()]
    internal class HSMExecuteExcrypt
    {
        #region Variables
        HSMConnect _hsmConnect = null;
        #endregion

        #region Constructor

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Constructor, default prot 9000 will be used
        /// </summary>
        /// <param name="primaryHSMLongIP">Long IP of Primary HSM</param>
        /// <param name="secondaryHSMLongIP">Long IP of Secondary HSM</param>
        internal HSMExecuteExcrypt(long primaryHSMLongIP, long secondaryHSMLongIP)
        {
            _hsmConnect = new HSMConnect(primaryHSMLongIP, secondaryHSMLongIP);
        }

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="primaryHSMLongIP">Long IP of Primary HSM</param>
        /// <param name="secondaryHSMLongIP">Long IP of Secondary HSM</param>
        /// <param name="port">Excrypt Port</param>
        internal HSMExecuteExcrypt(long primaryHSMLongIP, long secondaryHSMLongIP, int port)
        {
            _hsmConnect = new HSMConnect(primaryHSMLongIP, secondaryHSMLongIP, port);
        }

        #endregion

        #region Methods

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Execute Exrypt Command
        /// </summary>
        /// <param name="request">Request</param>
        /// <param name="endConnection">Close Client connection or not?</param>
        /// <returns></returns>
        private string executeExcrypt(string request, bool endConnection)
        {
            string endChar = "]";
            return executeExcrypt(request, endChar, endConnection);
        }

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Execute Exrypt Command
        /// </summary>
        /// <param name="request">Request</param>
        /// <param name="endChar">Last Char of Request</param>
        /// <param name="endConnection">Close Client connection or not?</param>
        /// <returns></returns>
        private string executeExcrypt(string request, string endChar, bool endConnection)
        {
            DateTime dtReqTime = DateTime.Now;
            string response = string.Empty;

            if (!_hsmConnect.IsConnected)
            {
                _hsmConnect.Connect();

                if (!_hsmConnect.IsConnected)
                    throw new Exception(string.Format("Unable to Connect Primary {0} and Secondary HSM {1}", _hsmConnect.PrimaryHSMIP, _hsmConnect.SecondaryHSMIP));
            }

            response = _hsmConnect.PostRequest(request, endChar);

            if (endConnection) _hsmConnect.Disconnect();
            return response;
        }

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Get Signed data
        /// </summary>
        /// <param name="hexData">Hex plain data, Max 4096 chars</param>
        /// <param name="privateKeyIndex">signing key index</param>
        /// <param name="isContinue">True if hexdata are in chunk of 4096 chars</param>
        /// <param name="algorithm">Signing Algorithm</param>
        /// <returns>HSMSigningResult</returns>
        internal HSMSigningResult GetSignData(string hexData, short privateKeyIndex, bool isContinue, EnumSigningAlgorithm algorithm, string sChcommand = "")
        {
            HSMSigningResult objResult = new HSMSigningResult();
            objResult.IsSuccess = false;

            string commandParam = string.Empty; ;
            string cmd = string.Empty;

            if (sChcommand.Trim() == "")
            {
                commandParam = "[AORSAS;RC{0};RF{1};RG{2};BN{3};KY1;ZA1;]"; //*KY1(BER encoding of the HASH); *ZA1(Padding (Default))
                cmd = string.Format(commandParam,
                    privateKeyIndex,  //{0} private key index 
                    hexData,          //{1} Data used to generate the signature
                    (short)algorithm, //{2} Hash algorithm
                    isContinue ? "1" : "0");//{3} Send Data in chunk or not
            }
            else
            {
                //This section of command need to build when we have to pass CH parameter. 
                commandParam = "[AORSAS;CH{0};RC{1};RF{2};RG{3};BN{4};KY1;ZA1;]"; //*KY1(BER encoding of the HASH); *ZA1(Padding (Default))
                cmd = string.Format(commandParam,
                    sChcommand,        //{0} CH command for split data. 
                    privateKeyIndex,  //{1} private key index 
                    hexData,          //{2} Data used to generate the signature
                    (short)algorithm, //{3} Hash algorithm
                    isContinue ? "1" : "0");//{3} Send Data in chunk or not
            }

            string response = executeExcrypt(cmd, !isContinue);
            string functionID = "";

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split(';');

            string message = string.Empty;

            for (int i = 0; i < resultArray.Length - 1; i++)
            {
                string str = resultArray[i].Substring(0, 2);
                string data = resultArray[i].Substring(2);
                switch (str)
                {
                    case "AO":
                        functionID = data.ToUpper();
                        break;
                    case "BB":
                        message = data;
                        break;
                    case "BN":
                        if (data.ToUpper() == "CONTINUE")
                        {
                            objResult.IsSuccess = true;
                            objResult.IsContinue = true;
                        }
                        else
                            objResult.IsContinue = false;

                        break;
                    case "RH":
                        objResult.IsSuccess = true;
                        objResult.SignedData = data;
                        break;
                    case "CH":
                        objResult.CHCommand = data;
                        break;
                }
            }

            if (functionID == "ERRO")
            {
                objResult.IsSuccess = false;
                objResult.ErrorMessage = message;
            }
            else if (functionID != "RSAS")
            {
                objResult.IsSuccess = false;
                objResult.ErrorMessage = message;
            }
            return objResult;
        }

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Verified Signed data
        /// </summary>
        /// <param name="hexData">Hex plain data Max 4096 chars</param>
        /// <param name="signedData">Signed data</param>
        /// <param name="publicKeyIndex">public key index</param>
        /// <param name="isContinue">True if hexdata are in chunk of 4096 chars</param>
        /// <param name="algorithm">Signing Algorithm</param>
        /// <returns>HSMSigningResult</returns>
        internal HSMSigningResult VerifySignData(string hexData, string signedData, short publicKeyIndex, bool isContinue, EnumSigningAlgorithm algorithm)
        {
            HSMSigningResult objResult = new HSMSigningResult();
            objResult.IsSuccess = false;

            string commandParam = "[AORSAV;RH{0};RF{1};RD{2};RG{3};BN{4};]"; // For CVV2 only
            string cmd = string.Format(commandParam,
                    signedData,         //{0}Signature Data 
                    hexData,            //{1} Data used to generate signature
                    publicKeyIndex,     //{2} private key index 
                    (short)algorithm,   //{3} Hash algorithm
                    isContinue ? "1" : "0");//{4} Send Data in chunk or not

            string response = executeExcrypt(cmd, !isContinue);
            string functionID = "";

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split(';');

            string message = string.Empty;

            for (int i = 0; i < resultArray.Length - 1; i++)
            {
                string str = resultArray[i].Substring(0, 2);
                string data = resultArray[i].Substring(2);
                switch (str)
                {
                    case "AO":
                        functionID = data.ToUpper();
                        break;
                    case "BB":
                        message = data;
                        break;
                    case "BN":
                        if (data.ToUpper() == "CONTINUE")
                        {
                            objResult.IsSuccess = true;
                            objResult.IsContinue = true;
                        }
                        else
                            objResult.IsContinue = false;

                        break;
                }
            }

            if (functionID == "ERRO")
            {
                objResult.IsSuccess = false;
                objResult.ErrorMessage = message;
            }
            else if (functionID == "RSAV")
            {
                objResult.IsSuccess = true;
                objResult.IsVerified = message.ToUpper() == "Y" ? true : false;
            }

            return objResult;
        }

        #endregion


        public HSMResult GetEncryptedPinBlock(string cardNumber, string clearPin)
        {

            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";
            string avCardNumber = cardNumber.Substring(3, 12);

            string commandParam = "[AOEPIN;AXBD{0};AF{1};AV{2};]";
            int BT_PEK_SLOT_NO = 200; // Pulin
            string cmd = string.Format(commandParam,
                BT_PEK_SLOT_NO,                 //{0}PIN Encryption Key encrypted under modifier 1 of the MFK
                clearPin,                       //{1}Clear PIN to be encrypted
                avCardNumber);                  //{2}Rightmost 12 PAN digits minus the check digit

            string response = executeExcrypt(cmd, true);

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split(';');

            for (int i = 0; i < resultArray.Length - 1; i++)
            {
                string str = resultArray[i].Substring(0, 2);
                string data = resultArray[i].Substring(2);
                switch (str)
                {
                    case "AO":
                        functionID = data;
                        break;
                    case "AL":
                        objResult.PinBlock = data;
                        break;
                }
            }

            if (functionID == "EPIN" && objResult.PinBlock.Length > 0)
                objResult.IsSuccess = true;
            else
            {
                objResult.PinBlock = string.Empty;
                objResult.PVV = string.Empty;
            }


            return objResult;
        }
    }
}
