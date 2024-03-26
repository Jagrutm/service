
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Soap;
using System.Xml.XPath;
using System.Xml.Xsl;


namespace CredECard.Common.BusinessService
{
	/// <summary>
	/// Summary description for GenerateHTML.
	/// </summary>
	public class GenerateHTML
	{
        /// <summary>
        /// Constructor
        /// </summary>
		public GenerateHTML()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        /// <summary>
        /// Gets HTML 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="xslFile"></param>
        /// <param name="folderPath"></param>
        /// <returns></returns>
		public static string GetHTML(object data,string xslFile,string folderPath)
		{

			MemoryStream fs= null;
			StreamReader strRead= null;
			try
			{
				
                XslCompiledTransform transForm = new XslCompiledTransform();
				// load xsl file for transform data
                transForm.Load(xslFile);
                				
				// memory stream object for contain transform data into memory stream
				fs=new MemoryStream();

				// transforming data into html		

				DateFormatter	objDate = new DateFormatter();
				XsltArgumentList args=new XsltArgumentList();

				//string UserAppDataDir=Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				SpecialFolderPath spPath=new SpecialFolderPath(folderPath);

				//Add an object to convert  DateTime format.
				args.AddExtensionObject("credecard:date-format", objDate);
				args.AddExtensionObject("credecard:SpecialFolderPath", spPath);

				transForm.Transform(new XPathDocument(GetSoapSerializeData(data)), args,fs);

				// set pointer at the start of the stream
				fs.Seek(0,SeekOrigin.Begin);

				// create stream reader object for return string data from stream
				strRead=new StreamReader(fs);
				string htmlString=strRead.ReadToEnd();		

				return htmlString;
			}
			finally
			{
				if (strRead != null) strRead.Close();
				if (fs != null) fs.Close();
			}
		}


		private static MemoryStream GetSoapSerializeData(object data)
		{	
			SoapFormatter soapFormat = new SoapFormatter();
			//string fileName = Path.GetTempPath() +  "Customers.xml"; 
			soapFormat.AssemblyFormat = FormatterAssemblyStyle.Simple;
			soapFormat.TypeFormat = FormatterTypeStyle.XsdString;
			//FileStream fs = new FileStream(fileName,FileMode.OpenOrCreate);
			MemoryStream fs = new MemoryStream();
			//foreach (DataItem item in data)
			soapFormat.Serialize( fs , data);
			//fs.Close();
			fs.Seek(0, SeekOrigin.Begin);	
	
			return fs;
		}



	}
}
