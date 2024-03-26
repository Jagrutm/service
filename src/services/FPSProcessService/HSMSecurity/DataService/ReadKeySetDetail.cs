namespace CredECard.CardProduction.DataService
{
    using System.Data;
    using System.Data.SqlClient;
    using CredECard.Common.BusinessService;
    using CredECard.CardProduction.BusinessService;
    using System;

    internal class ReadKeySetDetail
    {
        /// <author>Keyur Parekh</author>
        /// <created>13-Aug-2010</created>
        /// <summary>
        /// Read data list from DB
        /// </summary>
        /// <param name="data">KeySetDetail</param>
        internal static KeySetDetailList ReadList() //(int? bin)
        {
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetKeySetDetails", myConnection);

            //SqlParameter paramBin = new SqlParameter("@intBIN", SqlDbType.Int, 4);
            //paramBin.Value = bin;
            //myCommand.Parameters.Add(paramBin);

            myCommand.CommandType = CommandType.StoredProcedure;

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            KeySetDetail data = null;
            KeySetDetailList list = null;
            try
            {
                while (myReader.Read())
                {
                    if (list == null) list = new KeySetDetailList();
                    data = new KeySetDetail();

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

        /// <author>Keyur Parekh</author>
        /// <created>13-Aug-2010</created>
        /// <summary>
        /// Read data from DB
        /// </summary>
        /// <param name="data">set object value</param>
        private static void setValues(SqlDataReader myReader, KeySetDetail data)
        {
            //data._bin = (int)myReader["intBIN"];
            //data._keySetID = (short)myReader["intKeySetID"];
            data._keySetDetailID = (short)myReader["intKeySetDetailID"];
            data._keyID = (short)myReader["intKeyID"];
            data._keyIndex = (short)myReader["intKeyIndex"];

            if (!(myReader["intSlotNo"] is DBNull)) data._slotNo = (int)myReader["intSlotNo"];
            if (!(myReader["strKeyValue"] is DBNull))data._keyValue = (string)myReader["strKeyValue"];
            
        }


        /// <author>Bhavin Shah</author>
        /// <created>11-Feb-2013</created>
        /// <summary>
        /// Read data from DB
        /// </summary>
        internal static KeySetDetailList GetKeySetDetailInfo()
        {
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetKeySetDetailInfo", myConnection);

            myCommand.CommandType = CommandType.StoredProcedure;

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            KeySetDetail data = null;
            KeySetDetailList list = null;
            try
            {
                while (myReader.Read())
                {
                    if (list == null) list = new KeySetDetailList();
                    data = new KeySetDetail();

                    data._keyIndex =(short)myReader["intKeyIndex"];

                    if (myReader["intSlotNo"] != DBNull.Value) data._slotNo = (int)myReader["intSlotNo"];
                    if (myReader["strKCV"] != DBNull.Value) data._kcv = (string)myReader["strKCV"];
                    data._KeySetName = (string)myReader["strKeySetName"];
                    data._KeyName = (string)myReader["strKeyName"];
                    //data._keySetID = (short)myReader["intKeySetID"];
                    data.KeySetDetailID = (short)myReader["intKeySetDetailID"];
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
        /// <created>11-Feb-2013</created>
        /// <summary>
        /// Read data from DB
        /// </summary>
        /// <param name="data">KeySetDetail</param>
        internal static KeySetDetail ReadSpecific(short keySetDetailID)
        {
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetSpecificKeySetDetail", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramID = new SqlParameter("@intKeySetDetailID", SqlDbType.SmallInt, 2);
            paramID.Value = keySetDetailID;
            myCommand.Parameters.Add(paramID);

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            KeySetDetail data = null;

            try
            {
                if (myReader.Read())
                {
                    data = new KeySetDetail();

                    data._keyIndex = (short)myReader["intKeyIndex"];

                    if (myReader["intSlotNo"] != DBNull.Value) data._slotNo = (int)myReader["intSlotNo"];
                    if (myReader["strKCV"] != DBNull.Value) data._kcv = (string)myReader["strKCV"];
                    data._KeySetName = (string)myReader["strKeySetName"];
                    data._KeyName = (string)myReader["strKeyName"];
                    //data._keySetID = (short)myReader["intKeySetID"];
                    data.KeySetDetailID = (short)myReader["intKeySetDetailID"];
                    data.KeyValue =(string)myReader["strKeyValue"];
                                        
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
