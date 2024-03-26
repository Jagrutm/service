using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CredEcard.CredEncryption.BusinessService;
using System.Data.SqlClient;
using System.Data;
using CredECard.Common.BusinessService;
using BaseDatabase;

namespace CredEncryption.BusinessService
{
    internal class ReadFileEncDecKeys
    {
        /// <author>vipul</author>
        /// <created>25-Jun-2014</created>
        /// <summary>
        /// Read data from DB
        /// </summary>
        /// <param name="data">FileEncDecKeys</param>
        internal static FileEncDecKeys ReadSpecific(int id)
        {
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetFileEncDecKeys", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramID = new SqlParameter("@intFileEncDecKeyID", SqlDbType.Int, 4);
            paramID.Value = id;
            myCommand.Parameters.Add(paramID);

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            FileEncDecKeys data = null;

            try
            {
                if (myReader.Read())
                {
                    data = new FileEncDecKeys();

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
        /// Read data from DB
        /// </summary>
        /// <param name="data">set object value</param>
        private static void setValues(SqlDataReader myReader, FileEncDecKeys data)
        {
            data._fileEncDecKeyID = (int)myReader["intFileEncDecKeyID"];
            data._providerFileSetupID = (int)myReader["intProviderFileSetupID"];
            if (!(myReader["intEncKeyTypeID"] is DBNull))
                data._encKeyTypeID = (int)myReader["intEncKeyTypeID"];
            if (!(myReader["intDecKeyTypeID"] is DBNull))
                data._decKeyTypeID = (int)myReader["intDecKeyTypeID"];
        }

    }
}
