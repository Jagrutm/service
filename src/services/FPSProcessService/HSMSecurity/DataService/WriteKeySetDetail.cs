namespace CredECard.CardProduction.DataService
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using CredECard.Common.BusinessService;
    using CredECard.CardProduction.BusinessService;

    internal class WriteKeySetDetail : StandardDataService
    {

        internal WriteKeySetDetail(DataController conn)
            : base(conn)
        {
        }

        /// <author>Keyur Parekh</author>
        /// <created>13-Aug-2010</created>
        /// <summary>
        /// Write data to DB
        /// </summary>
        /// <param name="data">KeySetDetail</param>
        internal void WriteData(KeySetDetail data)
        {

            SqlCommand myCommand = new SqlCommand("spSaveKeySetDetails");
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramKeySetDetailID = new SqlParameter("@intKeySetDetailID", SqlDbType.SmallInt, 2);
            paramKeySetDetailID.Value = data._keySetDetailID;
            paramKeySetDetailID.Direction = ParameterDirection.InputOutput;
            myCommand.Parameters.Add(paramKeySetDetailID);

            //SqlParameter paramKeySetID = new SqlParameter("@intKeySetID", SqlDbType.SmallInt, 2);
            //paramKeySetID.Value = data._keySetID;
            //myCommand.Parameters.Add(paramKeySetID);
       
            SqlParameter paramKeyID = new SqlParameter("@intKeyID", SqlDbType.SmallInt, 2);
            paramKeyID.Value = data._keyID;
            myCommand.Parameters.Add(paramKeyID);

            SqlParameter paramKeyValue = new SqlParameter("@strKeyValue", SqlDbType.NVarChar, 32);
            paramKeyValue.Value = data._keyValue;
            myCommand.Parameters.Add(paramKeyValue);

            SqlParameter paramKeyIndex = new SqlParameter("@intKeyIndex", SqlDbType.SmallInt, 2);
            paramKeyIndex.Value = data._keyIndex;
            myCommand.Parameters.Add(paramKeyIndex);

            if (data._slotNo > 0)
            {
            SqlParameter paramSlotNo = new SqlParameter("@intSlotNo", SqlDbType.Int, 4);
            paramSlotNo.Value =data._slotNo;
            myCommand.Parameters.Add(paramSlotNo);
            }

            if (data._kcv != string.Empty)
            {
            SqlParameter paramKCV = new SqlParameter("@strKCV", SqlDbType.NVarChar, 8);
            paramKCV.Value = data._kcv;
            myCommand.Parameters.Add(paramKCV);
            }

            try
            {
                this.Controller.AddCommand(myCommand);
                myCommand.ExecuteNonQuery();

              //  data._keySetDetailID =(short)paramKeySetDetailID.Value;
            }
            finally
            {
                myCommand.Dispose();
            }
        }
    }
}
