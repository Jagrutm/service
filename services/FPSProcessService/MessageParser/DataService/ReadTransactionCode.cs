using System;
using System.Data;
using System.Data.SqlClient;

namespace ContisGroup.MessageParser.ISO8586Parser
{
    /// <author>Sapan Patibandha</author>
    /// <created>30-Aug-2011</created>
    /// <summary>
    /// </summary>
    class ReadTransactionCode
    {
        /// <author>Sapan Patibandha</author>
        /// <created>30-Aug-2011</created>
        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static TransactionCodeList GetTransactionCodeList()
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetTransactionCode", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            TransactionCodeList list = null;

            try
            {
                while (myReader.Read())
                {
                    if (list == null) list = new TransactionCodeList();
                    TransactionCode data = new TransactionCode();

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

        /// <author>Sapan Patibandha</author>
        /// <created>30-Aug-2011</created>
        /// <summary>
        /// </summary>
        /// <param name="myReader">
        /// </param>
        /// <param name="data">
        /// </param>
        private static void setValues(SqlDataReader myReader, TransactionCode data)
        {
            data._transactionCodeID = (int)myReader["intTransactionCodeID"];
            data._code = (string)myReader["strCode"];
            if (!(myReader["strDefination"] is DBNull)) data._defination = (string)myReader["strDefination"];
            data._isMonetary = (bool)myReader["bIsMonetary"];
        }
    }
}
