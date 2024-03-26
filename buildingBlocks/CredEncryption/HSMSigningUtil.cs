using CredECard.BugReporting.BusinessService;
using CredECard.Common.BusinessService;
using CredECard.CommonSetting.BusinessService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CredEncryption.BusinessService
{
    /// <author>Keyur Parekh</author>
    /// <created>05-Aug-2016</created>
    /// <summary>
    /// Signing Algorithm
    /// </summary>
    public enum EnumSigningAlgorithm
    {
        NoHash = 0,
        SHA1 = 1,
        MD5 = 2,
        RIPEMD = 3,
        SHA256 = 4
    }

    /// <author>Keyur Parekh</author>
    /// <created>05-Aug-2016</created>
    /// <summary>
    /// This class provide the utility to Sign and Verify the data
    /// </summary>
    [Serializable()]
    public class HSMSigningUtil
    {
        #region Variables
        private static Object sequentialProcessor = new Object();// generate static object to get thread safe lock
        HSMExecuteExcrypt _executeExcrpyt = null;
        const int RSAS_MAX_LENGTH = 4096; //Max length supported by HSM function

        #endregion

        #region Constructors

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Constructor, default prot 9000 will be used
        /// </summary>
        /// <param name="primaryHSMLongIP">Long IP of Primary HSM</param>
        /// <param name="secondaryHSMLongIP">Long IP of Secondary HSM</param>
        public HSMSigningUtil(long primaryHSMLongIP, long secondaryHSMLongIP)
        {
            _executeExcrpyt = new HSMExecuteExcrypt(primaryHSMLongIP, secondaryHSMLongIP);
        }

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="primaryHSMLongIP">Long IP of Primary HSM</param>
        /// <param name="secondaryHSMLongIP">Long IP of Secondary HSM</param>
        /// <param name="port">Excrypt Port</param>
        public HSMSigningUtil(long primaryHSMLongIP, long secondaryHSMLongIP, int port)
        {
            _executeExcrpyt = new HSMExecuteExcrypt(primaryHSMLongIP, secondaryHSMLongIP, port);
        }

        #endregion

        #region Methods

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Get Signature
        /// </summary>
        /// <param name="dataToSign">Clear text data</param>
        /// <param name="signingKeyIndex">signing key index</param>
        /// <param name="algorithm">Signing Algorithm</param>
        /// <returns></returns>
        public string GetSignData(string dataToSign, short signingKeyIndex, EnumSigningAlgorithm algorithm)
        {
            string signedData = string.Empty;

            lock (sequentialProcessor) //open thread safe lock -> until process done system will not allow other thread/Instance to perform activity covered inside Lock
            {
                signedData = generateSign(dataToSign, signingKeyIndex, algorithm);
            }

            return signedData;
        }

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Split string in chunk of 4096 chars
        /// </summary>
        /// <param name="str">String</param>
        /// <param name="chunkSize">Chunksize</param>
        /// <returns>List Array</returns>
        private static List<string> Split(string str, int chunkSize)
        {
            List<string> listArray = new List<string>();
            int remaining = 0;

            IEnumerable<string> splitArray = Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));

            if (splitArray != null)
            {
                listArray = splitArray.ToList();
                remaining = listArray.Count;
            }

            if (remaining * chunkSize < str.Length)
                listArray.Add(str.Substring(remaining * chunkSize));

            return listArray;
        }

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Generate Signature
        /// </summary>
        /// <param name="hexString">Hex string</param>
        /// <param name="signingKeyIndex">signing key index</param>
        /// <param name="algorithm">Signing Algorithm</param>
        /// <returns></returns>
        private string generateSign(string hexString, short signingKeyIndex, EnumSigningAlgorithm algorithm)
        {
            string signedData = string.Empty;
            string chCommand = string.Empty;

            HSMSigningResult objResult = new HSMSigningResult();
            objResult.IsSuccess = false;
            List<string> hexChunkList = Split(hexString, RSAS_MAX_LENGTH);

            for (int i = 0; i < hexChunkList.Count; i++)
            {
                objResult = _executeExcrpyt.GetSignData(
                    hexChunkList[i],
                    signingKeyIndex,
                    i == hexChunkList.Count - 1 ? false : true,
                    algorithm, chCommand != string.Empty ? chCommand : "");

                chCommand = objResult.CHCommand;

                if (objResult.IsSuccess)
                {
                    if (!objResult.IsContinue)
                    {
                        signedData = objResult.SignedData;
                        break;
                    }
                }
                else
                {
                    throw new Exception(string.Format("Unable to sign data - {0}", objResult.ErrorMessage));
                }
            }

            return signedData;
        }

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Verify Sign
        /// </summary>
        /// <param name="data">clear text data</param>
        /// <param name="signedData">Sign</param>
        /// <param name="signingKeyIndex">public key index</param>
        /// <param name="algorithm">Signing Algorithm</param>
        /// <returns></returns>
        public bool VerifySignData(string data, string signedData, short signingKeyIndex, EnumSigningAlgorithm algorithm)
        {
            bool isVerify = false;
            //string strHexString = HexEncoding.ToString(Encoding.Default.GetBytes(data));
            string strHexString = data;

            isVerify = verifySign(strHexString, signedData, signingKeyIndex, algorithm);

            return isVerify;
        }

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Verify sign
        /// </summary>
        /// <param name="hexString">Hex string</param>
        /// <param name="signedData">Signature</param>
        /// <param name="signingKeyIndex">public key idndex</param>
        /// <param name="algorithm">Signing Algorithm</param>
        /// <returns></returns>
        private bool verifySign(string hexString, string signedData, short signingKeyIndex, EnumSigningAlgorithm algorithm)
        {
            bool isVerified = false;

            HSMSigningResult objResult = new HSMSigningResult();
            objResult.IsSuccess = false;

            List<string> hexChunkList = Split(hexString, RSAS_MAX_LENGTH); //Max length supported by HSM function

            for (int i = 0; i < hexChunkList.Count; i++)
            {
                objResult = _executeExcrpyt.VerifySignData(hexChunkList[i], signedData, signingKeyIndex, i == hexChunkList.Count - 1 ? false : true, algorithm);

                if (objResult.IsSuccess)
                {
                    if (!objResult.IsContinue)
                    {
                        isVerified = objResult.IsVerified;
                        break;
                    }
                }
                else
                {
                    throw new Exception(string.Format("Unable to verify sign - {0}", objResult.ErrorMessage));
                }
            }

            return isVerified;
        }

        /// <author>Aarti Meswania</author>
        /// <created>12-Oct-2016</created>
        /// <summary>Verify whether sign match with actual data using cerificate</summary>
        /// <param name="certificateData">byte[]</param>
        /// <param name="dataToBeVerified">data needs to be matched with signature</param>
        /// <param name="signToBeVerified">signature needs to be matched with data</param>
        /// <param name="signingKeyIndex">signing private key index</param>
        /// <returns>Returns - whether sign is matched with data</returns>
        public bool VerifySign(byte[] certificateData, string dataToBeVerified, string signToBeVerified)
        {
            bool isVerified = false;
            try
            {
                X509Certificate2 certificate = new X509Certificate2(certificateData);
                byte[] dcodeSignature = Convert.FromBase64String(signToBeVerified);

                UTF8Encoding encoder = new System.Text.UTF8Encoding();

                byte[] combined = encoder.GetBytes(dataToBeVerified);

                ContentInfo content = new ContentInfo(combined);
                SignedCms verifyCms = new SignedCms(content, true);
                verifyCms.Decode(dcodeSignature);

                byte[] rsaPublicKey = certificate.GetPublicKey();

                string certificatePublicKey = Convert.ToBase64String(rsaPublicKey);

                byte[] rsaFilePublickey = verifyCms.Certificates[0].GetPublicKey();

                string filePublickey = Convert.ToBase64String(rsaFilePublickey);

                if (certificatePublicKey != filePublickey)
                {
                    throw new Exception(CommonMessage.GetMessage(EnumErrorConstants.INVALID_PUBLIC_KEY));
                }
                if (certificate.Thumbprint.ToLower() != verifyCms.Certificates[0].Thumbprint.ToLower())
                {
                    throw new Exception(CommonMessage.GetMessage(EnumErrorConstants.INVALID_THUMB_PRINT));
                }

                verifyCms.CheckSignature(true);
                isVerified = true;
            }
            catch (Exception ex)
            {
                PostToBugscout.PostDataToBugScout(ex);
            }
            return isVerified;
        }
        #endregion

        public bool VerifySign(string BACSSigningAttribute, string dataToBeVerified,
             string signToBeVerified, short signVerificationPubKeyIndex, EnumSigningAlgorithm signingAlgorithm,
             byte[] certificate1, byte[] certificate2, byte[] certificate_BetaTest, bool isVerifyHSMSignature)
        {
            string hashOfDataTobeVerifed = string.Empty;
            if (signingAlgorithm == EnumSigningAlgorithm.SHA1) hashOfDataTobeVerifed = HashCheck.GetHashSHA1(dataToBeVerified);
            else if (signingAlgorithm == EnumSigningAlgorithm.SHA256) hashOfDataTobeVerifed = HashCheck.GetHash256(dataToBeVerified);

            byte[] dcodeSignature = Convert.FromBase64String(signToBeVerified);

            UTF8Encoding encoder = new UTF8Encoding();

            byte[] combined = encoder.GetBytes(dataToBeVerified);

            ContentInfo content = new ContentInfo(combined);
            SignedCms verifyCms = new SignedCms(content, true);
            verifyCms.Decode(dcodeSignature);

            foreach (var signCerttificate in verifyCms.Certificates)
            {
                X509Certificate2 X509Certificate2Certificate1 = new X509Certificate2(certificate1);
                X509Certificate2 X509Certificate2Certificate2 = new X509Certificate2(certificate2);
                X509Certificate2 X509Certificate2Certificate_BetaTesting = new X509Certificate2(certificate_BetaTest);

                bool certificatedValidated = this.ValidateCertificate(signCerttificate, X509Certificate2Certificate1);
                if (!certificatedValidated) certificatedValidated = this.ValidateCertificate(signCerttificate, X509Certificate2Certificate2);
                if (!certificatedValidated) certificatedValidated = this.ValidateCertificate(signCerttificate, X509Certificate2Certificate_BetaTesting);
                
                if (!certificatedValidated)
                {
                    string message = string.Concat(CommonMessage.GetMessage(EnumErrorConstants.INVALID_PUBLIC_KEY)
                        , " or ", CommonMessage.GetMessage(EnumErrorConstants.INVALID_THUMB_PRINT));
                    throw new Exception(message);
                }
            }

            SignerInfo signerInfos = null;
            CryptographicAttributeObjectCollection cryptographicAttributeObjects = null;
            if (verifyCms.SignerInfos.Count > 0) signerInfos = verifyCms.SignerInfos[0];

            CryptographicAttributeObject cryptographicMessageDigest = null;
            //CryptographicAttributeObject cryptographicSignatureAlgorithm = null;
            CryptographicAttributeObject cryptographicSigningTime = null;

            if (signerInfos != null) cryptographicAttributeObjects = signerInfos.SignedAttributes;
            if (cryptographicAttributeObjects != null)
            {
                foreach (var data in cryptographicAttributeObjects)
                {
                    if (data.Oid == null) continue;

                    if (data.Oid.Value == "1.2.840.113549.1.9.5")
                    {
                        cryptographicSigningTime = data; continue;
                    }
                    else if (data.Oid.Value == "1.2.840.113549.1.9.4")
                    {
                        cryptographicMessageDigest = data; continue;
                    }
                }
            }

            string messageDigestHex = string.Empty;
            string timestampHex = string.Empty;
            string signatureHex = string.Empty;

            if (cryptographicMessageDigest != null)
            {
                if (cryptographicMessageDigest.Values.Count > 0 && cryptographicMessageDigest.Values[0] is Pkcs9MessageDigest)
                {
                    messageDigestHex = HexEncoding.ToString(((Pkcs9MessageDigest)cryptographicMessageDigest.Values[0]).MessageDigest);
                }
            }

            if (cryptographicSigningTime != null)
            {
                if (cryptographicSigningTime.Values.Count > 0 && cryptographicSigningTime.Values[0] is Pkcs9SigningTime)
                {
                    timestampHex = ((Pkcs9SigningTime)cryptographicSigningTime.Values[0]).SigningTime.ToString("yyMMddHHmmssZ");
                    timestampHex = HexEncoding.ToString(Encoding.UTF8.GetBytes(timestampHex));
                }
            }

            if (!string.Equals(hashOfDataTobeVerifed, messageDigestHex) || string.IsNullOrEmpty(messageDigestHex))
            {
                throw new Exception("Hash Value does not match!");
            }

            Org.BouncyCastle.Cms.CmsSignedData objCmsSignedData = (new Org.BouncyCastle.Cms.CmsSignedData(dcodeSignature));
            Org.BouncyCastle.Asn1.Cms.ContentInfo contentInfo = null;
            if (objCmsSignedData != null) contentInfo = objCmsSignedData.ContentInfo;

            if (contentInfo == null || contentInfo.Content == null)
            {
                throw new Exception("Org.BouncyCastle.Asn1.Cms.ContentInfo is null");
            }

            var sequenceString = contentInfo.Content.ToString();
            var lastCommaIndex = sequenceString.LastIndexOf(",");
            signatureHex = sequenceString.Substring(lastCommaIndex + 3).Replace("]", string.Empty);

            string sbSigningAttribute = BACSSigningAttribute;
            sbSigningAttribute = sbSigningAttribute.Replace("#TIMESTAMP#", timestampHex);
            sbSigningAttribute = sbSigningAttribute.Replace("#DATAHASH#", messageDigestHex);

            bool isVerified = !isVerifyHSMSignature;
            if (isVerifyHSMSignature)
            {
                isVerified = this.VerifySignData(sbSigningAttribute, signatureHex.ToUpper(), signVerificationPubKeyIndex, signingAlgorithm);
            }
            if (isVerified)
            {
                isVerified = false;
                verifyCms.CheckSignature(true);
                isVerified = true;
            }            
            return isVerified;
        }

        bool ValidateCertificate(X509Certificate2 signedCertificate, X509Certificate2 certificate)
        {
            bool success = true;
            byte[] rsaPublicKey = certificate.GetPublicKey();

            string certificatePublicKey = Convert.ToBase64String(rsaPublicKey);

            byte[] rsaFilePublickey = signedCertificate.GetPublicKey();

            string filePublickey = Convert.ToBase64String(rsaFilePublickey);

            if (certificatePublicKey != filePublickey)
            {
                success = false;
                //throw new Exception(CommonMessage.GetMessage(EnumErrorConstants.INVALID_PUBLIC_KEY));
            }
            if (certificate.Thumbprint.ToLower() != signedCertificate.Thumbprint.ToLower())
            {
                success = false;
                //throw new Exception(CommonMessage.GetMessage(EnumErrorConstants.INVALID_THUMB_PRINT));
            }
            return success;
        }

        public bool VerifySignWithHash(string signToBeVerified)
        {
            bool isValid = true;

            try
            {
                byte[] dcodeSignature = Convert.FromBase64String(signToBeVerified);

                Org.BouncyCastle.Cms.CmsSignedData signedFile = new Org.BouncyCastle.Cms.CmsSignedData(dcodeSignature);
                //var sequenceString = signedFile.ContentInfo.Content.ToString();
                Org.BouncyCastle.X509.Store.IX509Store certStore = signedFile.GetCertificates("Collection");

                System.Collections.ICollection certs = certStore.GetMatches(new Org.BouncyCastle.X509.Store.X509CertStoreSelector());
                Org.BouncyCastle.Cms.SignerInformationStore signerStore = signedFile.GetSignerInfos();
                System.Collections.ICollection signers = signerStore.GetSigners();

                foreach (object tempCertification in certs)
                {
                    Org.BouncyCastle.X509.X509Certificate certification = tempCertification as Org.BouncyCastle.X509.X509Certificate;

                    foreach (object tempSigner in signers)
                    {
                        Org.BouncyCastle.Cms.SignerInformation signer = tempSigner as Org.BouncyCastle.Cms.SignerInformation;
                        //var ab = signer.GetContentDigest();
                        byte[] ab1 = signer.GetEncodedSignedAttributes();
                        string signingattribute = Convert.ToBase64String(ab1);
                        //var ab1 = signer.g
                        if (!signer.Verify(certification))
                        {
                            isValid = false;
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                isValid = false;
            }

            return isValid;
        }

        public string HexToBase64(string strInput)
        {
            try
            {
                var bytes = new byte[strInput.Length / 2];
                for (var i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = Convert.ToByte(strInput.Substring(i * 2, 2), 16);
                }
                return Convert.ToBase64String(bytes);
            }
            catch (Exception)
            {
                return "-1";
            }
        }

        public static RSACryptoServiceProvider ImportPublicKey(string pem)
        {
            var pr = new Org.BouncyCastle.OpenSsl.PemReader(new StringReader(pem));
            var publicKey = (Org.BouncyCastle.Crypto.AsymmetricKeyParameter)pr.ReadObject();
            var rsaParameters = Org.BouncyCastle.Security.DotNetUtilities.ToRSAParameters((Org.BouncyCastle.Crypto.Parameters.RsaKeyParameters)publicKey);
            var csp = new RSACryptoServiceProvider();
            csp.ImportParameters(rsaParameters);
            return csp;
        }

        private static byte[] HashBody(string content)
        {
            var contentBytes = Encoding.UTF8.GetBytes(content);

            using (var provider = new SHA256Managed())
            {
                var hash = provider.ComputeHash(contentBytes);

                return hash;
            }
        }

        private string getHash256(string input)
        {
            using (SHA256Managed sha2 = new SHA256Managed())
            {
                var hash = sha2.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public string GetEncryptedPinBlock(string cardNumber, string clearPIN, short pvki)
        {
            //HSMGeneration objHelper = new HSMGeneration(cardNumber, pvki);
            HSMResult result = _executeExcrpyt.GetEncryptedPinBlock(cardNumber, clearPIN);

            string pinBlock = string.Empty;

            if (result.IsSuccess)
                pinBlock = result.PinBlock;

            return pinBlock;
        }

    }
}
