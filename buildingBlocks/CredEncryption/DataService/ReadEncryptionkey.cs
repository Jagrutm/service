using System;
using System.Data;
using System.Data.SqlClient;
using CredECard.Common.BusinessService;
using CredEcard.CredEncryption.BusinessService;
using BaseDatabase;

namespace CredEcard.CredEncryption.DataService
{
    internal class ReadEncryptionkey
    {
        /// <author>vipul patel</author>
        /// <created>19-Jun-2014</created>
        /// <summary>
        /// Read data from DB
        /// </summary>
        /// <param name="data">Encryptionkey</param>
        internal static Encryptionkey ReadSpecific(int id)
        {
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetEncryptionkey", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramID = new SqlParameter("@intKeyTypeID", SqlDbType.Int, 4);
            paramID.Value = id;
            myCommand.Parameters.Add(paramID);

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            Encryptionkey data = null;

            try
            {
                if (myReader.Read())
                {
                    data = new Encryptionkey();

                    setValues(myReader, data);
                }
            }
            finally
            {
                if (myReader != null) myReader.Close();
            }

            return data;
        }


        /// <author>vipul</author>
        /// <created>25-Jun-2014</created>
        /// <summary>
        /// Read data list from DB
        /// </summary>
        /// <param name="data">Encryptionkey</param>
        internal static EncryptionkeyList ReadList()
        {
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetEncryptionkey", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            Encryptionkey data = null;
            EncryptionkeyList list = null;
            try
            {
                while (myReader.Read())
                {
                    if (list == null) list = new EncryptionkeyList();
                    data = new Encryptionkey();

                    setValues(myReader, data);

                    list.Add(data);
                }
            }
            finally
            {
                if (myReader != null) myReader.Close();
            }

            return list;
        }


        /// <author>vipul</author>
        /// <created>25-Jun-2014</created>
        /// <summary>
        /// Read data list from DB
        /// </summary>
        /// <param name="data">Encryptionkey</param>
        internal static EncryptionkeyList GetEncryptionkeyList(bool isEncrypt)
        {
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetEncryptionkey", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramIsEncrypt = new SqlParameter("@bEncrypt", SqlDbType.Bit);
            paramIsEncrypt.Value = isEncrypt;
            myCommand.Parameters.Add(paramIsEncrypt);

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            Encryptionkey data = null;
            EncryptionkeyList list = null;
            try
            {
                while (myReader.Read())
                {
                    if (list == null) list = new EncryptionkeyList();
                    data = new Encryptionkey();

                    setValues(myReader, data);

                    list.Add(data);
                }
            }
            finally
            {
                if (myReader != null) myReader.Close();
            }

            return list;
        }

        /// <author>vipul patel</author>
        /// <created>19-Jun-2014</created>
        /// <summary>
        /// Read data from DB
        /// </summary>
        /// <param name="data">set object value</param>
        private static void setValues(SqlDataReader myReader, Encryptionkey data)
        {
            data._keyTypeID = (int)myReader["intKeyTypeID"];
            data._keyType = (string)myReader["strKeyType"];
            data._keyValue = (string)myReader["strKeyValue"];
            data._description = (string)myReader["strDescription"];
            if (!(myReader["strPhrase"] is DBNull))
                data._phrase = (string)myReader["strPhrase"];
            data._credatedOn = (DateTime)myReader["dteCredatedOn"];
            if (!(myReader["strIV"] is DBNull))
                data._iV = (string)myReader["strIV"];
            if (!(myReader["intEncryptionMethodID"] is DBNull)) 
                data._encryptionMethodID = (short)myReader["intEncryptionMethodID"];
            if (!(myReader["intEncryptionFileFormatID"] is DBNull)) 
                data._encryptionFileFormatID = (short)myReader["intEncryptionFileFormatID"];
            if (!(myReader["intSecretKey"] is DBNull))
                data._secretKey = (long)myReader["intSecretKey"];
            if (!(myReader["intSymmetricKeyAlgorithmID"] is DBNull)) 
                data._symmetricKeyAlgorithmID = (short)myReader["intSymmetricKeyAlgorithmID"];
            data._encrypt = (bool)myReader["bEncrypt"];
            if (!(myReader["intSignkeyTypeID"] is DBNull))
                data._signkeyTypeID = (int)myReader["intSignkeyTypeID"];
            if (!(myReader["strFileExtension"] is DBNull))
                data._fileExtension = (string)myReader["strFileExtension"];
            data._checkIntegrity = (bool)myReader["bCheckIntegrity"];
        }


        /// <author>Keyur Parekh</author>
        /// <created>23-Jun-2015</created>
        /// <summary>
        /// Read data from DB
        /// </summary>
        /// <param name="data">Encryptionkey</param>
        internal static Encryptionkey ReadSpecificByCode(int code)
        {
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetEncryptionkeyByCode", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramCode = new SqlParameter("@intKeyCode", SqlDbType.Int, 4);
            paramCode.Value = code;
            myCommand.Parameters.Add(paramCode);

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            Encryptionkey data = null;

            try
            {
                if (myReader.Read())
                {
                    data = new Encryptionkey();

                    if (!(myReader["strKeyValue"] is DBNull)) data._keyValue = (string)myReader["strKeyValue"];
                    if (!(myReader["strIV"] is DBNull)) data._iV = (string)myReader["strIV"];
                }
            }
            finally
            {
                if (myReader != null) myReader.Close();
            }

            return data;
        }

    }
}
