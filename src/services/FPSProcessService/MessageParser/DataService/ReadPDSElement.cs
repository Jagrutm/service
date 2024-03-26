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
    class ReadPDSElement
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
        public static PDSElement GetTagWisePDSElement(int tagNo)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetPrivateDataSubElements", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter param = null;

            param = new SqlParameter("@intTagNo", SqlDbType.Int, 4);
            param.Value = tagNo;
            myCommand.Parameters.Add(param);

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            PDSElement data = null;
            try
            {
                if (myReader.Read())
                {
                    data = new PDSElement();
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
        /// <param name="data">it is a PDSElement object.
        /// </param>
        private static void SetValues(SqlDataReader myReader, PDSElement data)
        {
            data.DataRepresentation = (EnumDataRepresentment)myReader["intDataRepresentmentID"];
            data.TagNumber = (string)myReader["strTagNo"];
            data.ElementFormat = (string)myReader["strDataRepresentFormat"];
            data.ElementLength = (int)myReader["intPDSLength"];
            data.HasSubElements = (bool)myReader["bHasSubFields"];
            data.IsClearingSingleElement = (bool)myReader["bClearingSingleElement"];
            if (!(myReader["intMultipleOf"] is DBNull)) data.MultipleOf = (int)myReader["intMultipleOf"];
            if (!(myReader["strTagName"] is DBNull)) data.TagName = (string)myReader["strTagName"];    
            data._dataElementID  = (int)myReader["intDataElementID"];
            data.DataFormat = (EnumDataFormat)myReader["intDataFormatID"];
            data._pdsElementID = (int)myReader["intPDSElementID"];
            if (!(myReader["intParantPDSElementID"] is DBNull)) data.ParantPDSElementID = (int)myReader["intParantPDSElementID"];
        
        //,intPDSElementID
        //,intParantPDSElementID
        }

        /// <summary>
        /// Fetch unsettled reversal list for given date
        /// </summary>
        /// <param name="settlementDate">DateTime</param>
        /// <returns>PaypointOnlineDataList</returns>
        public static SubElementList GetPDSElementList(EnumProductType productType)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetPrivateDataSubElements", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;
                
            // Add Parameters to SPROC
            SqlParameter param = null;

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
                    PDSElement data = new PDSElement();
                    SetValues(myReader, data);
                    if (!(myReader["intAuthPresenceNotationID"] is DBNull))
                    data._authPresenceNotationID  = (EnumPresenceNotation)myReader["intAuthPresenceNotationID"];
                   
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
