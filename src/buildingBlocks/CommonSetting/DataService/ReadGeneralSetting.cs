using CredECard.Common.BusinessService;
using CredECard.CommonSetting.BusinessService;
using System;
using System.Data;
using System.Data.SqlClient;

namespace CredECard.CommonSetting.DataService
{
    /// <author>Chirag Khilosia</author>
    /// <created>10/6/2005</created>
    /// <summary>
    /// Summary description for ReadGeneralSetting.
    /// </summary>
    public class ReadGeneralSetting
    {

        /// <author>Chirag Khilosia</author>
        /// <created>10/6/2005</created>
        /// <summary>
        /// read a record based on settingID
        /// </summary>
        /// <param name="settingID">int
        /// </param>
        /// <returns>
        /// GeneralSetting
        /// </returns>
        public static GeneralSetting ReadGeneralSettingDetails(int settingID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetApplicationSetting", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC

            SqlParameter param = new SqlParameter("@intSettingID", SqlDbType.Int, 4);
            param.Value = settingID;
            myCommand.Parameters.Add(param);

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            GeneralSetting data = null;
            try
            {
                if (myReader.Read())
                {
                    data = new GeneralSetting();
                    SetValues(myReader, data);

                    data._settingCategoryID = (int)myReader["SettingCategoryID"];
                    data._settingCategoryDescription = (string)myReader["SettingCategoryDescription"];

                }
            }
            finally
            {
                if (myReader != null) myReader.Close();
            }

            return data;

        }

        /// <author>Prashant Soni</author>
        /// <created>20-Jan-2006</created>
        /// <summary>
        /// read a record based on settingID and connection string
        /// </summary>
        /// <param name="settingID">int
        /// </param>
        /// <param name="connectString">Connection string to connect with other database
        /// </param>
        /// <returns>
        /// GeneralSetting
        /// </returns>
        public static GeneralSetting ReadGeneralSettingDetails(int settingID, string connectString)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(connectString);
            SqlCommand myCommand = new SqlCommand("spGetApplicationSetting", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC

            SqlParameter param = new SqlParameter("@intSettingID", SqlDbType.Int, 4);
            param.Value = settingID;
            myCommand.Parameters.Add(param);

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            GeneralSetting data = null;
            try
            {
                if (myReader.Read())
                {
                    data = new GeneralSetting();
                    SetValues(myReader, data);
                }
            }
            finally
            {
                if (myReader != null) myReader.Close();
            }
            return data;
        }

        /// <author>Chirag Khilosia</author>
        /// <created>10/6/2005</created>
        /// <summary>
        /// read all records for general settings
        /// </summary>
        /// <returns>
        /// GeneralSettingList
        /// </returns>
        public static GeneralSettingList LoadGeneralSettings()
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetApplicationSetting", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            GeneralSettingList listdata = new GeneralSettingList();
            GeneralSetting data = null;

            try
            {
                while (myReader.Read())
                {
                    data = new GeneralSetting();
                    SetValues(myReader, data);
                    data._settingCategoryID = (int)myReader["SettingCategoryID"];
                    data._settingCategoryDescription = (string)myReader["SettingCategoryDescription"];
                    listdata.Add(data);
                }
            }
            finally
            {
                if (myReader != null) myReader.Close();
            }

            return listdata;
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>11-Apr-2006</created>
        /// <summary>This will get the list of all general settings for given category
        /// </summary>
        /// <param name="categoryID">int
        /// </param>
        /// <returns>This will return list of general settings 
        /// </returns>
        public static GeneralSettingList LoadGeneralSettingsByCategory(int categoryID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetApplicationSetting", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paraCategoryID = new SqlParameter("@intSettingCategoryID", SqlDbType.Int, 4);
            paraCategoryID.Value = categoryID;
            myCommand.Parameters.Add(paraCategoryID);

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            GeneralSettingList listdata = null;
            GeneralSetting data = null;

            try
            {
                while (myReader.Read())
                {
                    if (listdata == null)
                    {
                        listdata = new GeneralSettingList();
                        listdata.CategoryID = categoryID;
                    }

                    data = new GeneralSetting();
                    SetValues(myReader, data);
                    data._settingCategoryID = (int)myReader["SettingCategoryID"];
                    data._settingCategoryDescription = (string)myReader["SettingCategoryDescription"];
                    listdata.Add(data);
                }
            }
            finally
            {
                if (myReader != null) myReader.Close();
            }

            return listdata;
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>13-Apr-2006</created>
        /// <summary>This will get the list of all general settings for given category
        /// </summary>
        /// <param name="categoryID">int
        /// </param>
        /// <returns>This will return list of general settings 
        /// </returns>
        public static SiteGeneralSettingList LoadSiteGeneralSettingsByCategory(int categoryID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetApplicationSetting", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paraCategoryID = new SqlParameter("@intSettingCategoryID", SqlDbType.Int, 4);
            paraCategoryID.Value = categoryID;
            myCommand.Parameters.Add(paraCategoryID);

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            SiteGeneralSettingList listdata = null;
            GeneralSetting data = null;

            try
            {
                while (myReader.Read())
                {
                    if (listdata == null)
                    {
                        listdata = new SiteGeneralSettingList();
                        listdata.CategoryID = categoryID;
                    }

                    data = new GeneralSetting();
                    SetValues(myReader, data);
                    data._settingCategoryID = (int)myReader["SettingCategoryID"];
                    data._settingCategoryDescription = (string)myReader["SettingCategoryDescription"];
                    listdata.Add(data);
                }
            }
            finally
            {
                if (myReader != null) myReader.Close();
            }

            return listdata;
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>11-Apr-2006</created>
        /// <summary>This will assign values to  general seting object
        /// </summary>
        /// <param name="myReader">SqlDataReader
        /// </param>
        /// <param name="data">GeneralSetting
        /// </param>
        private static void SetValues(SqlDataReader myReader, GeneralSetting data)
        {
            data._settingID = (int)myReader["SettingID"];
            data._isEncrypted = (bool)myReader["IsEncrypted"];
            data._settingName = (string)myReader["SettingName"];
            data._settingValue = (string)myReader["SettingValue"];
            if (!(myReader["SettingDescription"] is DBNull)) data._settingDescription = (string)myReader["SettingDescription"];
        }

        /// <author>Keyur Parekh</author>
        /// <created>14-Jul-2010</created>
        /// <summary>
        /// Read General setting value with open connection
        /// </summary>
        /// <param name="settingID">Integer</param>
        /// <param name="conn">DataController</param>
        /// <returns>String</returns>
        public static GeneralSetting ReadGeneralSettingDetailsWithConnection(int settingID,DataController conn)
        {
            // Create Instance of Connection and Command Object
            SqlCommand myCommand = new SqlCommand("spGetApplicationSetting");

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC

            SqlParameter param = new SqlParameter("@intSettingID", SqlDbType.Int, 4);
            param.Value = settingID;
            myCommand.Parameters.Add(param);

            conn.AddCommand(myCommand);

            SqlDataReader myReader = myCommand.ExecuteReader();
            GeneralSetting data = null;
            try
            {
                if (myReader.Read())
                {
                    data = new GeneralSetting();
                    SetValues(myReader, data);

                    data._settingCategoryID = (int)myReader["SettingCategoryID"];
                    data._settingCategoryDescription = (string)myReader["SettingCategoryDescription"];

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
