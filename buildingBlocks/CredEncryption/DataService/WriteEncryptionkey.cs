using System;
using System.Data;
using System.Data.SqlClient;
using CredECard.Common.BusinessService;
using CredEcard.CredEncryption.BusinessService;
using BaseDatabase;

namespace CredEcard.CredEncryption.DataService
{

    internal class WriteEncryptionkey : StandardDataService
    {
        internal WriteEncryptionkey(DataController conn)
            : base(conn)
        {
        }

        /// <author>vipul patel</author>
        /// <created>19-Jun-2014</created>
        /// <summary>
        /// Write data to DB
        /// </summary>
        /// <param name="data">Encryptionkey</param>
        internal void WriteData(Encryptionkey data)
        {

            SqlCommand myCommand = new SqlCommand("spSaveEncryptionkey");
            myCommand.CommandType = CommandType.StoredProcedure;


            SqlParameter paramKeyTypeID = new SqlParameter("@intKeyTypeID", SqlDbType.Int, 4);
            paramKeyTypeID.Value = data._keyTypeID;            
            myCommand.Parameters.Add(paramKeyTypeID);

            SqlParameter paramKeyType = new SqlParameter("@strKeyType", SqlDbType.NVarChar, 30);
            paramKeyType.Value = data._keyType;
            myCommand.Parameters.Add(paramKeyType);

            SqlParameter paramKeyValue = new SqlParameter("@strKeyValue", SqlDbType.NVarChar);
            paramKeyValue.Value = data.EncryptedKeyValue;
            myCommand.Parameters.Add(paramKeyValue);

            SqlParameter paramDescription = new SqlParameter("@strDescription", SqlDbType.NVarChar, 200);
            paramDescription.Value = data._description;
            myCommand.Parameters.Add(paramDescription);

            if (data._phrase != string.Empty)
            {
                SqlParameter paramPhrase = new SqlParameter("@strPhrase", SqlDbType.NVarChar, 200);
                paramPhrase.Value = data._phrase;
                myCommand.Parameters.Add(paramPhrase);
            }                      

            if (data._iV != string.Empty)
            {
                SqlParameter paramIV = new SqlParameter("@strIV", SqlDbType.NVarChar, 32);
                paramIV.Value = data._iV;
                myCommand.Parameters.Add(paramIV);
            }

            if (data._encryptionMethodID > 0)
            {
                SqlParameter paramEncryptionMethodID = new SqlParameter("@intEncryptionMethodID", SqlDbType.SmallInt, 2);
                paramEncryptionMethodID.Value = data._encryptionMethodID;
                myCommand.Parameters.Add(paramEncryptionMethodID);
            }

            if (data._encryptionFileFormatID > 0)
            {
                SqlParameter paramEncryptionFileFormatID = new SqlParameter("@intEncryptionFileFormatID", SqlDbType.SmallInt, 2);
                paramEncryptionFileFormatID.Value = data._encryptionFileFormatID;
                myCommand.Parameters.Add(paramEncryptionFileFormatID);
            }

            SqlParameter paramSecretKey = new SqlParameter("@intSecretKey", SqlDbType.BigInt, 8);
            paramSecretKey.Value = data._secretKey;
            myCommand.Parameters.Add(paramSecretKey);

            if (data._symmetricKeyAlgorithmID > 0)
            {
                SqlParameter paramSymmetricKeyAlgorithmID = new SqlParameter("@intSymmetricKeyAlgorithmID", SqlDbType.SmallInt, 2);
                paramSymmetricKeyAlgorithmID.Value = data._symmetricKeyAlgorithmID;
                myCommand.Parameters.Add(paramSymmetricKeyAlgorithmID);
            }

            SqlParameter paramEncrypt = new SqlParameter("@bEncrypt", SqlDbType.Bit, 1);
            paramEncrypt.Value = data._encrypt;
            myCommand.Parameters.Add(paramEncrypt);

            if (data._signkeyTypeID > 0)
            {
                SqlParameter paramSignKeyTypeID = new SqlParameter("@intSignkeyTypeID", SqlDbType.Int, 4);
                paramSignKeyTypeID.Value = data._signkeyTypeID;
                myCommand.Parameters.Add(paramSignKeyTypeID);
            }

            SqlParameter paramFileExtension = new SqlParameter("@strFileExtension", SqlDbType.NVarChar, 4);
            paramFileExtension.Value = data._fileExtension;
            myCommand.Parameters.Add(paramFileExtension);

            if (data._keyCode > 0)
            {
                SqlParameter paramKeyCode = new SqlParameter("@intKeyCode", SqlDbType.Int, 4);
                paramKeyCode.Value = data._keyCode;
                myCommand.Parameters.Add(paramKeyCode);
            }

            try
            {
                this.Controller.AddCommand(myCommand);
                myCommand.ExecuteNonQuery();

               // data._keyTypeID = (int)paramKeyTypeID.Value;
            }
            finally
            {
                myCommand.Dispose();
            }
        }
    }
	
}
