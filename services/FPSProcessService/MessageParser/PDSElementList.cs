using System;

using ContisGroup.Common.BusinessService;
using System.Text;
//using CredECard.ServiceProvider.DataService;

namespace ContisGroup.MessageParser.ISO8586Parser
{
    [Serializable()]
	public class PDSElementList : DataCollection,ICloneable
	{

        public PDSElementList()
		{
		}

        public void Add(PDSElement itemToAdd)
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

        public void Remove(PDSElement itemToRemove)
		{
			List.Remove(itemToRemove);
		}

        public PDSElement this[int index]
		{
			get
			{
				if (index > this.Count - 1 || index < 0)
				{
					throw new IndexOutOfRangeException("Specified index not found");
				}
				else
				{
                    return (PDSElement)List[index];
				}
			}
		}

        public PDSElement this[string fieldNo]
        {
            get
            {
                PDSElement messageFound = null;

                for (int i = 0; i < this.Count; i++)
                {
                    if (((PDSElement)List[i]).FieldNo == Convert.ToInt32(fieldNo))
                    {
                        messageFound = (PDSElement)List[i];
                        break;
                    }
                }
                return messageFound;
            }
        }

        public static PDSElementList GetPDSElementList()
        {
            return ReadPDSElement.GetPDSElementList();
        }

        public void ClearValue()
        {
            foreach (PDSElement element in this)
            {
                element.FieldValue = string.Empty;
                element.ActualElementLength = 0;
            }
        }

        //public static PDSElementList LoadTagSubDataElements(PDSElementList list, int tagNo)
        //{
        //    var newList = from PDSElement selement in list
        //                  where selement.TagNumber == tagNo
        //                  select selement;

        //    if (newList == null) return null;

        //    PDSElementList newFilterlist = new PDSElementList();
        //    foreach (PDSElement element in newList)
        //    {
        //        newFilterlist.Add(element);
        //    }

        //    return newFilterlist;
        //}

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
            foreach (SubDataElement element in this)
            {
                SubDataElement newElement = element.Clone();
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
            foreach (SubDataElement element in this)
            {
                response.Append(element.ToString());
            }
            return response.ToString();
        }
    }
    
}
