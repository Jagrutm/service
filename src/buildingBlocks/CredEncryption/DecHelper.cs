﻿using System;
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
    /// <summary>This will used to decrypt card data
    /// </summary>
    /// <exception cref="FileNotFoundException">
    /// </exception>
    public class DecHelper
    {
        #region Decrypt File

        /// <author>Arvind Ashapuri</author>
        /// <created>26-Nov-2008</created>
        /// <summary>This will decrypt the data
        /// </summary>
        /// <param name="crypto">SymmetricAlgorithm object
        /// </param>
        /// <param name="encText">string
        /// </param>
        /// <param name="Key">byte array
        /// </param>
        /// <param name="IV">byte array
        /// </param>
        /// <param name="textTencoding">Encoding object
        /// </param>
        /// <returns>string
        /// </returns>
        public static string DecString(SymmetricAlgorithm crypto, string encText, byte[] Key, byte[] IV, Encoding textTencoding)
        {
            byte[] bytIn = System.Convert.FromBase64String(encText);

            return textTencoding.GetString(DecData(crypto, bytIn, Key, IV));
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>25-Nov-2008</created>
        /// <summary>This will decrypt card data
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
        public static byte[] DecData(SymmetricAlgorithm crypto, byte[] bytIn, byte[] Key, byte[] IV)
        {
            crypto.Key = Key;
            crypto.IV = IV;

            ICryptoTransform encrypto = crypto.CreateDecryptor();

            // create Crypto Stream that transforms a stream using the decryption
            MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);

            // read out the result from the Crypto Stream
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

            byte[] data = ms.ToArray();

            return data;
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>25-Nov-2008</created>
        /// <summary>This will decrypt file
        /// </summary>
        /// <param name="crypto">SymmetricAlgorithm object
        /// </param>
        /// <param name="filePath">string
        /// </param>
        /// <param name="Key">byte array
        /// </param>
        /// <param name="IV">byte array
        /// </param>
        /// <returns>MemoryStream
        /// </returns>
        public static MemoryStream DecFile(SymmetricAlgorithm crypto, string filePath, byte[] Key, byte[] IV)
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
                return new MemoryStream(DecData(crypto, bytIn, Key, IV));
            else
                return null;

        }

        //#endregion

        //#region Decrypt card data using KMS

        ///// <author>Arvind Ashapuri</author>
        ///// <created>25-Nov-2008</created>
        ///// <summary>This will decrypt card data using KMS service
        ///// </summary>
        ///// <param name="curKMClient">KMClient object
        ///// </param>
        ///// <param name="data">string
        ///// </param>
        ///// <param name="name">string
        ///// </param>
        ///// <returns>string
        ///// </returns>
        //public static string DecryptDataUsingKMS(KMSconfig objKMSconfig, string data, string name)
        //{
        //    KMClient curKMClient = new KMClient(IPAddress.Parse(objKMSconfig.PrimaryKMSClientIP), objKMSconfig.KMSClientPort, objKMSconfig.ClientKeysPath);
        //    string[] val = null;

        //    string encData = data.Replace("\r\n", "\n");

        //    try
        //    {
        //        val = curKMClient.KMSDecryptS(encData.Split('\n'), name);
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

        //            val = curKMClient.KMSDecryptS(encData.Split('\n'), name);
        //        }
        //        catch (SocketException ise)
        //        {
        //            PostToBugscout.PostDataToBugScout(ise, objKMSconfig.CECSystemID, objKMSconfig.PostToBugScoutValue, objKMSconfig.ConnectionString);
        //        }
        //    }

        //    string dataB64 = String.Join("\r\n", val);

        //    return Encoding.Unicode.GetString(Convert.FromBase64String(dataB64));
        //}

        #endregion
    }
}
