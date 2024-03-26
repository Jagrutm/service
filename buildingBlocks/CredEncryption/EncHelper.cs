using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using CredECard.BugReporting.BusinessService;
//using KeyClient;
using CredECard.Common.BusinessService;

namespace CredEcard.CredEncryption.BusinessService
{
    /// <author>Arvind Ashapuri</author>
    /// <created>25-Nov-2008</created>
    /// <summary>This will used to encrypt card data
    /// </summary>
    public class EncHelper
    {
        #region Encrypt File

        /// <author>Arvind Ashapuri</author>
        /// <created>26-Nov-2008</created>
        /// <summary>This will encrypt data
        /// </summary>
        /// <param name="crypto">SymmetricAlgorithm object
        /// </param>
        /// <param name="plainText">string
        /// </param>
        /// <param name="Key">byte array
        /// </param>
        /// <param name="IV">byte array
        /// </param>
        /// <param name="textTencoding">Encoding object
        /// </param>
        /// <returns>string
        /// </returns>
        public static string EncString(SymmetricAlgorithm crypto, string plainText, byte[] Key, byte[] IV, Encoding textTencoding)
        {
            byte[] bytIn = textTencoding.GetBytes(plainText);

            return Convert.ToBase64String(EncData(crypto, bytIn, Key, IV));
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>25-Nov-2008</created>
        /// <summary>This will encrypt card data
        /// </summary>
        /// <param name="crypto">SymmetricAlgorithm object
        /// </param>
        /// <param name="bytIn">byte array
        /// </param>
        /// <param name="Key">byte array
        /// </param>
        /// <param name="IV">byte array
        /// </param>
        /// <returns>byte array
        /// </returns>
        public static byte[] EncData(SymmetricAlgorithm crypto, byte[] bytIn, byte[] Key, byte[] IV)
        {
            // create a MemoryStream so that the process can be done without I/O files
            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            // set the private key
            crypto.Key = Key;
            crypto.IV = IV;

            // create an Encryptor from the Provider Service instance
            ICryptoTransform encrypto = crypto.CreateEncryptor();

            // Write raw data to encrypted stream
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            try
            {
                cs.Write(bytIn, 0, bytIn.Length);
                cs.Flush();
            }
            finally
            {
                try
                {
                    cs.Close();
                }
                catch
                { }
            }
            byte[] encData = ms.ToArray();
            return encData;
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>25-Nov-2008</created>
        /// <summary>This will encrypt file
        /// </summary>
        /// <param name="crypto">SymmetricAlgorithm object
        /// </param>
        /// <param name="filePath">string
        /// </param>
        /// <param name="Key">byte array
        /// </param>
        /// <param name="IV">byte array
        /// </param>
        /// <returns>MemoryStream object
        /// </returns>
        public static MemoryStream EncFile(SymmetricAlgorithm crypto, string filePath, byte[] Key, byte[] IV)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException("File not found: " + filePath);

            byte[] bytIn = null;

            //open read-only file stream for input
            FileStream fileIn = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int numRead = 0;
                int fileLength = (int)fileIn.Length;

                if (fileLength > 0)
                {
                    bytIn = new byte[fileLength];
                    do
                    {
                        numRead += fileIn.Read(bytIn, 0, fileLength);
                    } while (numRead < fileLength);
                }
            }
            finally
            {
                try
                {
                    fileIn.Close();  //close input file 
                    fileIn.Dispose(); // We loaded all the data so don't need this file anymore
                }
                catch
                { }
            }

            // create a MemoryStream so that the process can be done without I/O files
            if (bytIn != null)
                return new MemoryStream(EncData(crypto, bytIn, Key, IV));
            else
                return null;

        }
        //#endregion

        //#region Encrypt card data using KMS

        ///// <author>Arvind Ashapuri</author>
        ///// <created>25-Nov-2008</created>
        ///// <summary>This will encrypt card data using KMS service
        ///// </summary>
        ///// <param name="curKMClient">KMClient object
        ///// </param>
        ///// <param name="data">string
        ///// </param>
        ///// <param name="name">string
        ///// </param>
        ///// <returns>string
        ///// </returns>
        //public static string EncDataUsingKMS(KMSconfig objKMSconfig, string data, string name)
        //{
        //    KMClient curKMClient = new KMClient(IPAddress.Parse(objKMSconfig.PrimaryKMSClientIP), objKMSconfig.KMSClientPort, objKMSconfig.ClientKeysPath);
        //    string[] val = null;

        //    string dataB64 = Convert.ToBase64String(Encoding.Unicode.GetBytes(data.Replace("\r\n", "\n")));

        //    try
        //    {
        //        val = curKMClient.KMSEncryptS(dataB64.Split('\n'), name);
        //        if (KMSconfig.Cache["IsBugPostedForPrimaryKMSClientIP"] != null) KMSconfig.Cache.Remove("IsBugPostedForPrimaryKMSClientIP");
        //    }
        //    catch (SocketException se)
        //    {
        //        if (KMSconfig.Cache["IsBugPostedForPrimaryKMSClientIP"] == null || !(bool)KMSconfig.Cache["IsBugPostedForPrimaryKMSClientIP"])
        //            PostToBugscout.PostDataToBugScout(se, objKMSconfig.CECSystemID, objKMSconfig.PostToBugScoutValue, objKMSconfig.ConnectionString);

        //        KMSconfig.SetCacheValue("IsBugPostedForPrimaryKMSClientIP", true);

        //        try
        //        {
        //            curKMClient = new KMClient(IPAddress.Parse(objKMSconfig.SecondaryKMSClientIP), objKMSconfig.KMSClientPort, objKMSconfig.ClientKeysPath);

        //            val = curKMClient.KMSEncryptS(dataB64.Split('\n'), name);
        //        }
        //        catch (SocketException ise)
        //        {
        //            PostToBugscout.PostDataToBugScout(ise, objKMSconfig.CECSystemID, objKMSconfig.PostToBugScoutValue, objKMSconfig.ConnectionString);
        //        }
        //    }

        //    return string.Join("\r\n", val);
        //}
        #endregion

    }
}
