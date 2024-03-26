using System;
using System.Linq;
using CredECard.Common.BusinessService;
using System.Text;
using CredECard.Common.Enums.Authorization;
//using CredECard.ServiceProvider.DataService;

namespace ContisGroup.MessageParser.ISO8586Parser
{
    [Serializable()]
	public class SubFieldList : DataCollection,ICloneable
	{

        public SubFieldList()
		{
		}

        public void Add(SubField itemToAdd)
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

        public void Remove(SubField itemToRemove)
		{
			List.Remove(itemToRemove);
		}

        public SubField this[int index]
		{
			get
			{
				if (index > this.Count - 1 || index < 0)
				{
					throw new IndexOutOfRangeException("Specified index not found");
				}
				else
				{
                    return (SubField)List[index];
				}
			}
		}

        public SubField this[string fieldNo]
        {
            get
            {
                SubField messageFound = null;

                for (int i = 0; i < this.Count; i++)
                {
                    if (((SubField)List[i]).SubFieldNumber == Convert.ToInt32(fieldNo))
                    {
                        messageFound = (SubField)List[i];
                        break;
                    }
                }
                return messageFound;
            }
        }

        public static SubFieldList GetSubFieldList()
        {
            return ReadSubField.GetSubFieldList();
        }

        public static SubFieldList GetSubFieldList(EnumProductType productType)
        {
            return ReadSubField.GetSubFieldList(productType);
        }

        public static SubFieldList LoadSubElementWiseSubFields(int subDataElementsID)
        {
            return ReadSubField.LoadSubElementWiseSubFields(subDataElementsID);
        }

        public void ClearValue()
        {
            foreach (SubField element in this)
            {
                element.FieldValue = string.Empty;
                element.ActualElementLength = 0;
            }
        }

        public static SubFieldList LoadSubElementWiseSubFields(SubFieldList list, int subDataElementsID)
        {
            var newList = from SubField selement in list
                          where selement.SubDataElementID == subDataElementsID 
                          select selement;

            if (newList == null) return null;

            SubFieldList newFilterlist = new SubFieldList();
            foreach (SubField element in newList)
            {
                newFilterlist.Add(element);
            }

            return newFilterlist;
        }

        public static SubFieldList LoadPDSTagWiseSubFields(SubFieldList list, string tagNo)
        {
            var newList = from SubField selement in list
                          where selement.TagNumber == tagNo
                          select selement;

            if (newList == null) return null;

            SubFieldList newFilterlist = new SubFieldList();
            foreach (SubField element in newList)
            {
                newFilterlist.Add(element);
            }

            return newFilterlist;
        }


        #region ICloneable Members

        /// <summary>
        /// clone AgreementLimit object
        /// </summary>
        /// <returns></returns>
        public SubFieldList Clone()
        {
            return (SubFieldList)((ICloneable)this).Clone();
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// clone AgreementLimit object
        /// </summary>
        /// <returns>object</returns>
        object ICloneable.Clone()
        {
            SubFieldList list = new SubFieldList();
            foreach (SubField element in this)
            {
                SubField newElement = element.Clone();
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
            foreach (SubField element in this)
            {
                response.Append(element.ToString());
            }
            return response.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>string</returns>
        public string LogData()
        {
            if (this == null) return string.Empty;
            StringBuilder response = new StringBuilder();
            SubField firstSubfield = null;
            foreach (SubField element in this)
            {
                if (firstSubfield == null && element.DataRepresentation == EnumDataRepresentment.Bit_only)
                {
                    response.Append("\t\t Sub Field : ");
                    firstSubfield = element;
                }
                response.Append(element.LogData());
            }
            if (firstSubfield != null && firstSubfield.DataRepresentation == EnumDataRepresentment.Bit_only)
                response.AppendLine();

            return response.ToString();
        }
    }
    
}
