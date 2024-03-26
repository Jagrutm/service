using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace ContisGroup.MessageParser.ISO8586Parser
{
    /// <author>Sapan Patibandha</author>
    /// <created>30-Aug-2011</created>
    /// <summary>
    /// </summary>
    internal class ReadTransactionComponentRecords
    {
        /// <author>Sapan Patibandha</author>
        /// <created>30-Aug-2011</created>
        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static TransactionComponentRecordsList GetTCRList()
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetTransactionComponentRecords", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            TransactionComponentRecordsList list = null;

            try
            {
                while (myReader.Read())
                {
                    if (list == null) list = new TransactionComponentRecordsList();
                    TransactionComponentRecords data = new TransactionComponentRecords();

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
        private static void setValues(SqlDataReader myReader, TransactionComponentRecords data)
        {
            data._tcrID = (int)myReader["intTCRID"];
            data._tcrCode = (string)myReader["strTCRCode"];
            if (!(myReader["strDescription"] is DBNull)) data._description = (string)myReader["strDescription"];
        }
    }
}
