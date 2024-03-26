using System;
using System.Data;
using System.Data.SqlClient;
using CredECard.Common.BusinessService;
using CredEcard.CredEncryption.BusinessService;
using BaseDatabase;

namespace CredEcard.CredEncryption.DataService
{

    internal class ReadFileEncryptionMethod
    {
        /// <author>Bhavin Shah</author>
        /// <created>21-Sep-2012</created>
        /// <summary>
        /// Read data from DB
        /// </summary>
        /// <param name="data">FileEncryptionMethod</param>
        internal static FileEncryptionMethod ReadSpecific(short id)
        {
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetFileEncryptionMethods", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramID = new SqlParameter("@intFileEncryptionMethodID", SqlDbType.SmallInt, 2);
            paramID.Value = id;
            myCommand.Parameters.Add(paramID);

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            FileEncryptionMethod data = null;

            try
            {
                if (myReader.Read())
                {
                    data = new FileEncryptionMethod();

                    setValues(myReader, data);
                }
            }
            finally
            {
                if (myReader != null) myReader.Close();
            }

            return data;
        }

        /// <author>Bhavin Shah</author>
        /// <created>21-Sep-2012</created>
        /// <summary>
        /// Read data list from DB
        /// </summary>
        /// <param name="data">FileEncryptionMethod</param>
        internal static FileEncryptionMethodList ReadList()
        {
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetFileEncryptionMethods", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            FileEncryptionMethod data = null;
            FileEncryptionMethodList list = null;
            try
            {
                while (myReader.Read())
                {
                    if (list == null) list = new FileEncryptionMethodList();
                    data = new FileEncryptionMethod();

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


        /// <author>Bhavin Shah</author>
        /// <created>21-Sep-2012</created>
        /// <summary>
        /// Read data from DB
        /// </summary>
        /// <param name="data">set object value</param>
        private static void setValues(SqlDataReader myReader, FileEncryptionMethod data)
        {
            data._fileEncryptionMethodID = (short)myReader["intFileEncryptionMethodID"];
            if (!(myReader["strMethodName"] is DBNull))
                data._methodName = (string)myReader["strMethodName"];
        }
    }
}