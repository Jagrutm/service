//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text;

//namespace BaseDatabase
//{
//    [Serializable()]
//    public class FieldErrorList : CollectionBase
//    {
//        /// <summary>
//        /// Add item to list
//        /// </summary>
//        /// <param name="itemToAdd"></param>
//        public void Add(FieldError itemToAdd)
//        {
//            List.Add(itemToAdd);
//        }

//        /// <summary>
//        /// Add range of items to this list
//        /// </summary>
//        /// <param name="range"></param>
//		public void AddRange(FieldErrorList range)
//        {
//            if (range != null && range.Count > 0)
//            {
//                foreach (FieldError singleError in range)
//                {
//                    this.Add(singleError);
//                }
//            }
//        }

//        /// <summary>
//        /// Get field error at given index
//        /// </summary>
//        /// <param name="index">int</param>
//        /// <returns>FieldError</returns>
//		public FieldError this[int index]
//        {
//            get
//            {
//                if (index > this.Count - 1 || index < 0)
//                {
//                    throw new IndexOutOfRangeException("Specified index is not found.");// CommonMessage.GetMessage(EnumErrorConstants.SPECIFIED_INDEX_NOT_FOUND));
//                }
//                else
//                {
//                    return (FieldError)List[index];
//                }
//            }
//        }

//        /// <summary>
//        /// Gets the FieldError for given fieldname
//        /// </summary>
//        /// <param name="item">string - name of field</param>
//        /// <returns>FieldError</returns>
//		public FieldError this[string item]
//        {
//            get
//            {

//                for (int i = 0; i < this.Count; i++)
//                {
//                    if (((FieldError)List[i]).FieldName == item)
//                        return (FieldError)List[i];
//                }
//                throw new IndexOutOfRangeException("Specified index is not found.");// CommonMessage.GetMessage(EnumErrorConstants.SPECIFIED_INDEX_NOT_FOUND));
//            }
//        }

//        /// <summary>
//        /// checks if error exists for given field name 
//        /// </summary>
//        /// <param name="item">string - field name</param>
//        /// <returns>string</returns>
//        public bool Exists(string item)
//        {
//            for (int i = 0; i < this.Count; i++)
//            {
//                if (((FieldError)List[i]).FieldName == item)
//                    return true;
//            }
//            return false;
//        }


//        /// <summary>
//        /// checks if error exists for given error number 
//        /// </summary>
//        /// <param name="item">int - error number</param>
//        /// <returns>bool</returns>
//        public bool Exists(int item)
//        {
//            for (int i = 0; i < this.Count; i++)
//            {
//                if (((FieldError)List[i]).ErrorNumber == item)
//                    return true;
//            }
//            return false;
//        }

//    }
//}
