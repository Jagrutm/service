using CredECard.Common.BusinessService;
//using CredECard.CredEncryption.BusinessService;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CredEncryption.BusinessService
{
    /// <author>Sapan</author>
    /// <created>2-Nov-2018</created>
    /// <summary>
    /// signature generation utility for PKCS10
    /// </summary>
    [Serializable()]
    public class PKCS10SigningUtil
    {
        #region PKCS10 from DB

        /// <author>Sapan</author>
        /// <created>2-Nov-2018</created>
        /// <summary>
        /// Generates the digital signature.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <param name="privateKeyText">The private key text.</param>
        /// <returns></returns>
        public static string GenerateDigitalSignature(string body, string privateKeyText)
        {
            var hash = HashBody(body);

            byte[] signedHash;
            using (var privateKey = ImportPrivateKey(privateKeyText))
            {
                signedHash = privateKey.SignHash(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }

            var encodedHash = Convert.ToBase64String(signedHash);

            return encodedHash;
        }

        /// <author>Sapan</author>
        /// <created>2-Nov-2018</created>
        /// <summary>
        /// Hashes the body.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        private static byte[] HashBody(string content)
        {
            var contentBytes = Encoding.UTF8.GetBytes(content);

            using (var provider = new SHA256Managed())
            {
                var hash = provider.ComputeHash(contentBytes);

                return hash;
            }
        }

        /// <author>Sapan</author>
        /// <created>2-Nov-2018</created>
        /// <summary>
        /// Imports the private key.
        /// </summary>
        /// <param name="pem">The pem.</param>
        /// <returns></returns>
        private static RSACryptoServiceProvider ImportPrivateKey(string pem)
        {
            var pr = new PemReader(new StringReader(pem));
            //var rsaParameters = DotNetUtilities.ToRSAParameters((((AsymmetricCipherKeyPair))pr.ReadObject()));

            AsymmetricCipherKeyPair KeyPair = (AsymmetricCipherKeyPair)pr.ReadObject();
            RSAParameters rsaParameters = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)KeyPair.Private);

            var csp = new RSACryptoServiceProvider();
            csp.ImportParameters(rsaParameters);
            return csp;
        }

        /// <author>Sapan</author>
        /// <created>26-Nov-2018</created>
        /// <summary>
        /// Verifies the digital signature.
        /// </summary>
        /// <param name="digitalSignature">The digital signature.</param>
        /// <param name="contents">The contents.</param>
        /// <param name="publicKeyText">The public key text.</param>
        /// <returns></returns>
        public static bool VerifyDigitalSignature(string digitalSignature, string contents, string publicKeyText)
        {
            var hash = HashBody(contents);

            bool verified;
            try
            {
                using (var publicKey = ImportPublicKey(publicKeyText))
                {
                    verified = publicKey.VerifyHash(hash, Convert.FromBase64String(digitalSignature), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                }
            }
            catch
            {
                verified = false;
            }

            return verified;
        }

        /// <author>Sapan</author>
        /// <created>26-Nov-2018</created>
        /// <summary>
        /// Imports the public key.
        /// </summary>
        /// <param name="pem">The pem.</param>
        /// <returns></returns>
        public static RSACryptoServiceProvider ImportPublicKey(string pem)
        {
            var pr = new PemReader(new StringReader(pem));
            var publicKey = (AsymmetricKeyParameter)pr.ReadObject();
            var rsaParameters = DotNetUtilities.ToRSAParameters((RsaKeyParameters)publicKey);
            var csp = new RSACryptoServiceProvider();
            csp.ImportParameters(rsaParameters);
            return csp;
        }
        #endregion


        #region HSM related code

        /// <author>Sapan</author>
        /// <created>24-Jan-2018</created>
        /// <summary>
        /// generate PKCS10 signature from HSM for clear bank 
        /// </summary>
        /// <param name="primaryHSMLongIP"></param>
        /// <param name="secondaryHSMLongIP"></param>
        /// <param name="port"></param>
        /// <param name="signingBody"></param>
        /// <param name="signingKeyIndex"></param>
        /// <returns></returns>
        public static string GenerateDegitalSignature(
            long primaryHSMLongIP,
            long secondaryHSMLongIP,
            int port,
            string signingBody,
            short signingKeyIndex)
        {

            string hashOfInputData = HexEncoding.ToString(Encoding.UTF8.GetBytes(signingBody));

            HSMSigningUtil objSigning = new HSMSigningUtil(primaryHSMLongIP, secondaryHSMLongIP, port);
            string HSMSignature = objSigning.GetSignData(hashOfInputData, signingKeyIndex, EnumSigningAlgorithm.SHA256);

            byte[] body = CredECard.Common.BusinessService.HexEncoding.GetBytes(HSMSignature);

            string signature = Convert.ToBase64String(body);

            return signature;
        }

        #endregion
    }
}
