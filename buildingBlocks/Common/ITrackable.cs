using System;

namespace CredECard.Common.BusinessService
{
	/// <summary>
	/// Summary description for ITrackable.
	/// </summary>
	public interface ITrackable
	{
		int ChangeTypeID 
		{
			get;
			set;
		}

		bool HasChanged
		{
			get;
		}

		DateTime LastChange
		{
			get;
		}

		void SetChanged();

		void Reset();
	}
}
