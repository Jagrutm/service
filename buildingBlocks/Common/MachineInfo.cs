namespace CredECard.Common.BusinessService
{    
	using System;
	using System.Net;

	/// <summary>
	/// Summary description for MachineInfo.
	/// </summary>
	public class MachineInfo
	{
		public MachineInfo()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <author>Falguni Parikh</author>
		/// <created>9-Mar-2006</created>
		/// <summary>
		/// To get the IP Address
		/// </summary>
		/// <returns>string
		/// </returns>
		public static string GetLocalIPAddress()
		{
			string hostName = Dns.GetHostName();
			string _localIPAddress=string.Empty;
			IPHostEntry localIP = Dns.GetHostEntry(hostName);    
			foreach(IPAddress ipaddress in localIP.AddressList)
			{
				_localIPAddress= ipaddress.ToString(); 
			}
			return _localIPAddress;
		}

		/// <author>Falguni Parikh</author>
		/// <created>9-Mar-2006</created>
		/// <summary>
		/// To get the operating system
		/// </summary>
		/// <returns>string
		/// </returns>
		public static string GetOSPlatform()
		{
			string osType=string.Empty;
			System.OperatingSystem osInfo = System.Environment.OSVersion;
			switch(osInfo.Platform)
			{
				case System.PlatformID.Win32Windows:
				{
					switch (osInfo.Version.Minor)
					{
						case 0:
							osType="Windows 95";
							break;
						case 10:
							if(osInfo.Version.Revision.ToString()=="2222A")
								osType="Windows 98 Second Edition";
							else
								osType="Windows 98";
							break;
						case  90:
							osType="Windows Me";
							break;
					}

				}
					break;
				case System.PlatformID.Win32NT:
				{
					switch(osInfo.Version.Major)
					{
						case 3:
							osType="Windows NT 3.51";
							break;
						case 4:
							osType="Windows NT 4.0";
							break;
						case 5:
							if (osInfo.Version.Minor==0)
								osType="Windows 2000";
							else
								osType="Windows XP";
							break;
					}
				}	
					break;
			}
			return osType;
		}

	}
}
