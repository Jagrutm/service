namespace ContisGroup.MessageParser.ISO8586Parser
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using CredECard.Common.BusinessService;

    internal class ReadIssuerScriptCommand
    {
        /// <author>Nidhi Thakrar</author>
        /// <created>23-Dec-2014</created>
        /// <summary>
        /// Read data list from DB
        /// </summary>
        /// <param name="data">IssuerScriptCommand</param>
        internal static IssuerScriptCommandList ReadList(int fieldNo)
        {
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetIssuerScriptCommands", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            if (fieldNo > 0)
            {
                SqlParameter paramFieldNo = new SqlParameter("@intFieldNo", SqlDbType.Int, 4);
                paramFieldNo.Value = fieldNo;
                myCommand.Parameters.Add(paramFieldNo); 
            }

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            IssuerScriptCommand data = null;
            IssuerScriptCommandList list = null;
            try
            {
                while (myReader.Read())
                {
                    if (list == null) list = new IssuerScriptCommandList();
                    data = new IssuerScriptCommand();

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


        /// <author>Nidhi Thakrar</author>
        /// <created>23-Dec-2014</created>
        /// <summary>
        /// Read data from DB
        /// </summary>
        /// <param name="data">set object value</param>
        private static void setValues(SqlDataReader myReader, IssuerScriptCommand data)
        {
            data._issuerScriptCommandID = (int)myReader["intIssuerScriptCommandID"];
            data._commandName = (string)myReader["strCommandName"];
            data._class = (string)myReader["strClass"];
            data._instruction = (string)myReader["strInstruction"];
            data._parameter = (string)myReader["strParameter"];
            if (!(myReader["strSubParameter"] is DBNull))
                data._subParameter = (string)myReader["strSubParameter"];
            data._dataElementID = (int)myReader["intDataElementID"];
            data._fieldNo = (int)myReader["intFieldNo"];
        }
    }
}
