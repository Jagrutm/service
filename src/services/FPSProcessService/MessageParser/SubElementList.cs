using System;
using System.IO;
using System.Linq;
using System.Text;
using CredECard.Common.BusinessService;
using CredECard.Common.Enums.Authorization;
//using CredECard.ServiceProvider.DataService;

namespace ContisGroup.MessageParser.ISO8586Parser
{
    [Serializable()]
	public class SubElementList : DataCollection,ICloneable
	{

        public SubElementList()
		{
		}

        public void Add(Element itemToAdd)
		{
			List.Add(itemToAdd);
		}

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

        public void Remove(Element itemToRemove)
		{
			List.Remove(itemToRemove);
		}

        public Element this[int index]
		{
			get
			{
				if (index > this.Count - 1 || index < 0)
				{
					throw new IndexOutOfRangeException("Specified index not found");
				}
				else
				{
                    return (Element)List[index];
				}
			}
		}

        public Element this[string fieldNo]
        {
            get
            {
                Element messageFound = null;

                for (int i = 0; i < this.Count; i++)
                {
                    if (List[i] is SubDataElement)
                    {
                        if (((SubDataElement)List[i]).SubElementNumber == Convert.ToInt32(fieldNo))
                        {
                            messageFound = (SubDataElement)List[i];
                            break;
                        }
                    }
                    else
                    {
                        if (((Element)List[i]).FieldNo == Convert.ToInt32(fieldNo))
                        {
                            messageFound = (Element)List[i];
                            break;
                        }
                    }
                }
                return messageFound;
            }
        }

        public Element this[string fieldNo, string tagNo]
        {
            get
            {
                Element messageFound = null;

                for (int i = 0; i < this.Count; i++)
                {
                    if (List[i] is PDSElement)
                    {
                        if (((PDSElement)List[i]).TagNumber == tagNo)
                        {
                            messageFound = (PDSElement)List[i];
                            break;
                        }
                    }
                    else
                    {
                        if (((PDSElement)List[i]).TagNumber == tagNo)
                        {
                            messageFound = (PDSElement)List[i];
                            break;
                        }
                    }
                }
                return messageFound;
            }
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>01-Mar-2013</created>
        /// <summary>Gets the sub element list.</summary>
        public static SubElementList GetSubElementList(EnumElementType ElementType)
        {
            return ReadSubDataElement.GetSubElementList(ElementType);
        }

        public static SubElementList GetSubDataElementList(EnumISOVesion iso8583Version,EnumProductType productType)
        {
            return ReadSubDataElement.GetSubElementList(iso8583Version, productType);
        }

        /// <author>Vipul Patel</author>
        /// <created>29-Jan-2021</created>
        /// <summary>Gets the sub element list.</summary>
        public static SubElementList MC_GetSubDataElementList(EnumISOVesion iso8583Version, EnumProductType productType,EnumDataFormat dataFormat)
        {
            return ReadSubDataElement.MC_GetSubElementList(iso8583Version, productType, dataFormat);
        }
        public static SubElementList LoadFieldwiseSubDataElements(string fieldNo, EnumISOVesion iso8583Vesion)
        {
            return ReadSubDataElement.LoadDataElementWiseSubDataElements(fieldNo, iso8583Vesion);
        }

        public void ClearValue()
        {
            foreach (Element element in this)
            {
                element.FieldValueinByte = null;
                element.FieldValue = string.Empty;
                element.ActualElementLength = 0;
            }
        }

        //public static SubElementList LoadFieldwiseSubDataElements(SubElementList list, int dataElementID)
        //{
        //    return LoadFieldwiseSubDataElements(list, dataElementID);
        //}

        public static SubElementList LoadFieldwiseSubDataElements(SubElementList list, int dataElementID)
        {
            var newList = from SubDataElement selement in list
                          where selement.DataElementID == dataElementID
                          select selement;

            if (newList == null) return null;

            SubElementList newFilterlist = new SubElementList();
            foreach (SubDataElement element in newList)
            {
                newFilterlist.Add(element);
            }

            return newFilterlist;
        }

        public static SubElementList GetPDSElementList(EnumProductType productType)
        {
            return ReadPDSElement.GetPDSElementList(productType);
        }

        public static SubElementList LoadSubElementwisePDSElementList(SubElementList list,int dataElementID, int parantPdsElementID)
        {
            var newList = from PDSElement selement in list
                          where selement.ParantPDSElementID == parantPdsElementID
                          select selement;

            if (newList == null) return null;

            SubElementList newFilterlist = new SubElementList();
            foreach (PDSElement element in newList)
            {
                newFilterlist.Add(element);
            }

            return newFilterlist;
        }

        #region ICloneable Members

        /// <summary>
        /// clone AgreementLimit object
        /// </summary>
        /// <returns>AgreementLimit</returns>
        public SubElementList Clone()
        {
            return (SubElementList)((ICloneable)this).Clone();
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// clone AgreementLimit object
        /// </summary>
        /// <returns>object</returns>
        object ICloneable.Clone()
        {
            SubElementList list = new SubElementList();
            foreach (Element element in this)
            {
                Element newElement = null;
                if (element is PDSElement)
                    newElement = ((PDSElement)element).Clone();
                else
                    newElement = ((SubDataElement)element).Clone();
                list.Add(newElement);
            }
            return list;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (this == null) return string.Empty;
            StringBuilder response = new StringBuilder();
            foreach (Element element in this)
            {
                string toString = element is PDSElement ? ((PDSElement)element).ToString() : ((SubDataElement)element).ToString();

                response.Append(toString);
            }
            return response.ToString();
        }

        public byte[] ToByte()
        {
            if (this == null) return null;
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            foreach (SubElement element in this)
            {
                byte[] data = null;
                if (element is PDSElement)
                {
                    if (((PDSElement)element).ParsedPDSSubFieldList != null)
                    {
                        MemoryStream msSub = new MemoryStream();
                        BinaryWriter bwSub = new BinaryWriter(msSub);
                        byte[] subPDSByte = null;
                        int discard = 0;
                        for (int i = 0; i < ((PDSElement)element).ParsedPDSSubFieldList.Count; i++)
                        {
                            PDSElement subPDS = ((PDSElement)((PDSElement)element).ParsedPDSSubFieldList[i]);
                            if (subPDS != null)
                            {
                                if (subPDS.TagNumber != string.Empty)
                                {
                                    byte[] subPdsByte = subPDS.ToByte();
                                    if (element.ProductType == EnumProductType.MasterCard)
                                    {
                                        int len = Encoding.ASCII.GetBytes(subPDS.TagNumber).Length;
                                        bwSub.Write(HexEncoding.ConvertAsciiToEbcdic(subPDS.TagNumber));
                                        bwSub.Write(HexEncoding.ConvertAsciiToEbcdic(subPdsByte.Length.ToString().PadLeft(len, '0')));
                                        bwSub.Write(subPdsByte);
                                    }
                                    else
                                    {
                                        bwSub.Write(HexEncoding.GetBytes(subPDS.TagNumber, out discard));
                                        bwSub.Write(HexEncoding.ByteToHexByte(subPDS.ToByte().Length));
                                        bwSub.Write(subPDS.ToByte());
                                    }
                                }
                            }
                        }

                        bwSub.Flush();

                        subPDSByte = msSub.ToArray();
                        bwSub.Close();

                        if (subPDSByte != null)
                        {
                            if (element.ProductType == EnumProductType.MasterCard)
                            {
                                string tag = ((PDSElement)element).TagNumber;
                                int len = tag.Length;
                                bw.Write(HexEncoding.ConvertAsciiToEbcdic(tag));
                                bw.Write(HexEncoding.ConvertAsciiToEbcdic(subPDSByte.Length.ToString().PadLeft(len, '0')));
                                bw.Write(subPDSByte);
                            }
                            else
                            {
                                bw.Write(HexEncoding.GetBytes(((PDSElement)element).TagNumber, out discard));
                                bw.Write(HexEncoding.Int16toHexBytes(subPDSByte.Length));
                                bw.Write(subPDSByte);
                            }
                        }
                    }
                    else
                    {
                        if (element.ProductType == EnumProductType.MasterCard)
                        {
                            int len = 0;
                            bool addInreasponse = false;
                            if (element is PDSElement)
                            {
                                if ( ((PDSElement)element).DataRepresentation == EnumDataRepresentment.PrivateDataSubElement
                                    && ((PDSElement)element).AuthPresenceNotationID == EnumPresenceNotation.Mandatory
                                    )
                                    addInreasponse = true;

                                if (addInreasponse)
                                {
                                    if (((PDSElement)element).DataFormat == EnumDataFormat.HEX || ((PDSElement)element).DataFormat == EnumDataFormat.BCD)
                                    {
                                        len = HexEncoding.GetBytes(((PDSElement)element).TagNumber).Length;
                                        bw.Write(HexEncoding.GetBytes(((PDSElement)element).TagNumber));
                                        bw.Write(HexEncoding.ByteToHexByte(element.FieldValueinByte.Length));
                                    }
                                    else
                                    {
                                        len = Encoding.ASCII.GetBytes(((PDSElement)element).TagNumber).Length;
                                        bw.Write(HexEncoding.ConvertAsciiToEbcdic(((PDSElement)element).TagNumber));
                                        bw.Write(HexEncoding.ConvertAsciiToEbcdic(element.FieldValueinByte.Length.ToString().PadLeft(len, '0')));
                                    }
                                    bw.Write(element.FieldValueinByte);
                                }
                            }
                        }
                        else
                        {
                            int discard = 0;
                            bw.Write(HexEncoding.GetBytes(((PDSElement)element).TagNumber, out discard));
                            bw.Write(HexEncoding.ByteToHexByte(element.ToByte().Length));
                            bw.Write(element.ToByte());
                        }
                    }
                }
                else
                    data = element.ToByte();

                if (data != null)
                    bw.Write(data);
            }

            bw.Flush();

            byte[] binaryMessage = ms.ToArray();
            bw.Close();

            return binaryMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>string</returns>
        public string LogData()
        {
            if (this == null) return string.Empty;
            StringBuilder response = new StringBuilder();
            foreach (SubElement element in this)
            {
                response.Append(element.LogData());

                if (element.ParsedSubFieldList != null)
                {
                    response.Append(element.ParsedSubFieldList.LogData());
                }
            }
            return response.ToString();
        }
    }
    
}
