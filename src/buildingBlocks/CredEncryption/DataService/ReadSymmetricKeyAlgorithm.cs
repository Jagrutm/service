using System;
using System.Data;
using System.Data.SqlClient;
using CredECard.Common.BusinessService;
using CredEcard.CredEncryption.BusinessService;
using BaseDatabase;

namespace CredEcard.CredEncryption.DataService
{

    internal class ReadSymmetricKeyAlgorithm
    {
        /// <author>Bhavin Shah</author>
        /// <created>21-Sep-2012</created>
        /// <summary>
        /// Read data from DB
        /// </summary>
        /// <param name="data">SymmetricKeyAlgorithm</param>
        internal static SymmetricKeyAlgorithm ReadSpecific(short id)
        {
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetSymmetricKeyAlgorithms", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramID = new SqlParameter("@intSymmetricKeyAlgorithmID", SqlDbType.SmallInt, 2);
            paramID.Value = id;
            myCommand.Parameters.Add(paramID);

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            SymmetricKeyAlgorithm data = null;

            try
            {
                if (myReader.Read())
                {
                    data = new SymmetricKeyAlgorithm();

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
        /// <param name="data">SymmetricKeyAlgorithm</param>
        internal static SymmetricKeyAlgorithmList ReadList()
        {
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetSymmetricKeyAlgorithms", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            SymmetricKeyAlgorithm data = null;
            SymmetricKeyAlgorithmList list = null;
            try
            {
                while (myReader.Read())
                {
                    if (list == null) list = new SymmetricKeyAlgorithmList();
                    data = new SymmetricKeyAlgorithm();

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
        private static void setValues(SqlDataReader myReader, SymmetricKeyAlgorithm data)
        {
            data._symmetricKeyAlgorithmID = (short)myReader["intSymmetricKeyAlgorithmID"];
            if (!(myReader["strAlgorithmName"] is DBNull)) data._algorithmName = (string)myReader["strAlgorithmName"];
        }
    }
}