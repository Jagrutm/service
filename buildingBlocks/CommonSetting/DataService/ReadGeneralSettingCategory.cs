using CredECard.CommonSetting.BusinessService;
using System.Data;
using System.Data.SqlClient;

namespace CredECard.CommonSetting.DataService
{
	/// <summary>
	/// Summary description for ReadGeneralSettingCategory.
	/// </summary>
	public class ReadGeneralSettingCategory
	{
		public ReadGeneralSettingCategory()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <author>Falguni Parikh</author>
		/// <created>11-Apr-2006</created>
		/// <summary>Fetches all records for GeneralSettingCategory
		/// </summary>
		/// <returns>This will return general settings list.
		/// </returns>
		public static GeneralSettingCategoryList LoadGeneralSettingCategoryList()
		{
			SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
			SqlCommand myCommand = new SqlCommand("spGetSettingCategories", myConnection);

			myCommand.CommandType = CommandType.StoredProcedure;
					
			myConnection.Open();
			SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

			GeneralSettingCategory data = null;
			GeneralSettingCategoryList listData = null;

            try
            {
                while (myReader.Read())
                {
                    if (listData == null) listData = new GeneralSettingCategoryList();
                    data = new GeneralSettingCategory();
                    data.SettingCategoryID = (int)myReader["SettingCategoryID"];
                    data.SettingCategoryDescription = (string)myReader["SettingCategoryDescription"];

                    listData.Add(data);
                }
            }
            finally
            {
                if (myReader != null) myReader.Close();
            }

			return listData;
		}

		/// <author>Falguni Parikh</author>
		/// <created>11-Apr-2006</created>
		/// <summary>Get data of specific record of GeneralSettingCategory
		/// </summary>
		/// <param name="SettingCategoryID">int
		/// </param>
		/// <returns>This will return general setting.
		/// </returns>
		public static GeneralSettingCategory LoadGeneralSettingCategory(int SettingCategoryID)
		{
			SqlConnection myConnection = new SqlConnection(CredECard.Common.BusinessService.CredECardConfig.GetReadOnlyConnectionString());
			SqlCommand myCommand = new SqlCommand("spGetSettingCategories", myConnection);

			myCommand.CommandType = CommandType.StoredProcedure;
					
			SqlParameter paramSettingCategoryID = new SqlParameter("@intSettingCategoryID", SqlDbType.Int, 4);
			paramSettingCategoryID.Value = SettingCategoryID;
			myCommand.Parameters.Add(paramSettingCategoryID);

			myConnection.Open();
			SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

			GeneralSettingCategory data = new GeneralSettingCategory();

            try
            {
                while (myReader.Read())
                {
                    data.SettingCategoryID = (int)myReader["SettingCategoryID"];
                    data.SettingCategoryDescription = (string)myReader["SettingCategoryDescription"];
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
