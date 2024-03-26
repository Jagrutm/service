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
    public class WriteDataWiseValue : StandardDataService
    {
        /// <author>Vipul patel</author>
        /// <created>11-July-2016</created>
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data"></param>
        public WriteDataWiseValue(DataController data)
            : base(data)
        {
        }

        /// <author>Kalpendu patel</author>
        /// <created>02-May-2006</created>
        /// <summary>
        /// Save data into database.
        /// </summary>
        /// <param name="data">TransactionType</param>
        public void UpdateDataWiseValue(DateWiseValue data)
        {
            SqlCommand myCommand = new SqlCommand("spUpdateDateWiseValue");

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC		
            SqlParameter paramintValueTypeID = new SqlParameter("@intValueTypeID", SqlDbType.Int, 4);
            paramintValueTypeID.Value = data.ValueTypeID;
            myCommand.Parameters.Add(paramintValueTypeID);

            SqlParameter paramvalue = new SqlParameter("@intValue", SqlDbType.BigInt, 8);
            paramvalue.Value = data.Value;
            myCommand.Parameters.Add(paramvalue);

            if (data.Date > DateTime.MinValue)
            {
                SqlParameter paramDate = new SqlParameter("@dteDate", SqlDbType.Date);
                paramDate.Value = data.Date;
                myCommand.Parameters.Add(paramDate);
            }

            try
            {
                this.Controller.AddCommand(myCommand);
                myCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                myCommand.Dispose();
            }
        }
    }
}
