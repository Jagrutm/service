using System;
using System.Data;
using System.Data.SqlClient;
using CredECard.Common.Enums.Authorization;




namespace ContisGroup.MessageParser.ISO8586Parser
{
    /// <author>Prashant Soni</author>
    /// <created>26-Sep-2006</created>
    /// <summary>
    /// This class contains methods to read PaypointOnline Data from  database
    /// </summary>
    class ReadSubDataElement
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
        public static SubElementList LoadDataElementWiseSubDataElements(string fieldNo,EnumISOVesion iso8583Vesion)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetSubdataElements", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter param = null;

            param = new SqlParameter("@intFieldNo", SqlDbType.Int,4);
            param.Value = fieldNo;
            myCommand.Parameters.Add(param);

            param = new SqlParameter("@intISOVersionID", SqlDbType.Int, 4);
            param.Value = (int)iso8583Vesion;
            myCommand.Parameters.Add(param);

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            SubElementList list = null;
            try
            {
                while (myReader.Read())
                {
                    list = new SubElementList();
                    SubDataElement data = new SubDataElement();
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
        /// <param name="data">it is a SubDataElement object.
        /// </param>
        private static void SetValues(SqlDataReader myReader, SubDataElement data)
        {
            data.SubDataElementID = (int)myReader["intSubDataElementsID"];
            data.FieldNo = (int)myReader["intFieldNo"];
            data.MessageType = (string)myReader["strMessageType"];
            data.DataRepresentation = (EnumDataRepresentment)myReader["intDataRepresentmentID"];
            data.SubElementNumber = (int)myReader["intSubElementNo"];
            data.ElementFormat = (string)myReader["strDataRepresentFormat"];
            data.ElementLength = (int)myReader["intSubDataElementLength"];
            data.HasSubElements = (bool)myReader["bHasSubFields"];
            if (!(myReader["bDependsOnDataLength"] is DBNull)) data.IsFieldDependOnLength = (bool)myReader["bDependsOnDataLength"];
            
            if (!(myReader["intMultipleOf"] is DBNull)) data.MultipleOf = (int)myReader["intMultipleOf"];
            if (!(myReader["strSubElementName"] is DBNull)) data.SubElementName = (string)myReader["strSubElementName"];
            data._dataElementID = (int)myReader["intDataElementID"];
            data.DataFormat = (EnumDataFormat)Convert.ToInt32(myReader["intDataFormatID"]);
        }

        /// <summary>
        /// Fetch unsettled reversal list for given date
        /// </summary>
        /// <param name="settlementDate">DateTime</param>
        /// <returns>PaypointOnlineDataList</returns>
        public static SubElementList GetSubElementList(EnumISOVesion iso8583Vesion,EnumProductType productType)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetSubdataElements", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter param = null;

            param = new SqlParameter("@intISOVersionID", SqlDbType.Int, 4);
            param.Value = (int)iso8583Vesion;
            myCommand.Parameters.Add(param);

            param = new SqlParameter("@intProductTypeId", SqlDbType.Int, 4);
            param.Value = (int)productType;
            myCommand.Parameters.Add(param);

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            SubElementList list = null;

            try
            {
                while (myReader.Read())
                {
                    if (list == null) list = new SubElementList();
                    SubDataElement data = new SubDataElement();
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

        /// <summary>
        /// Fetch unsettled reversal list for given date
        /// </summary>
        /// <param name="settlementDate">DateTime</param>
        /// <returns>PaypointOnlineDataList</returns>
        public static SubElementList MC_GetSubElementList(EnumISOVesion iso8583Vesion, EnumProductType productType, EnumDataFormat dataFormat)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("MC_spGetSubdataElements", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter param = null;

            param = new SqlParameter("@intISOVersionID", SqlDbType.Int, 4);
            param.Value = (int)iso8583Vesion;
            myCommand.Parameters.Add(param);

            param = new SqlParameter("@intProductTypeId", SqlDbType.Int, 4);
            param.Value = (int)productType;
            myCommand.Parameters.Add(param);

            param = new SqlParameter("@intDataFormatID", SqlDbType.Int, 4);
            param.Value = (int)dataFormat;
            myCommand.Parameters.Add(param);

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            SubElementList list = null;

            try
            {
                while (myReader.Read())
                {
                    if (list == null) list = new SubElementList();
                    SubDataElement data = new SubDataElement();
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
        internal static SubElementList GetSubElementList(EnumElementType elementType)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetSubdataElements", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC

            SqlParameter paramElementType = new SqlParameter("@intElementType", SqlDbType.SmallInt);
            paramElementType.Value = (Int16)elementType;
            myCommand.Parameters.Add(paramElementType);

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            SubElementList list = null;

            try
            {
                while (myReader.Read())
                {
                    if (list == null) list = new SubElementList();
                    SubDataElement data = new SubDataElement();
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
