using CommonSetting.BusinessService;
using CredECard.Common.BusinessService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonSetting.DataService
{
    internal class ReadDateWiseValue
    {
        /// <author>Mahesh Vala</author>
        /// <created>07-Feb-2017</created>
        /// <summary>
        /// Read data from DB
        /// </summary>
        /// <param name="data">DateWiseValue</param>
        internal static DateWiseValue ReadDateWiseValueByValueTypeID(int valueTypeID)
        {
            SqlConnection myConnection = new SqlConnection(CredECardConfig.GetReadWriteConnectionString()); //Imp: Please dont change connection to readonly otherwise to duplicate process generate : Case 157428
            SqlCommand myCommand = new SqlCommand("spGetDateWiseValue", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramID = new SqlParameter("@intValueTypeID", SqlDbType.Int, 4);
            paramID.Value = valueTypeID;
            myCommand.Parameters.Add(paramID);

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            DateWiseValue data = null;

            try
            {
                if (myReader.Read())
                {
                    data = new DateWiseValue();

                    setValues(myReader, data);
                }
            }
            finally
            {
                if (myReader != null) myReader.Close();
            }

            return data;
        }

        /// <author>Mahesh Vala</author>
        /// <created>07-Feb-2017</created>
        /// <summary>
        /// Read data from DB
        /// </summary>
        /// <param name="data">set object value</param>
        private static void setValues(SqlDataReader myReader, DateWiseValue data)
        {
            data._dateWiseValueID = (int)myReader["intDateWiseValueID"];
            if (!(myReader["dteDate"] is DBNull))
                data._date = (DateTime)myReader["dteDate"];
            data._valueTypeID = (int)myReader["intValueTypeID"];
            data._value = (Int64)myReader["intValue"];
            if (!(myReader["intMinValue"] is DBNull))
                data._minValue = (Int64)myReader["intMinValue"];
            if (!(myReader["intMaxValue"] is DBNull))
                data._maxValue = (Int64)myReader["intMaxValue"];
        }
    }
}
