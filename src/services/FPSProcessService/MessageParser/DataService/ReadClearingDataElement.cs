using System;
using System.Data;
using System.Data.SqlClient;
using CredECard.Common.Enums.Authorization;

namespace ContisGroup.MessageParser.ISO8586Parser
{
    /// <author>Sapan Patibandha</author>
    /// <created>30-Aug-2011</created>
    /// <summary>
    /// </summary>
    class ReadClearingDataElement
    {

        /// <author>Sapan Patibandha</author>
        /// <created>10-Jul-2020</created>
        /// <summary>
        /// Get Clearing data elements by product type
        /// </summary>
        /// <param name="productType"></param>
        /// <returns></returns>
        public static ClearingDataElementsList GetClearingDataElementList(EnumProductType productType)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetClearingDataElementsByProductType", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramProductType = new SqlParameter("@intProductTypeId", SqlDbType.Int, 4);
            paramProductType.Value = (int)productType;
            myCommand.Parameters.Add(paramProductType);

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            ClearingDataElementsList list = null;

            try
            {
                while (myReader.Read())
                {
                    if (list == null) list = new ClearingDataElementsList();
                    ClearingDataElements data = new ClearingDataElements();

                    setValues(myReader, data);
                    if (!(myReader["intClearingDataTypeID"] is DBNull))
                        data._clearingDataTypeID = (short)myReader["intClearingDataTypeID"];

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
        /// <returns>
        /// </returns>
        public static ClearingDataElementsList GetClearingDataElementList(bool isIncoming)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetClearingDataElements", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramIsIncoming = new SqlParameter("@bIsIncoming", SqlDbType.Bit);
            paramIsIncoming.Value = isIncoming;
            myCommand.Parameters.Add(paramIsIncoming);

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            ClearingDataElementsList list = null;

            try
            {
                while (myReader.Read())
                {
                    if (list == null) list = new ClearingDataElementsList();
                    ClearingDataElements data = new ClearingDataElements();

                    setValues(myReader, data);

                    setValuesForTCnTCR(myReader, data);

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
        /// <created>10-Jul-2020</created>
        /// <summary>
        /// set values for TC and TCR
        /// </summary>
        /// <param name="myReader"></param>
        /// <param name="data"></param>
        private static void setValuesForTCnTCR(SqlDataReader myReader, ClearingDataElements data)
        {
            TransactionComponentRecords objTCR = new TransactionComponentRecords();
            data._tcrID = (int)myReader["intTCRID"];
            objTCR._tcrID = (int)myReader["intTCRID"];
            objTCR._tcrCode = (string)myReader["strTCRCode"];
            if (!(myReader["TCRDescription"] is DBNull)) objTCR._description = (string)myReader["TCRDescription"];
            data.TCR = objTCR;

            TransactionCode objTC = new TransactionCode();
            data._transactionCodeID = (int)myReader["intTransactionCodeID"];
            objTC._transactionCodeID = (int)myReader["intTransactionCodeID"];
            objTC._code = (string)myReader["strCode"];
            objTC._isMonetary = (bool)myReader["bIsMonetary"];
            if (!(myReader["TCDescription"] is DBNull)) objTCR._description = (string)myReader["TCDescription"];
            data.TransactionCode = objTC;
        }

        /// <author>Sapan Patibandha</author>
        /// <created>30-Aug-2011</created>
        /// <summary>
        /// </summary>
        /// <param name="myReader">
        /// </param>
        /// <param name="data">
        /// </param>
        private static void setValues(SqlDataReader myReader, ClearingDataElements data)
        {
            data.FieldNo = (int)myReader["intFieldNo"];
            data.DataRepresentation = (EnumDataRepresentment)myReader["intDataRepresentmentID"];
            data.ISODataElementLength = (int)myReader["intISODataElementLength"];
            data.ElementFormat = (string)myReader["strDataRepresentFormat"];
            if (!(myReader["intMCDataElementLength"] is DBNull))
                data.ElementLength = (int)myReader["intMCDataElementLength"];
            else
                data.ElementLength = (int)myReader["intISODataElementLength"];

            if (!(myReader["bHasSubElements"] is DBNull)) data.HasSubElements = (bool)myReader["bHasSubElements"];

            if (!(myReader["bIsLoggable"] is DBNull)) data.IsLoggableElement = (bool)myReader["bIsLoggable"];

            if (!(myReader["strDataElementName"] is DBNull)) data.FieldName = (string)myReader["strDataElementName"];

            if (!(myReader["bClearingSingleElement"] is DBNull)) data.IsClearingSingleElement = (bool)myReader["bClearingSingleElement"];

            data.ProductType = (EnumProductType)Convert.ToInt32(myReader["intProductTypeId"]);
            data._dataElementID = (int)myReader["intDataElementID"];
            data.DataFormat = (EnumDataFormat)Convert.ToInt32(myReader["intDataFormatID"]);
            data.ElementType = (EnumElementType)Convert.ToInt32(myReader["intElementType"]);
            data.IsConditionalHeaderField = (bool)myReader["bIsConditionalHeader"];
            if (!(myReader["intUsageNumber"] is DBNull)) data._usageNumber = (int)myReader["intUsageNumber"];

            data._clearingDataFieldID = (int)myReader["intClearingDataFieldID"];
            data._startPosition = (int)myReader["intStartPosition"];

            if (!(myReader["intSettlementReportSubGroupID"] is DBNull)) data._settlementReportSubGroupID = (short)myReader["intSettlementReportSubGroupID"];

            if (!(myReader["intSequence"] is DBNull)) data._sequence = (int)myReader["intSequence"];
        }
    }
}