using System;

using CredECard.Common.BusinessService;
using CredECard.Common.Enums.Authorization;
using System.Text;
using System.Linq;
using System.IO;
//using CredECard.ServiceProvider.DataService;

namespace ContisGroup.MessageParser.ISO8586Parser
{
    /// <author>Prashant Soni</author>
    /// <created>08-Sep-2011</created>
    /// <summary>
    /// </summary>
    /// <exception cref="IndexOutOfRangeException">
    /// </exception>
    [Serializable()]
	public class DataElementList : DataCollection,ICloneable
	{

        /// <author>Prashant Soni</author>
        /// <created>08-Sep-2011</created>
        /// <summary>
        /// </summary>
        public DataElementList()
		{
		}

        /// <author>Prashant Soni</author>
        /// <created>08-Sep-2011</created>
        /// <summary>
        /// </summary>
        /// <param name="itemToAdd">
        /// </param>
        public void Add(DataElement itemToAdd)
		{
			List.Add(itemToAdd);
		}

        /// <author>Prashant Soni</author>
        /// <created>08-Sep-2011</created>
        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <exception cref="IndexOutOfRangeException">
        /// </exception>
		public void Remove(int index)
		{
			if (index > this.Count - 1 || index < 0)
			{
				throw new IndexOutOfRangeException("Specified index not found");
			}
			else
			{
				List.RemoveAt(index); 
			}
		}

        /// <author>Prashant Soni</author>
        /// <created>08-Sep-2011</created>
        /// <summary>
        /// </summary>
        /// <param name="itemToRemove">
        /// </param>
        public void Remove(DataElement itemToRemove)
		{
			List.Remove(itemToRemove);
		}

        /// <author>Prashant Soni</author>
        /// <created>08-Sep-2011</created>
        /// <summary>
        /// </summary>
        /// <param name="fieldNo">
        /// </param>
        /// <param name="element">
        /// </param>
        /// <returns>
        /// </returns>
        public DataElementList Insert(int fieldNo, DataElement element)
        {
            DataElementList temp = new DataElementList();
            bool isElementAdded = false;
            foreach (DataElement item in this)
            {
                if (item.FieldNo > fieldNo && !isElementAdded)
                {
                    temp.Add(element);
                    temp.Add(item);
                    isElementAdded = true;
                }
                else
                {
                    temp.Add(item);
                }
            }
            return temp;
        }

        /// <author>Prashant Soni</author>
        /// <created>08-Sep-2011</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        /// <exception cref="IndexOutOfRangeException">
        /// </exception>
        public DataElement this[int index]
		{
			get
			{
				if (index > this.Count - 1 || index < 0)
				{
					throw new IndexOutOfRangeException("Specified index not found");
				}
				else
				{
                    return (DataElement)List[index];
				}
			}
            set
            {
				if (index > this.Count - 1 || index < 0)
				{
					throw new IndexOutOfRangeException("Specified index not found");
				}
				else
				{
                    List[index] = value;
				}
            }
		}

        /// <author>Prashant Soni</author>
        /// <created>08-Sep-2011</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public DataElement this[EnumDataElement enumDataElement]
        {
            get
            {
                int index = (int)enumDataElement;
                return this[index.ToString()];
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>08-Sep-2011</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public DataElement this[EnumIPMDataElement enumDataElement]
        {
            get
            {
                int index = (int)enumDataElement;
                return this[index.ToString()];
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>08-Sep-2011</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public DataElement this[string fieldNo]
        {
            get
            {
                DataElement messageFound = null;

                for (int i = 0; i < this.Count; i++)
                {
                    if (((DataElement)List[i]).FieldNo == Convert.ToInt32(fieldNo))
                    {
                        messageFound = (DataElement)List[i];
                        break;
                    }
                }
                return messageFound;
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>08-Sep-2011</created>
        /// <summary>
        /// </summary>
        /// <param name="iso8583Vesion">
        /// </param>
        /// <param name="productType">
        /// </param>
        /// <returns>
        /// </returns>
        public static DataElementList GetDataElementList(EnumISOVesion iso8583Vesion, EnumProductType productType)
        {
            return ReadDataElement.GetDataElementList(iso8583Vesion, productType,EnumElementType.Data);
        }

        /// <author>Prashant Soni</author>
        /// <created>08-Sep-2011</created>
        /// <summary>
        /// </summary>
        /// <param name="iso8583Vesion">
        /// </param>
        /// <param name="productType">
        /// </param>
        /// <returns>
        /// </returns>
        public static DataElementList GetHeaderElementList(EnumISOVesion iso8583Vesion, EnumProductType productType)
        {
            return ReadDataElement.GetDataElementList(iso8583Vesion, productType,EnumElementType.Header);
        }

        /// <author>Prashant Soni</author>
        /// <created>08-Sep-2011</created>
        /// <summary>
        /// </summary>
        /// <param name="iso8583Vesion">
        /// </param>
        /// <param name="productType">
        /// </param>
        /// <param name="elementType">
        /// </param>
        /// <returns>
        /// </returns>
        public static DataElementList GetHeaderElementList(EnumISOVesion iso8583Vesion, EnumProductType productType,EnumElementType elementType)
        {
            return ReadDataElement.GetDataElementList(iso8583Vesion, productType, elementType);
        }
        //public static 
        /// <author>Prashant Soni</author>
        /// <created>08-Sep-2011</created>
        /// <summary>
        /// </summary>
        /// <param name="list">
        /// </param>
        /// <param name="usageNumber">
        /// </param>
        /// <returns>
        /// </returns>
        public static DataElementList LoadUsagewiseDataElements(DataElementList list, int usageNumber)
        {
            var newList = from DataElement element in list
                          where element.UsageNumber == usageNumber | element.UsageNumber == 0
                          select element;

            if (newList == null) return null;

            DataElementList newFilterlist = new DataElementList();
            foreach (DataElement element in newList)
            {
                newFilterlist.Add(element);
            }

            return newFilterlist;
        }


        /// <author>Prashant Soni</author>
        /// <created>08-Sep-2011</created>
        /// <summary>
        /// </summary>
        public void ClearValue()
        {
            foreach (DataElement element in this)
            {
                element.FieldValue = string.Empty;
                element.ActualElementLength  = 0;
            }
        }

        #region ICloneable Members

        /// <author>Prashant Soni</author>
        /// <created>08-Sep-2011</created>
        /// <summary>
        /// clone AgreementLimit object
        /// </summary>
        /// <returns>AgreementLimit</returns>
        public DataElementList Clone()
        {
            return (DataElementList)((ICloneable)this).Clone();
        }

        #endregion

        #region ICloneable Members

        /// <author>Prashant Soni</author>
        /// <created>08-Sep-2011</created>
        /// <summary>
        /// clone AgreementLimit object
        /// </summary>
        /// <returns>object</returns>
        object ICloneable.Clone()
        {
            DataElementList list = new DataElementList();
            foreach (DataElement element in this)
            {
                DataElement newElement = element.Clone();
                list.Add(newElement);
            }
            return list;
        }
        #endregion

        /// <author>Prashant Soni</author>
        /// <created>08-Sep-2011</created>
        /// <summary>
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (this == null) return string.Empty;
            StringBuilder response = new StringBuilder();
            foreach (DataElement element in this)
            {
                response.Append(element.ToString());
            }
            return response.ToString();
        }

        /// <author>Prashant Soni</author>
        /// <created>08-Sep-2011</created>
        /// <summary>
        /// This method returns Element's value in Byte array. if element is fixed length then it will not append length other wise it 
        /// include length byte into returned byte array.
        /// </summary>
        /// <returns>
        /// </returns>
        public byte[] ToByte()
        {
            if (this == null) return null;
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            foreach (DataElement element in this)
            {
                byte[] data = element.ToByte();
                if (data != null)
                    bw.Write(data);
            }

            bw.Flush();

            byte[] binaryMessage = ms.ToArray();
            bw.Close();

            return binaryMessage;            
        }

        /// <author>Prashant Soni</author>
        /// <created>08-Sep-2011</created>
        /// <summary>
        /// </summary>
        /// <returns>string</returns>
        public string LogData()
        {
            if (this == null) return string.Empty;

            StringBuilder response = new StringBuilder();
            foreach (DataElement element in this)
            {
                if (element.IsElementExist) response.Append(element.LogData());
                if (element.IsLoggableElement)
                {
                    if (element.ParsedSubElementList != null)
                    {
                        response.Append(element.ParsedSubElementList.LogData());
                    }
                }
            }
            return response.ToString();
        }
    }
}
