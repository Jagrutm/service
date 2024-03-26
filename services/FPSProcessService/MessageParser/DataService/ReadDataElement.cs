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
    class ReadDataElement
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
        public static DataElement LoadDataElement(int dataElementID)
        {
            //bool _isDataPassed = false;
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetdataelements", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter param = null;

            param = new SqlParameter("@intDataElementID", SqlDbType.Int, 4);
            param.Value = dataElementID;
            myCommand.Parameters.Add(param);

           
            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            DataElement data = null;
            try
            {
                if (myReader.Read())
                {
                    data = new DataElement();
                    SetValues(myReader, data);
                }
            }
            finally
            {
                if (myReader != null) myReader.Close();
            }
            return data;
        }

        /// <author>Prashant Soni</author>
        /// <created>06-Oct-2010</created>
        /// <summary>This method set values to the object.
        /// </summary>
        /// <param name="myReader">it is a data reader object.
        /// </param>
        /// <param name="data">it is a DataElement object.
        /// </param>
        private static void SetValues(SqlDataReader myReader, DataElement data)
        {
            data.FieldNo = (int)myReader["intFieldNo"];
            data.DataRepresentation = (EnumDataRepresentment)myReader["intDataRepresentmentID"];
            data.ISODataElementLength = (int)myReader["intISODataElementLength"];
            data.ElementFormat = (string)myReader["strDataRepresentFormat"];
            if (!(myReader["intMCDataElementLength"] is DBNull))
                data.ElementLength = (int)myReader["intMCDataElementLength"];
            else
                data.ElementLength = (int)myReader["intISODataElementLength"];
            data.HasSubElements = (bool)myReader["bHasSubElements"];
            data.IsLoggableElement = (bool)myReader["bIsLoggable"];
            if (!(myReader["strDataElementName"] is DBNull)) data.FieldName = (string)myReader["strDataElementName"];
            data.IsClearingSingleElement = (bool)myReader["bClearingSingleElement"];

            data.ProductType = (EnumProductType)Convert.ToInt32(myReader["intProductTypeId"]);
            data._dataElementID = (int)myReader["intDataElementID"];
            data.DataFormat = (EnumDataFormat)Convert.ToInt32(myReader["intDataFormatID"]);
            data.ElementType = (EnumElementType)Convert.ToInt32(myReader["intElementType"]);
            data.IsConditionalHeaderField = (bool)myReader["bIsConditionalHeader"];
            if (!(myReader["intUsageNumber"] is DBNull)) data._usageNumber=  (int)myReader["intUsageNumber"];
        }

        /// <summary>
        /// Fetch unsettled reversal list for given date
        /// </summary>
        /// <param name="settlementDate">DateTime</param>
        /// <returns>PaypointOnlineDataList</returns>
        public static DataElementList GetDataElementList(EnumISOVesion iso8583Vesion, EnumProductType productType,EnumElementType elementType)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetdataelements", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter param = null;

            param = new SqlParameter("@intISOVersionID", SqlDbType.Int, 4);
            param.Value = (int)iso8583Vesion;
            myCommand.Parameters.Add(param);

            param = new SqlParameter("@intElementType", SqlDbType.TinyInt, 1);
            param.Value = (System.Byte)elementType;
            myCommand.Parameters.Add(param);

            param = new SqlParameter("@intProductTypeId", SqlDbType.SmallInt, 1);
            param.Value = (System.Int16)productType;
            myCommand.Parameters.Add(param);

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            DataElementList list = null;

            try
            {
                while (myReader.Read())
                {
                    if (list == null) list = new DataElementList();
                    DataElement data = new DataElement();
                    SetValues(myReader, data);
                    list.Add(data);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (myReader != null) myReader.Close();
            }

            return list;
        }
    }
}
