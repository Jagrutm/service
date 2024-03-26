using System;

namespace CredECard.Common.BusinessService
{
	/// <summary>
	/// Summary description for PinGenerator.
	/// </summary>
	public class PinGenerator
	{
		private const int intlength = 4;
        	
		public PinGenerator()
		{
		}
        
        /// <summary>
        /// Generate new random pin no.
        /// </summary>
        /// <returns>int</returns>
		private int GetNewPinNo()
		{
			string strPinNo=string.Empty;
			
			//get new pinno using random class.
			int randomNum;
			Random tempRndnum = new System.Random(unchecked((int)DateTime.Now.Ticks));
			randomNum = tempRndnum.Next( 1000,9999);
			return randomNum ;			
			
		}

        /// <summary>
        /// Gets new Pin no.
        /// </summary>
        /// <returns>string</returns>
		public string Generate()
		{
			
			string strPinNo = GetNewPinNo().ToString() ;
//			string encPinNo = string.Empty; 
//			// Create the enc version
//			if (strPinNo.Length != 0)
//			{
//				SymmCrypto objCrypto = new SymmCrypto(SymmCrypto.SymmProvEnum.DES);
//				encPinNo = objCrypto.Encrypting(strPinNo, "xp-Ps/*8");
//			}
			//return encPinNo;
			return strPinNo;
		}


		
	} 
	
}
