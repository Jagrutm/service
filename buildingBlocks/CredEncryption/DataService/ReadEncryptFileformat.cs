using System;
using System.Data;
using System.Data.SqlClient;
using CredECard.Common.BusinessService;
using CredEcard.CredEncryption.BusinessService;
using BaseDatabase;

namespace CredEcard.CredEncryption.DataService
{
    internal class ReadEncryptFileformat
    {
        /// <author>Bhavin Shah</author>
        /// <created>21-Sep-2012</created>
        /// <summary>
        /// Read data from DB
        /// </summary>
        /// <param name="data">EncryptFileformat</param>
        internal static EncryptFileformat ReadSpecific(short id)
        {
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetEncryptFileformats", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramID = new SqlParameter("@intEncryptFileFormatID", SqlDbType.SmallInt, 2);
            paramID.Value = id;
            myCommand.Parameters.Add(paramID);

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            EncryptFileformat data = null;

            try
            {
                if (myReader.Read())
                {
                    data = new EncryptFileformat();

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
        /// <param name="data">EncryptFileformat</param>
        internal static EncryptFileformatList ReadList()
        {
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetEncryptFileformats", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            EncryptFileformat data = null;
            EncryptFileformatList list = null;
            try
            {
                while (myReader.Read())
                {
                    if (list == null) list = new EncryptFileformatList();
                    data = new EncryptFileformat();

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
        private static void setValues(SqlDataReader myReader, EncryptFileformat data)
        {
            data._encryptFileFormatID = (short)myReader["intEncryptFileFormatID"];
            if (!(myReader["strFormatName"] is DBNull))
                data._formatName = (string)myReader["strFormatName"];
        }
    }
}