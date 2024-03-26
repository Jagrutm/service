using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using CredECard.Common.Enums.Authorization;




namespace ContisGroup.MessageParser.ISO8586Parser
{
    /// <author>Prashant Soni</author>
    /// <created>26-Sep-2006</created>
    /// <summary>
    /// This class contains methods to read PaypointOnline Data from  database
    /// </summary>
    class ReadSubField
    {
        /// <author>Prashant Soni</author>
        /// <created>26-Sep-2006</created>
        /// <summary>This method returns paypointonlinedata based on retrival reference no. passed.
        /// </summary>
        /// <param name="retrivalReferenceNo"> it is a retrival reference no.
        /// </param>
        /// <returns>
        /// PaypointOnlineData
        /// </returns>
        public static SubFieldList LoadSubElementWiseSubFields(int subDataElementsID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetSubFields", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter param = null;

            param = new SqlParameter("@intSubDataElementsID", SqlDbType.Int, 4);
            param.Value = subDataElementsID;
            myCommand.Parameters.Add(param);

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            SubFieldList list = null;
            try
            {
                while (myReader.Read())
                {
                    list = new SubFieldList();
                    SubField data = new SubField();
                    SetValues(myReader, data);
                    list.Add(data);
                }
            }
            finally
            {
                if (myReader != null) myReader.Close();
            }
            return list;
        }

        /// <author>Prashant Soni</author>
        /// <created>06-Oct-2010</created>
        /// <summary>This method set values to the object.
        /// </summary>
        /// <param name="myReader">it is a data reader object.
        /// </param>
        /// <param name="data">it is a SubField object.
        /// </param>
        private static void SetValues(SqlDataReader myReader, SubField data)
        {
            data.SubFieldsID = (int)myReader["intSubFieldsID"];
            if (!(myReader["intSubDataElementsID"] is DBNull)) data.SubDataElementID = (int)myReader["intSubDataElementsID"];
            data.SubFieldLength = (int)myReader["intSubFieldLength"];
            data.SubFieldNumber = (int)myReader["intSubFieldNo"];
            data.DataRepresentation = (EnumDataRepresentment)myReader["intDataRepresentmentID"];
            data.ElementFormat = (string)myReader["strDataRepresentFormat"];
            data.DataFormat = (EnumDataFormat)myReader["intDataFormatID"];
            if (!(myReader["strTagNo"] is DBNull)) data.TagNumber = (string)myReader["strTagNo"];
            if (!(myReader["strSubFieldName"] is DBNull)) data.SubFieldName = (string)myReader["strSubFieldName"];
            if (!(myReader["intSubElementNo"] is DBNull)) data.SubElementNumber = (int)myReader["intSubElementNo"];
            if (!(myReader["intFieldNo"] is DBNull)) data.FieldNo = (int)myReader["intFieldNo"];
            if (!(myReader["strMessageType"] is DBNull)) data.MessageType = (string)myReader["strMessageType"];
        }


        public static SubFieldList GetSubFieldList()
        {
            return GetSubFieldList(EnumProductType.None);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settlementDate"></param>
        /// <returns></returns>
        public static SubFieldList GetSubFieldList(EnumProductType productType)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetSubFields", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter param = null;

            if (productType != EnumProductType.None)
            {
                param = new SqlParameter("@intProductTypeID", SqlDbType.Int, 4);
                param.Value = (int)productType;
                myCommand.Parameters.Add(param);
            }

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            SubFieldList list = null;

            try
            {
                while (myReader.Read())
                {
                    if (list == null) list = new SubFieldList();
                    SubField data = new SubField();
                    SetValues(myReader, data);
                    list.Add(data);
                }
            }
            finally
            {
                if (myReader != null) myReader.Close();
            }

            return list;
        }

    }
}
