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
	/// Summary description for ReadPurchase.
	/// </summary>
	public class WriteGeneralSetting : StandardDataService
	{
		/// <author>Chirag Khilosia</author>
		/// <created>10/6/2005</created>
		/// <summary>
		/// Constructor
		/// </summary>
        /// <param name="data">DataController
		/// </param>
		public WriteGeneralSetting(DataController data) : base(data)
		{
		}

		/// <author>Chirag Khilosia</author>
		/// <created>10/6/2005</created>
		/// <summary>
		/// Write a record in tblSettings
		/// </summary>
        /// <param name="data">GeneralSetting
		/// </param>
		public void WriteGeneralSettingData(GeneralSetting data)
		{
			SqlCommand myCommand = new SqlCommand("spSaveApplicationSetting");

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;
            
			// Add Parameters to SPROC		
			
			SqlParameter paramSettingName = new SqlParameter("@strSettingName", SqlDbType.NVarChar, 50);
			paramSettingName.Value = data._settingName;
			myCommand.Parameters.Add(paramSettingName);

			SqlParameter paramSettingValue = new SqlParameter("@strSettingValue", SqlDbType.NVarChar); //Sejal:06/09/07:5913 - this sp is also updated
			paramSettingValue.Value = data._settingValue;
			myCommand.Parameters.Add(paramSettingValue);	
		
			SqlParameter paramSettingEncrypted = new SqlParameter("@bEncrypted", SqlDbType.Bit);
			paramSettingEncrypted.Value = data._isEncrypted;
			myCommand.Parameters.Add(paramSettingEncrypted);

			SqlParameter paramSettingDescription = new SqlParameter("@strSettingDescription", SqlDbType.NVarChar,200);
			paramSettingDescription.Value = data._settingDescription;
			myCommand.Parameters.Add(paramSettingDescription);

			SqlParameter paramSettingCategoryID = new SqlParameter("@intSettingCategoryID", SqlDbType.Int, 4);
			paramSettingCategoryID.Value = data._settingCategoryID;
			myCommand.Parameters.Add(paramSettingCategoryID);

			SqlParameter paramSettingID = new SqlParameter("@intSettingID", SqlDbType.Int, 4);
			paramSettingID.Value = data._settingID;
			paramSettingID.Direction = ParameterDirection.InputOutput;
			myCommand.Parameters.Add(paramSettingID);
            
			try
			{
				this.Controller.AddCommand(myCommand);
				myCommand.ExecuteNonQuery();
			
				data._settingID  = (int)paramSettingID.Value;
			}
			catch(Exception ex)
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
