using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CredECard.Common.BusinessService;
using CredEcard.CredEncryption.BusinessService;
using System.Data.SqlClient;
using System.Data;
using BaseDatabase;

namespace CredEncryption.DataService
{
    internal class WriteFileEncDecKeys : StandardDataService
    {
        internal WriteFileEncDecKeys(DataController conn)
            : base(conn)
        {
        }

        /// <author>vipul</author>
        /// <created>25-Jun-2014</created>
        /// <summary>
        /// Write data to DB
        /// </summary>
        /// <param name="data">FileEncDecKeys</param>
        internal void WriteData(FileEncDecKeys data)
        {

            SqlCommand myCommand = new SqlCommand("spSaveFileEncDecKeys");
            myCommand.CommandType = CommandType.StoredProcedure;


            SqlParameter paramFileEncDecKeyID = new SqlParameter("@intFileEncDecKeyID", SqlDbType.Int, 4);
            paramFileEncDecKeyID.Value = data._fileEncDecKeyID;
            paramFileEncDecKeyID.Direction = ParameterDirection.InputOutput;
            myCommand.Parameters.Add(paramFileEncDecKeyID);

            SqlParameter paramProviderFileSetupID = new SqlParameter("@intProviderFileSetupID", SqlDbType.Int, 4);
            paramProviderFileSetupID.Value = data._providerFileSetupID;
            myCommand.Parameters.Add(paramProviderFileSetupID);

            if (data._encKeyTypeID > 0)
            {
                SqlParameter paramEncKeyTypeID = new SqlParameter("@intEncKeyTypeID", SqlDbType.Int, 4);
                paramEncKeyTypeID.Value = data._encKeyTypeID;
                myCommand.Parameters.Add(paramEncKeyTypeID);
            }

            if (data._decKeyTypeID > 0)
            {
                SqlParameter paramDecKeyTypeID = new SqlParameter("@intDecKeyTypeID", SqlDbType.Int, 4);
                paramDecKeyTypeID.Value = data._decKeyTypeID;
                myCommand.Parameters.Add(paramDecKeyTypeID);
            }

            try
            {
                this.Controller.AddCommand(myCommand);
                myCommand.ExecuteNonQuery();

                data._fileEncDecKeyID = (int)paramFileEncDecKeyID.Value;
            }
            finally
            {
                myCommand.Dispose();
            }
        }

    }
}
